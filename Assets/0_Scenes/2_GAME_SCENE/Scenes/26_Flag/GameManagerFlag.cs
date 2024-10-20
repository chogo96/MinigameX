using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManagerFlag : GameManager
{
    // [SerializeField] AudioClip[] KrAudioClips;

    private AudioSource audioSource;

    private int numberRound = 10;
    private int nowRound = -1;
    private int nowTeamAPoint = 0;
    private int memberPoint = 10;
    private int teamPoint = 50;
    private int speedPoint = 30;
    [SerializeField] TextMeshProUGUI PointCounter;
    [SerializeField] TextMeshProUGUI RoundCounter;

    [SerializeField] AudioClip CorrectSound;
    [SerializeField] AudioClip InCorrectSound;

    PlayerFlag[] playerFlags;
    public FlagRound nowFlagRound;
    [SerializeField] GameObject[] PlayersPos;
    [SerializeField] GameObject TimeGauge;
    [SerializeField]
    float RoundTime = 1.0f;
    float NowRoundTime = 0f;
    float RoundIdleTime = 0.5f;
    private PhotonView photonView;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();


        nowFlagRound = null;
        photonView = GetComponent<PhotonView>();
        GameInit();
    }

    void Update()
    {

    }




    // Coroutine을 사용하여 3초 후에 게임 상태를 변경
    IEnumerator ChangeStateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameState = GameState.GameStart;
        Debug.Log("Game State Changed to: " + gameState);
        GameStart();
    }
    public override void GameInit()
    {
        // PhotonView 컴포넌트 확인


        // 현재 클라이언트의 소유 객체인지 확인
        if (photonView.IsMine)
        {
            // playerNumber에 따라 리소스에서 프리팹을 가져와서 생성
            InstantiatePlayer();
        }
        playerFlags = FindObjectsByType<PlayerFlag>(FindObjectsSortMode.None);
        // PlayerGenerate
        StartCoroutine(ChangeStateAfterDelay(3f));
    }

    private void InstantiatePlayer()
    {
        // playerNumber에 따라 리소스에서 프리팹 가져오기 (예: "PlayerPrefab1", "PlayerPrefab2")
        string prefabPath = $"Player/Flag/Player";  // 리소스 경로 (Resource 폴더 안에 있음)
        GameObject playerPrefab = Resources.Load<GameObject>(prefabPath);

        if (playerPrefab != null)
        {
            // PlayersPos 배열에서 해당 플레이어의 위치 가져오기
            Vector3 spawnPos = PlayersPos[PhotonNetwork.LocalPlayer.ActorNumber].transform.position;

            // PhotonNetwork.Instantiate를 사용해 네트워크상에서 인스턴스화
            PhotonNetwork.Instantiate(prefabPath, spawnPos, Quaternion.identity);
        }
        else
        {
            Debug.LogError($"Player prefab not found at path: {prefabPath}");
        }
    }
    public override void GameStart()
    {
        // base.GameStart();
        Debug.Log("GameStart");
        FlagRound[] flagRounds = new FlagRound[numberRound];
        PlayersReset();
        // 각 인스턴스를 생성해 배열에 넣어야 함
        for (int i = 0; i < flagRounds.Length; i++)
        {
            flagRounds[i] = new FlagRound(i);  // 배열의 각 원소에 인스턴스 할당
        }
        nowRound = 1;
        StartCoroutine(GamePlay(flagRounds));
    }
    IEnumerator GamePlay(FlagRound[] flagRounds)
    {
        Debug.Log("GamePlay");
        // UI변경
        for (int i = 0; i < flagRounds.Length; i++)
        {
            TimeGauge.transform.localScale = Vector3.one;
            RoundCounter.text = "Round : " + nowRound;
            Debug.Log($"{nowRound}번째 라운드 시작!");
            nowFlagRound = flagRounds[i];



            // 클립 재생 시작
            yield return StartCoroutine(PlayClipsSequentially(flagRounds[i].roundClips, i));

            // NowRoundTime을 RoundTime으로 초기화
            NowRoundTime = RoundTime;

            // 타임 게이지 업데이트를 위한 코루틴 시작
            yield return StartCoroutine(UpdateTimeGauge());

            CheckAnswerAndGetPoint();
            // 2초 대기
            yield return new WaitForSeconds(RoundIdleTime);
            Debug.Log("라운드 종료2");


            // 플레이어 리셋
            PlayersReset();
            nowRound++;
        }

        yield return null;
    }

    IEnumerator UpdateTimeGauge()
    {
        Vector3 initialScale = TimeGauge.transform.localScale;

        // 시간이 0 이하가 될 때까지 실행
        while (NowRoundTime > 0)
        {
            NowRoundTime -= Time.deltaTime;

            // 남은 시간에 비례하여 TimeGauge의 X 크기를 줄임
            float timeRatio = NowRoundTime / RoundTime;
            TimeGauge.transform.localScale = new Vector3(initialScale.x * timeRatio, initialScale.y, initialScale.z);

            yield return null;  // 다음 프레임까지 대기
        }

        // 타임 종료 시 처리
        NowRoundTime = 0;
        Debug.Log("라운드 종료");
    }


    void PlayersReset()
    {
        foreach (PlayerFlag player in playerFlags)
        {
            player.Reset();
        }
    }
    private IEnumerator PlayClipsSequentially(AudioClip[] roundClips, int index)
    {


        // for 구문으로 변환
        for (int j = 0; j < roundClips.Length; j++)
        {
            audioSource.clip = roundClips[j];
            audioSource.Play();
            Debug.Log($"{index}, {j} : PlayClipsSequentially");
            // 현재 재생 중인 클립이 끝날 때까지 대기
            while (audioSource.isPlaying)
            {
                yield return null;
            }

        }

        // 모든 클립 재생 후 2초 대기
        Debug.Log($"{index}, 모든 오디오 클립이 재생되었습니다. 2초 후 종료됩니다.");


        Debug.Log("종료되었습니다.");
    }
    void CheckAnswerAndGetPoint()
    {
        PlayerFlag playerFlag = FindObjectOfType<PlayerFlag>();

        if (nowFlagRound.AnswerBlueFlagState == playerFlag.blueFlagState && nowFlagRound.AnswerWhiteFlagState == playerFlag.whiteFlagState)
        {
            Debug.Log("정답입니다! +point50");
            nowTeamAPoint += memberPoint;
            PointCounter.text = nowTeamAPoint.ToString();
            audioSource.PlayOneShot(CorrectSound);
        }
        else
        {
            Debug.Log($"오답! +Answer: {nowFlagRound.AnswerBlueFlagState}, {nowFlagRound.AnswerWhiteFlagState}\n" +
            $"You: {playerFlag.blueFlagState} , {playerFlag.whiteFlagState}");
            audioSource.PlayOneShot(InCorrectSound);

        }
    }
}