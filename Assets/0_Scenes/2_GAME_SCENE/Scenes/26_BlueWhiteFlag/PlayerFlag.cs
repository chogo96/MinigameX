using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlag : MonoBehaviour
{
    [SerializeField] private GameObject BlueFlag;
    [SerializeField] private GameObject WhiteFlag;
    [SerializeField] AudioClip[] WavingFlagSound; // 여러 사운드를 배열로 지정
    private AudioSource audioSource;

    // 각도 변수
    private readonly float RotationAngle = 70f;

    void Start()
    {
        // AudioSource 컴포넌트 가져오기
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        HandleBlueFlagRotation();
        HandleWhiteFlagRotation();
    }

    void HandleBlueFlagRotation()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            // BlueFlag를 Up 방향으로 고정된 각도로 설정 (X축 70도)
            BlueFlag.transform.eulerAngles = new Vector3(RotationAngle, BlueFlag.transform.eulerAngles.y, BlueFlag.transform.eulerAngles.z);
            PlayWavingSound(); // 사운드 재생
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            // BlueFlag를 Down 방향으로 고정된 각도로 설정 (X축 -70도)
            BlueFlag.transform.eulerAngles = new Vector3(-RotationAngle, BlueFlag.transform.eulerAngles.y, BlueFlag.transform.eulerAngles.z);
            PlayWavingSound(); // 사운드 재생
        }
    }

    void HandleWhiteFlagRotation()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // WhiteFlag를 Up 방향으로 고정된 각도로 설정 (X축 70도)
            WhiteFlag.transform.eulerAngles = new Vector3(RotationAngle, WhiteFlag.transform.eulerAngles.y, WhiteFlag.transform.eulerAngles.z);
            PlayWavingSound(); // 사운드 재생
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // WhiteFlag를 Down 방향으로 고정된 각도로 설정 (X축 -70도)
            WhiteFlag.transform.eulerAngles = new Vector3(-RotationAngle, WhiteFlag.transform.eulerAngles.y, WhiteFlag.transform.eulerAngles.z);
            PlayWavingSound(); // 사운드 재생
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
}
