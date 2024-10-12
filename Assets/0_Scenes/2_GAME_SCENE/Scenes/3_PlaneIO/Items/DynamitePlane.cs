using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class DynamitePlane : MonoBehaviour
{
    // Start is called before the first frame update
    private ParticleSystem explosionParticle;
    [SerializeField]
    float speed = 5f;
    [SerializeField]
    float radius = 3f;
    [SerializeField] float damage = 10f;
    void Start()
    {
        explosionParticle = GetComponentInChildren<ParticleSystem>(true);
        explosionParticle.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

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
                    //Explode
                    PhotonView photonView = GetComponent<PhotonView>();
                    if (photonView != null)
                    {
                        photonView.RPC("ExplodeRPC", RpcTarget.AllBuffered);
                    }
                }

            }


        }
    }
    [PunRPC]
    void ExplodeRPC()
    {
        // 폭발 효과 시작
        explosionParticle.gameObject.SetActive(true);

        // 현재 객체의 스프라이트 렌더러를 비활성화해서 객체를 안 보이게 함
        GetComponent<SpriteRenderer>().enabled = false;

        // 필요하다면 plane을 비활성화
        // plane.SetActive(false);

        // 나의 위치를 기준으로 범위 내의 콜라이더들을 모두 찾음
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);
        PlayerPlane shootingPlayer = GetComponentInParent<PlayerPlane>();
        foreach (Collider2D hit in hits)
        {
            // 각 충돌체에서 PhotonView가 있는지 확인
            PlayerPlane player = hit.GetComponentInParent<PlayerPlane>();
            if (player == null) { return; }
            if (player.IsMine() && shootingPlayer != player)
            {
                player.Damaged(101);
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
