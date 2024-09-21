using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Players_DodgeMisile : MonoBehaviourPun
{
    [SerializeField] GameObject[] planes;
    [SerializeField] private AudioClip dieAudioClip; // 오디오 클립을 할당할 변수
    private AudioSource audioSource;
    float minX, maxX, minY, maxY;
    [SerializeField] private ParticleSystem explosionParticle;
    public float planeSize = 0f;
    public int planeSpeed = 0;
    public int sizeIndex = -1;
    public int planeTypeIndex = 0;
    [SerializeField] int playerIndex = -1;
    float moveSpeed = 0f; // 움직임 속도
    GameObject plane;
    private Vector3 inputDir = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        //방장 기준
        if (PhotonNetwork.IsMasterClient)
        {
            // 방장일 경우에만 SetBounds 함수 실행
            photonView.RPC("SetBounds", RpcTarget.AllBuffered);
        }
        if (GetComponent<PhotonView>().IsMine)
        {
            Debug.Log("respawnPlane!!!");
            respawnPlane();
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.Instance.gameState == GameState.GameStart || GameManager.Instance.gameState == GameState.GameReady)
        {

            ///이게 문제네 PhotonView중에 어떤 걸 움직일 것인가? 어떤게 내껀데?
            if (GetComponent<PhotonView>().IsMine)
            {

                GetInput();
                Move();
            }

        }
    }

    public void playerDie()
    {

        if (dieAudioClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(dieAudioClip);
        }
        else
        {
            Debug.LogWarning("Die Audio Clip is not assigned or AudioSource is missing.");
        }
        plane.SetActive(false);
        explosionParticle.gameObject.SetActive(true);

        //모두 다 죽으면 -> GameEnd
    }


    public void respawnPlane()
    {
        this.transform.position = Vector2.zero;

        // 랜덤으로 선택된 plane
        planeTypeIndex = UnityEngine.Random.Range(0, planes.Length);
        GameObject selectedPlane = planes[planeTypeIndex];

        // 프리팹 이름을 저장
        string planeName = selectedPlane.name;

        float[] possibleSizes = { 0.5f, 1f, 1.5f, 2f };
        planeSpeed = Random.Range(0, 7); // 0부터 6까지의 값
        moveSpeed = 0.2f + planeSpeed / 1.5f;


        sizeIndex = UnityEngine.Random.Range(0, possibleSizes.Length);
        planeSize = possibleSizes[sizeIndex];

        // GameObject가 아닌 프리팹 이름을 전송
        photonView.RPC("SelectPlaneRPC", RpcTarget.AllBuffered, planeName, planeSize, planeSpeed);
    }


    [PunRPC]
    void SelectPlaneRPC(string planeName, float planeSize, int planeSpeed)
    {

        // planeName을 통해 프리팹 로드
        GameObject selectedPlane = Resources.Load<GameObject>("Player/DodgeMisile/Plane/" + planeName);
        if (selectedPlane == null)
        {
            Debug.LogError("해당 이름의 프리팹을 찾을 수 없습니다: " + planeName);
            return;
        }

        // 선택된 프리팹을 인스턴스화
        plane = Instantiate(selectedPlane);

        // 인스턴스화된 오브젝트를 부모의 자식으로 설정
        plane.transform.SetParent(this.transform, false);
        plane.transform.localPosition = Vector3.zero;

        // 크기 조정
        this.transform.localScale = new Vector3(planeSize, planeSize, planeSize);

        // 색상 설정
        SpriteRenderer spriteRenderer = plane.GetComponent<SpriteRenderer>();
        switch (planeSpeed)
        {
            case 0:
                spriteRenderer.color = Color.white; // White
                break;
            case 1:
                spriteRenderer.color = Color.yellow; // Yellow
                break;
            case 2:
                spriteRenderer.color = Color.green; // Green
                break;
            case 3:
                spriteRenderer.color = Color.blue; // Blue
                break;
            case 4:
                spriteRenderer.color = Color.red; // Red
                break;
            case 5:
                spriteRenderer.color = new Color(0.5f, 0f, 0.5f); // Purple
                break;
            case 6:
                spriteRenderer.color = new Color(1f, 0.5f, 0f); // Orange
                break;
        }


    }
    [PunRPC]
    void SetBounds()
    {
        // 카메라의 뷰포트를 기준으로 가로 및 세로 경계를 설정
        Camera mainCamera = Camera.main;
        float cameraZ = transform.position.z - mainCamera.transform.position.z;

        // 뷰포트의 네 모서리를 월드 좌표로 변환
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, cameraZ));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, cameraZ));

        // 가로 경계 설정
        minX = bottomLeft.x;
        maxX = topRight.x;

        // 세로 경계 설정 (배너 높이를 고려하여 조정)
        float bannerHeight = 100f; // 배너의 높이 (픽셀 단위)

        // 카메라의 월드 좌표에서 세로 해상도를 얻기 위한 계산
        float screenHeight = topRight.y - bottomLeft.y;
        float worldHeightPerPixel = screenHeight / Screen.height;
        float bannerHeightInWorld = bannerHeight * worldHeightPerPixel;
        Debug.Log("bannerHeightInWorld:" + bannerHeightInWorld);
        // 배너를 제외한 유효 영역을 설정
        minY = bottomLeft.y;
        maxY = topRight.y - bannerHeightInWorld; ;
    }



    void GetInput()
    {
        inputDir.x = Input.GetAxisRaw("Horizontal");
        inputDir.y = Input.GetAxisRaw("Vertical");

        if (inputDir.magnitude > 1)
        {
            inputDir.Normalize();
        }
    }


    void Move()
    {
        // inputDir 방향으로 속도를 곱해 움직임 계산
        Vector3 movement = inputDir * moveSpeed * Time.deltaTime;

        // 현재 위치에 movement를 더하여 이동
        this.transform.Translate(movement);

        // 현재 위치를 가져와 경계를 설정
        Vector3 currentPosition = this.transform.position;

        // 경계 값 내에서 현재 위치를 클램프 (제한)
        currentPosition.x = Mathf.Clamp(currentPosition.x, minX, maxX);
        currentPosition.y = Mathf.Clamp(currentPosition.y, minY, maxY);

        // 클램프된 위치로 이동
        this.transform.position = currentPosition;
    }

}
