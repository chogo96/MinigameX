using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;

// 파이프와 아이템을 스폰하는 메인 스크립트
public class FB_PipeAndItemSpawner : MonoBehaviourPun
{
    public RectTransform topPipePrefab;      // 리버스 파이프 (위쪽에 있는 파이프)
    public RectTransform bottomPipePrefab;   // 정방향 파이프 (아래쪽에 있는 파이프)
    public RectTransform coinPrefab;         // 동전 아이템
    public RectTransform speedBoostPrefab;   // 스피드 아이템
    public RectTransform canvasRectTransform;
    public RectTransform balloonRectTransform;
    public TMP_Text scoreText;               // 점수 텍스트
    public TMP_Text comboText;               // 콤보 텍스트 추가
    public float spawnInterval = 2f;         // 파이프 스폰 간격
    public float minYPosition = -200f;       // 무작위 아이템 스폰 Y축 최소 위치 (기본값)
    public float maxYPosition = 200f;        // 무작위 아이템 스폰 Y축 최대 위치 (기본값)
    public float itemSpacing = 150f;         // 아이템 간 가로 간격
    private int score = 0;
    private int speedLevel = 0;              // 0~5단계로 나뉨
    private int comboMultiplier = 1;         // 최대 10배
    private float baseSpeed = 200f;          // 기본 X축 속도
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

    // 파이프와 아이템을 동시에 스폰하는 함수
    void SpawnPipesAndItems()
    {
        float randomY = Random.Range(minYPosition, maxYPosition); // 아이템 스폰 Y축 범위
        float spawnXPosition = canvasRectTransform.rect.width;

        // 3가지 상황 중 랜덤으로 선택: 0 = 리버스 파이프만, 1 = 정방향 파이프만, 2 = 리버스 + 정방향
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
                // 리버스 파이프와 정방향 파이프가 있는 상황
                float reverseY2 = Random.Range(350f, 480f);
                float normalY2 = Random.Range(-450f, -350f);
                SpawnPipe(topPipePrefab, spawnXPosition, reverseY2);
                SpawnPipe(bottomPipePrefab, spawnXPosition, normalY2);
                break;
        }

