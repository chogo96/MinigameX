using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager_Mole : MonoBehaviour
{

	enum State
	{
		START,
		PLAY,
		GAMEOVER,
	}

	public static float time;
	public float timeLimit = 30;
	const float waitTime = 5;

	Animator anim;
	MoleManager moleManager;
	Text remainingTIme;
	AudioSource audio;

	State state;
	float timer;

	void Start()
	{
		Application.targetFrameRate = 60;

		this.state = State.START;
		this.timer = 0;
		this.anim = GameObject.Find("Canvas").GetComponent<Animator>();
		this.moleManager = GameObject.Find("GameManager").GetComponent<MoleManager>();
		this.remainingTIme = GameObject.Find("RemainingTime").GetComponent<Text>();
		this.audio = GetComponent<AudioSource>();
	}

	void Update()
	{
		if (this.state == State.START)
		{
			if (Input.GetMouseButtonDown(0))
			{
				this.state = State.PLAY;

				// hide start label
				this.anim.SetTrigger("StartTrigger");

				// start to generate moles
				this.moleManager.StartGenerate();

				this.audio.Play();
			}
		}
		else if (this.state == State.PLAY)
		{
			this.timer += Time.deltaTime;
			time = this.timer / timeLimit;

			if (this.timer > timeLimit)
			{
				this.state = State.GAMEOVER;

				// show gameover label
				this.anim.SetTrigger("GameOverTrigger");

				// stop to generate moles
				this.moleManager.StopGenerate();

				this.timer = 0;

				// stop audio
				this.audio.loop = false;
			}

			this.remainingTIme.text = "Time: " + ((int)(timeLimit - timer)).ToString("D2");
		}
		else if (this.state == State.GAMEOVER)
		{
			this.timer += Time.deltaTime;

			if (this.timer > waitTime)
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}

			this.remainingTIme.text = "";
		}
	}
}
