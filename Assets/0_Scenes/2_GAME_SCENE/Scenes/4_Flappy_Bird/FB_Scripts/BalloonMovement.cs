using UnityEngine;

public class BalloonMovement : MonoBehaviour
{
    public float normalSpeed = 2f;    // 기본 속도
    public float increasedSpeed = 5f; // 속도 증가 아이템을 먹었을 때의 속도
    public Transform cameraTransform; // 카메라의 Transform

    private bool isSpeedIncreased = false;

    void Update()
    {
        // 속도 적용
        float moveSpeed = isSpeedIncreased ? increasedSpeed : normalSpeed;

        // 열기구가 X축으로 이동
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);

        // 카메라를 열기구에 고정
        cameraTransform.position = new Vector3(transform.position.x, cameraTransform.position.y, cameraTransform.position.z);
    }

    public void IncreaseSpeed()
    {
        isSpeedIncreased = true;
        Invoke("ResetSpeed", 5f); // 5초 후 속도를 원래대로 되돌림
    }

    void ResetSpeed()
    {
        isSpeedIncreased = false;
    }
}
