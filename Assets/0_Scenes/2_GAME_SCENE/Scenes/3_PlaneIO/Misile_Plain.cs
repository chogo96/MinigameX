using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Misile_Plain : WeaponPlane
{
    GameObject missilePrefab;



    void Start()
    {
        fireRate = 1f; // 미사일 발사 간격 (1초)
                       // Resources 폴더에서 미사일 프리팹 로드
        missilePrefab = Resources.Load<GameObject>("Weapon/Bullet");
        if (missilePrefab == null)
        {
            Debug.LogError("Missile prefab not found in Resources/Weapon/Bullet");
        }
    }

    public override void Fire()
    {
        Debug.Log("Missile: Launching Missile!");
        if (missilePrefab == null)
        {
            Debug.Log("Missile is null!");
        }
        if (player == null)
        {
            Debug.Log("player is null!");
        }
        //  Instantiate(missilePrefab, player.transform.position, player.transform.rotation);
        GameObject missile = Instantiate(missilePrefab, player.transform.position, player.DirectionObject.transform.rotation, player.transform);
        Destroy(missile, lifeTime);
    }
}
