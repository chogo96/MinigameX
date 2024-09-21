using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    private Vector3 mousePos; // Vector2 대신 Vector3로 변경
    public ParticleSystem[] clickParticles; // GameObject 배열을 public으로 선언하여 Unity Inspector에서 설정할 수 있게 함
    public GameObject[] followCursor;
    void Start()
    {
        // 모든 파티클 비활성화
        foreach (ParticleSystem particle in clickParticles)
        {
            //   particle.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // 마우스 클릭 위치를 화면 좌표에서 월드 좌표로 변환
        mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(Camera.main.transform.position.z); // Z 값을 카메라와의 거리에 따라 설정

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Debug.Log($"Input.mousePosition: {Input.mousePosition.ToString()}");
        Debug.Log($"Mouse Click: {worldPos.ToString()}");



        foreach (GameObject particle in followCursor)
        {
            particle.SetActive(true);
            particle.transform.position = new Vector3(worldPos.x, worldPos.y, 0f);
        }
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log($"Click!~~");


            // 각 파티클을 활성화하고 위치를 설정
            foreach (ParticleSystem particle in clickParticles)
            {
                Debug.Log($"Click!~~222");
                // particle.gameObject.SetActive(false);
                // particle.gameObject.SetActive(true);
                particle.Stop();
                particle.Play();
                Debug.Log($"Play particle! {particle.isPlaying}");
                particle.transform.position = new Vector3(worldPos.x, worldPos.y, 0f);
            }

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
