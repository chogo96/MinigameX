using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_EatTheLand_Freeze : Player_EatTheLand_BaseState
{
    private Rigidbody _rb => GetComponent<Rigidbody>();
    [SerializeField] private float _durationFreezeTime;
    [SerializeField] private float _time;

    public Player_EatTheLand_Freeze(Player_EatTheLand player) : base(player){}
    public override void OnStateEnter()
    {
        _rb.velocity = Vector3.zero;
    }

    public override void OnStateExit()
    {
        throw new System.NotImplementedException();
    }

    public override void OnStateUpdate()
    {
        _time += Time.deltaTime;
        if(_time >= _durationFreezeTime)
        {
            _time = 0;
            Debug.Log("기본으로 풀려야함");
        }
    }
    
}
