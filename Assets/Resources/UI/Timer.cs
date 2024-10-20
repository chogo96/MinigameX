using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    float PlayerTime = 10f; // 초기 시간 설정 (10초)
    float remainTime = 0f; // 남은 시간
    TextMeshProUGUI TimerText;
    bool isStart = false; // 타이머 상태를 저장하는 변수
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        TimerText = GetComponent<TextMeshProUGUI>();
        TimerText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart && remainTime > 0)
        {
            // 남은 시간을 감소시킴
            remainTime -= Time.deltaTime;

            // 남은 시간이 0보다 작아지면 0으로 고정
            remainTime = Mathf.Max(remainTime, 0);

            // 시간을 00:00 형식으로 포맷
            int minutes = Mathf.FloorToInt(remainTime / 60); // 분 단위 계산
            int seconds = Mathf.FloorToInt(remainTime % 60); // 초 단위 계산

            // 텍스트에 00:00 형식으로 시간 표시
            TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            // 타이머가 종료되면 isStart를 false로 설정
            if (remainTime <= 0)
            {
                TimerEnd();
            }
        }
    }
    void TimerEnd()
    {
        isStart = false;
        Debug.Log("Timer Ended");
        if (gameManager != null)
        {
            gameManager.GameEnd();
        }
    }
    // 타이머 시작 함수
    public void TimerStart(GameManager gameManager)
    {
        Debug.Log("TimerStart");
        remainTime = PlayerTime; // 남은 시간을 초기화
        isStart = true; // 타이머가 시작되었음을 표시
    }
}
