using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerFrogger : GameManager
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
    }

    // Update is called once per frame
    void Update()
    {

    }

}
