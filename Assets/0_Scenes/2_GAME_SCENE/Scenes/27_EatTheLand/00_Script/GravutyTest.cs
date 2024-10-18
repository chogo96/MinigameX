using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravutyTest : MonoBehaviour
{
    [SerializeField] private float _gravity;
    Rigidbody _rb => GetComponent<Rigidbody>();
    void Start()
    {
        Physics.gravity = new Vector3(0, -25, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
