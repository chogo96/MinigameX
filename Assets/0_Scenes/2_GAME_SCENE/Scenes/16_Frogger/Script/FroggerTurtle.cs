using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FroggerTurtle : MonoBehaviour
{
    TutleState turtleState = TutleState.Normal;
    private bool isRunning = false; // 코루틴이 실행 중인지 확인하는 플래그
    int loopNum = 0;
    SpriteRenderer[] childSpriteRenderers;
    Color redColor = new Color(0.9f, 0f, 0f);
    Color greenColor = new Color(0f, 1f, 0f);
    private bool isBlinking = false;
    // Start is called before the first frame update
    void Start()
    {
        childSpriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        // 상태 변경 루틴 시작
        StartCoroutine(ChangeStateLoop());
    }

    // 상태 변경 루틴을 무한히 반복하는 코루틴
    IEnumerator ChangeStateLoop()
    {
        while (true) // 무한 루프
        {
            if (!isRunning) // 코루틴이 실행 중이지 않을 때만
            {
                loopNum++;
                isRunning = true; // 코루틴 실행 중으로 설정
                                  // Debug.Log("Frogger roop start" + loopNum);
                                  // 4 ~ 15초 사이의 랜덤 시간 대기 후 Warning 상태로 변경
                float randomTimeForWarning = Random.Range(4f, 15f);
                yield return new WaitForSeconds(randomTimeForWarning);

                ChangeToWarning();

                // Warning 상태에서 3 ~ 5초 사이의 랜덤 시간 대기 후 Hide 상태로 변경
                float randomTimeForHide = Random.Range(3f, 5f);
                yield return new WaitForSeconds(randomTimeForHide);

                ChangeToHide();


                // Hide 상태에서 2초 대기 후 Normal 상태로 변경
                yield return new WaitForSeconds(2f);


                ChangeToNormal();

                isRunning = false; // 코루틴이 끝났음을 표시
            }

            // 잠시 대기 후 루프 재시작 (지나치게 빠른 루프 방지)
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 현재 상태 확인
        //  Debug.Log("Current State: " + turtleState);
        WarningAct();
    }

    void ChangeToWarning()
    {
        // Debug.Log("State changed to Warning");

        turtleState = TutleState.Warning;
        // 자식 오브젝트의 모든 스프라이트를 빨간색으로 변경
        foreach (SpriteRenderer spriteRenderer in childSpriteRenderers)
        {
            spriteRenderer.color = redColor;
        }
    }
    // 경고 상태일 때 자식들의 SpriteRenderer를 깜빡이게 하는 함수
    void WarningAct()
    {
        if (turtleState == TutleState.Warning && !isBlinking)
        {
            // 코루틴이 중복 실행되지 않도록 플래그 설정
            isBlinking = true;
            StartCoroutine(BlinkSprites());
        }
    }

    // 자식 오브젝트들의 SpriteRenderer를 0.5초마다 깜빡이게 하는 코루틴
    // 자식 오브젝트들의 SpriteRenderer를 0.2초 동안 끄고, 0.5초 동안 켜지게 하는 코루틴
    IEnumerator BlinkSprites()
    {

        while (turtleState == TutleState.Warning) // Warning 상태인 동안 반복
        {
            // 자식들의 SpriteRenderer 비활성화 (꺼짐)
            foreach (SpriteRenderer spriteRenderer in childSpriteRenderers)
            {
                spriteRenderer.enabled = false; // 스프라이트 비활성화
            }

            // 0.2초 대기 (꺼진 상태 유지)
            yield return new WaitForSeconds(0.2f);

            // 상태가 Warning이 아니라면 코루틴 종료
            if (turtleState != TutleState.Warning)
            {
                break; // 빠져나와서 복구 단계로
            }

            // 자식들의 SpriteRenderer 활성화 (켜짐)
            foreach (SpriteRenderer spriteRenderer in childSpriteRenderers)
            {
                spriteRenderer.enabled = true; // 스프라이트 활성화
            }

            // 0.5초 대기 (켜진 상태 유지)
            yield return new WaitForSeconds(0.5f);

            // 상태가 Warning이 아니라면 코루틴 종료
            if (turtleState != TutleState.Warning)
            {
                break; // 빠져나와서 복구 단계로
            }
        }

        // 깜빡임이 종료되면 SpriteRenderer를 다시 활성화 (원래 상태로 복구)
        foreach (SpriteRenderer spriteRenderer in childSpriteRenderers)
        {
            spriteRenderer.enabled = false; // 모든 SpriteRenderer 활성화
        }

        isBlinking = false; // 깜빡임 종료
    }
    void ChangeToNormal()
    {
        // Debug.Log("State changed to Normal");
        turtleState = TutleState.Normal;
        // 자식 오브젝트의 모든 스프라이트를 빨간색으로 변경
        foreach (SpriteRenderer spriteRenderer in childSpriteRenderers)
        {
            spriteRenderer.color = greenColor;
        }
        //초록색으로 만들고 보이기

        //원래대로 하기
        foreach (SpriteRenderer spriteRenderer in childSpriteRenderers)
        {
            spriteRenderer.enabled = true; // 모든 SpriteRenderer 활성화
        }
        GetComponent<BoxCollider2D>().enabled = true;

    }
    void ChangeToHide()
    {
        // Debug.Log("State changed to Hide");
        turtleState = TutleState.Hide;
        //숨기기
        foreach (SpriteRenderer spriteRenderer in childSpriteRenderers)
        {
            spriteRenderer.enabled = false; // 모든 SpriteRenderer 비활성화
        }
        GetComponent<BoxCollider2D>().enabled = false;
    }
}

public enum TutleState
{
    Normal, Warning, Hide
}
