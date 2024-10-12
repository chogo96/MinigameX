using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Misile_Plain : WeaponPlane
{
    [SerializeField] string WeaponName;



    void Start()
    {
        // fireRate = 0.3f; // 미사일 발사 간격 (1초)
        // Resources 폴더에서 미사일 프리팹 로드
        localScale = 1.0f;
    }

    public override void Fire()
    {
        Debug.Log("Misile: Launching Missile!");

        if (player == null)
        {
            Debug.Log("Player is null!");
            return;
        }

        // PhotonNetwork를 사용해 미사일을 생성
        GameObject missile = PhotonNetwork.Instantiate("Weapon/Plane/" + WeaponName, player.transform.position, player.DirectionObject.transform.rotation);
        // 미사일의 PhotonView 가져오기
        PhotonView missilePhotonView = missile.GetComponent<PhotonView>();

        missilePhotonView.RPC("SetMissileParentRPC", RpcTarget.All, player.GetComponent<PhotonView>().ViewID, localScale);



        // 마스터 클라이언트에서 일정 시간이 지나면 미사일 삭제
        if (PhotonNetwork.IsMasterClient && lifeTime > 0)
        {
            StartCoroutine(DestroyMissileAfterTime(missile, lifeTime));
        }
    }


}
