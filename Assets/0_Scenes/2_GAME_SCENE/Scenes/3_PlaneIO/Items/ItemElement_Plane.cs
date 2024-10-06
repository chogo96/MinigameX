using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemElement_Plane : MonoBehaviour
{
    [SerializeField]
    GameObject newWeaponPrefab;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // 2D Trigger에 다른 Collider가 들어왔을 때 호출되는 메서드
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Something Collide");

        // 충돌한 객체의 태그가 "Player"인지 확인
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger: " + other.gameObject.name);

            // 충돌한 객체가 PlayerPlane 컴포넌트를 가지고 있으면 무기 변경
            PlayerPlane player = other.GetComponent<PlayerPlane>();
            if (player != null && newWeaponPrefab != null)
            {
                // 무기 프리팹을 사용해 무기를 변경
                player.ChangeWeapon(newWeaponPrefab);
                Debug.Log("Weapon Changed to: " + newWeaponPrefab.name);

                // 필요하다면 아이템을 파괴 (혹은 다른 로직 처리)
                Destroy(gameObject);
            }


        }
    }
}
