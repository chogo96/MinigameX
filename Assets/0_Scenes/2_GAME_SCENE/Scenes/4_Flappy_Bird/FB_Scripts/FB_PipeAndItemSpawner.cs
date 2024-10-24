using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;

// �������� �������� �����ϴ� ���� ��ũ��Ʈ
public class FB_PipeAndItemSpawner : MonoBehaviourPun
{
    public RectTransform topPipePrefab;      // ������ ������ (���ʿ� �ִ� ������)
    public RectTransform bottomPipePrefab;   // ������ ������ (�Ʒ��ʿ� �ִ� ������)
    public RectTransform coinPrefab;         // ���� ������
    public RectTransform speedBoostPrefab;   // ���ǵ� ������
    public RectTransform canvasRectTransform;
    public RectTransform balloonRectTransform;
    public TMP_Text scoreText;               // ���� �ؽ�Ʈ
    public TMP_Text comboText;               // �޺� �ؽ�Ʈ �߰�
    public float spawnInterval = 2f;         // ������ ���� ����
    public float minYPosition = -200f;       // ������ ������ ���� Y�� �ּ� ��ġ (�⺻��)
    public float maxYPosition = 200f;        // ������ ������ ���� Y�� �ִ� ��ġ (�⺻��)
    public float itemSpacing = 150f;         // ������ �� ���� ����
    private int score = 0;
    private int speedLevel = 0;              // 0~5�ܰ�� ����
    private int comboMultiplier = 1;         // �ִ� 10��
    private float baseSpeed = 200f;          // �⺻ X�� �ӵ�
    private float currentSpeed;
    private bool isSpawningPaused = false;
    private float spawnTimer = 0f;

    private void Start()
    {
        currentSpeed = baseSpeed;
        UpdateScoreText();
    }

