using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Normal : MonoBehaviour, IPlayer
{
    [SerializeField] private Vector3 _movDir;
    [SerializeField] private float _movSpeed;
    [SerializeField] private float _rotSpeed;
    [SerializeField] private float _yVelocity;
    [SerializeField] private float _dashPower;

    [SerializeField] PlayerInputAction _input;
    private UpDownBoxCheck _upDownBoxCheck;
    private Gravity _gravity;
    private Rigidbody _rb;


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
    }


    //  움직임을 구현하는 키 받아서 이동
    void OnMove(InputAction.CallbackContext context)
    {
        Vector3 _inputDir = context.ReadValue<Vector3>();
        _movDir = new Vector3(_inputDir.x, 0, _inputDir.z).normalized; 
        Debug.Log(_movDir);
        _movDir += _movDir * Time.fixedDeltaTime;
       
    }

    void Move()
    {
        
         if(!_upDownBoxCheck.CheckBox())
        {
            _yVelocity += _gravity.OnGravity();
        }
        else
        {
            _yVelocity = 0;
        }
        Debug.Log(_upDownBoxCheck.CheckBox());
        _movDir.y = _yVelocity;
        _rb.velocity = _movDir;

        // Rotate();
    }

    //  회전
    void Rotate()
    {
        Vector2 rotVec = new Vector2(_movDir.x, _movDir.z);
        if(rotVec.sqrMagnitude > 0.1f)
        {
            float angle = Mathf.Atan2(_movDir.x, _movDir.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, angle,0), _rotSpeed * Time.deltaTime);
        }
    }
    public void Init()
    {
        Move();
        Rotate();
    }

    void Update()
    {        
        Init();
    }
    
}
