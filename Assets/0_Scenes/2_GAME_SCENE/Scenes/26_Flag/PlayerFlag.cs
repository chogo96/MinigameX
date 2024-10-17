using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlag : MonoBehaviour
{
    [SerializeField] private GameObject BlueFlag;
    [SerializeField] private GameObject WhiteFlag;
    [SerializeField] AudioClip[] WavingFlagSound; // 여러 사운드를 배열로 지정
    private AudioSource audioSource;
    public FlagState blueFlagState;
    public FlagState whiteFlagState;
    private bool CanChangeBlue = true;
    private bool CanChangeWhite = true;

    // 각도 변수
    private readonly float RotationAngle = 70f;
    private GameManagerFlag gameManager;
    void Start()
    {
        // AudioSource 컴포넌트 가져오기
        audioSource = GetComponent<AudioSource>();
        gameManager = FindObjectOfType<GameManagerFlag>();
    }

    void Update()
    {
        //만약에 이미 설정되어있으면 못 움직임;
        //그리고 설정 된 값으로 점수 나중에 맞는지 확인하고 가져오는 방식임
        if (CanChangeBlue)
        {
            HandleBlueFlagRotation();
        }
        if (CanChangeWhite) { HandleWhiteFlagRotation(); }
    }

    void HandleBlueFlagRotation()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            // BlueFlag를 Up 방향으로 고정된 각도로 설정 (X축 70도)
            BlueFlag.transform.eulerAngles = new Vector3(RotationAngle, BlueFlag.transform.eulerAngles.y, BlueFlag.transform.eulerAngles.z);
            PlayWavingSound(); // 사운드 재생
            blueFlagState = FlagState.Up;
            CheckBlueFlag();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            // BlueFlag를 Down 방향으로 고정된 각도로 설정 (X축 -70도)
            BlueFlag.transform.eulerAngles = new Vector3(-RotationAngle, BlueFlag.transform.eulerAngles.y, BlueFlag.transform.eulerAngles.z);
            PlayWavingSound(); // 사운드 재생
            blueFlagState = FlagState.Down;
            CheckBlueFlag();
        }
    }
    void CheckBlueFlag()
    {
        if (gameManager.gameState != GameState.GameStart) { return; }
        if (blueFlagState != gameManager.nowFlagRound.AnswerBlueFlagState)
        {
            CanChangeBlue = false;
        }
    }
    void CheckWhiteFlag()
    {
        if (gameManager.gameState != GameState.GameStart) { return; }
        if (whiteFlagState != gameManager.nowFlagRound.AnswerWhiteFlagState)
        {
            CanChangeWhite = false;

        }

    }
    void HandleWhiteFlagRotation()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // WhiteFlag를 Up 방향으로 고정된 각도로 설정 (X축 70도)
            WhiteFlag.transform.eulerAngles = new Vector3(RotationAngle, WhiteFlag.transform.eulerAngles.y, WhiteFlag.transform.eulerAngles.z);
            PlayWavingSound(); // 사운드 재생
            whiteFlagState = FlagState.Up;
            CheckWhiteFlag();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // WhiteFlag를 Down 방향으로 고정된 각도로 설정 (X축 -70도)
            WhiteFlag.transform.eulerAngles = new Vector3(-RotationAngle, WhiteFlag.transform.eulerAngles.y, WhiteFlag.transform.eulerAngles.z);
            PlayWavingSound(); // 사운드 재생
            whiteFlagState = FlagState.Down;
            CheckWhiteFlag();
        }
    }

    // 깃발이 움직일 때 사운드 재생 함수
    void PlayWavingSound()
    {
        if (WavingFlagSound.Length > 0 && audioSource != null)
        {
            // 랜덤으로 사운드를 재생하거나 고정된 사운드를 재생 가능
            int randomIndex = Random.Range(0, WavingFlagSound.Length);
            audioSource.PlayOneShot(WavingFlagSound[randomIndex]);
        }
    }
    public void Reset()
    {
        // PlayWavingSound();
        Cloth blueCloth = BlueFlag.GetComponentInChildren<Cloth>();
        Cloth whiteCloth = WhiteFlag.GetComponentInChildren<Cloth>();
        blueCloth.enabled = false;
        whiteCloth.enabled = false;
        BlueFlag.transform.eulerAngles = new Vector3(0, BlueFlag.transform.eulerAngles.y, BlueFlag.transform.eulerAngles.z);

        WhiteFlag.transform.eulerAngles = new Vector3(0, WhiteFlag.transform.eulerAngles.y, WhiteFlag.transform.eulerAngles.z);
        blueCloth.enabled = true;
        whiteCloth.enabled = true;
        blueFlagState = FlagState.Stay;
        whiteFlagState = FlagState.Stay;
        CanChangeBlue = true;
        CanChangeWhite = true;
    }
}