    private void Update()
    {
        if (!isSpawningPaused)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnInterval)
            {
                SpawnPipesAndItems();
                spawnTimer = 0f;
            }
        }
    }

    // �������� �������� ���ÿ� �����ϴ� �Լ�
    void SpawnPipesAndItems()
    {
        float randomY = Random.Range(minYPosition, maxYPosition); // ������ ���� Y�� ����
        float spawnXPosition = canvasRectTransform.rect.width;

        // 3���� ��Ȳ �� �������� ����: 0 = ������ ��������, 1 = ������ ��������, 2 = ������ + ������
        int situation = Random.Range(0, 3);

        switch (situation)
        {
            case 0:
                float reverseY = Random.Range(230f, 480f);
                SpawnPipe(topPipePrefab, spawnXPosition, reverseY);
                break;

            case 1:
                float normalY = Random.Range(-450f, -230f);
                SpawnPipe(bottomPipePrefab, spawnXPosition, normalY);
                break;

            case 2:
                // ������ �������� ������ �������� �ִ� ��Ȳ
                float reverseY2 = Random.Range(350f, 480f);
                float normalY2 = Random.Range(-450f, -350f);
                SpawnPipe(topPipePrefab, spawnXPosition, reverseY2);
                SpawnPipe(bottomPipePrefab, spawnXPosition, normalY2);
                break;
        }

        // ������ ���̿� ������ ���� (1:3 ������ ���ǵ� ������:����)
        SpawnItems(spawnXPosition, randomY);
    }

    // ������ ���� �Լ�
    void SpawnPipe(RectTransform pipePrefab, float xPosition, float yPosition)
    {
        RectTransform pipe = Instantiate(pipePrefab, canvasRectTransform);
        pipe.anchoredPosition = new Vector2(xPosition, yPosition);

        // ������ �浹 �ڵ鷯 �߰�
        pipe.gameObject.AddComponent<PipeCollisionHandler>().Setup(this, balloonRectTransform.GetComponent<FB_BalloonTeamController>());

        StartCoroutine(MovePipe(pipe));
    }

    // ������ ���� �Լ�: �������� ��ġ�� �ʵ��� üũ�� �� ����
    void SpawnItems(float startX, float startY)
    {
        for (int i = 0; i < 4; i++)
        {
            RectTransform itemPrefab = (i == 0) ? speedBoostPrefab : coinPrefab;  // 1:3 ������ ������ ����
            float xPosition = startX - (i * itemSpacing);  // ���� �������� ����
            float yPosition = startY + Random.Range(-50f, 50f);  // �ణ�� ���� ������ �༭ �ڿ�������

            if (!IsOverlappingWithPipes(new Vector2(xPosition, yPosition))) // �������� ��ġ���� üũ
            {
                SpawnItem(itemPrefab, new Vector2(xPosition, yPosition));
            }
        }
    }

    // �������� �������� ��ġ���� Ȯ���ϴ� �Լ�
    bool IsOverlappingWithPipes(Vector2 itemPosition)
    {
        foreach (RectTransform pipe in canvasRectTransform.GetComponentsInChildren<RectTransform>())
        {
            if (pipe.gameObject.CompareTag("Pipe"))
            {
                if (RectTransformOverlaps(pipe, new RectTransform { anchoredPosition = itemPosition }))
                {
                    return true;  // �������� ��ģ ���
                }
            }
        }
        return false;
    }

    // RectTransform ��ħ Ȯ�� �Լ� �߰�
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

    // ������ ���� �Լ�: ������ ���� �� �浹 �ڵ鷯 ����
    void SpawnItem(RectTransform itemPrefab, Vector2 position)
    {
        RectTransform item = Instantiate(itemPrefab, canvasRectTransform);
        item.anchoredPosition = position;

        // ������ Ÿ�Կ� ���� �ڵ鷯 ���� (���� �Ǵ� ���ǵ� ������)
        string itemType = itemPrefab == coinPrefab ? "Coin" : "SpeedBoost";
        item.gameObject.AddComponent<ItemCollisionHandler>().Setup(this, itemType);

        StartCoroutine(MoveItem(item));  // ������ �̵� ����
    }

    // ������ �̵� �Լ�
    IEnumerator MovePipe(RectTransform pipe)
    {
        while (pipe.anchoredPosition.x > -canvasRectTransform.rect.width / 2)
        {
            pipe.anchoredPosition += Vector2.left * currentSpeed * Time.deltaTime;
            yield return null;
        }
        Destroy(pipe.gameObject);
    }

    // ������ �̵� �Լ�
    IEnumerator MoveItem(RectTransform item)
    {
        while (item.anchoredPosition.x > -canvasRectTransform.rect.width / 2)
        {
            item.anchoredPosition += Vector2.left * currentSpeed * Time.deltaTime;
            yield return null;
        }
        Destroy(item.gameObject);
    }

    // ������ �Ծ��� �� ó��: ���� �� �ӵ� ����, �޺� ������Ʈ
    public void HandleItemPickup(string itemType)
    {
        if (itemType == "Coin")
        {
            score += 100 * comboMultiplier;
            comboMultiplier = Mathf.Min(comboMultiplier + 1, 10);  // �ִ� �޺� 10��
            UpdateComboText();  // �޺� �ؽ�Ʈ ������Ʈ
        }
        else if (itemType == "SpeedBoost")
        {
            if (speedLevel < 5) speedLevel++;
            currentSpeed = baseSpeed * (1 + 0.1f * speedLevel);  // �ӵ� 5�ܰ���� ����
        }
        UpdateScoreText();
    }

    // ������ �浹 ó��: �ӵ� �ʱ�ȭ �� ���� ����, �޺� �ʱ�ȭ, ������
    public void HandleCollision()
    {
        score -= 100;
        speedLevel = 0;
        currentSpeed = baseSpeed;
        comboMultiplier = 1;  // �޺� �ʱ�ȭ
        UpdateComboText();    // �޺� �ؽ�Ʈ ������Ʈ
        UpdateScoreText();
        StartCoroutine(BlinkBalloon());  // ���ⱸ �����̱�
    }

    // ���ⱸ �����̴� �ڷ�ƾ
    IEnumerator BlinkBalloon()
    {
        SpriteRenderer spriteRenderer = balloonRectTransform.GetComponent<SpriteRenderer>();
        for (int i = 0; i < 5; i++)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }

    // ���� ������Ʈ �Լ�
    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    // �޺� ������Ʈ �Լ�
    void UpdateComboText()
    {
        comboText.text = "Combo: x" + comboMultiplier;
    }
}



// ������ �浹 �ڵ鷯
public class PipeCollisionHandler : MonoBehaviour
{
    private bool hasCollided = false;
    private FB_PipeAndItemSpawner pipeSpawner;
    private FB_BalloonTeamController balloonController;

    public void Setup(FB_PipeAndItemSpawner spawner, FB_BalloonTeamController balloon)
    {
        pipeSpawner = spawner;
        balloonController = balloon;
    }

    private void Update()
    {
        // �浹 ����: ���ⱸ�� �������� ��ĥ ��
        if (!hasCollided && RectTransformOverlaps(GetComponent<RectTransform>(), pipeSpawner.balloonRectTransform))
        {
            if (!balloonController.isInvincible)
            {
                hasCollided = true;
                pipeSpawner.HandleCollision();  // �浹 ó��
                balloonController.TriggerInvincibility();  // ���� ���
            }
        }
    }

    // RectTransform�� �浹 Ȯ��
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

// ������ �浹 �ڵ鷯
public class ItemCollisionHandler : MonoBehaviour
{
    private bool hasCollided = false;
    private FB_PipeAndItemSpawner pipeSpawner;
    private string itemType;

    public void Setup(FB_PipeAndItemSpawner spawner, string type)
    {
        pipeSpawner = spawner;
        itemType = type;
    }

    private void Update()
    {
        // �浹 ����: ���ⱸ�� �������� ��ĥ ��
        if (!hasCollided && RectTransformOverlaps(GetComponent<RectTransform>(), pipeSpawner.balloonRectTransform))
        {
            hasCollided = true;
            pipeSpawner.HandleItemPickup(itemType);  // ������ ó��
            Destroy(gameObject);  // ������ �ı�
        }
    }

    // RectTransform�� �浹 Ȯ��
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
