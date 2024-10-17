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
            // �߷� ����
            _yVelocity += _gravity.OnGravity();

            // ���� ���� ���� ���
            Vector3 _explosionDir = transform.forward * _explosionForwardPower + transform.up * _explosionUpPower;

            // y�� �ӵ� ����
            _explosionDir.y = _yVelocity;

            // Rigidbody�� �ӵ� ����
            _rb.velocity = _explosionDir;
        }
        else
        {
            // ���ڰ� �����Ǹ� ���¸� NORMAL�� ��ȯ
            _control._yVelocity = 0;
            _control.ChangeState(EPlayer.NORMAL);
        }
    }


}
