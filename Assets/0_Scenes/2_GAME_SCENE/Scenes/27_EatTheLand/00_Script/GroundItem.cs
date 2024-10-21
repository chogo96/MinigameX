using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GroundItem : MonoBehaviour
{
    public ItemObject item;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        item.OnHit(other);
        gameObject.SetActive(false);
    }
}
