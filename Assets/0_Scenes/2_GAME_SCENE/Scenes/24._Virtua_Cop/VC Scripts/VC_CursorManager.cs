using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VC_CursorManager : MonoBehaviour
{
    private Vector3 mousePos;
    public ParticleSystem[] clickParticles;
    public GameObject[] followCursor;

    void Start()
    {
        // 모든 파티클 비활성화
        foreach (ParticleSystem particle in clickParticles)
        {
            particle.Stop();
        }
    }

    void Update()
    {
        // 마우스 클릭 위치를 화면 좌표에서 월드 좌표로 변환
        mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(Camera.main.transform.position.z);

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.z = Camera.main.nearClipPlane + 0.01f; // 커서가 카메라 가까이에 있도록 설정

        // 커서를 따라다니는 오브젝트 위치 갱신
        foreach (GameObject cursor in followCursor)
        {
            cursor.SetActive(true);
            cursor.transform.position = worldPos;
        }

        // 마우스 클릭 시 레이캐스트로 적을 감지
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // 적을 맞췄을 때만 파티클 재생
                if (hit.collider.CompareTag("VC_Enemy") || hit.collider.CompareTag("VC_Hostage"))
                {
                    string targetType = hit.collider.CompareTag("VC_Enemy") ? "적" : "인질";
                    Debug.Log($"{targetType}을 맞췄습니다!");

                    // 맞은 부위에 파티클 재생
                    Vector3 hitPoint = hit.point;
                    hitPoint.z = Camera.main.nearClipPlane + 0.01f; // 카메라와 가까운 위치에서 파티클 재생

                    foreach (ParticleSystem particle in clickParticles)
                    {
                        if (!particle.isPlaying)
                        {
                            particle.Stop();
                            particle.Clear();
                            // 파티클이 맞은 지점에서 발생하도록 설정
                            particle.transform.position = hitPoint;
                            particle.Play();
                            Debug.Log($"파티클 재생 시작: {particle.name} at {hitPoint}");
                        }
                        else
                        {
                            Debug.Log($"파티클이 이미 재생 중: {particle.name}");
                        }
                    }
                }
                else
                {
                    Debug.Log("적이 아닌 다른 오브젝트를 맞췄습니다.");
                }
            }
            else
            {
                Debug.Log("어떠한 것도 맞추지 못했습니다.");
            }
        }
    }
}
