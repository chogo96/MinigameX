using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerFrogger : GameManager
{
    private AudioSource audioSource;
    [SerializeField] GameObject[] playerPos;
    [SerializeField] GameObject PlayerParent;

    void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        //Gen Player
    }

    // Update is called once per frame
    void Update()
    {

    }

}
