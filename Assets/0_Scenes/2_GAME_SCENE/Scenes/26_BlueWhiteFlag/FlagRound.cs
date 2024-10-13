using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class FlagRound
{
    public int round;
    private int[] ActionArray = new int[3];
    private string SoundResourceString = "Sound/Effect/Flag/flag-man-kr_1.4x/";
    public AudioClip[] roundClips = new AudioClip[3];
    public FlagState AnswerBlueFlagState = FlagState.Stay;
    public FlagState AnswerWhiteFlagState = FlagState.Stay;

    void GenerateRound()
    {
        List<int> numbers = new List<int> { -4, -3, -2, -1, 1, 2, 3, 4 }; // 0을 제외한 배열
                                                                          /////////////////////////////////////////////////////////////////
        int FirstActionNum = numbers[Random.Range(0, numbers.Count)];
        FlagActionType FirstAction = (FlagActionType)FirstActionNum;

        // 선택된 FlagActionType 값을 출력
        Debug.Log($"{round}랜덤으로 선택된 FirstAction: " + FirstAction);
        if (!numbers.Contains(0) && FirstAction > 0)
        {
            numbers.Add(0);
        }
        int[] removeArray1 = removeNumArray(FirstActionNum);
        foreach (int removeNum in removeArray1)
        {
            numbers.RemoveAll(x => x == removeNum);
        }
        ////////////////////////////////////////////////////////////////
        int SecondActionNum = numbers[Random.Range(0, numbers.Count)];
        FlagActionType SecondAction = (FlagActionType)SecondActionNum;

        Debug.Log($"{round}랜덤으로 선택된 SecondAction: " + SecondAction);
        if (!numbers.Contains(0) && SecondActionNum > 0)
        {
            numbers.Add(0);
        }
        int[] removeArray2 = removeNumArray(SecondActionNum);
        foreach (int removeNum in removeArray2)
        {
            numbers.RemoveAll(x => x == removeNum);
        }
        int ThirdActionNum = numbers[Random.Range(0, numbers.Count)];
        FlagActionType ThirdAction = (FlagActionType)ThirdActionNum;
        Debug.Log($"{round}랜덤으로 선택된 third Action: " + ThirdAction);
        ActionArray[0] = FirstActionNum;
        ActionArray[1] = SecondActionNum;
        ActionArray[2] = ThirdActionNum;
        CheckActions(FirstActionNum, SecondActionNum, ThirdActionNum);
        ///여기에 AudioClip array를 만든다
        roundClips[0] = GenGameSound(FirstActionNum, SecondActionNum); //array 0에 배치
        roundClips[1] = GenGameSound(SecondActionNum, ThirdActionNum); //array 1에 배치
        roundClips[2] = GenGameSound(ThirdActionNum, 0); //array 2에 배치
        ///PlayRound를 실행시킨다.
        //StartCoroutine(PlayRoundAudioClip(roundClips)); ; // 첫 클립 재생
    }

    public void CheckActions(int FirstActionNum, int SecondActionNum, int ThirdActionNum)
    {
        // 1이 있는지 검사
        if (FirstActionNum == 1 || SecondActionNum == 1 || ThirdActionNum == 1)
        {
            Debug.Log("1이 있습니다.");
            AnswerWhiteFlagState = FlagState.Down;
        }

        // 2가 있는지 검사
        if (FirstActionNum == 2 || SecondActionNum == 2 || ThirdActionNum == 2)
        {
            Debug.Log("2가 있습니다.");
            AnswerWhiteFlagState = FlagState.Up;
        }

        // 3이 있는지 검사
        if (FirstActionNum == 3 || SecondActionNum == 3 || ThirdActionNum == 3)
        {
            Debug.Log("3이 있습니다.");
            AnswerBlueFlagState = FlagState.Down;
        }

        // 4가 있는지 검사
        if (FirstActionNum == 4 || SecondActionNum == 4 || ThirdActionNum == 4)
        {
            Debug.Log("4가 있습니다.");
            AnswerBlueFlagState = FlagState.Up;
        }
    }

    int[] removeNumArray(int selectedNum)
    {
        switch (selectedNum)
        {
            case -4: return new int[] { 4, -4 };
            case -3: return new int[] { 3, -3 };
            case -2: return new int[] { 2, -2 };
            case -1: return new int[] { 1, -1 };
            case 0: return new int[] { -4, -3, -2, -1, 1, 2, 3, 4 };
            case 1: return new int[] { 1, 2, -1 };
            case 2: return new int[] { 1, 2, -2 };
            case 3: return new int[] { 3, 4, -3 };
            case 4: return new int[] { 3, 4, -4 };
            default: return new int[] { selectedNum }; // 기본적으로 빈 배열 반환
        }
    }


    AudioClip GenGameSound(int actionNum, int nextNum)
    {
        string SoundClipName = "";
        if (actionNum == 0)
        {
            Debug.Log("0이네 이거");

            return null;
        }

        if (System.Math.Abs(actionNum) > 2) // 절대값이 2보다 크면
        {
            SoundClipName += "blue_"; // abc에 "blue"를 추가
        }
        else
        {
            SoundClipName += "white_"; // abc에 "blue"를 추가
        }
        if (actionNum < 0)
        {
            SoundClipName += "not_";
        }
        if (System.Math.Abs(actionNum) % 2 == 1)
        {
            SoundClipName += "down_";
        }
        else
        {
            SoundClipName += "up_";
        }
        if (nextNum != 0)
        {
            SoundClipName += "and_";
        }
        ///and를 붙일지 말지 고민해야함,
        ///다음 element가 null 이 아니고 0도 아님
        SoundClipName += "man_kr";
        Debug.Log($"SoundClip:{SoundClipName}");
        AudioClip tempsoundClip = Resources.Load<AudioClip>(SoundResourceString + SoundClipName);
        return tempsoundClip;
    }
    public FlagRound(int i)
    {

        round = i;
        Debug.Log("FlagRound 시작");
        GenerateRound();
    }

}
