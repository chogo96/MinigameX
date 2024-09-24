using UnityEngine;
using UnityEngine.UI; // UnityEngine.UI를 사용하여 최신 UI 시스템을 사용
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class GameManagerMole : MonoBehaviourPun
{
    public TextMeshProUGUI catchcount_txt; // GUIText를 Text로 변경
    public TextMeshProUGUI _time_txt; // GUIText를 Text로 변경

    [HideInInspector]
    public int Bed_Count;
    [HideInInspector]
    public int Good_Count;

    public float limitTime = 20f;
    public GameObject End_GUI;
    public Text Final_BedCount; // GUIText를 Text로 변경
    public Text Final_GoodCount; // GUIText를 Text로 변경
    public Text Final_Score; // GUIText를 Text로 변경
    public GameObject Red_Screen;

    public AudioClip GoSound;
    private AudioSource audioSource; // AudioSource를 위한 변수 추가
    [HideInInspector]
    public bool Play;
    private bool End;
    public PhotonView[] pointers; // 각 포인터의 PhotonView 배열
    void Awake()
    {
        // AudioSource 컴포넌트를 가져옴
        audioSource = GetComponent<AudioSource>();

        // AudioSource가 없으면 자동으로 추가
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Start()
    {

        GenerateMousePointer();

        // AssignPointerOwnership();
    }


    void Update()
    {
        // 게임이 끝나지 않았고 플레이 중인 경우
        if (limitTime >= 0 && End == false && Play == true)
        {
            catchcount_txt.text = string.Format("{0}", Bed_Count); // CatchCount를 Text에 작성
            limitTime -= Time.deltaTime; // 시간 흐름
            _time_txt.text = string.Format("{0:N2}", limitTime); // 남은 시간을 Text에 작성
        }

        // 시간이 다 된 경우
        if (limitTime <= 0 && End == false)
        {
            _time_txt.text = "0";
            Game_End();
        }
    }

    // 첫 두더지 등장 시
    public void GO()
    {
        Play = true;
        audioSource.clip = GoSound;
        audioSource.Play();
    }

    // Mole 스크립트에서 호출되는 함수
    public void CatchCount_Up(bool Bed)
    {
        if (Bed)
        {
            Bed_Count += 1;
        }
        else
        {
            Good_Count += 1;
            Red_Screen.SetActive(false);
            Fade f = Red_Screen.GetComponent<Fade>();
            f.FadeIn_ing = true;
            Red_Screen.SetActive(true);
        }
    }

    // 시간 제한 종료 시
    public void Game_End()
    {
        End = true;
        Final_BedCount.text = string.Format("{0}", Bed_Count); // 최종 Bed_Count를 Text에 작성
        Final_GoodCount.text = string.Format("{0}", Good_Count); // 최종 Good_Count를 Text에 작성
        Final_Score.text = string.Format("{0}", Bed_Count * 100 + Good_Count * -1000); // 최종 점수 계산
        End_GUI.SetActive(true);
    }

    void AssignPointerOwnership()
    {
        Debug.Log("AssignPointerOwnership");
        // 현재 접속한 플레이어 리스트 가져오기
        Player[] players = PhotonNetwork.PlayerList;
        Debug.Log("players size : " + players.Length);
        // 플레이어 수와 포인터 수 비교 후 순서대로 소유권 할당
        for (int i = 0; i < players.Length && i < pointers.Length; i++)
        {
            // 포인터의 소유권을 플레이어에게 할당
            pointers[i].TransferOwnership(players[i]);
            Debug.Log($"Pointer {i + 1} 소유권이 {players[i].NickName}에게 할당되었습니다.");
        }
    }
    void GenerateMousePointer()
    {
        GameObject newPlayerMousePointer = PhotonNetwork.Instantiate("Player/FPS/" + "MousePointer", Vector3.zero, Quaternion.identity);

    }

}
