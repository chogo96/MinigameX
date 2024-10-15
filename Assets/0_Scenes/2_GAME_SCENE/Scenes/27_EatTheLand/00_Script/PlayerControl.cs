using System.Collections.Generic;
using UnityEngine;
public enum EPlayer { NORMAL, DASH, ICED, GRABED, WIND, EXPLOSION }
public class PlayerControl : MonoBehaviour
{
    public PlayerBaseState CurrentState { get; private set; }
    private Dictionary<EPlayer, PlayerBaseState> _states;


    public PlayerInputAction _input;
    public UpDownBoxCheck _upDownBoxCheck;
    public Gravity _gravity;
    public Rigidbody _rb;


    public Vector3 _movDir;
    public float _movSpeed;
    public float _rotSpeed;
    public float _yVelocity;

    public float _jumpPower;

    private void Awake()
    {
        _input = new PlayerInputAction();
        _upDownBoxCheck = GetComponent<UpDownBoxCheck>();
        _gravity = GetComponent<Gravity>();
        _rb = GetComponent<Rigidbody>();
        // Initialize states
        _states = new Dictionary<EPlayer, PlayerBaseState>
        {
            { EPlayer.NORMAL, new Player_DefaultState(this) },            
        };

      

        // 초기화가 제대로 되었는지 로그로 확인
        Debug.Assert(_upDownBoxCheck != null, "UpDownBoxCheck is not assigned!");
        Debug.Assert(_gravity != null, "Gravity is not assigned!");
        Debug.Assert(_rb != null, "Rigidbody is not assigned!");
        Debug.Assert(_input != null, "PlayerInputAction is not assigned!");
    }

    private void Start()
    {

        ChangeState(EPlayer.NORMAL);
    }

    private void Update()
    {

        CurrentState?.OnStateUpdate();
    }

    public void ChangeState(EPlayer newState)
    {
        CurrentState?.OnStateExit();

        CurrentState = _states[newState];

        CurrentState.OnStateEnter();
    }
}
