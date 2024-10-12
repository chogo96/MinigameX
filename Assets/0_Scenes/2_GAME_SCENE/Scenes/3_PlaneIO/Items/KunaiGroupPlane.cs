using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class KunaiGroupPlane : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
