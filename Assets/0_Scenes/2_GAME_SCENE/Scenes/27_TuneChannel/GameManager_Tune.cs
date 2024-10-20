using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager_Tune : GameManager
{
    Sprite[] TvChannelsSource;
    int[] SourceChannels;
    int[] PlayerTvChannels;
    int[] RoundChannels;
    [SerializeField] Tv MasterTv;
    int RoundQuantity = 10;
    int nowRound = -1;
    int nowChannel = -1;
    int ChannelQuantity = 7;
    [SerializeField] Timer timer;
    [SerializeField]
    GameObject Players;
    float RoundIdleTime = 2f;
    [SerializeField] TextMeshProUGUI RoundCounter;
    void Start()
    {
        PlayerTvChannels = new int[ChannelQuantity];
        RoundChannels = new int[RoundQuantity];
        TvChannelsSource = Resources.LoadAll<Sprite>("Images/TuneTv/Channel");
        GameInit();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void GameInit()
    {
        SourceChannels = GenerateSourceChannels(TvChannelsSource.Length);
        Debug.Log("GameInit()");
        //사용할 채널 만들기
        PlayerTvChannels = GenerateRNDChannel(ChannelQuantity, SourceChannels);
        do
        {
            RoundChannels = GenerateRNDChannel(RoundQuantity, PlayerTvChannels, true);
        }
        while (RoundChannels[0] == PlayerTvChannels[0]);
        GenerationPlayersTV();

        StartCoroutine(ChangeStateAfterDelay(3f));
    }
    void GenerationPlayersTV()
    {
        Debug.Log("GenerationPlayersTV()");
        Tv[] tvComponents = Players.GetComponentsInChildren<Tv>();

        // 찾은 Tv 컴포넌트를 가진 오브젝트들의 이름을 로그로 출력
        foreach (Tv tv in tvComponents)
        {
            Debug.Log("Found Tv in child: " + tv.gameObject.name);
            tv.SettingChannels(PlayerTvChannels);
        }
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

        //TimerStart
        nowRound = 1;
        StartCoroutine(GamePlay(RoundChannels));
    }
    // void GenerateRound()
    // {

    // }
    int[] GenerateRNDChannel(int ChannelQuantity, int[] parentList, bool allowDuplicates = false)
    {
        // 선택된 채널 인덱스를 저장할 리스트 생성
        List<int> ChannelIndexes = new List<int>();

        int[] newChannel = new int[ChannelQuantity];
        int intElement = -1;
        // 중복되지 않게 TvChannelsSource에서 ChannelQuantity만큼 인덱스를 선택
        for (int i = 0; i < ChannelQuantity; i++)
        {
            int randomIndex;

            // 고유한 인덱스를 찾을 때까지 반복
            do
            {
                randomIndex = Random.Range(0, parentList.Length);
                intElement = parentList[randomIndex];

            }
            //직전과 같으면 안된다, 중복허용이 안되면 , 중복시키면 안된다
            while ((ChannelIndexes.Contains(intElement) && !allowDuplicates) || i == 0 ? false : newChannel[i - 1] == intElement);
            // 고유한 인덱스를 리스트에 추가
            ChannelIndexes.Add(intElement);

            // 선택된 인덱스를 TvChannels 배열에 할당
            newChannel[i] = intElement; // Sprite 대신 인덱스를 저장
        }
        return newChannel;
    }

    IEnumerator GamePlay(int[] Rounds)
    {
        Debug.Log("GamePlay");
        timer.TimerStart(this);
        // UI변경
        for (int i = 0; i < Rounds.Length; i++)
        {

            RoundCounter.text = "Round : " + nowRound;
            Debug.Log($"{nowRound}번째 라운드 시작!");
            nowChannel = Rounds[i];
            MasterTv.ChannelChange(TvChannelsSource[Rounds[i]]);


            // // 클립 재생 시작
            // yield return StartCoroutine(PlayClipsSequentially(flagRounds[i].roundClips, i));
            CheckAnswerAndGetPoint();
            // // 2초 대기
            yield return new WaitForSeconds(RoundIdleTime * 5);
            Debug.Log("라운드 종료");


            // // 플레이어 리셋
            // PlayersReset();
            nowRound++;
        }

        yield return null;
    }

    int[] GenerateSourceChannels(int length)
    {
        // channelsSource 배열의 크기만큼 SourceChannels 배열을 생성
        int[] sourceChannels = new int[length];

        for (int i = 0; i < length; i++)
        {
            // 0부터 시작해서 1씩 증가하는 값을 sourceChannels에 저장
            sourceChannels[i] = i;
        }

        return sourceChannels;
    }
    void CheckAnswerAndGetPoint()
    {

    }
}