using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerFlag : GameManager
{
    [SerializeField] AudioClip[] KrAudioClips;

    private AudioSource audioSource;
    private int currentClipIndex = 0; // 현재 재생 중인 클립 인덱스
    private int numberRound = 10;
    private int nowRound = -1;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(ChangeStateAfterDelay(3f));
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
        base.GameStart();
        FlagRound[] flagRounds = new FlagRound[3];
        Debug.Log("GameStart");
        //GenerateRound();
    }
}