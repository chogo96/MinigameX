using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;

public class FB_PipeSpawner : MonoBehaviourPun
{
    public RectTransform topPipePrefab;
    public RectTransform bottomPipePrefab;
    public RectTransform coinPrefab;
    public RectTransform speedBoostPrefab;
    public RectTransform canvasRectTransform;
    public RectTransform balloonRectTransform;
    public float spawnInterval = 2f;
    public float minYPosition = -200f;
    public float maxYPosition = 200f;
    public float gapSize = 200f;
    public float itemSpacing = 50f;
    public TMP_Text scoreText;

    private int score = 0;
    private int speedLevel = 1;
    private float basePipeSpeed = 200f;
    private float reversePipeSpeed = 300f; // 충돌 시 오른쪽으로 밀려날 속도
    private bool isReversing = false; // 파이프와 아이템이 오른쪽으로 이동 중인지 여부
    private FB_BalloonTeamController balloonController;

    private void Start()
    {
        balloonController = balloonRectTransform.GetComponent<FB_BalloonTeamController>();
        balloonController.OnInvincibilityEnd += ResetPipeDirection;
        InvokeRepeating("SpawnPipesAndItems", 1f, spawnInterval);
        UpdateScoreText();
    }

    void SpawnPipesAndItems()
    {
        float randomY = Random.Range(minYPosition, maxYPosition);

        RectTransform topPipe = Instantiate(topPipePrefab, canvasRectTransform);
        topPipe.anchoredPosition = new Vector2(canvasRectTransform.rect.width / 2, randomY + gapSize / 2);
        topPipe.gameObject.AddComponent<PipeCollisionHandler>().Setup(this, balloonController);

        RectTransform bottomPipe = Instantiate(bottomPipePrefab, canvasRectTransform);
        bottomPipe.anchoredPosition = new Vector2(canvasRectTransform.rect.width / 2, randomY - gapSize / 2);
        bottomPipe.gameObject.AddComponent<PipeCollisionHandler>().Setup(this, balloonController);

        float startY = randomY - (gapSize / 2) + itemSpacing;
        float endY = randomY + (gapSize / 2) - itemSpacing;
        SpawnItemsBetweenPipes(startY, endY);

        StartCoroutine(MovePipe(topPipe));
        StartCoroutine(MovePipe(bottomPipe));
    }

    void SpawnItemsBetweenPipes(float startY, float endY)
    {
        float itemYPosition = startY;

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

        string itemType = itemPrefab == coinPrefab ? "Coin" : "SpeedBoost";
        item.gameObject.AddComponent<ItemCollisionHandler>().Setup(this, itemType);

        StartCoroutine(MoveItem(item));
    }

    IEnumerator MovePipe(RectTransform pipe)
    {
        while (pipe.anchoredPosition.x > -canvasRectTransform.rect.width / 2)
        {
            float pipeSpeed = isReversing ? reversePipeSpeed : basePipeSpeed;
            Vector2 direction = isReversing ? Vector2.right : Vector2.left;
            pipe.anchoredPosition += direction * pipeSpeed * Time.deltaTime;
            yield return null;
        }
        Destroy(pipe.gameObject);
    }

    IEnumerator MoveItem(RectTransform item)
    {
        while (item.anchoredPosition.x > -canvasRectTransform.rect.width / 2)
        {
            float pipeSpeed = isReversing ? reversePipeSpeed : basePipeSpeed;
            Vector2 direction = isReversing ? Vector2.right : Vector2.left;
            item.anchoredPosition += direction * pipeSpeed * Time.deltaTime;
            yield return null;
        }
        Destroy(item.gameObject);
    }

    public void HandleItemPickup(string itemType)
    {
        if (itemType == "Coin")
        {
            score += 200;
        }
        else if (itemType == "SpeedBoost")
        {
            if (speedLevel < 10) speedLevel++;
        }
        UpdateScoreText();
    }

    public void HandleCollision()
    {
        if (!isReversing)
        {
            StartCoroutine(ReversePipeDirection());  // 파이프와 아이템이 오른쪽으로 밀리는 효과 시작
        }

        score -= 100;
        speedLevel = 0;
        UpdateScoreText();
    }

    private IEnumerator ReversePipeDirection()
    {
        isReversing = true;  // 파이프와 아이템이 오른쪽으로 이동 시작
        yield return new WaitForSeconds(0.5f);  // 0.5초 동안 오른쪽으로 이동
        isReversing = false; // 다시 왼쪽으로 이동
    }

    void ResetPipeDirection()
    {
        isReversing = false; // 무적 상태가 끝나면 왼쪽으로 이동하도록 보장
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
    private FB_BalloonTeamController balloonController;

    public void Setup(FB_PipeSpawner spawner, FB_BalloonTeamController balloon)
    {
        pipeSpawner = spawner;
        balloonController = balloon;
    }

    private void Update()
    {
        if (!hasCollided && RectTransformOverlaps(GetComponent<RectTransform>(), pipeSpawner.balloonRectTransform))
        {
            if (!balloonController.isInvincible)
            {
                hasCollided = true;
                pipeSpawner.HandleCollision();
                balloonController.TriggerInvincibility();
            }
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
            Destroy(gameObject);
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
