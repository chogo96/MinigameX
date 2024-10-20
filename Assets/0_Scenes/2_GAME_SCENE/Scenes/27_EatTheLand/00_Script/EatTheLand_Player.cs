using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EatTheLand_Player : MonoBehaviour
{
    private Rigidbody _rb => GetComponent<Rigidbody>();
    private PlayerInputAction _input;
    private UpDownBoxCheck _upDownCheckBox => GetComponent<UpDownBoxCheck>();


    private Vector3 _inputDir;
    public Vector3 moveDir;

    [SerializeField] private float _moveSpeed = 7;
    [SerializeField] private float _gravity = 20;
    [SerializeField] private float _jumpPower = 10;
    [SerializeField] private float _dashPower = 5;
    [SerializeField] private float _groundCastLength = 2;

    [SerializeField] private bool _isDash;

    void Awake()
    {
        _input = new PlayerInputAction();
    }
    void OnEnable()
    {
        _input.Player.Enable();
        _input.Player.Move3D.performed += OnMove;
        _input.Player.Move3D.canceled += OnMove;
        _input.Player.Jump.performed += OnJump;
    }
    void OnDisable()
    {
        _input.Player.Move3D.performed -= OnMove;
        _input.Player.Move3D.canceled -= OnMove;
        _input.Player.Jump.performed -= OnJump;
        _input.Player.Enable();
    }
    void Start()
    {
        _isDash = true;
        Physics.gravity = new Vector3(0, _gravity, 0);
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

    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
