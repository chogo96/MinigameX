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
                Debug.Log("Hit: " + hit.collider.name); // Ray가 맞춘 오브젝트의 이름 출력

                // 적을 감지
                if (hit.collider.CompareTag("VC_Enemy"))
                {
                    Debug.Log("Enemy hit!"); // 적을 맞췄는지 확인

                    VC_Enemy enemy = hit.collider.GetComponent<VC_Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(100); // 클릭 시 적에게 100의 데미지 부여
                        Debug.Log("Enemy took damage!"); // 데미지를 준 상태 출력
                    }
                }
                else if (hit.collider.CompareTag("VC_Hostage"))
                {
                    Debug.Log("Hostage hit!"); // 적을 맞췄는지 확인

                    VC_Enemy enemy = hit.collider.GetComponent<VC_Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(100); // 클릭 시 적에게 100의 데미지 부여
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
                Debug.Log("Ray did not hit anything."); // 레이가 맞춘 것이 없을 때 로그 출력
            }
        }
    }
}
