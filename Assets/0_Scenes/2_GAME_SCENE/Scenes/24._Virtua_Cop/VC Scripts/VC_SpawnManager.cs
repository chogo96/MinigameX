using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;

public class VC_SpawnManager : MonoBehaviourPun
{
    public GameObject enemyPrefab;        // 적 프리팹
    public GameObject hostagePrefab;      // 인질 프리팹
    public Transform[] spawnPoints;       // 소환 위치 배열
    public float spawnInterval = 2f;      // 소환 간격
    public TMP_Text gameOverText;         // 게임 종료 후 점수를 표시할 텍스트 UI
    private int score = 0;                // 플레이어 점수
    private float gameDuration = 60f;     // 게임 시간 1분
    private bool gameActive = true;       // 게임이 활성화되어 있는지 여부

    void Start()
    {
        gameOverText.gameObject.SetActive(false); // 게임 종료 텍스트 비활성화
        StartCoroutine(SpawnEnemiesAndHostages()); // 적과 인질을 스폰
        StartCoroutine(GameTimer()); // 1분 후 게임 종료
    }

    IEnumerator SpawnEnemiesAndHostages()
    {
        while (gameActive)
        {
            yield return new WaitForSeconds(spawnInterval);

            int randomIndex = Random.Range(0, spawnPoints.Length);
            Vector3 spawnPosition = spawnPoints[randomIndex].position;

            // 스폰 위치에 이미 적 또는 인질이 있는지 확인
            if (!IsSpawnPointOccupied(spawnPosition))
            {
                // 적과 인질을 랜덤하게 스폰
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
        gameActive = false; // 1분이 지나면 게임 비활성화
        gameOverText.gameObject.SetActive(true); // 게임 종료 후 점수 표시
        gameOverText.text = $"GameOver!";
    }

    // 스폰 위치에 오브젝트가 있는지 확인하는 함수
    bool IsSpawnPointOccupied(Vector3 spawnPosition)
    {
        Collider[] colliders = Physics.OverlapSphere(spawnPosition, 1f); // 반경 1m 내의 오브젝트 검사
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("VC_Enemy") || collider.CompareTag("VC_Hostage"))
            {
                // 적 또는 인질이 이미 해당 위치에 있으면 true 반환
                return true;
            }
        }
        return false;
    }
}
