using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager instance;
    private readonly string version = "1,0";

    void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);            
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        //  마스터 클라이언트의 씬 자동 동기화 옵션
        PhotonNetwork.AutomaticallySyncScene = true;
        //  버전
        PhotonNetwork.GameVersion = version;

        //  포톤 서버 접속
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터 접속 완료");
        Debug.Log("로비에 들어와 있는가? = " + PhotonNetwork.InLobby);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("로비 접속 완료");
    }
}
