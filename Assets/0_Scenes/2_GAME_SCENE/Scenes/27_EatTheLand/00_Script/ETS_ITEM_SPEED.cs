using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[CreateAssetMenu(fileName = "SPEED_ITEM", menuName = "ITEM/PLAYER/BUFF/SPEED")]
public class ETS_ITEM_SPEED : ItemObject
{
    public float _speed;
    public float _durationTime;
    public override void OnHit(Collider other)
    {
        // PhotonView view = other.GetComponent<PhotonView>();

        // if(view != null && view.IsMine)
        // {
        //     view.RPC("", RpcTarget.All, _durationTime, _speed);            
        // }

        Player_EatTheLand_DefaultState player = other.GetComponent<Player_EatTheLand_DefaultState>();
        player.Start_Player_SPEEDBUFF(_speed, _durationTime);
        
        
    }
}
