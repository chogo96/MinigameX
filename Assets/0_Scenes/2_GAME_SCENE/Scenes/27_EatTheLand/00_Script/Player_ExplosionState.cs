using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class Player_ExplosionState : PlayerBaseState
{
    float _yVelocity;
    float _explosionForwardPower;
    float _explosionUpPower;
    Gravity _gravity;
    Rigidbody _rb;
    UpDownBoxCheck _upDownBoxCheck;
    Transform transform => _control.transform;
    public Player_ExplosionState(PlayerControl control) : base(control)
    {
        control._yVelocity= 0;
        control._rb.velocity = Vector3.zero;
        _upDownBoxCheck = control._upDownBoxCheck;

        _explosionForwardPower = control.explosionForwardPower;
        _explosionUpPower = control.explosionUpPower;
        _rb = control._rb;
        _gravity = control._gravity;
    }

    public override void OnStateEnter()
    {
        _yVelocity= 30;

    }

    public override void OnStateExit()
    {
        //_rb.velocity = Vector3.zero;
        _yVelocity= 0;
    }

    public override void OnStateUpdate()
    {
        if (!_upDownBoxCheck.CheckBox())
        {
            // 중력 갱신
            _yVelocity += _gravity.OnGravity();

            // 폭발 방향 벡터 계산
            Vector3 _explosionDir = transform.forward * _explosionForwardPower + transform.up * _explosionUpPower;

            // y축 속도 적용
            _explosionDir.y = _yVelocity;

            // Rigidbody에 속도 설정
            _rb.velocity = _explosionDir;
        }
        else
        {
            // 상자가 감지되면 상태를 NORMAL로 전환
            _control._yVelocity = 0;
            _control.ChangeState(EPlayer.NORMAL);
        }
    }


}
