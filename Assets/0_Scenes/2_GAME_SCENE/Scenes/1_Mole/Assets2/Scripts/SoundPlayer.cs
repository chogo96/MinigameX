using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
	// 여러 개의 오디오 클립을 배열로 관리
	public AudioClip[] Sound;

	// 0 - 두더지 등장할 때의 소리
	// 1 - 나쁜 두더지 잡힐 때의 소리
	// 2 - 좋은 두더지 잡힐 때의 소리

	private AudioSource audioSource;

	void Awake()
	{
		// AudioSource 컴포넌트를 가져옴
		audioSource = GetComponent<AudioSource>();

		// AudioSource가 없으면 자동으로 추가
		if (audioSource == null)
		{
			audioSource = gameObject.AddComponent<AudioSource>();
		}
	}

	public void SoundPlay(int Sound_Number)
	{
		// 유효한 범위 내에서만 사운드를 재생하도록 체크
		if (Sound_Number >= 0 && Sound_Number < Sound.Length)
		{
			// AudioClip을 설정하고 재생
			audioSource.clip = Sound[Sound_Number];
			audioSource.Play();
		}
		else
		{
			Debug.LogWarning("Invalid Sound_Number: " + Sound_Number);
		}
	}
}
