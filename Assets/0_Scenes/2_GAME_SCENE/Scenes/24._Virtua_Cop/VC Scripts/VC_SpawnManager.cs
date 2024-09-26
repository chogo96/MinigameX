using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;

public class VC_SpawnManager : MonoBehaviourPun
{
    public GameObject enemyPrefab;        // �� ������
    public GameObject hostagePrefab;      // ���� ������
    public Transform[] spawnPoints;       // ��ȯ ��ġ �迭
    public float spawnInterval = 2f;      // ��ȯ ����
    public TMP_Text gameOverText;         // ���� ���� �� ������ ǥ���� �ؽ�Ʈ UI
    private int score = 0;                // �÷��̾� ����
    private float gameDuration = 60f;     // ���� �ð� 1��
    private bool gameActive = true;       // ������ Ȱ��ȭ�Ǿ� �ִ��� ����

    void Start()
    {
        gameOverText.gameObject.SetActive(false); // ���� ���� �ؽ�Ʈ ��Ȱ��ȭ
        StartCoroutine(SpawnEnemiesAndHostages()); // ���� ������ ����
        StartCoroutine(GameTimer()); // 1�� �� ���� ����
    }

    IEnumerator SpawnEnemiesAndHostages()
    {
        while (gameActive)
        {
            yield return new WaitForSeconds(spawnInterval);

            int randomIndex = Random.Range(0, spawnPoints.Length);
            Vector3 spawnPosition = spawnPoints[randomIndex].position;

            // ���� ��ġ�� �̹� �� �Ǵ� ������ �ִ��� Ȯ��
            if (!IsSpawnPointOccupied(spawnPosition))
            {
                // ���� ������ �����ϰ� ����
                if (Random.value < 0.5f)
                {
                    Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                }
                else
                {
                    Instantiate(hostagePrefab, spawnPosition, Quaternion.identity);
                }
            }
        }
    }

    IEnumerator GameTimer()
    {
        yield return new WaitForSeconds(gameDuration);
        gameActive = false; // 1���� ������ ���� ��Ȱ��ȭ
        gameOverText.gameObject.SetActive(true); // ���� ���� �� ���� ǥ��
        gameOverText.text = $"GameOver!";
    }

    // ���� ��ġ�� ������Ʈ�� �ִ��� Ȯ���ϴ� �Լ�
    bool IsSpawnPointOccupied(Vector3 spawnPosition)
    {
        Collider[] colliders = Physics.OverlapSphere(spawnPosition, 1f); // �ݰ� 1m ���� ������Ʈ �˻�
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("VC_Enemy") || collider.CompareTag("VC_Hostage"))
            {
                // �� �Ǵ� ������ �̹� �ش� ��ġ�� ������ true ��ȯ
                return true;
            }
        }
        return false;
    }
}
