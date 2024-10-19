using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class Player_EatTheLand_ExplosionState : Player_EatTheLand_BaseState
{
    private Rigidbody _rb => GetComponent<Rigidbody>();
    private UpDownBoxCheck _upDownCheck => GetComponent<UpDownBoxCheck>();

    [SerializeField] private float _explosionFwPower;
    [SerializeField] private float _explosionUpPower;

    public Player_EatTheLand_ExplosionState(Player_EatTheLand player) : base(player){}
    
    public override void OnStateEnter()
    {
        _rb.AddForce(Vector3.up * _explosionUpPower + Vector3.forward * _explosionFwPower, ForceMode.Impulse);
    }

    public override void OnStateExit()
    {

    }

    public override void OnStateUpdate()
    {
        if(_upDownCheck.CheckBox() && _rb.velocity.y < 0)
        {
            Debug.Log("바꿔야지");
        }
    }


}
