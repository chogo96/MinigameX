using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // 이동 속도
    public Vector3 cameraOffset = new Vector3(0, 0, -10); // 카메라가 플레이어와의 Z축 거리 유지

    void Update()
    {
        Vector3 inputVector = GetInput();
        Move(inputVector);
        FollowCamera();
    }

    // 플레이어 입력을 받아 이동 벡터 계산
    Vector3 GetInput()
    {
        float horizontal = UnityEngine.Input.GetAxisRaw("Horizontal");  // 좌우 입력 (A/D, 왼쪽/오른쪽 화살표)
        float vertical = UnityEngine.Input.GetAxisRaw("Vertical");      // 상하 입력 (W/S, 위/아래 화살표)

        // 입력 벡터를 생성 (X축, Y축에 따라)
        Vector3 inputVector = new Vector3(horizontal, vertical, 0f);

        // 벡터를 정규화하여 크기가 1인 방향 벡터로 만듦
        if (inputVector.magnitude > 0)
        {
            inputVector = inputVector.normalized;
        }

        return inputVector;
    }

    // 정규화된 벡터로 이동 처리
    void Move(Vector3 direction)
    {
        // 시간에 따라 이동량을 일정하게 하여 프레임에 독립적으로 이동
        Vector3 moveVector = direction * moveSpeed * Time.deltaTime;

        // 현재 위치에서 이동 벡터만큼 이동
        transform.Translate(moveVector);
    }

    // 카메라가 플레이어를 따라다니도록 설정
    void FollowCamera()
    {
        // 카메라 위치를 플레이어의 위치에 맞추고 Z축은 고정
        Camera.main.transform.position = transform.position + cameraOffset;
    }
}