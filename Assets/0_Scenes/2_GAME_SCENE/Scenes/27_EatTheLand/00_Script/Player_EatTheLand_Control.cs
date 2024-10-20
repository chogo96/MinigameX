using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_EatTheLand_Control : Player_EatTheLand
{
    private EPlayer_State _curState;
    private Player_EatTheLand_FSM fsm;
    void Start()
    {
        _curState = EPlayer_State.DEFAULT;

        fsm = new Player_EatTheLand_FSM(new Player_EatTheLand_DefaultState(this));
    }

    // Update is called once per frame
    void Update()
    {
        switch (_curState)
        {
            case EPlayer_State.DEFAULT:
                ChangeState(EPlayer_State.DEFAULT);
                break;
            case EPlayer_State.EXPLOSION:
                ChangeState(EPlayer_State.EXPLOSION);

                break;
            case EPlayer_State.FREEZE:
                ChangeState(EPlayer_State.FREEZE);

                break;
        }
    }

    private void ChangeState(EPlayer_State state)
    {
        _curState = state;
        switch (_curState)
        {
            case EPlayer_State.DEFAULT:
                fsm.ChangeState(new Player_EatTheLand_DefaultState(this));
                break;
            case EPlayer_State.EXPLOSION:
                fsm.ChangeState(new Player_EatTheLand_ExplosionState(this));
                break;
            case EPlayer_State.FREEZE:
                fsm.ChangeState(new Player_EatTheLand_Freeze(this));
                break;
        }
    }
}
