using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Normal : MonoBehaviour, IPlayer
{
    [SerializeField] private Vector3 _movDir;
    [SerializeField] private float _movSpeed;
    [SerializeField] private float _rotSpeed;
    [SerializeField] private float _yVelocity;
    [SerializeField] private float _dashPower;
    [SerializeField] private float _jumpPower;

    [SerializeField] PlayerInputAction _input;
    private UpDownBoxCheck _upDownBoxCheck;
    private Gravity _gravity;
    private Rigidbody _rb;
    private bool _isDashable;

    [SerializeField] private bool _isDash;
    private float _dashTimer;
    [SerializeField] private float _dashDuration;

    void Awake()
    {
        _upDownBoxCheck = GetComponent<UpDownBoxCheck>();
        _gravity = GetComponent<Gravity>();
        _input = new PlayerInputAction();
    }
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    //  키 입력값 받기
    void OnEnable()
    {
        _input.Player.Enable();
        _input.Player.Move3D.performed += OnMove;
        _input.Player.Move3D.canceled += OnMove;
        _input.Player.Jump.performed += OnJump;
    }


    //  움직임을 구현하는 키 받아서 이동
    void OnMove(InputAction.CallbackContext context)
    {
        Vector3 _inputDir = context.ReadValue<Vector3>();
        _movDir = new Vector3(_inputDir.x, 0, _inputDir.z).normalized;
        Debug.Log(_movDir);
        _movDir += _movDir * Time.fixedDeltaTime;

    }

    void OnJump(InputAction.CallbackContext context)
    {
        if (!_isDashable)
        {
            Debug.Log("점프");
            // _rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
            // _upDownBoxCheck.isLandObj = null;
            _yVelocity = _jumpPower;
        }
        else
        {
            Debug.Log("점프 대쉬!");
            // _rb.AddForce(transform.forward * _dashPower, ForceMode.Impulse);
            // _rb.velocity = transform.forward * _rb.velocity.z * _dashPower;
            StartDash();
        }
    }

    void StartDash()
    {
        if (!_isDash)
        {
            _isDash = true;
            _dashTimer = 0;
        }
    }

    void Dash()
    {
        _dashTimer += Time.fixedDeltaTime;
        _rb.velocity = transform.forward * _dashPower;
        if (_dashTimer >= _dashDuration)
        {
            _isDash = false;
            _rb.velocity = Vector3.zero;
        }

    }
    void Move()
    {
        if (!_upDownBoxCheck.CheckBox())
        {
            _yVelocity += _gravity.OnGravity();
            _isDashable = true;
        }
        else if (_upDownBoxCheck.CheckBox() && _yVelocity <= 0)
        {
            _isDashable = false;
            _yVelocity = 0;
        }
        // Debug.Log(_upDownBoxCheck.CheckBox());
        _movDir.y = _yVelocity;
        _rb.velocity = _movDir;

        // Rotate();
    }

    //  회전
    void Rotate()
    {
        Vector2 rotVec = new Vector2(_movDir.x, _movDir.z);
        if (rotVec.sqrMagnitude > 0.1f)
        {
            float angle = Mathf.Atan2(_movDir.x, _movDir.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, angle, 0), _rotSpeed * Time.deltaTime);
        }
    }
    public void Init()
    {
        Move();
        Rotate();
    }

    void FixedUpdate()
    {
        if (_isDash)
        {
            Dash(); // 대쉬 처리
        }

        Init();
    }
    // void Update()
    // {
    //     Init();
    // }

}
