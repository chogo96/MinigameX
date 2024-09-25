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
        // ��� ��ƼŬ ��Ȱ��ȭ
        foreach (ParticleSystem particle in clickParticles)
        {
            particle.Stop();
        }
    }

    void Update()
    {
        // ���콺 Ŭ�� ��ġ�� ȭ�� ��ǥ���� ���� ��ǥ�� ��ȯ
        mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(Camera.main.transform.position.z);

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.z = Camera.main.nearClipPlane + 0.01f; // Ŀ���� ī�޶� �����̿� �ֵ��� ����

        // Ŀ���� ����ٴϴ� ������Ʈ ��ġ ����
        foreach (GameObject cursor in followCursor)
        {
            cursor.SetActive(true);
            cursor.transform.position = worldPos;
        }

        // ���콺 Ŭ�� �� ����ĳ��Ʈ�� ���� ����
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // ���� ������ ���� ��ƼŬ ���
                if (hit.collider.CompareTag("VC_Enemy") || hit.collider.CompareTag("VC_Hostage"))
                {
                    string targetType = hit.collider.CompareTag("VC_Enemy") ? "��" : "����";
                    Debug.Log($"{targetType}�� ������ϴ�!");

                    // ���� ������ ��ƼŬ ���
                    Vector3 hitPoint = hit.point;
                    hitPoint.z = Camera.main.nearClipPlane + 0.01f; // ī�޶�� ����� ��ġ���� ��ƼŬ ���

                    foreach (ParticleSystem particle in clickParticles)
                    {
                        if (!particle.isPlaying)
                        {
                            particle.Stop();
                            particle.Clear();
                            // ��ƼŬ�� ���� �������� �߻��ϵ��� ����
                            particle.transform.position = hitPoint;
                            particle.Play();
                            Debug.Log($"��ƼŬ ��� ����: {particle.name} at {hitPoint}");
                        }
                        else
                        {
                            Debug.Log($"��ƼŬ�� �̹� ��� ��: {particle.name}");
                        }
                    }
                }
                else
                {
                    Debug.Log("���� �ƴ� �ٸ� ������Ʈ�� ������ϴ�.");
                }
            }
            else
            {
                Debug.Log("��� �͵� ������ ���߽��ϴ�.");
            }
        }
    }
}
