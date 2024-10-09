using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BulletPlane : MonoBehaviour
{
    [SerializeField]
    float speed = 5f;
    [SerializeField] float damage = 10f;
    // Update is called once per frame
    void Update()
    {
        // 자신의 회전 방향으로 이동 (2D 게임 기준)
        transform.position += transform.up * speed * Time.deltaTime;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("Bullet Collide");

        // 충돌한 객체의 태그가 "Player"인지 확인
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger: " + other.gameObject.name);
            //other는 맞은 플레이어
            // 충돌한 객체가 PlayerPlane 컴포넌트를 가지고 있으면 무기 변경
            PlayerPlane shootingPlayer = GetComponentInParent<PlayerPlane>();
            PlayerPlane damagedPlayer = other.GetComponent<PlayerPlane>();
            if (shootingPlayer == null)
            {
                Debug.Log("shootingPlayer==null");
            }
            if (shootingPlayer.GetComponent<PhotonView>() == null)
            {
                Debug.Log("shootingPlayer.GetComponent<PhotonView>()");
            }
            if (damagedPlayer == null)
            {
                Debug.Log("damagedPlayer == null");
            }
            if (damagedPlayer.GetComponent<PhotonView>() == null)
            {
                Debug.Log("damagedPlayer.GetComponent<PhotonView>() == null");
            }
            if (damagedPlayer != null)
            {
                if (damagedPlayer.GetComponent<PhotonView>().ViewID == shootingPlayer.GetComponent<PhotonView>().ViewID)
                {
                    // Debug.Log("내가 쏜 거임");
                    return;
                }
                //포톤뷰가 내꺼일 때만 실행 내부는 RPC작업
                if (GetComponent<PhotonView>().IsMine)
                {
                    Debug.Log("나의 총알에 누군가 맞음");
                    damagedPlayer.Damaged(damage);
                }

            }


        }
    }


    [PunRPC]
    void SetMissileParentRPC(int playerViewID, float localScale)
    {
        Debug.Log("SetMissileParentRPC!!");

        // Player의 PhotonView를 ID로 찾아옴
        PhotonView playerView = PhotonView.Find(playerViewID);

        if (playerView != null)
        {
            GameObject playerObject = playerView.gameObject;
            transform.SetParent(playerObject.transform); // 미사일의 부모를 Player로 설정
            transform.localScale = new Vector3(localScale, localScale, localScale);
        }
        else
        {
            Debug.Log("Player View is Null");
        }
    }
}