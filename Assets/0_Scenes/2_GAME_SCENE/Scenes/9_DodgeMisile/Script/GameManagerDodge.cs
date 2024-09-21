using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class GameManagerDodge : GameManager
{
    [SerializeField] private GameObject playerPrefab; // Photon으로 생성할 플레이어 프리팹
    [SerializeField] private GameObject players;

    [SerializeField] private List<Transform> playerPositionList; // 플레이어가 생성될 위치 리스트
    bool isTimerOn = false;
    [SerializeField] TextMeshProUGUI timerText;
    public float timer { get; private set; } = 0f;
    //start 하면 PunObject로 Players랑 Bullets 만들기
    //Bullets는 일단 
    void Start()
    {
        Debug.Log("Start");
        GameReady();
        //소리나고 Generate player
    }
    public override void GameStart()
    {
        Debug.Log("Start");

        BulletManager bulletManager = FindObjectOfType<BulletManager>();
        Debug.Log("Start2");
        bulletManager.generateBullet();
        Debug.Log("Start3");
        isTimerOn = true;
        Debug.Log("Start4");
        gameState = GameState.GameStart;
        Debug.Log("Start5, GameState: " + gameState);
    }

    void Update()
    {
        // 매 프레임 업데이트 코드 작성
        // 타이머가 켜져 있으면 매 프레임마다 시간 업데이트
        if (isTimerOn)
        {
            TimerUpdate();
        }
    }


    void TimerUpdate()
    {
        timer += Time.deltaTime;
        UpdateTimerText();
    }
    private void UpdateTimerText()
    {
        // 시간을 00.0s 형식으로 포맷하여 TextMeshProUGUI에 적용
        timerText.text = timer.ToString("00.00") + "s";
    }

    public void GameReady()
    {
        gameState = GameState.GameReady;
        // 자신의 인덱스를 가져옴 (예를 들어 LocalPlayer의 ActorNumber를 인덱스로 사용)
        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        Debug.Log("playerPrefab.name : " + playerPrefab.name);
        if (playerIndex < playerPositionList.Count)
        {
            Vector3 spawnPosition = playerPositionList[playerIndex].position;
            // 자신의 플레이어만 생성 (다른 클라이언트는 자신의 플레이어만 생성)
            GameObject newPlayer = PhotonNetwork.Instantiate("Player/DodgeMisile/" + playerPrefab.name, spawnPosition, Quaternion.identity);
            // 생성된 플레이어를 players 오브젝트의 자식으로 설정
            newPlayer.transform.SetParent(players.transform);

        }
        else
        {
            Debug.LogError("Player index가 플레이어 포지션 리스트를 초과했습니다.");
        }
        // 4초 후에 GameStart 메서드 호출
        StartCoroutine(GameStartAfterDelay(3f));
    }

    // 지정된 시간(초) 후에 GameStart를 호출하는 코루틴
    private IEnumerator GameStartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 지정된 시간만큼 대기
        GameStart(); // GameStart 메서드 호출
    }
}
