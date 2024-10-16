using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class Player_DashState : PlayerBaseState
{    
    UpDownBoxCheck _updDownBoxCheck;
    Rigidbody _rb;
    private Gravity _gravity;
    public float _durationTime;
    public float _dashPower;
    private float _time;
    private float _yVelocity;

    private Vector3 _dashDir;
    private Transform transform => _control.transform;
    public Player_DashState(PlayerControl control) : base(control) 
    {
        _updDownBoxCheck = control._upDownBoxCheck;
        _rb = control._rb;
        _gravity = control._gravity;
        _dashPower= control._dashPower;
        _durationTime = control._dashDurationTime;
        control._movDir = Vector3.zero;
        control._yVelocity = 0;

    }


    public override void OnStateEnter()
    {
        _time = 0;
        //_durationTime = 3;
        
        Debug.Log("대쉬 시작!!");
        //_yVelocity = _gravity.OnGravity();
        _dashDir = transform.forward.normalized * _dashPower;
        _rb.velocity = _dashDir;

    }

    public override void OnStateUpdate()
    {
        _time += Time.deltaTime;
        // CheckBox가 false일 때 상태를 NORMAL로 변경
        if (_updDownBoxCheck.CheckBox() || _time >= _durationTime)
        {
            //_rb.AddForce(transform.forward * 300, ForceMode.Impulse);
           
            _control.ChangeState(EPlayer.NORMAL);
            
            Debug.Log("대쉬 중 1");

        }
    }

    public override void OnStateExit()
    {
        //  상태전환
        Debug.Log("대쉬 종료");
    }
}