        // 파이프 사이에 아이템 스폰 (1:3 비율로 스피드 아이템:동전)
        SpawnItems(spawnXPosition, randomY);
    }

    // 파이프 스폰 함수
    void SpawnPipe(RectTransform pipePrefab, float xPosition, float yPosition)
    {
        RectTransform pipe = Instantiate(pipePrefab, canvasRectTransform);
        pipe.anchoredPosition = new Vector2(xPosition, yPosition);

        // 파이프 충돌 핸들러 추가
        pipe.gameObject.AddComponent<PipeCollisionHandler>().Setup(this, balloonRectTransform.GetComponent<FB_BalloonTeamController>());

        StartCoroutine(MovePipe(pipe));
    }

    // 아이템 스폰 함수: 파이프와 겹치지 않도록 체크한 후 스폰
    void SpawnItems(float startX, float startY)
    {
        for (int i = 0; i < 4; i++)
        {
            RectTransform itemPrefab = (i == 0) ? speedBoostPrefab : coinPrefab;  // 1:3 비율로 아이템 스폰
            float xPosition = startX - (i * itemSpacing);  // 가로 방향으로 스폰
            float yPosition = startY + Random.Range(-50f, 50f);  // 약간의 높이 변동을 줘서 자연스러움

            if (!IsOverlappingWithPipes(new Vector2(xPosition, yPosition))) // 파이프와 겹치는지 체크
            {
                SpawnItem(itemPrefab, new Vector2(xPosition, yPosition));
            }
        }
    }

    // 파이프와 아이템이 겹치는지 확인하는 함수
    bool IsOverlappingWithPipes(Vector2 itemPosition)
    {
        foreach (RectTransform pipe in canvasRectTransform.GetComponentsInChildren<RectTransform>())
        {
            if (pipe.gameObject.CompareTag("Pipe"))
            {
                if (RectTransformOverlaps(pipe, new RectTransform { anchoredPosition = itemPosition }))
                {
                    return true;  // 파이프와 겹친 경우
                }
            }
        }
        return false;
    }

    // RectTransform 겹침 확인 함수 추가
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

    // 아이템 스폰 함수: 아이템 스폰 시 충돌 핸들러 설정
    void SpawnItem(RectTransform itemPrefab, Vector2 position)
    {
        RectTransform item = Instantiate(itemPrefab, canvasRectTransform);
        item.anchoredPosition = position;

        // 아이템 타입에 따라 핸들러 설정 (동전 또는 스피드 아이템)
        string itemType = itemPrefab == coinPrefab ? "Coin" : "SpeedBoost";
        item.gameObject.AddComponent<ItemCollisionHandler>().Setup(this, itemType);

        StartCoroutine(MoveItem(item));  // 아이템 이동 시작
    }

    // 파이프 이동 함수
    IEnumerator MovePipe(RectTransform pipe)
    {
        while (pipe.anchoredPosition.x > -canvasRectTransform.rect.width / 2)
        {
            pipe.anchoredPosition += Vector2.left * currentSpeed * Time.deltaTime;
            yield return null;
        }
        Destroy(pipe.gameObject);
    }

    // 아이템 이동 함수
    IEnumerator MoveItem(RectTransform item)
    {
        while (item.anchoredPosition.x > -canvasRectTransform.rect.width / 2)
        {
            item.anchoredPosition += Vector2.left * currentSpeed * Time.deltaTime;
            yield return null;
        }
        Destroy(item.gameObject);
    }

    // 아이템 먹었을 때 처리: 점수 및 속도 증가, 콤보 업데이트
    public void HandleItemPickup(string itemType)
    {
        if (itemType == "Coin")
        {
            score += 100 * comboMultiplier;
            comboMultiplier = Mathf.Min(comboMultiplier + 1, 10);  // 최대 콤보 10배
            UpdateComboText();  // 콤보 텍스트 업데이트
        }
        else if (itemType == "SpeedBoost")
        {
            if (speedLevel < 5) speedLevel++;
            currentSpeed = baseSpeed * (1 + 0.1f * speedLevel);  // 속도 5단계까지 증가
        }
        UpdateScoreText();
    }

    // 파이프 충돌 처리: 속도 초기화 및 점수 감소, 콤보 초기화, 깜빡임
    public void HandleCollision()
    {
        score -= 100;
        speedLevel = 0;
        currentSpeed = baseSpeed;
        comboMultiplier = 1;  // 콤보 초기화
        UpdateComboText();    // 콤보 텍스트 업데이트
        UpdateScoreText();
        StartCoroutine(BlinkBalloon());  // 열기구 깜빡이기
    }

    // 열기구 깜빡이는 코루틴
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

    // 점수 업데이트 함수
    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    // 콤보 업데이트 함수
    void UpdateComboText()
    {
        comboText.text = "Combo: x" + comboMultiplier;
    }
}



// 파이프 충돌 핸들러
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
        // 충돌 감지: 열기구와 파이프가 겹칠 때
        if (!hasCollided && RectTransformOverlaps(GetComponent<RectTransform>(), pipeSpawner.balloonRectTransform))
        {
            if (!balloonController.isInvincible)
            {
                hasCollided = true;
                pipeSpawner.HandleCollision();  // 충돌 처리
                balloonController.TriggerInvincibility();  // 무적 모드
            }
        }
    }

    // RectTransform의 충돌 확인
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

// 아이템 충돌 핸들러
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
        // 충돌 감지: 열기구와 아이템이 겹칠 때
        if (!hasCollided && RectTransformOverlaps(GetComponent<RectTransform>(), pipeSpawner.balloonRectTransform))
        {
            hasCollided = true;
            pipeSpawner.HandleItemPickup(itemType);  // 아이템 처리
            Destroy(gameObject);  // 아이템 파괴
        }
    }

    // RectTransform의 충돌 확인
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
