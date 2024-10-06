using UnityEngine;

public class HpBarPlane : MonoBehaviour
{
    private SpriteRenderer greenBar; // 초록색 HP 바를 담당하는 SpriteRenderer
    private PlayerPlane player; // 플레이어의 HP 정보를 가져오기 위한 참조

    // Start는 오브젝트 생성 시 한 번 호출됩니다.
    void Start()
    {
        //부모에서 찾기
        player = GetComponentInParent<PlayerPlane>(); // 플레이어 컴포넌트 가져오기
        greenBar = transform.Find("GreenBar").GetComponent<SpriteRenderer>(); // 초록색 바 찾기
    }

    // FixedUpdate는 일정 시간마다 호출되며, HP 바 업데이트에 사용됩니다.
    void FixedUpdate()
    {
        UpdateHpBar(); // 플레이어의 HP에 따라 HP 바 업데이트
    }

    // HP 바를 플레이어의 현재 HP에 따라 업데이트하는 함수
    void UpdateHpBar()
    {
        if (player == null)
        {
            Debug.Log("Player is null");
        }
        // player.hp는 현재 체력, player.maxHp는 최대 체력이라고 가정
        float hpRatio = player.Hp / 100; // 체력 비율 계산
        if (hpRatio < 0)
        {
            hpRatio = 0;
        }
        Debug.Log($"hpRatio : {hpRatio}, player.Hp: {player.Hp}");

        // 초록색 바의 크기를 현재 HP에 맞게 조정 (왼쪽에서부터 줄어듦)
        greenBar.transform.localScale = new Vector3(hpRatio, 1f, 1f);

        // 초록색 바를 왼쪽부터 차오르게 위치 조정
        greenBar.transform.localPosition = new Vector3(-(1f - hpRatio) / 2f, 0, 0);
    }
}