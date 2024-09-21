using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFrogger : MonoBehaviour
{
    [SerializeField] float moveDistance = 1f; // 한 번 이동할 거리 (1 단위)
    [SerializeField] float moveSpeed = 5f;    // 이동 속도
    private bool isMoving = false;            // 움직이고 있는지 체크
    private Vector3 targetPosition;           // 이동할 목표 위치
    [SerializeField] AudioClip DieSound;
    private AudioSource audioSource;
    [SerializeField] private LayerMask wallLayer; // Wall 레이어를 Inspector에서 설정할 수 있도록

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        targetPosition = transform.position;  // 시작 위치를 현재 위치로 설정
    }

    void Update()
    {
        GetInput();
        Move();
    }

    void GetInput()
    {
        // 움직이지 않고 있을 때 입력을 받음
        if (!isMoving)
        {
            Vector3 direction = Vector3.zero;

            // 위로 이동
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                direction = Vector3.up;
            }
            // 아래로 이동
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                direction = Vector3.down;
            }
            // 왼쪽으로 이동
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                direction = Vector3.left;
            }
            // 오른쪽으로 이동
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                direction = Vector3.right;
            }

            // 방향이 설정된 경우에만 레이캐스트 수행
            if (direction != Vector3.zero)
            {
                // Raycast: 플레이어가 이동하려는 방향으로 1f 거리만큼 쏘기
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, moveDistance, wallLayer);

                if (hit.collider == null) // 벽이 없으면 이동
                {
                    targetPosition = transform.position + direction * moveDistance;
                    isMoving = true;
                }
                else
                {
                    Debug.Log("Wall detected! Cannot move.");
                }
            }
        }
    }

    void Move()
    {
        if (isMoving)
        {
            // 현재 위치에서 목표 위치로 부드럽게 이동
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // 목표 위치에 도달하면 움직임을 멈춤
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;  // 목표 위치에 정확히 위치
                isMoving = false;                    // 움직임 종료
            }
        }
    }

    public void PlayerDie()
    {
        audioSource.PlayOneShot(DieSound);
    }
}
