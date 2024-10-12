using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;

public class FB_PipeSpawner : MonoBehaviourPun
{
    public RectTransform topPipePrefab;         // 위쪽 파이프 프리팹
    public RectTransform bottomPipePrefab;      // 아래쪽 파이프 프리팹
    public RectTransform coinPrefab;            // 동전 프리팹
    public RectTransform speedBoostPrefab;      // 속도 가속 아이템 프리팹
    public RectTransform canvasRectTransform;   // Canvas의 RectTransform
    public RectTransform balloonRectTransform;  // 열기구의 RectTransform (충돌 대상)
    public float spawnInterval = 2f;            // 파이프 소환 간격
    public float minYPosition = -200f;          // 파이프 Y축 최소 위치 (Canvas 좌표 기준)
    public float maxYPosition = 200f;           // 파이프 Y축 최대 위치 (Canvas 좌표 기준)
    public float gapSize = 200f;                // 위-아래 파이프 간의 최소 빈 공간
    public float itemSpacing = 50f;             // 아이템 간의 간격
    public TMP_Text scoreText;                  // 점수를 표시할 텍스트

    private int score = 0;
    private int speedLevel = 1;                 // 속도 단계 (1부터 10까지)
    private float basePipeSpeed = 200f;         // 파이프의 기본 이동 속도

    private void Start()
    {
        InvokeRepeating("SpawnPipesAndItems", 1f, spawnInterval);
        UpdateScoreText();
    }

    void SpawnPipesAndItems()
    {
        float randomY = Random.Range(minYPosition, maxYPosition);

        // 상단 파이프 생성
        RectTransform topPipe = Instantiate(topPipePrefab, canvasRectTransform);
        topPipe.anchoredPosition = new Vector2(canvasRectTransform.rect.width / 2, randomY + gapSize / 2);
        topPipe.gameObject.AddComponent<PipeCollisionHandler>().Setup(this);

        // 하단 파이프 생성
        RectTransform bottomPipe = Instantiate(bottomPipePrefab, canvasRectTransform);
        bottomPipe.anchoredPosition = new Vector2(canvasRectTransform.rect.width / 2, randomY - gapSize / 2);
        bottomPipe.gameObject.AddComponent<PipeCollisionHandler>().Setup(this);

        // 파이프 사이 빈 공간에 아이템 배치
        float startY = randomY - (gapSize / 2) + itemSpacing;  // 아래쪽 파이프 위쪽에서 시작
        float endY = randomY + (gapSize / 2) - itemSpacing;    // 위쪽 파이프 아래쪽에서 끝

        SpawnItemsBetweenPipes(startY, endY);

        // 파이프 이동 시작
        StartCoroutine(MovePipe(topPipe));
        StartCoroutine(MovePipe(bottomPipe));
    }

    void SpawnItemsBetweenPipes(float startY, float endY)
    {
        float itemYPosition = startY;

        // 아이템 간격을 두고 파이프 사이에 순서대로 배치
        if (itemYPosition <= endY)
        {
            SpawnItem(coinPrefab, itemYPosition);
            itemYPosition += itemSpacing;

            if (itemYPosition <= endY)
            {
                SpawnItem(speedBoostPrefab, itemYPosition);
            }
        }
    }

    void SpawnItem(RectTransform itemPrefab, float yPosition)
    {
        RectTransform item = Instantiate(itemPrefab, canvasRectTransform);
        item.anchoredPosition = new Vector2(canvasRectTransform.rect.width / 2, yPosition);

        // 아이템 충돌 핸들러 추가
        string itemType = itemPrefab == coinPrefab ? "Coin" : "SpeedBoost";
        item.gameObject.AddComponent<ItemCollisionHandler>().Setup(this, itemType);

        // 아이템 이동 시작
        StartCoroutine(MoveItem(item));
    }

