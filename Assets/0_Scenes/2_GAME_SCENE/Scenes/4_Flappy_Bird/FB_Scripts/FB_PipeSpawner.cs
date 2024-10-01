using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FB_PipeSpawner : MonoBehaviour
{
    public RectTransform pipePrefab;  // 파이프 프리팹 (Canvas 내에서 사용)
    public RectTransform canvasRectTransform;  // Canvas의 RectTransform
    public RectTransform balloonRectTransform;  // 열기구의 RectTransform (충돌 대상)
    public float spawnInterval = 2f;  // 파이프 소환 간격
    public float minYPosition = -200f; // 파이프 Y축 최소 위치 (Canvas 좌표 기준)
    public float maxYPosition = 200f;  // 파이프 Y축 최대 위치 (Canvas 좌표 기준)
    public float gapSize = 200f;       // 위-아래 파이프 간의 최소 빈 공간
    public TMP_Text scoreText;  // 점수를 표시할 텍스트

    private int score = 0;

    private void Start()
    {
        // 주기적으로 파이프를 소환
        InvokeRepeating("SpawnPipes", 1f, spawnInterval);
        UpdateScoreText();
    }

    void SpawnPipes()
    {
        // 파이프의 랜덤 Y 위치 설정
        float randomY = Random.Range(minYPosition, maxYPosition);

        // 상단 파이프 생성
        RectTransform topPipe = Instantiate(pipePrefab, canvasRectTransform);
        topPipe.anchoredPosition = new Vector2(canvasRectTransform.rect.width / 2, randomY + gapSize / 2);

        // 하단 파이프 생성
        RectTransform bottomPipe = Instantiate(pipePrefab, canvasRectTransform);
        bottomPipe.anchoredPosition = new Vector2(canvasRectTransform.rect.width / 2, randomY - gapSize / 2);

        // 파이프 이동 시작
        StartCoroutine(MovePipe(topPipe));
        StartCoroutine(MovePipe(bottomPipe));
    }

    // 파이프가 왼쪽으로 이동하는 코루틴
    IEnumerator MovePipe(RectTransform pipe)
    {
        while (pipe.anchoredPosition.x > -canvasRectTransform.rect.width / 2)
        {
            pipe.anchoredPosition += Vector2.left * 200f * Time.deltaTime; // 파이프가 왼쪽으로 이동

            // 충돌 감지
            if (RectOverlaps(pipe, balloonRectTransform))
            {
                HandleCollision();
            }

            yield return null;
        }

        // 화면을 벗어나면 파이프 삭제
        Destroy(pipe.gameObject);
    }

    // 두 RectTransform의 충돌을 감지하는 함수
    bool RectOverlaps(RectTransform rect1, RectTransform rect2)
    {
        return rect1.rect.Overlaps(rect2.rect);
    }

    // 파이프에 닿았을 때 점수 감소
    void HandleCollision()
    {
        score -= 100;
        UpdateScoreText();
    }

    // 점수 업데이트 함수
    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }
}
