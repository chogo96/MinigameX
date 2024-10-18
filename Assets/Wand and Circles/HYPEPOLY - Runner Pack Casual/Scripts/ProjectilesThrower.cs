using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilesThrower : MonoBehaviour
{
    public Transform projectileSpawnParent;
    public GameObject projectile;
    public float impulsePower = 100f;
    void Start()
    {
        SpawnProjectile();
    }
    public void SpawnProjectile()
    {
        Instantiate(projectile, projectileSpawnParent);
    }
    public void ThrowProjectile()
    {
        Rigidbody projectileRB = projectileSpawnParent.GetChild(0).gameObject.GetComponent<Rigidbody>();
        projectileRB.transform.parent = null;
        projectileRB.isKinematic = false;
        projectileRB.useGravity = true;
        projectileRB.AddForce(projectileRB.transform.TransformDirection(new Vector3(0f,0f,1f)) * impulsePower);
    }
}
