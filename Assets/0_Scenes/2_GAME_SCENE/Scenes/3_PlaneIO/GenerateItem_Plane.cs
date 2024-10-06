using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateItems_Plane : MonoBehaviour
{
    [SerializeField] GameObject[] itemPrefabs; // 생성할 아이템의 프리팹 배열
    [SerializeField] float spawnInterval = 5f; // 아이템 생성 간격
    [SerializeField] Vector2 spawnRangeX = new Vector2(-35f, 35f); // X축에서 랜덤 생성 범위


    private float spawnTimer = 0f; // 타이머

    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = 0f; // 타이머 초기화
    }

    // Update is called once per frame
    void Update()
    {
        // 타이머 업데이트
        spawnTimer += Time.deltaTime;

        // 타이머가 생성 간격을 초과하면 아이템을 생성
        if (spawnTimer >= spawnInterval)
        {
            GenerateRandomItem();
            spawnTimer = 0f; // 타이머 초기화
        }
    }

    // 랜덤한 위치에 랜덤한 아이템을 생성하는 함수
    void GenerateRandomItem()
    {
        // Debug.Log("GenerateRandomItem");
        if (itemPrefabs != null && itemPrefabs.Length > 0)
        {
            // 랜덤한 아이템 선택
            int randomIndex = Random.Range(0, itemPrefabs.Length);
            GameObject randomItem = itemPrefabs[randomIndex];

            // 랜덤한 위치 계산
            float randomX = Random.Range(spawnRangeX.x, spawnRangeX.y);

            Vector2 spawnPosition = new Vector2(randomX, this.transform.position.y);

            // 아이템 생성 및 부모로 this.transform 지정
            GameObject newItem = Instantiate(randomItem, spawnPosition, Quaternion.identity);
            newItem.transform.SetParent(this.transform); // this.transform 안에 넣기
        }
        else
        {
            Debug.LogWarning("Item prefabs are missing!");
        }
    }
}
