using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ItemElement_Plane : MonoBehaviour
{
    [SerializeField]
    GameObject newWeaponPrefab;
    GameObject itemParent;
    [SerializeField] ItemTypePlane itemType;
    // Start is called before the first frame update
    void Start()
    {
        itemParent = GameObject.Find("Items");
        this.transform.SetParent(itemParent.transform);
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

            switch (itemType)
            {
                case ItemTypePlane.Weapon:
                    {
                        // Weapon 아이템 타입에 대한 처리 로직
                        Debug.Log("Weapon 아이템이 선택되었습니다.");
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
                        break;
                    }
                case ItemTypePlane.Shield:
                    {
                        // Shield 아이템 타입에 대한 처리 로직
                        Debug.Log("Shield 아이템이 선택되었습니다.");
                        PlayerPlane player = other.GetComponent<PlayerPlane>();
                        if (player.GetComponentInChildren<Shield_Plane>() != null)
                        {
                            Debug.Log("Shield가 이미 있음");
                            Destroy(gameObject);
                            break;
                        }
                        if (player != null)
                        {
                            PhotonView photonView = player.GetComponent<PhotonView>();

                            if (photonView != null)
                            {
                                // 모든 플레이어에게 RPC를 실행 (자신 포함)
                                photonView.RPC("EquiptShield", RpcTarget.All);
                                Destroy(gameObject);
                            }
                        }
                        break;
                    }


                case ItemTypePlane.Heal:
                    {
                        // Shield 아이템 타입에 대한 처리 로직
                        Debug.Log("Heal 아이템이 선택되었습니다.");
                        PlayerPlane player = other.GetComponent<PlayerPlane>();
                        if (player != null)
                        {
                            player.RestoreHp();
                            Destroy(gameObject);
                        }
                        break;
                    }
                // 다른 케이스들도 추가 가능
                default:
                    {
                        Debug.Log("알 수 없는 아이템 타입입니다.");
                        break;
                    }
            }




        }
    }
}
