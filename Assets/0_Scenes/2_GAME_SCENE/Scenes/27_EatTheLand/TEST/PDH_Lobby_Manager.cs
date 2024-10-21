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

    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("PDH_LOBBY_TEST");
    }
    public Scene lobbyScene;
}
