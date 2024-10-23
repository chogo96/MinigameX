using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
//using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ETL_State
{
    NORMAL,
    SPRING,
    FREEZE
}
public class Player_EatTheLand_DefaultState : MonoBehaviour
{
    public bool isControlable;
    private ETL_State _curState;
    public void ChangeState(ETL_State _state)
    {
        _curState = _state;
    }
    private Rigidbody _rb;
    private PlayerInputAction _input;
    private UpDownBoxCheck _upDownCheckBox => GetComponent<UpDownBoxCheck>();

    private Vector3 _inputDir;
    public Vector3 moveDir;
    public Vector3 externalVect;

    [SerializeField] private float _moveSpeed = 7;
    [SerializeField] private float _gravity = 20;
    [SerializeField] private float _jumpPower = 10;
    [SerializeField] private float _dashPower = 5;
    [SerializeField] private float _groundCastLength = 2;

    [SerializeField] private float _springFwdPower;
    [SerializeField] private float _springUpPower;

    [SerializeField] private float _freezeTime;

    [SerializeField] private bool _isDash;
    void Awake()
    {
        _input = new PlayerInputAction();
        _rb = GetComponent<Rigidbody>();
        _isDash = true;
        externalVect = Vector3.zero;
        Physics.gravity = new Vector3(0, _gravity, 0);

    }
    void OnEnable()
    {
        _input.Enable();
        _input.Player.Move3D.performed += OnMove;
        _input.Player.Move3D.canceled += OnMove;
        _input.Player.Jump.performed += OnJump;
    }
    void OnDisable()
    {
        _input.Player.Move3D.performed -= OnMove;
        _input.Player.Move3D.canceled -= OnMove;
        _input.Player.Jump.performed -= OnJump;
        _input.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _inputDir = context.ReadValue<Vector3>();
        Debug.Log(_inputDir);
    }


    private void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("점프를 했다");
        if (_upDownCheckBox.CheckBox())
        {
            _rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
            _isDash = true;
            Debug.Log("점프");
        }
        else if (!_upDownCheckBox.CheckBox() && _isDash)
        {
            // isControlable = false;
            _rb.AddForce(transform.forward * _dashPower, ForceMode.Impulse);
            _isDash = false;
            Debug.Log("대쉬!");
        }
    }

    private void Move()
    {
        moveDir = _inputDir.normalized * _moveSpeed * Time.fixedDeltaTime;
        // _rb.velocity = moveDir;
        _rb.Move(transform.position + moveDir + externalVect, Quaternion.identity);
        Rotation();
    }
    private void Rotation()
    {
        if (_inputDir != Vector3.zero)
        {
            float angle = Mathf.Atan2(_inputDir.x, _inputDir.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
    }
    void Update()
    {

        switch (_curState)
        {
            case ETL_State.NORMAL:
                Move();            
                break;
            case ETL_State.SPRING:
                OnSpring();
                break;
            case ETL_State.FREEZE:
                Debug.Log("얼었다");
                OnFreeze();
                break;
        }

        if (_upDownCheckBox.CheckBox())
        {
            isControlable = true;
        }
        else isControlable = false;
    }

    void OnSpring()
    {
        _rb.AddForce(transform.forward * _springFwdPower + transform.up * _springUpPower, ForceMode.Impulse);
        // Invoke("ChangeState(ETL_State.NORMAL)", 1.5f);
        ChangeState(ETL_State.NORMAL);
        // StartCoroutine(STATE_TO_DEFAULT(1.5f));
    }

    void OnFreeze()
    {
        float _time = 0;
        _time += Time.deltaTime;
        _rb.velocity = Vector3.zero;
        if (_time >= _freezeTime)
        {
            ChangeState(ETL_State.NORMAL);
        }
    }

    [SerializeField] float _speed_Time;
    public IEnumerator STATE_TO_DEFAULT(float durationTime)
    {
        yield return new WaitForSeconds(durationTime);
        ChangeState(ETL_State.NORMAL);

    }

    public IEnumerator FREEZE(float durationTime)
    {
        _rb.velocity = Vector3.zero;
        yield return durationTime;
    }

    
    public IEnumerator SPEED_BUFF(float buffSpeed, float durationTime)
    {
        float defaultSpeed = _moveSpeed;
        _moveSpeed = buffSpeed;

        Debug.Log(_moveSpeed + "속도 업!");
        yield return new WaitForSeconds(durationTime);
        _moveSpeed = defaultSpeed;
        Debug.Log("속도 다운!");
    }

    public void Start_Player_SPEEDBUFF(float buffSpeed, float durationTime)
    {
        StartCoroutine(SPEED_BUFF(buffSpeed, durationTime));
    }
}
