using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PDH_Lobby_Manager : MonoBehaviourPunCallbacks
{
    public Button nickNameBtn;
    [SerializeField] private TMP_InputField _nickName;
    [SerializeField] private TMP_Text _notice;
    [SerializeField] private GameObject nickNmae;
    [SerializeField] private GameObject connecting;

    private readonly string version = "1,0";


    //private PhotonManager photonManager =>GetComponent<PhotonManager>();


    private void Awake()
    {
        //  마스터 클라이언트의 씬 자동 동기화 옵션
        PhotonNetwork.AutomaticallySyncScene = true;
        //  버전
        PhotonNetwork.GameVersion = version;

        //  포톤 서버 접속
        PhotonNetwork.ConnectUsingSettings();
        connecting.SetActive(true);
        nickNmae.SetActive(false);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터 접속 완료");
        Debug.Log("로비에 들어와 있는가? = " + PhotonNetwork.InLobby);
        connecting.SetActive(false);
        nickNmae.SetActive(true);
        //PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("로비 접속 완료");

        SceneManager.LoadScene("PDH_LOBBY_TEST");
    }


    public void MakeNickName()
    {
        string inputNick = _nickName.text.Trim();
        Color noticeColor = Color.red;
        if(string.IsNullOrEmpty(inputNick))
        {
            Debug.Log("닉네임을 입력하시오");
            _notice.text = "Please enter a nickname.";
            _notice.color = noticeColor;
            return;
        }
        if(inputNick.Contains(" "))
        {
            Debug.Log("닉네임에 공백을 포함할 수 없습니다.");
            // nickNameBtn.colors = nickNam;
            _notice.text = "Nickname cannot contain spaces.";
            _notice.color = noticeColor;
            return;
        }
        
        PhotonNetwork.NickName = inputNick;
        _notice.text = "";
        Debug.Log("닉네임은 " + PhotonNetwork.NickName);
        PhotonNetwork.JoinLobby();

    }

    
    public Scene lobbyScene;
}
