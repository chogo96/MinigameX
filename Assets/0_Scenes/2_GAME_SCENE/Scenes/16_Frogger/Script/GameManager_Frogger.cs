using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameManagerFrogger : GameManager
{
    private AudioSource audioSource;
    [SerializeField] GameObject[] playerPos;

    // [SerializeField] GameObject playerPrefab;
    void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        //Gen Player
        GenPlayers();

    }


    // Update is called once per frame
    void Update()
    {

    }

    void GenPlayers()
    {
        //Gen Player
        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        Vector3 spawnPosition = playerPos[playerIndex].transform.position;
        GameObject newPlayer = PhotonNetwork.Instantiate("Player/Frogger/" + "Player1", spawnPosition, Quaternion.identity);

    }
}
