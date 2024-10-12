using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Kunai_Plain : WeaponPlane
{
    //public float spreadAngle = 15f;

    void Start()
    {
        fireRate = 0.3f; // 쿠나이 발사 간격 (0.3초)

        localScale = 0.3f;
    }




    public override void Fire()
    {
        Debug.Log("Kunai: Launching Missile!");



        if (player == null)
        {
            Debug.Log("Player is null!");
            return;
        }

        // PhotonNetwork를 사용해 미사일을 생성
        GameObject missile = PhotonNetwork.Instantiate("Weapon/Plane/Kunai", player.transform.position, player.DirectionObject.transform.rotation);

        // 생성된 미사일의 parent를 지정 (미사일을 플레이어에 종속시킴)
        PhotonView missilePhotonView = missile.GetComponent<PhotonView>();

        missilePhotonView.RPC("SetMissileParentRPC", RpcTarget.All, player.GetComponent<PhotonView>().ViewID, localScale);

        // 마스터 클라이언트에서 일정 시간이 지나면 미사일 삭제
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(DestroyMissileAfterTime(missile, lifeTime));
        }
    }


}
