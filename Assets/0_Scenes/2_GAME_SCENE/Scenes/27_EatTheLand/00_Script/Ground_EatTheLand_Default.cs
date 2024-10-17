using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground_EatTheLand_Default : MonoBehaviour
{
    public Material[] materials;    
    private MeshRenderer _meshRender;
    private UpDownBoxCheck _upDownCheck;
    private Rigidbody _rb;

    [SerializeField] private float _explsionForwardPower;
    [SerializeField] private float _explsionUpPower;
    [SerializeField] private float _bounceForce;
    [SerializeField] int _bouncePercent;


    private void Awake()
    {
        _meshRender = GetComponent<MeshRenderer>();
        _upDownCheck = GetComponent<UpDownBoxCheck>();
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(_upDownCheck.detatchedObj)
        {
            bool isIn = true;
            int index = (int)_upDownCheck.detatchedObj.GetComponent<PlayerControl>().team;
            _meshRender.material = materials[index];

           
           

            this.gameObject.tag = "EXPLOSION";
            Invoke("BeSpring", 0.1f);
            //if (isIn)
            //{
            //    BeSpring();
            //    isIn= false;
            //}
            
        }
        else
        {
            return;
        }
    }

    private void BeSpring()
    {
        //int percent = Random.Range(1, 10);
        //Debug.Log(percent);
        Vector3 bounceDir = Vector3.up;

        //this.gameObject.tag = "EXPLOSION";

        _rb.AddForce(bounceDir * _bounceForce, ForceMode.Impulse);
        this.gameObject.tag = "Untagged";
        //if (percent <= _bouncePercent)
        //{
        //    Vector3 bounceDir = Vector3.up;

        //    this.gameObject.tag = "EXPLOSION";
        //    _rb.AddForce(bounceDir * _bounceForce, ForceMode.Impulse);
        //    this.gameObject.tag = "Untagged";
        //    //StartCoroutine(WaitForBeSpring());  
        //}
    }

    IEnumerator WaitForBeSpring()
    {
        yield return new WaitForSeconds(1);
        Vector3 bounceDir = Vector3.up;
        this.gameObject.tag = "EXPLOSION";
        _rb.AddForce(bounceDir * _bounceForce, ForceMode.Impulse);
        this.gameObject.tag = "Untagged";
    }
}
