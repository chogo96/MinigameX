using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class WeaponPlane : MonoBehaviour
{
    [SerializeField] protected float fireRate; // 발사 간격 (초 단위)
    protected bool isFire = false; // 발사 중 여부
    public PlayerPlane player;
    [SerializeField] protected float lifeTime = 1f; // 미사일의 생명 주기 (초 단위)
    [SerializeField] protected float localScale = 1.0f;
    // 무기 발사 메서드
    public virtual void Fire()
    {
        Debug.Log("Weapon: Fire!");

    }

    void Update()
    {
        HandleFireInput();
    }

    // 발사 상태를 업데이트
    public void UpdateFireInput()
    {
        if (!player.IsMine()) { return; }
        if (!isFire)
        {
            isFire = true; // 발사 상태로 설정
            Fire(); // 발사 메서드 호출
            StartCoroutine(ResetFireStateAfterDelay(fireRate)); // 일정 시간이 지나면 발사 상태를 초기화

        }
    }

    // 일정 시간이 지나면 isFire를 false로 설정하는 코루틴
    private IEnumerator ResetFireStateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isFire = false; // 다시 발사 가능 상태로 설정
    }

    // 플레이어를 설정하는 메서드
    public void SetPlayer(PlayerPlane player)
    {
        this.player = player;
    }

    void HandleFireInput()
    {
        // 마우스 왼쪽 버튼이 눌려있는 동안 무기 발사 타이머를 업데이트
        if (Input.GetMouseButton(0))
        {
            UpdateFireInput(); // DeltaTime을 사용하여 타이머 증가
        }
    }

    protected IEnumerator DestroyMissileAfterTime(GameObject missile, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (missile == null)
        {
            Debug.Log("missile is null");
        }
        if (missile != null && player.GetComponent<PhotonView>().IsMine)
        {
            PhotonNetwork.Destroy(missile); // 마스터 클라이언트에서 미사일 삭제
        }
    }
}