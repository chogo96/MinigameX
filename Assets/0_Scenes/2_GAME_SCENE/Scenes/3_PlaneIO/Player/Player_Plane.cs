using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerPlane : MonoBehaviour
{
    private GameObject MousePointer;
    [SerializeField] public GameObject DirectionObject; // 방향을 표시하기 위해 사용 (Optional)
    [SerializeField] float speed = 5f; // 속도
    public float Hp = 100;

    private Rigidbody2D rb;
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 0, -10); // Z축을 포함한 오프셋
    Vector2 directionVector = Vector2.zero;
    [SerializeField] private LayerMask wallLayer; // 벽 레이어를 지정 (충돌 감지 대상)
    private PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D 가져오기
        SetPlayer();
        Hp = 100;
        MousePointer = FindObjectOfType<MousePointer>().gameObject;
        // photonView 컴포넌트를 가져옴
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        MoveCamera();


    }
    public bool IsMine()
    {
        return photonView.IsMine;
    }
    void MoveCamera()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        Camera.main.transform.position = transform.position + cameraOffset;
    }
    void Move()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        // MousePointer 방향으로 Force를 가함
        directionVector = (MousePointer.transform.position - transform.position); // 방향 벡터 계산

        // 벡터 크기 (magnitude) 계산
        if (directionVector.magnitude > 1f)
        {
            // 벡터를 정규화하고 크기를 1로 설정
            directionVector = directionVector.normalized * 1f;
        }

        // 벽과의 거리 확인 (Raycast)
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionVector, 1f, wallLayer);

        if (hit.collider == null)
        {
            // 벽이 1 유닛 내에 없으면 이동
            rb.velocity = directionVector * speed;
        }
        else
        {
            //  Debug.Log("벽과 가까운");
            // 벽이 가까우면 속도를 0으로 설정 (이동하지 않음)
            rb.velocity = Vector2.zero;
        }

        // Optional: Direction 오브젝트가 있으면 방향을 그쪽으로 회전시킴
        if (DirectionObject != null)
        {
            DirectionObject.transform.up = directionVector;
            DirectionObject.transform.localScale = new Vector3(directionVector.magnitude, directionVector.magnitude, DirectionObject.transform.localScale.z);
        }
    }


    public void ChangeWeapon(GameObject newWeapon)
    {
        // Weapon이라는 이름을 가진 오브젝트를 찾음
        Transform weaponTransform = transform.Find("Weapon");

        if (weaponTransform != null)
        {
            // Weapon을 제외한 모든 자식 오브젝트 삭제
            foreach (Transform child in weaponTransform)
            {
                Destroy(child.gameObject);
            }
            // newWeapon이 프리팹일 경우 인스턴스화 필요
            GameObject weaponInstance = Instantiate(newWeapon.gameObject);
            // 새로운 무기를 Weapon의 자식으로 추가
            weaponInstance.transform.SetParent(weaponTransform);
            weaponInstance.transform.localPosition = Vector3.zero; // 필요 시 위치 초기화
            SetPlayer();
        }
        else
        {
            Debug.LogWarning("Weapon 오브젝트가 없습니다.");
        }
    }
    void SetPlayer()
    {
        // 자식 객체들 중 WeaponPlane 컴포넌트를 가진 모든 객체 가져오기
        WeaponPlane[] weaponPlanes = GetComponentsInChildren<WeaponPlane>();

        // 각 무기에 대해 SetPlayer 호출
        foreach (WeaponPlane weapon in weaponPlanes)
        {
            weapon.SetPlayer(this); // player는 현재 플레이어 객체를 전달
            Debug.Log("Weapon assigned to player: " + weapon.gameObject.name);
        }

    }

    void Die()
    {
        //
    }

    public void Damaged(float Damage)
    {
        Hp -= Damage;
        if (Hp < 0)
        {
            Die();
        }
    }
}