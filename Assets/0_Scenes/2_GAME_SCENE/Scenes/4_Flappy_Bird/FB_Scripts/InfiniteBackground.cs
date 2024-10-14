using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    public RectTransform[] backgrounds;  // 두 개 이상의 배경 이미지
    public float scrollSpeed = 200f;     // 배경의 기본 스크롤 속도
    public RectTransform balloon;        // 열기구의 RectTransform (속도에 따라 배경 스크롤)

    private float backgroundWidth;       // 배경 하나의 실제 너비 (월드 스케일 반영)

    void Start()
    {
        // 배경 하나의 너비를 정확히 계산
        backgroundWidth = backgrounds[0].rect.width * backgrounds[0].localScale.x;
    }

    void Update()
    {
        // 열기구의 속도에 맞춰 배경 스크롤 속도 설정
        float moveSpeed = balloon.anchoredPosition.x * Time.deltaTime;

        foreach (RectTransform background in backgrounds)
        {
            // 배경을 왼쪽으로 이동
            background.anchoredPosition += Vector2.left * scrollSpeed * Time.deltaTime;

            // 배경이 화면 왼쪽 밖으로 나가면 뒤로 재배치
            if (background.anchoredPosition.x <= -backgroundWidth)
            {
                Vector2 newPos = background.anchoredPosition;
                newPos.x += backgroundWidth * backgrounds.Length;
                background.anchoredPosition = newPos;
            }
        }
    }
}
