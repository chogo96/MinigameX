using Photon.Pun;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    private Vector3 mousePos; // Vector2 대신 Vector3로 변경
    public ParticleSystem[] clickParticles; // GameObject 배열을 public으로 선언하여 Unity Inspector에서 설정할 수 있게 함
    public GameObject[] followCursor;
    Vector3 worldPos = Vector3.zero;
    [SerializeField] GameObject PlayerParent;
    private PhotonView photonView;
    void Start()
    {
        // 모든 파티클 비활성화
        foreach (ParticleSystem particle in clickParticles)
        {
            //   particle.gameObject.SetActive(false);
        }
        PlayerParent = GameObject.Find("MousePointers");
        if (PlayerParent != null)
        {
            Debug.Log("Players 오브젝트를 찾았습니다.");
        }
        else
        {
            Debug.LogError("Players 오브젝트를 찾을 수 없습니다.");
        }

        photonView = GetComponent<PhotonView>();
        photonView.RPC("SetPlayerParent", RpcTarget.AllBuffered);
    }

    void Update()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            MousePos();
            if (Input.GetMouseButtonDown(0))
            {
                Click();
            }

            // if (Input.GetMouseButtonUp(0))
            // {
            //     foreach (GameObject particle in clickParticles)
            //     {
            //         particle.SetActive(false);
            //     }
            // }
        }

    }
    void MousePos()
    {
        // 마우스 클릭 위치를 화면 좌표에서 월드 좌표로 변환
        mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(Camera.main.transform.position.z); // Z 값을 카메라와의 거리에 따라 설정

        worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        // Debug.Log($"Input.mousePosition: {Input.mousePosition.ToString()}");
        // Debug.Log($"Mouse Click: {worldPos.ToString()}");

        // 마우스 위치를 다른 클라이언트에 전송 (RPC 호출)
        photonView.RPC("MoveCursor", RpcTarget.All, worldPos.x, worldPos.y);
        // foreach (GameObject particle in followCursor)
        // {
        //     //커서 옮기기
        //     // particle.SetActive(true);
        //     particle.transform.position = new Vector3(worldPos.x, worldPos.y, 0f);
        // }
    }
    void Click()
    {
        Debug.Log($"Click!~~");
        // 마우스 클릭 위치를 화면 좌표에서 월드 좌표로 변환
        mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(Camera.main.transform.position.z); // Z 값을 카메라와의 거리에 따라 설정

        worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        // 각 파티클을 활성화하고 위치를 설정

        photonView.RPC("Trigger", RpcTarget.All, worldPos.x, worldPos.y);

    }

    [PunRPC]
    void SetPlayerParent()
    {
        transform.SetParent(PlayerParent.transform);
    }
    [PunRPC]
    void Trigger(float x, float y)
    {
        transform.position = new Vector3(x, y, 0f);
        foreach (ParticleSystem particle in clickParticles)
        {

            // particle.gameObject.SetActive(false);
            // particle.gameObject.SetActive(true);
            particle.Stop();
            particle.Play();
            Debug.Log($"Play particle! {particle.isPlaying}");
        }
    }

    [PunRPC]
    void MoveCursor(float x, float y)
    {
        transform.position = new Vector3(x, y, 0f);
        // // 전달받은 좌표로 파티클들의 위치를 업데이트
        // foreach (GameObject particle in followCursor)
        // {
        //     particle.transform.position = new Vector3(x, y, 0f); // 2D이므로 Z는 0으로 고정
        // }
    }
}
