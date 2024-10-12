using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public GameObject mousePointer;
    Vector3 worldPos = Vector3.zero;
    private Vector3 mousePos = Vector3.zero; // Vector2 대신 Vector3로 변경
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MousePos();

    }

    void MousePos()
    {
        // 마우스 클릭 위치를 화면 좌표에서 월드 좌표로 변환
        mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(Camera.main.transform.position.z); // Z 값을 카메라와의 거리에 따라 설정

        worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = new Vector3(worldPos.x, worldPos.y, 0f);
    }

}
