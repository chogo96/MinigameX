using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GenerateItems_Plane : MonoBehaviour
{
    string[] itemPrefabAddress = { "Items/Plane/Item_Kunai", "Items/Plane/Item_Normal", "Items/Plane/Item_Shield", "Items/Plane/Item_Life",
    "Items/Plane/Item_Dynamite",
     }; // 생성할 아이템의 프리팹 배열

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
        // 마스터 클라이언트인지 확인
        if (!PhotonNetwork.IsMasterClient)
        {
            // Debug.Log("마스터 클라이언트만 아이템을 생성할 수 있습니다.");
            return;
        }

        // Debug.Log("GenerateRandomItem");
        if (itemPrefabAddress != null && itemPrefabAddress.Length > 0)
        {
            // 랜덤한 아이템 선택
            int randomIndex = Random.Range(0, itemPrefabAddress.Length);
            string randomItemAddress = itemPrefabAddress[randomIndex]; // 프리팹의 이름 사용

            // 랜덤한 위치 계산
            float randomX = Random.Range(spawnRangeX.x, spawnRangeX.y);
            Vector2 spawnPosition = new Vector2(randomX, this.transform.position.y);

            // PhotonNetwork.Instantiate로 아이템 생성
            GameObject newItem = PhotonNetwork.Instantiate(randomItemAddress, spawnPosition, Quaternion.identity);

            // 생성된 아이템의 부모 설정
            newItem.transform.SetParent(this.transform);
        }
        else
        {
            Debug.LogWarning("Item prefabs are missing!");
        }
    }
}


