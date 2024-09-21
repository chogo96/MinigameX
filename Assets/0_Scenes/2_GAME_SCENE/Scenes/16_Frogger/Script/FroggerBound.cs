using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FroggerBound : MonoBehaviour
{
    [SerializeField] GameObject NextBound;
    [SerializeField] Direction boundDirection;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start Log\n" + this.gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnTriggerEnter2D(Collider2D other)  // Collider2D 사용
    {
        if (other.GetComponent<FroggerMover>().moveDirection == boundDirection)
        {
            // Debug.Log("Trigger entered by: " + other.gameObject.name);

            // other 오브젝트의 현재 위치 가져오기
            Vector3 currentPosition = other.transform.position;

            // x 좌표를 NextBound의 x 좌표로 설정, 나머지 좌표는 유지
            currentPosition.x = NextBound.transform.position.x;

            // 오브젝트의 위치를 업데이트
            other.transform.position = currentPosition;
        }
    }
}
