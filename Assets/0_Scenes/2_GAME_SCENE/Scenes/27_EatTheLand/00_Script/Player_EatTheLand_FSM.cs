using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_EatTheLand_FSM : MonoBehaviour
{
    private Player_EatTheLand_BaseState _curState;
    public Player_EatTheLand_FSM(Player_EatTheLand_BaseState initState)
    {
        _curState = initState;
    }

    public void ChangeState(Player_EatTheLand_BaseState nextState)
    {
        //  현재랑 같은 것이 실행중이면 그냥 메서드 종료시킨다.
        if(nextState == _curState)
        {
            return;
        }
        //  현재가 비어있다면        
        if(_curState != null)
        {
            //  현재의 것을 끝낸다.
            _curState.OnStateExit();
        }
        //  현재의 것을 변경하고
        _curState = nextState;
        //  현재의 것의 시작 
        _curState.OnStateEnter();
    }

    public void UpdateState()
    {
        if(_curState != null)
        {
            _curState.OnStateUpdate();
        }
    }
}
