using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class Plane_DodgeMisile : MonoBehaviour
{
    // Start is called before the first frame update


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 적 오브젝트의 태그를 확인
        if (other.CompareTag("Bullet"))
        {
            Debug.Log("Enemy has entered the trigger area!");
            Players_DodgeMisile player = this.GetComponentInParent<Players_DodgeMisile>();
            if (player.GetComponent<PhotonView>().IsMine)
            {
                player.playerDie();
            }

        }
    }



}
