using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VC_CursorManager : MonoBehaviour
{
    public Image crosshairImage;         // 크로스헤어 이미지
    public ParticleSystem[] hitEffect;   // 타격 시 파티클 효과 (적과 인질 각각의 파티클)
    public Camera fpsCamera;             // FPS 카메라
    public Canvas canvas;                // 캔버스 (Screen Space - Camera 모드)
    public TMP_Text scoreText;               // 점수 표시 UI 텍스트
    private int score = 0;               // 점수 변수

    void Start()
    {
        UpdateScoreText();               // 처음 시작할 때 점수 UI 업데이트
    }

    void Update()
    {
        MoveCrosshair();

        if (Input.GetButtonDown("Fire1")) // 왼쪽 마우스 클릭
        {
            Shoot();
        }
    }

    // 크로스헤어를 마우스 위치로 자연스럽게 이동시키는 함수
    void MoveCrosshair()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 localPoint;

        // RectTransformUtility를 사용하여 스크린 좌표를 캔버스의 로컬 좌표로 변환
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            mousePosition,
            fpsCamera,
            out localPoint
        );

        // 크로스헤어의 anchoredPosition을 캔버스의 로컬 좌표로 설정
        crosshairImage.rectTransform.anchoredPosition = localPoint;
    }

    // 마우스 클릭 시 Raycast로 타격한 지점에 파티클 생성 및 점수 업데이트
    void Shoot()
    {
        Ray ray = fpsCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("VC_Enemy") || hit.collider.CompareTag("VC_Hostage"))
            {
                string targetType = hit.collider.CompareTag("VC_Enemy") ? "적" : "인질";
                Debug.Log($"{targetType}을 맞췄습니다!");

                if (targetType == "적")
                {
                    // 적을 맞췄을 때 파티클 재생
                    ParticleSystem effect = Instantiate(hitEffect[0], hit.point, Quaternion.LookRotation(hit.normal));
                    effect.Play();
                    Destroy(effect.gameObject, 2f); // 2초 후 파티클 제거

                    // 점수 1점 추가
                    UpdateScore(1);

                    VC_Enemy enemy = hit.collider.GetComponent<VC_Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(100); // 적에게 100의 데미지 부여
                    }
                }
                else if (targetType == "인질")
                {
                    // 인질을 맞췄을 때 파티클 재생
                    ParticleSystem effect = Instantiate(hitEffect[1], hit.point, Quaternion.LookRotation(hit.normal));
                    effect.Play();
                    Destroy(effect.gameObject, 2f); // 2초 후 파티클 제거

                    // 점수 -2점 차감
                    UpdateScore(-2);
                }
            }
        }
    }

    // 점수 업데이트 함수
    public void UpdateScore(int scoreChange)
    {
        score += scoreChange;           // 점수 변경
        UpdateScoreText();              // UI에 점수 업데이트
    }

    // 점수 텍스트 업데이트
    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }
}
