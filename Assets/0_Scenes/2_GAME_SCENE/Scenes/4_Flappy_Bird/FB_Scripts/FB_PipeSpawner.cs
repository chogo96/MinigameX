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
    public float spawnInterval = 2f;  // ������ ��ȯ ����
    public float minYPosition = -200f;
    public float maxYPosition = 200f;
    public float gapSize = 200f;  // ��-�Ʒ� ������ �� �ּ� ����
    public float itemSpacing = 50f;  // ������ ���� ����
    public TMP_Text scoreText;

    private int score = 0;
    private int speedLevel = 1;
    private float basePipeSpeed = 200f;
    private float reversePipeSpeed = 300f;  // �ǰ� �� ���������� �з��� �ӵ�
    private bool isReversing = false;
    private bool isSpawningPaused = false;
    private float spawnTimer = 0f;  // ���� Ÿ�̸�
    private float timePausedDuringReverse = 0f;  // �Ͻ������� �ð��� ����ϴ� ����
    private FB_BalloonTeamController balloonController;

    private void Start()
    {
        balloonController = balloonRectTransform.GetComponent<FB_BalloonTeamController>();
        balloonController.OnInvincibilityEnd += ResetPipeDirection;
        UpdateScoreText();
    }

    private void Update()
    {
        // ���� Ÿ�̸� ���� (�Ͻ����� ���°� �ƴ϶��)
        if (!isSpawningPaused)
        {
            spawnTimer += Time.deltaTime;

            // ���� ���� ���� �� ������ �� ������ ��ȯ
            if (spawnTimer >= spawnInterval)
            {
                SpawnPipesAndItems();
                spawnTimer = 0f;
            }
        }
    }

    void SpawnPipesAndItems()
    {
        float randomY = Random.Range(minYPosition, maxYPosition);

        // ��� ������ ����
        RectTransform topPipe = Instantiate(topPipePrefab, canvasRectTransform);
        topPipe.anchoredPosition = new Vector2(canvasRectTransform.rect.width / 2, randomY + gapSize / 2);
        topPipe.gameObject.AddComponent<PipeCollisionHandler>().Setup(this, balloonController);

        // �ϴ� ������ ����
        RectTransform bottomPipe = Instantiate(bottomPipePrefab, canvasRectTransform);
        bottomPipe.anchoredPosition = new Vector2(canvasRectTransform.rect.width / 2, randomY - gapSize / 2);
        bottomPipe.gameObject.AddComponent<PipeCollisionHandler>().Setup(this, balloonController);

        // ������ ���̿� �������� ����
        float startY = bottomPipe.anchoredPosition.y + (gapSize / 2);
        float endY = topPipe.anchoredPosition.y - (gapSize / 2);
        SpawnItemsBetweenPipes(startY, endY);

        StartCoroutine(MovePipe(topPipe));
        StartCoroutine(MovePipe(bottomPipe));
        spawnTimer = 2f;
    }

    void SpawnItemsBetweenPipes(float startY, float endY)
    {
        // ������ 3���� ������ ������ �߰��� ��ġ
        float totalItemHeight = itemSpacing * 2f;
        float middleY = (startY + endY) / 2;
        float firstItemY = middleY - itemSpacing;

        for (int i = 0; i < 3; i++)
        {
            RectTransform itemPrefab = (i == 2 && Random.Range(0, 4) == 0) ? speedBoostPrefab : coinPrefab;
            SpawnItem(itemPrefab, firstItemY + (i * itemSpacing));
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
        spawnTimer = spawnInterval - (1.5f);  // 0.5�� ���� �и� �ð��� �ݿ��Ͽ� Ÿ�̸� ����
        if (!isReversing)
        {
            StartCoroutine(ReversePipeDirection());
        }

        score -= 100;
        speedLevel = 0;
        UpdateScoreText();
    }

    private IEnumerator ReversePipeDirection()
    {
        isReversing = true;
        isSpawningPaused = true;  // ���� Ÿ�̸� �Ͻ�����

        yield return new WaitForSeconds(0.5f);  // 0.5�� ���� �и��� �ð�

        isReversing = false;
        // ���� Ÿ�̸� �簳�� �Բ� �и� �ð���ŭ �߰�
        isSpawningPaused = false;
    }

    void ResetPipeDirection()
    {
        isReversing = false;
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }
}



// ������ �浹 �ڵ鷯
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

// ������ �浹 �ڵ鷯 Ŭ����
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
