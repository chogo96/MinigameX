using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
public enum EPlayer { NORMAL, DASH, ICED, GRABED, WIND, EXPLOSION }
public enum ETeam
{
    RED,
    BLUE
}
public class PlayerControl : MonoBehaviour
{
    public PlayerBaseState CurrentState { get; private set; }
    private Dictionary<EPlayer, PlayerBaseState> _states;
    public ETeam team;

    public PlayerInputAction _input;
    public UpDownBoxCheck _upDownBoxCheck;
    public Gravity _gravity;
    public Rigidbody _rb;

    public Vector3 _movDir;
    public Vector3 windDir;
    public float _movSpeed;
    public float _rotSpeed;
    public float _yVelocity;
    public float _dashPower;
    public float _dashDurationTime;

    public float _jumpPower;

    public float explosionForwardPower; 
    public float explosionUpPower;

    public float windPower;

    public bool isDashable;
    public bool isControllerable;
    public Coroutine coroutine;

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
            { EPlayer.DASH, new Player_DashState(this) },
            { EPlayer.EXPLOSION, new Player_ExplosionState(this) }
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
        //Physics.gravity = new Vector3(0, -20, 0);
    }

    private void Update()
    {
        Debug.Log(CurrentState);

        //CurrentState?.OnStateUpdate();
        if(CurrentState != null)
        {
            CurrentState.OnStateUpdate();
        }
    }

    public void ChangeState(EPlayer newState)
    {
        if(CurrentState != null && _states[newState] == CurrentState)
        {
            Debug.LogWarning("Trying to switch to the same State." + CurrentState);
            return;
        }

        if(CurrentState != null)//  기존 상태 종료
        {
            CurrentState.OnStateExit();
        }

        CurrentState = _states[newState];   //  새 상태로 전환

        CurrentState.OnStateEnter();    //  새 상태 시작
    }

    public void StartCorutine()
    {
        StartCoroutine(coroutine.ToString());
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("WIND"))
        {
            Debug.Log("뭐가 닿았다");
            Wind _wind = other.GetComponent<Wind>();
            Transform windEnd = _wind.wind_End;
            float wind = (windEnd.position - transform.position).magnitude * windPower;
            Vector3 tmp = windEnd.position - transform.position;
            windDir = new Vector3(tmp.x, 0, tmp.z).normalized * wind * Time.fixedDeltaTime;
            Debug.Log(wind);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("WIND"))
        {
            windDir = Vector3.zero;
        }
    }
}
