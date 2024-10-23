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
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion= version;

        if(PhotonNetwork.IsConnected == false)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

}
