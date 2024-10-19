using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_EatTheLand_DefaultState : Player_EatTheLand_BaseState
{    
    public Player_EatTheLand_DefaultState(Player_EatTheLand player) : base(player){}
    private Rigidbody _rb;
    private PlayerInputAction _input;
    private UpDownBoxCheck _upDownCheckBox => GetComponent<UpDownBoxCheck>();

    private Vector3 _inputDir;
    public Vector3 moveDir;

    
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _gravity;
    [SerializeField] private float _jumpPower;
    [SerializeField] private float _dashPower;
    [SerializeField] private float _groundCastLength;

    [SerializeField] private bool _isDash;
    void Awake()
    {
        _input = new PlayerInputAction();
        _rb = GetComponent<Rigidbody>();
        _isDash = true;
        Physics.gravity = new Vector3(0,_gravity,0);

    }
    void OnEnable()
    {
        _input.Enable();
        _input.Player.Move3D.performed += OnMove;
        _input.Player.Move3D.canceled += OnMove;
        _input.Player.Jump.performed += OnJump;
    }
    public override void OnStateEnter()
    {
        Move();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _inputDir = context.ReadValue<Vector3>();
        Debug.Log(_inputDir);
    }


    private void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("점프를 했다");
        if(_upDownCheckBox.CheckBox())
        {
            _rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
            _isDash = true;
            Debug.Log("점프");
        }
        else if(!_upDownCheckBox.CheckBox() && _isDash )
        {
            _rb.AddForce(transform.forward * _dashPower, ForceMode.Impulse);
            _isDash = false;
            Debug.Log("대쉬!");
        }
    }

    private void Move()
    {
        moveDir = _inputDir.normalized * _moveSpeed * Time.deltaTime;
        // _rb.velocity = moveDir;
        _rb.Move(transform.position + moveDir, Quaternion.identity);
    }

    
    public override void OnStateUpdate()
    {
        Move();
    }

    public override void OnStateExit()
    {
        throw new System.NotImplementedException();
    }

   void Update()
   {
        Move();
   }
}
