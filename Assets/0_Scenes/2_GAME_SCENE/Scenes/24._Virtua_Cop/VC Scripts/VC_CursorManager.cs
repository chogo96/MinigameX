using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VC_CursorManager : MonoBehaviour
{
    public Image crosshairImage;         // ũ�ν���� �̹���
    public ParticleSystem[] hitEffect;   // Ÿ�� �� ��ƼŬ ȿ�� (���� ���� ������ ��ƼŬ)
    public Camera fpsCamera;             // FPS ī�޶�
    public Canvas canvas;                // ĵ���� (Screen Space - Camera ���)
    public TMP_Text scoreText;               // ���� ǥ�� UI �ؽ�Ʈ
    private int score = 0;               // ���� ����

    void Start()
    {
        UpdateScoreText();               // ó�� ������ �� ���� UI ������Ʈ
    }

    void Update()
    {
        MoveCrosshair();

        if (Input.GetButtonDown("Fire1")) // ���� ���콺 Ŭ��
        {
            Shoot();
        }
    }

    // ũ�ν��� ���콺 ��ġ�� �ڿ������� �̵���Ű�� �Լ�
    void MoveCrosshair()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 localPoint;

        // RectTransformUtility�� ����Ͽ� ��ũ�� ��ǥ�� ĵ������ ���� ��ǥ�� ��ȯ
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            mousePosition,
            fpsCamera,
            out localPoint
        );

        // ũ�ν������ anchoredPosition�� ĵ������ ���� ��ǥ�� ����
        crosshairImage.rectTransform.anchoredPosition = localPoint;
    }

    // ���콺 Ŭ�� �� Raycast�� Ÿ���� ������ ��ƼŬ ���� �� ���� ������Ʈ
    void Shoot()
    {
        Ray ray = fpsCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("VC_Enemy") || hit.collider.CompareTag("VC_Hostage"))
            {
                string targetType = hit.collider.CompareTag("VC_Enemy") ? "��" : "����";
                Debug.Log($"{targetType}�� ������ϴ�!");

                if (targetType == "��")
                {
                    // ���� ������ �� ��ƼŬ ���
                    ParticleSystem effect = Instantiate(hitEffect[0], hit.point, Quaternion.LookRotation(hit.normal));
                    effect.Play();
                    Destroy(effect.gameObject, 2f); // 2�� �� ��ƼŬ ����

                    // ���� 1�� �߰�
                    UpdateScore(1);

                    VC_Enemy enemy = hit.collider.GetComponent<VC_Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(100); // ������ 100�� ������ �ο�
                    }
                }
                else if (targetType == "����")
                {
                    // ������ ������ �� ��ƼŬ ���
                    ParticleSystem effect = Instantiate(hitEffect[1], hit.point, Quaternion.LookRotation(hit.normal));
                    effect.Play();
                    Destroy(effect.gameObject, 2f); // 2�� �� ��ƼŬ ����

                    // ���� -2�� ����
                    UpdateScore(-2);
                }
            }
        }
    }

    // ���� ������Ʈ �Լ�
    public void UpdateScore(int scoreChange)
    {
        score += scoreChange;           // ���� ����
        UpdateScoreText();              // UI�� ���� ������Ʈ
    }

    // ���� �ؽ�Ʈ ������Ʈ
    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }
}
