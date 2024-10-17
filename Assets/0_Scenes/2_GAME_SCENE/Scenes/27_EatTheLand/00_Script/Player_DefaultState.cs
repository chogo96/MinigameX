using Photon.Pun.Demo.SlotRacer;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player_DefaultState : PlayerBaseState
{



    public Vector3 _movDir;
    public float _movSpeed;
    public float _rotSpeed;
    public float _yVelocity;

    public float _jumpPower;



    private bool _isDashable;
    public PlayerInputAction _input;
    public UpDownBoxCheck _upDownBoxCheck;
    public Gravity _gravity;
    public Rigidbody _rb;

    private Transform _playerTransform => _control.transform;
    public Player_DefaultState(PlayerControl control) : base(control)
    {
        _upDownBoxCheck = control._upDownBoxCheck ?? throw new System.NullReferenceException("UpDownBoxCheck is null");
        _gravity = control._gravity ?? throw new System.NullReferenceException("Gravity is null");
        _input = control._input ?? throw new System.NullReferenceException("PlayerInputAction is null");
        _rb = control._rb ?? throw new System.NullReferenceException("Rigidbody is null");

        _upDownBoxCheck = control._upDownBoxCheck;
        _gravity = control._gravity;
        _input = control._input;
        _rb = control._rb;

        _movDir = control._movDir;
        _movSpeed = control._movSpeed;
        _rotSpeed = control._rotSpeed;
        _yVelocity = control._yVelocity;

        _jumpPower = control._jumpPower;
    }



    //  움직임을 구현하는 키 받아서 이동
    void OnMove(InputAction.CallbackContext context)
    {
        Vector3 _inputDir = context.ReadValue<Vector3>();
        _movDir = new Vector3(_inputDir.x, 0, _inputDir.z).normalized;



        //Debug.Log(_movDir);

    }

    void OnJump(InputAction.CallbackContext context)
    {
        if (!_isDashable)
        {
            _yVelocity = _jumpPower;
        }
        else
        {
            Debug.Log("대쉬한다");
            if(_control.isDashable)
            {
                _control.ChangeState(EPlayer.DASH);
                _control.isDashable = false;

            }
        }
    }




    void Move()
    {
        Vector3 moveVector = Vector3.zero;

        // 이동 방향 벡터 계산
        if (_movDir.sqrMagnitude > 0)
        {
            moveVector = _movDir.normalized * _movSpeed * Time.deltaTime;
        }

        // 현재 플레이어가 땅에 있는지 확인
        bool isOnGround = _upDownBoxCheck.CheckBox();
        Transform obj;
        if(_upDownBoxCheck.detatchedObj != null)
        {
            obj = _upDownBoxCheck.detatchedObj.transform;
        }
        else
        {
            obj = null;
        }

        if (!isOnGround)  // 공중에 있을 때
        {
            _yVelocity += _gravity.OnGravity();  // 중력 적용
            _isDashable = true;
        }
 
        else if(isOnGround && _yVelocity <= 0) // 일반 착지 처리
        {
            _yVelocity = 0;
            _isDashable = false;
            _control.isControllerable= true;
            _rb.freezeRotation = false;
            _control.isDashable = true;
        }
        if(isOnGround && obj.gameObject.CompareTag("EXPLOSION") )
        {
            _yVelocity = _control.explosionUpPower;
            _control.isControllerable= false;
        }

        // y축 속도 적용 (중력 + 폭발 속도)
        moveVector.y = _yVelocity;

        // 제어 가능한 경우에만 속도 적용
        if (_control.isControllerable)
        {
            _rb.velocity = moveVector;
        }
        else
        {
            _rb.freezeRotation = true;
            //_yVelocity += _gravity.OnGravity();  // 중력 적용
            _rb.velocity = _control.transform.forward * _control.explosionForwardPower + new Vector3(0, _yVelocity, 0);
        }
    }

    //  회전
    void Rotate()
    {
        Vector2 rotVec = new Vector2(_movDir.x, _movDir.z);
        if (rotVec.sqrMagnitude > 0.1f)
        {
            float angle = Mathf.Atan2(_movDir.x, _movDir.z) * Mathf.Rad2Deg;
            _playerTransform.rotation = Quaternion.Slerp(_playerTransform.rotation, Quaternion.Euler(0, angle, 0), _rotSpeed * Time.deltaTime);
        }
    }



    public override void OnStateEnter()
    {
        _input.Player.Enable();
        _input.Player.Move3D.performed += OnMove;
        _input.Player.Move3D.canceled += OnMove;
        _input.Player.Jump.performed += OnJump;

        _control._yVelocity = 0;
        _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
        Debug.Log(_control._yVelocity);
        //_movDir = _movDir * _movSpeed * Time.fixedDeltaTime;

    }

    public override void OnStateUpdate()
    {
        Debug.Log("움직이는 중");

        Move();
        if(_control.isControllerable)
        {
            Rotate();

        }


    }

    public override void OnStateExit()
    {
        if (_input == null)
        {
            Debug.LogError("PlayerInputAction is not initialized!");
            return;
        }

        _input.Player.Move3D.performed -= OnMove;
        _input.Player.Move3D.canceled -= OnMove;
        _input.Player.Jump.performed -= OnJump;
        _input.Player.Disable();
        //  튀어오르는 것 방지용
        _yVelocity = 0;

    }
}
