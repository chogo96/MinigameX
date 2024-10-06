using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlane : MonoBehaviour
{
    [SerializeField]
    float speed = 5f;
    [SerializeField] float damage = 10f;
    // Update is called once per frame
    void Update()
    {
        // 자신의 회전 방향으로 이동 (2D 게임 기준)
        transform.position += transform.up * speed * Time.deltaTime;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("Bullet Collide");

        // 충돌한 객체의 태그가 "Player"인지 확인
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger: " + other.gameObject.name);
            //other는 맞은 플레이어
            // 충돌한 객체가 PlayerPlane 컴포넌트를 가지고 있으면 무기 변경
            PlayerPlane shootingPlayer = GetComponentInParent<PlayerPlane>();
            PlayerPlane damagedPlayer = other.GetComponent<PlayerPlane>();
            if (damagedPlayer != null)
            {
                if (damagedPlayer == shootingPlayer)
                {
                    return;
                }
                damagedPlayer.Damaged(damage);

            }


        }
    }

}