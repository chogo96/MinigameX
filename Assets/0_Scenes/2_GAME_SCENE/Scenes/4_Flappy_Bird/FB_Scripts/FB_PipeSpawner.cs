using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;

public class FB_PipeSpawner : MonoBehaviourPun
{
    public RectTransform topPipePrefab;         // ���� ������ ������
    public RectTransform bottomPipePrefab;      // �Ʒ��� ������ ������
    public RectTransform coinPrefab;            // ���� ������
    public RectTransform speedBoostPrefab;      // �ӵ� ���� ������ ������
    public RectTransform canvasRectTransform;   // Canvas�� RectTransform
    public RectTransform balloonRectTransform;  // ���ⱸ�� RectTransform (�浹 ���)
    public float spawnInterval = 2f;            // ������ ��ȯ ����
    public float minYPosition = -200f;          // ������ Y�� �ּ� ��ġ (Canvas ��ǥ ����)
    public float maxYPosition = 200f;           // ������ Y�� �ִ� ��ġ (Canvas ��ǥ ����)
    public float gapSize = 200f;                // ��-�Ʒ� ������ ���� �ּ� �� ����
    public float itemSpacing = 50f;             // ������ ���� ����
    public TMP_Text scoreText;                  // ������ ǥ���� �ؽ�Ʈ

    private int score = 0;
    private int speedLevel = 1;                 // �ӵ� �ܰ� (1���� 10����)
    private float basePipeSpeed = 200f;         // �������� �⺻ �̵� �ӵ�

    private void Start()
    {
        InvokeRepeating("SpawnPipesAndItems", 1f, spawnInterval);
        UpdateScoreText();
    }

    void SpawnPipesAndItems()
    {
        float randomY = Random.Range(minYPosition, maxYPosition);

        // ��� ������ ����
        RectTransform topPipe = Instantiate(topPipePrefab, canvasRectTransform);
        topPipe.anchoredPosition = new Vector2(canvasRectTransform.rect.width / 2, randomY + gapSize / 2);
        topPipe.gameObject.AddComponent<PipeCollisionHandler>().Setup(this);

        // �ϴ� ������ ����
        RectTransform bottomPipe = Instantiate(bottomPipePrefab, canvasRectTransform);
        bottomPipe.anchoredPosition = new Vector2(canvasRectTransform.rect.width / 2, randomY - gapSize / 2);
        bottomPipe.gameObject.AddComponent<PipeCollisionHandler>().Setup(this);

        // ������ ���� �� ������ ������ ��ġ
        float startY = randomY - (gapSize / 2) + itemSpacing;  // �Ʒ��� ������ ���ʿ��� ����
        float endY = randomY + (gapSize / 2) - itemSpacing;    // ���� ������ �Ʒ��ʿ��� ��

        SpawnItemsBetweenPipes(startY, endY);

        // ������ �̵� ����
        StartCoroutine(MovePipe(topPipe));
        StartCoroutine(MovePipe(bottomPipe));
    }

    void SpawnItemsBetweenPipes(float startY, float endY)
    {
        float itemYPosition = startY;

        // ������ ������ �ΰ� ������ ���̿� ������� ��ġ
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

        // ������ �浹 �ڵ鷯 �߰�
        string itemType = itemPrefab == coinPrefab ? "Coin" : "SpeedBoost";
        item.gameObject.AddComponent<ItemCollisionHandler>().Setup(this, itemType);

        // ������ �̵� ����
        StartCoroutine(MoveItem(item));
    }

    IEnumerator MovePipe(RectTransform pipe)
    {
        float pipeSpeed = basePipeSpeed + (speedLevel - 1) * 20f; // �ӵ� �ܰ迡 ���� �ӵ� ����
        while (pipe.anchoredPosition.x > -canvasRectTransform.rect.width / 2)
        {
            pipe.anchoredPosition += Vector2.left * pipeSpeed * Time.deltaTime;
            yield return null;
        }
        Destroy(pipe.gameObject);
    }

    IEnumerator MoveItem(RectTransform item)
    {
        float pipeSpeed = basePipeSpeed + (speedLevel - 1) * 20f; // �ӵ� �ܰ迡 ���� ������ �ӵ� ����
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
            score += 200; // ���� ȹ�� �� ���� ����
        }
        else if (itemType == "SpeedBoost")
        {
            if (speedLevel < 10) speedLevel++; // �ӵ� ���� ���� (�ִ� 10�ܰ�)
        }
        UpdateScoreText();
    }

    public void HandleCollision()
    {
        score -= 100;
        speedLevel = 0; // �ӵ� �ܰ� �ʱ�ȭ
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }
}

// ������ �浹 �ڵ鷯 Ŭ����
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
            hasCollided = true;  // �� ���� �浹 ó��
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
            Destroy(gameObject); // ������ ����
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
