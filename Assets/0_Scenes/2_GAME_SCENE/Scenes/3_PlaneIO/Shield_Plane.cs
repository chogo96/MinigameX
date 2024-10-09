using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Shield_Plane : MonoBehaviour
{
    private PhotonView photonView;
    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet") && photonView.IsMine)
        {

            //트리거에 들어온 객체의 정보를 로그로 출력
            Debug.Log($"{other.gameObject.name}이(가) 트리거에 접근했습니다.");
            if (other.GetComponentInParent<PlayerPlane>().IsMine())
            {
                Debug.Log("내 총알");
                return;
            }
            // RPC로 총알과 Shield 삭제
            photonView.RPC("DestroyObjects", RpcTarget.AllBuffered, other.GetComponent<PhotonView>().ViewID, photonView.ViewID);
        }
    }

    [PunRPC]
    void DestroyObjects(int bulletViewID, int shieldViewID)
    {
        // PhotonView ID로 찾아서 객체를 삭제
        PhotonView bulletView = PhotonView.Find(bulletViewID);
        PhotonView shieldView = PhotonView.Find(shieldViewID);

        if (bulletView != null && bulletView.IsMine)
        {
            PhotonNetwork.Destroy(bulletView.gameObject);
        }

        if (shieldView != null && shieldView.IsMine)
        {
            PhotonNetwork.Destroy(shieldView.gameObject);
        }
    }


    [PunRPC]
    void SetParentRPC(int playerViewID, float localScale)
    {
        Debug.Log("SetParentRPC!!");

        // Player의 PhotonView를 ID로 찾아옴
        PhotonView playerView = PhotonView.Find(playerViewID);

        if (playerView != null)
        {
            GameObject playerObject = playerView.gameObject;
            transform.SetParent(playerObject.transform); // 미사일의 부모를 Player로 설정
            //transform.localScale = new Vector3(localScale, localScale, localScale);
        }
        else
        {
            Debug.Log("Player View is Null");
        }
    }
}
