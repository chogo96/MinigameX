using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai_Plain : WeaponPlane
{
    GameObject kunaiPrefab;
    //public float spreadAngle = 15f;

    void Start()
    {
        fireRate = 0.3f; // 쿠나이 발사 간격 (0.3초)

        kunaiPrefab = Resources.Load<GameObject>("Weapon/Kunai");
        if (kunaiPrefab == null)
        {
            Debug.LogError("Missile prefab not found in Resources/Weapon/Kunai");
        }
    }

    // public override void Fire()
    // {
    //     Debug.Log("Kunai: Throwing Kunai in 3 directions!");

    //     // 기본 방향
    //     Instantiate(kunaiPrefab, player.transform.position, player.transform.rotation);

    //     // // 좌측 발사
    //     // Quaternion leftRotation = Quaternion.Euler(player.transform.rotation.eulerAngles + new Vector3(0, 0, spreadAngle));
    //     // Instantiate(kunaiPrefab, player.transform.position, leftRotation);

    //     // // 우측 발사
    //     // Quaternion rightRotation = Quaternion.Euler(player.transform.rotation.eulerAngles + new Vector3(0, 0, -spreadAngle));
    //     // Instantiate(kunaiPrefab, player.transform.position, rightRotation);
    // }


    public override void Fire()
    {
        Debug.Log("Kunai: Launching Missile!");
        if (kunaiPrefab == null)
        {
            Debug.Log("Kunai is null!");
        }
        if (player == null)
        {
            Debug.Log("player is null!");
        }
        GameObject missile = Instantiate(kunaiPrefab, player.transform.position, player.DirectionObject.transform.rotation, player.transform);
        // 일정 시간 후에 미사일을 삭제 (lifeTime 이후에 Destroy)
        Destroy(missile, lifeTime);
    }
}
