using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FroggerCar : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌한 오브젝트가 "Player" 태그를 가지고 있는지 확인
        if (other.gameObject.CompareTag("Player"))
        {
            // 로그 출력
            Debug.Log("Player has entered the trigger: " + other.gameObject.name + "\ncar name: " + this.gameObject.name);
            PlayerFrogger player = other.GetComponent<PlayerFrogger>();
            if (player != null)
            {
                player.PlayerDie();

            }
            else
            {
                Debug.Log("Player가 없습니다.");
            }
        }
    }
}
