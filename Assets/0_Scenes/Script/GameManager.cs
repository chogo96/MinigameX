using UnityEngine;

public class GameManager : MonoBehaviour, IGameManager
{
    // 싱글턴 인스턴스를 저장할 정적 필드
    public static GameManager Instance { get; private set; }
    public GameState gameState = GameState.GameInit;
    public static float WindowSize;
    public static float normalSize = 2073600f;
    // MonoBehaviour가 활성화될 때 호출되는 Unity 메서드
    private void Awake()
    {
        WindowSize = Screen.width * Screen.height;
        // 인스턴스가 이미 존재하는 경우 중복 방지
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 중복된 인스턴스를 파괴
            return;
        }

        // 인스턴스 설정 및 유지
        Instance = this;
        DontDestroyOnLoad(gameObject); // 씬 전환 시에도 파괴되지 않도록 설정
    }

    public virtual void GameInit()
    {
        Debug.Log("Game Init");
    }

    public virtual void GameStart()
    {
        Debug.Log("Game Started");
        // 게임 시작 로직 구현
    }

    public virtual void GameEnd()
    {
        Debug.Log("Game Ended");
        // 게임 종료 로직 구현
    }

    // Start는 유니티에서 스크립트가 활성화될 때 호출
    void Start()
    {

    }
}
