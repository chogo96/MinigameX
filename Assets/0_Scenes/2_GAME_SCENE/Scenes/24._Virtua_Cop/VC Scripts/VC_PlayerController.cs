//using UnityEngine;

//public class VC_PlayerController : MonoBehaviour
//{
//    public Camera mainCamera;
//    public float rayRange = 100f;

//    void Update()
//    {
//        AimAndShoot();
//    }

//    void AimAndShoot()
//    {
//        if (Input.GetMouseButtonDown(0))
//        {
//            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
//            RaycastHit hit;

//            if (Physics.Raycast(ray, out hit, rayRange))
//            {
//                if (hit.collider.CompareTag("VC_Enemy"))
//                {
//                    VC_Enemy enemy = hit.collider.GetComponent<VC_Enemy>();
//                    if (enemy != null)
//                    {
//                        enemy.TakeDamage(100);
//                    }
//                }
//            }
//        }
//    }
//}
using UnityEngine;

public class VC_PlayerController : MonoBehaviour
{
    public Camera mainCamera;
    public float rayRange = 100f;

    void Update()
    {
        AimAndShoot();
    }

    void AimAndShoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Debug.DrawRay(ray.origin, ray.direction * rayRange, Color.red, 1.0f);

            if (Physics.Raycast(ray, out hit, rayRange))
            {
                Debug.Log("Hit: " + hit.collider.name); // Ray�� ���� ������Ʈ�� �̸� ���

                // ���� ����
                if (hit.collider.CompareTag("VC_Enemy"))
                {
                    Debug.Log("Enemy hit!"); // ���� ������� Ȯ��

                    VC_Enemy enemy = hit.collider.GetComponent<VC_Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(100); // Ŭ�� �� ������ 100�� ������ �ο�
                        Debug.Log("Enemy took damage!"); // �������� �� ���� ���
                    }
                }
                else if (hit.collider.CompareTag("VC_Hostage"))
                {
                    Debug.Log("Hostage hit!"); // ���� ������� Ȯ��

                    VC_Enemy enemy = hit.collider.GetComponent<VC_Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(100); // Ŭ�� �� ������ 100�� ������ �ο�
                        Debug.Log("TooBad!");
                    }
                }
                else
                {
                    Debug.Log("hit another Things!");
                }
            }
            else
            {
                Debug.Log("Ray did not hit anything."); // ���̰� ���� ���� ���� �� �α� ���
            }
        }
    }
}
