using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    [SerializeField] private float _gravity;

    public float OnGravity()
    {
        return _gravity * Time.deltaTime;
    }
}
