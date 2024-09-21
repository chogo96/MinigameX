using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private Rect RectAreaA;
    private Rect RectAreaB;
    private Rect RectAreaC;
    [SerializeField] private GameObject[] bulletPrefabs; // 여러 개의 프리팹 배열
    private int bulletCount = 30;
    private float velocityFactor = 0.5f;
    private List<GameObject> bullets = new List<GameObject>();
    private Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;

        // 카메라 뷰포트의 왼쪽 아래와 오른쪽 위를 월드 좌표로 변환
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));

        // Rect A: 카메라 뷰포트에 맞춰 설정
        RectAreaA = new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);

        // Rect B: A보다 모든 방향에서 1씩 큰 영역
        RectAreaB = new Rect(RectAreaA.xMin - 1, RectAreaA.yMin - 1, RectAreaA.width + 2, RectAreaA.height + 2);

        // Rect C: B보다 모든 방향에서 1씩 큰 영역
        RectAreaC = new Rect(RectAreaB.xMin - 1, RectAreaB.yMin - 1, RectAreaB.width + 2, RectAreaB.height + 2);
        Debug.Log("RectA : " + RectAreaA);
        Debug.Log("RectAreaB : " + RectAreaB);
        Debug.Log("RectAreaC : " + RectAreaC);

    }

    void Update()
    {
        if (GameManager.Instance.gameState == GameState.GameStart)
        {
            foreach (GameObject bullet in bullets)
            {
                Vector2 bulletPosition = bullet.transform.position;

                // C 영역에 있지만 B 영역이 아닌 경우
                if (RectAreaC.Contains(bulletPosition) && !RectAreaB.Contains(bulletPosition))
                {
                    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                    // // B 영역 내에서 A 영역이 아닌 곳으로 이동
                    bullet.transform.position = GetRandomPositionInBNotInA();

                    // // A 영역 내의 랜덤 위치 얻기
                    Vector2 targetPosition = GetRandomPositionInA();

                    // // 방향 계산
                    Vector2 direction = (targetPosition - new Vector2(bullet.transform.position.x, bullet.transform.position.y)).normalized;

                    // // Rigidbody2D 가져오기


                    if (rb != null)
                    {
                        // 속도 설정

                        rb.velocity = direction * rb.velocity.magnitude;
                    }
                }
            }
        }

    }



    // B 영역 내에서 A 영역이 아닌 랜덤 위치 얻기
    private Vector2 GetRandomPositionInBNotInA()
    {
        Vector2 position;
        do
        {
            position = new Vector2(
                Random.Range(RectAreaB.xMin, RectAreaB.xMax),
                Random.Range(RectAreaB.yMin, RectAreaB.yMax)
            );
        } while (RectAreaA.Contains(position));
        return position;
    }

    public void generateBullet()
    {
        bulletCount = (int)(30f * GameManager.WindowSize / GameManager.normalSize);

        if (GameManager.Instance.gameState != GameState.GameStart && GameManager.Instance.gameState != GameState.GameReady)
        {
            Debug.Log("게임 시작이 되지 않았습니다");
            return;
        }
        Debug.Log("generateBullet");
        // B 영역 내에서 C를 제외한 영역에 Bullet 생성
        for (int i = 0; i < bulletCount; i++)
        {
            int TypeBulletIndex = 0;
            switch (i % 10)
            {
                case 8:
                    TypeBulletIndex = 1;
                    break;
                case 9:
                    TypeBulletIndex = 1;
                    break;

            }


            GenBulletIndex(TypeBulletIndex);

        }
    }
    public void GenBulletIndex(int index)
    {
        float velocity = 1.5f;
        Vector2 spawnPosition = GetRandomPositionInBNotInA();
        // 랜덤한 bulletPrefab 선택

        velocity = velocity + index * 1f;
        GameObject bullet = Instantiate(bulletPrefabs[index], spawnPosition, Quaternion.identity);
        bullet.transform.SetParent(this.transform);
        bullets.Add(bullet);

        // A 영역 내의 랜덤 위치 얻기
        Vector2 targetPosition = GetRandomPositionInA();

        // 방향 계산
        Vector2 direction = (targetPosition - spawnPosition).normalized;

        // Rigidbody2D 가져오기
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // 속도 설정

            rb.velocity = direction * velocity * velocityFactor;
        }
    }

    private Vector2 GetRandomPositionInA()
    {
        return new Vector2(
            Random.Range(RectAreaA.xMin, RectAreaA.xMax),
            Random.Range(RectAreaA.yMin, RectAreaA.yMax)
        );
    }
    public void AllBulletStop()
    {
        foreach (GameObject bullet in bullets)
        {
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // 속도 설정

                rb.velocity = Vector2.zero;
            }
        }
    }
    public void clearBullets()
    {
        // 자식 오브젝트가 있는지 확인
        foreach (Transform child in transform)
        {
            // 자식 오브젝트 삭제
            Destroy(child.gameObject);
        }
        bullets.Clear();
    }
}
