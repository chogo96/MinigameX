using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField] Transform wind_Start;
    public Transform wind_End;
    [SerializeField] float _windPower;

    //private void OnTriggerStay(Collider other)
    //{
    //    float windPower;
    //    windPower = (wind_End.position - other.transform.position).sqrMagnitude * _windPower * Time.fixedDeltaTime;
    //    Vector3 windDir = new Vector3(windPower,0, windPower);
    //    Rigidbody _rb = other.gameObject.GetComponent<Rigidbody>();
    //    _rb.velocity += windDir; 
    //    //Debug.Log("�ٶ� ũ�� =" + windPower+ "/ �׸��� ���� =" + windDir + " / �׸��� ���� = " + _rb.velocity);


    //}

}
