using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryPlane : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }




    // 2D Trigger에 다른 Collider가 들어왔을 때 호출되는 메서드
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 태그가 "Bullet" 또는 "Item"인지 확인
        if (other.CompareTag("Bullet") || other.CompareTag("Item"))
        {
            // 해당 객체 삭제
            // Destroy(other.gameObject);
            Debug.Log("Object with tag " + other.tag + " has been destroyed.");
        }
    }
}
