using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class CountdownText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText; // UI 텍스트를 연결할 변수
    [SerializeField] private int startCount = 3; // 시작 숫자 (기본값 3)
    [SerializeField] private float animationDuration = 1f; // 애니메이션 지속 시간

    private Coroutine countdownCoroutine; // 코루틴을 추적할 변수

    void Start()
    {
        // 초기화가 필요할 경우 여기에 작성
    }

    void OnEnable()
    {
        countdownText.enabled = true;
        // 이전에 실행 중인 코루틴이 있다면 중지
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }

        // 새 코루틴 시작
        countdownCoroutine = StartCoroutine(CountdownCoroutine());
    }

    private IEnumerator CountdownCoroutine()
    {
        for (int i = startCount; i > 0; i--)
        {
            countdownText.text = i.ToString(); // 현재 숫자 표시
            AnimateText(); // 애니메이션 실행
            yield return new WaitForSeconds(animationDuration); // 애니메이션 동안 대기
        }

        countdownText.text = "Go!"; // 카운트다운 후 "Go!" 표시
        AnimateText(); // 마지막 애니메이션 실행
        yield return new WaitForSeconds(animationDuration); // 대기 후 UI 텍스트 숨기기

        countdownText.enabled = false; // 카운트다운 종료 후 텍스트 숨기기

        countdownCoroutine = null; // 코루틴 종료 시 null로 설정
    }

    private void AnimateText()
    {
        // Scale Up 애니메이션
        countdownText.transform.DOScale(2f, animationDuration).SetEase(Ease.OutBack);

        // Fade Out 애니메이션
        countdownText.DOFade(0f, animationDuration).OnComplete(() =>
        {
            // 애니메이션 완료 후 스케일을 원래대로 되돌림
            countdownText.transform.localScale = Vector3.one;
            countdownText.DOFade(1f, 0f); // 투명도를 다시 원래대로 설정
        });
    }
}
