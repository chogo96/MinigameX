using UnityEngine;

public class BalloonMovement : MonoBehaviour
{
    public float normalSpeed = 2f;    // �⺻ �ӵ�
    public float increasedSpeed = 5f; // �ӵ� ���� �������� �Ծ��� ���� �ӵ�
    public Transform cameraTransform; // ī�޶��� Transform

    private bool isSpeedIncreased = false;

    void Update()
    {
        // �ӵ� ����
        float moveSpeed = isSpeedIncreased ? increasedSpeed : normalSpeed;

        // ���ⱸ�� X������ �̵�
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);

        // ī�޶� ���ⱸ�� ����
        cameraTransform.position = new Vector3(transform.position.x, cameraTransform.position.y, cameraTransform.position.z);
    }

    public void IncreaseSpeed()
    {
        isSpeedIncreased = true;
        Invoke("ResetSpeed", 5f); // 5�� �� �ӵ��� ������� �ǵ���
    }

    void ResetSpeed()
    {
        isSpeedIncreased = false;
    }
}