    IEnumerator MovePipe(RectTransform pipe)
    {
        float pipeSpeed = basePipeSpeed + (speedLevel - 1) * 20f; // 속도 단계에 따라 속도 증가
        while (pipe.anchoredPosition.x > -canvasRectTransform.rect.width / 2)
        {
            pipe.anchoredPosition += Vector2.left * pipeSpeed * Time.deltaTime;
            yield return null;
        }
        Destroy(pipe.gameObject);
    }

    IEnumerator MoveItem(RectTransform item)
    {
        float pipeSpeed = basePipeSpeed + (speedLevel - 1) * 20f; // 속도 단계에 따라 아이템 속도 증가
        while (item.anchoredPosition.x > -canvasRectTransform.rect.width / 2)
        {
            item.anchoredPosition += Vector2.left * pipeSpeed * Time.deltaTime;
            yield return null;
        }
        Destroy(item.gameObject);
    }

    public void HandleItemPickup(string itemType)
    {
        if (itemType == "Coin")
        {
            score += 200; // 동전 획득 시 점수 증가
        }
        else if (itemType == "SpeedBoost")
        {
            if (speedLevel < 10) speedLevel++; // 속도 레벨 증가 (최대 10단계)
        }
        UpdateScoreText();
    }

    public void HandleCollision()
    {
        score -= 100;
        speedLevel = 0; // 속도 단계 초기화
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }
}

// 파이프 충돌 핸들러 클래스
public class PipeCollisionHandler : MonoBehaviour
{
    private bool hasCollided = false;
    private FB_PipeSpawner pipeSpawner;

    public void Setup(FB_PipeSpawner spawner)
    {
        pipeSpawner = spawner;
    }

    private void Update()
    {
        if (!hasCollided && RectTransformOverlaps(GetComponent<RectTransform>(), pipeSpawner.balloonRectTransform))
        {
            hasCollided = true;  // 한 번만 충돌 처리
            pipeSpawner.HandleCollision();
        }
    }

    bool RectTransformOverlaps(RectTransform rect1, RectTransform rect2)
    {
        Vector3[] rect1Corners = new Vector3[4];
        Vector3[] rect2Corners = new Vector3[4];

        rect1.GetWorldCorners(rect1Corners);
        rect2.GetWorldCorners(rect2Corners);

        Rect rect1World = new Rect(rect1Corners[0].x, rect1Corners[0].y,
            rect1Corners[2].x - rect1Corners[0].x, rect1Corners[2].y - rect1Corners[0].y);

        Rect rect2World = new Rect(rect2Corners[0].x, rect2Corners[0].y,
            rect2Corners[2].x - rect2Corners[0].x, rect2Corners[2].y - rect2Corners[0].y);

        return rect1World.Overlaps(rect2World);
    }
}

// 아이템 충돌 핸들러 클래스
public class ItemCollisionHandler : MonoBehaviour
{
    private bool hasCollided = false;
    private FB_PipeSpawner pipeSpawner;
    private string itemType;

    public void Setup(FB_PipeSpawner spawner, string type)
    {
        pipeSpawner = spawner;
        itemType = type;
    }

    private void Update()
    {
        if (!hasCollided && RectTransformOverlaps(GetComponent<RectTransform>(), pipeSpawner.balloonRectTransform))
        {
            hasCollided = true;
            pipeSpawner.HandleItemPickup(itemType);
            Destroy(gameObject); // 아이템 제거
        }
    }

    bool RectTransformOverlaps(RectTransform rect1, RectTransform rect2)
    {
        Vector3[] rect1Corners = new Vector3[4];
        Vector3[] rect2Corners = new Vector3[4];

        rect1.GetWorldCorners(rect1Corners);
        rect2.GetWorldCorners(rect2Corners);

        Rect rect1World = new Rect(rect1Corners[0].x, rect1Corners[0].y,
            rect1Corners[2].x - rect1Corners[0].x, rect1Corners[2].y - rect1Corners[0].y);

        Rect rect2World = new Rect(rect2Corners[0].x, rect2Corners[0].y,
            rect2Corners[2].x - rect2Corners[0].x, rect2Corners[2].y - rect2Corners[0].y);

        return rect1World.Overlaps(rect2World);
    }
}
