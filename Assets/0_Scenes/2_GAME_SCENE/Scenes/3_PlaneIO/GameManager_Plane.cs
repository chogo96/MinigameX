using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameManager_Plane : GameManager
{
    [SerializeField] GameObject[] playerPos;
    [SerializeField] private GameObject players;
    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.GameStart;
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
        Debug.Log($"{playerIndex}번째 플레이어 Gen");
        Vector3 spawnPosition = playerPos[playerIndex].transform.position;
        GameObject newPlayer = PhotonNetwork.Instantiate("Player/PlaneIO/" + "Player", spawnPosition, Quaternion.identity);
        newPlayer.transform.SetParent(players.transform);

    }
}
