using UnityEngine;
using System.Collections;

public class Mole_Touch : MonoBehaviour
{
	private Animator _anim;
	private bool touch_possible; // 애니메이션 상태를 확인하기 위한 플래그
	public AudioClip[] audios; // 각 소리를 재생하기 위한 배열
	private AudioSource audioSource; // AudioSource를 사용하기 위한 변수
	private Mole_Manager mm; // CatchCount를 카운트하기 위한 변수
	private bool BedMole;

	void Start()
	{
		_anim = GetComponent<Animator>();
		mm = FindObjectOfType<Mole_Manager>();
		audioSource = GetComponent<AudioSource>();

		// AudioSource가 없으면 자동으로 추가
		if (audioSource == null)
		{
			audioSource = gameObject.AddComponent<AudioSource>();
		}
	}

	// 두더지가 나타날 때
	public void Open()
	{
		touch_possible = true; // 플레이어가 이 두더지를 잡을 수 있게 함
		audioSource.clip = audios[0]; // 소리를 설정
		audioSource.Play(); // 소리를 재생
		if (mm.Play == false)
		{
			mm.GO();
		}
	}

	// 두더지가 도망가고 있을 때
	public void Close()
	{
		touch_possible = false;
	}

	// 두더지가 완전히 사라졌을 때
	public IEnumerator PerfectClose()
	{
		touch_possible = false;
		yield return new WaitForSeconds(Random.Range(0.5f, 3.5f)); // 랜덤 시간 동안 대기

		int A = Random.Range(0, 100);

		if (A >= 30)
		{
			BedMole = true;
			_anim.SetBool("BedMole", true);
		}
		else
		{
			_anim.SetBool("BedMole", false);
			BedMole = false;
		}
		_anim.SetTrigger("Open"); // 다시 두더지 등장
	}

	// 두더지를 클릭했을 때
	void OnMouseDown()
	{
		if (touch_possible)
		{
			_anim.SetTrigger("Touch"); // 1. 잡기 애니메이션 재생
			touch_possible = false; // 2. 더블 클릭 방지
			audioSource.clip = audios[1]; // 3. 소리 설정
			audioSource.Play(); // 4. 소리 재생
			if (mm != null) mm.CatchCount_Up(BedMole); // 5. 매니저에 카운트 증가
		}
	}
}
