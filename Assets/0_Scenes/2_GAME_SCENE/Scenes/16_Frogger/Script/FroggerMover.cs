using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FroggerMover : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] public Direction moveDirection;
    [SerializeField] bool isCarrier;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    void Move()
    {
        Vector3 movement = Vector3.zero;

        // moveDirection에 따라 움직임 방향 설정
        switch (moveDirection)
        {
            case Direction.Up:
                movement = Vector3.up;
                break;
            case Direction.Down:
                movement = Vector3.down;
                break;
            case Direction.Left:
                movement = Vector3.left;
                break;
            case Direction.Right:
                movement = Vector3.right;
                break;
        }

        // 프레임마다 speed에 따라 이동
        // transform.Translate(movement * speed * Time.deltaTime);
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }
}
