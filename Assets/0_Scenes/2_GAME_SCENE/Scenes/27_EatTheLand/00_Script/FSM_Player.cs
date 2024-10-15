using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Player : MonoBehaviour
{
    private PlayerBaseState _curState;
    public FSM_Player(PlayerBaseState initState)
    {
        _curState= initState;
    }

    public void ChangeState(PlayerBaseState nextState)
    {
        if(nextState == _curState)
        {
            return;
        }

        if(_curState == null)
        {
            _curState.OnStateExit();
        }

        _curState = nextState;
        _curState.OnStateEnter();
    }

    public void UpdateState()
    {
        if(_curState != null )
        {
            _curState.OnStateUpdate();
        }
    }
}
