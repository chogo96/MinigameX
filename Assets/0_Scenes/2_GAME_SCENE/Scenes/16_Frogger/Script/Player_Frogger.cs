using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFrogger : MonoBehaviour
{
    [SerializeField] float moveDistance = 1f; // 한 번 이동할 거리 (1 단위)
    [SerializeField] float moveSpeed = 5f;    // 이동 속도
    public bool isMoving = false;            // 움직이고 있는지 체크
    public bool isGoalIn = false;            // 움직이고 있는지 체크
    private Vector3 targetPosition;           // 이동할 목표 위치
    [SerializeField] AudioClip DieSound;
    private AudioSource audioSource;
    [SerializeField] private LayerMask wallLayer; // Wall 레이어를 Inspector에서 설정할 수 있도록
    [SerializeField] private LayerMask waterLayer; // floorLayer를 Inspector에서 설정할 수 있도록
    [SerializeField] private LayerMask floorLayer; // floorLayer를 Inspector에서 설정할 수 있도록

    [SerializeField] private LayerMask carrierLayer; // floorLayer를 Inspector에서 설정할 수 있도록

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        targetPosition = transform.position;  // 시작 위치를 현재 위치로 설정
    }

    void Update()
    {
        GetInput();
        Move();
        if (!isMoving)
        {
            CheckFloor();
            Carrier();

        }

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
    void CheckFloor()
    {

        // Z축 방향으로 아래로 Raycast 발사 (Vector3.back은 Z축 방향으로 아래)
        RaycastHit2D hitFloor = Physics2D.Raycast(transform.position, Vector3.back, 2, floorLayer);
        RaycastHit2D hitWater = Physics2D.Raycast(transform.position, Vector3.back, 2, waterLayer);

        // 바닥에 닿지 않고 물에 닿았을 때
        if (hitFloor.collider == null && hitWater.collider != null)
        {

            Debug.Log("Water detected! You Die");
            isMoving = false;
            PlayerDie();
        }
        else
        {

            // 다른 로직을 처리할 부분
        }
        if (hitFloor.collider != null)
        {
            // hitFloor.collider가 "Goal" 태그를 가졌는지 확인
            if (hitFloor.collider.CompareTag("Goal") && !isGoalIn)
            {
                // 만약에 골인지점이라면 GoalIn() 실행
                GoalIn();
            }
        }

    }

    public void PlayerDie()
    {
        audioSource.PlayOneShot(DieSound);
        Debug.Log("PlayerDie");

        // 코루틴 실행
        StartCoroutine(RespawnAfterDelay());
    }

    IEnumerator RespawnAfterDelay()
    {
        // 2초 대기
        yield return new WaitForSeconds(2f);

        // Respawn 호출
        Respawn();
    }

    void Respawn()
    {
        Debug.Log("Respawn");
    }
    void Carrier()
    {
        //IsCarrier 라면
        RaycastHit2D carrierHit = Physics2D.Raycast(transform.position, Vector3.back, 2, carrierLayer);
        if (carrierHit.collider != null)
        {
            //위로 레이저를 쏴서 Player라면
            FroggerMover mover = carrierHit.collider.GetComponent<FroggerMover>();

            if (mover.isCarrier)
            {
                // Debug.Log("Mover " + carrierHit.collider.name + "is Carrier");

                transform.Translate(mover.movement * mover.speed * Time.deltaTime, Space.World);

            }
        }

    }
    void GoalIn()
    {
        Debug.Log("GOAL IN!!!");
        isGoalIn = true;
    }
}
