using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Dash : PlayerBaseState
{    
    UpDownBoxCheck _updDownBoxCheck;
    Rigidbody _rb;
    public float _durationTime;
    public float _dashPower;
    private float _time;

    public Player_Dash(PlayerControl control) : base(control) 
    {
        _updDownBoxCheck = control.GetComponent<UpDownBoxCheck>();
        _rb = control.GetComponent<Rigidbody>();
    }


    public override void OnStateEnter()
    {
        throw new System.NotImplementedException();
    }

    public override void OnStateUpdate()
    {
        throw new System.NotImplementedException();
    }

    public override void OnStateExit()
    {
        throw new System.NotImplementedException();
    }
}
