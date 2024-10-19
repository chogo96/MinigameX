using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player_EatTheLand_BaseState : MonoBehaviour
{
    protected Player_EatTheLand _player;

    protected Player_EatTheLand_BaseState(Player_EatTheLand player)
    {
        _player = player;
    }

    //  상태에 처음 진입시
    public abstract void OnStateEnter();
    //  상태 update에
    public abstract void OnStateUpdate();

    //  상태에서 나갈 때 실행
    public abstract void OnStateExit();

}
