using System.Collections;
using System.Collections.Generic;
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
    PlayerFlag[] playerFlags;
    public FlagRound nowFlagRound;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(ChangeStateAfterDelay(3f));
        playerFlags = FindObjectsByType<PlayerFlag>(FindObjectsSortMode.None);
        nowFlagRound = null;
    }

    void Update()
    {

    }

    // 다음 클립을 재생하는 함수
    // 다음 클립을 재생하는 함수


    IEnumerator PlayRoundAudioClip(AudioClip[] audioClips)
    {
        // AudioClip 배열이 비어있는지 확인
        if (audioClips == null || audioClips.Length == 0)
        {
            yield break;
        }

        foreach (AudioClip clip in audioClips)
        {
            if (clip != null) // null 체크
            {
                audioSource.clip = clip;
                audioSource.Play();

                // 오디오 클립이 끝날 때까지 대기
                while (audioSource.isPlaying)
                {
                    yield return null; // 매 프레임마다 대기
                }
            }
        }

        // 모든 클립이 끝난 후 할 작업
        Debug.Log("모든 오디오 클립이 끝났습니다.");
    }




    // Coroutine을 사용하여 3초 후에 게임 상태를 변경
    IEnumerator ChangeStateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameState = GameState.GameStart;
        Debug.Log("Game State Changed to: " + gameState);
        GameStart();
    }
    public override void GameStart()
    {
        // base.GameStart();
        Debug.Log("GameStart");
        FlagRound[] flagRounds = new FlagRound[numberRound];

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
        //UI변경,
        for (int i = 0; i < flagRounds.Length; i++)
        {
            RoundCounter.text = "Round : " + nowRound;
            Debug.Log($"{nowRound}번째 라운드 시작!");
            nowFlagRound = flagRounds[i];
            // 클립 재생 시작
            yield return StartCoroutine(PlayClipsSequentially(flagRounds[i].roundClips, i));

            // 2초 대기
            yield return new WaitForSeconds(2);
            CheckAnswerAndGetPoint();
            foreach (PlayerFlag player in playerFlags)
            {
                player.Reset();
            }

            nowRound++;


        }
        yield return null;

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
        }
        else
        {
            Debug.Log($"오답! +Answer: {nowFlagRound.AnswerBlueFlagState}, {nowFlagRound.AnswerWhiteFlagState}\n" +
            $"You: {playerFlag.blueFlagState} , {playerFlag.whiteFlagState}");
        }
    }
}