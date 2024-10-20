using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground_EatTheLand : MonoBehaviour
{
    [SerializeField] private Transform child;
    UpDownBoxCheck _upDownCheckBox => GetComponent<UpDownBoxCheck>();
    Rigidbody _rb => GetComponent<Rigidbody>();
    Animator _anim => GetComponent<Animator>();

    MeshRenderer _meshRender => child.GetComponent<MeshRenderer>();

    [SerializeField] private Material[] materials;
    [SerializeField] private float _bouncePower;
    [SerializeField] private float _time;
    [SerializeField] private float _durationTime;
    [Range(0, 5)]
    [SerializeField] private int _changePercent;
    [SerializeField] bool isSpring = false;
    
    // Update is called once per frame
    void Update()
    {
        if(_upDownCheckBox.detectedObj)
        {       
            int index = _upDownCheckBox.detectedObj.GetComponent<Test_CustomProperties>().LetYouKnowPlayerTeam();
            _meshRender.material = materials[index];
        }
        ChangeState();
        if(_upDownCheckBox.detectedObjs.Length > 0 && isSpring)
        {
            // _rb.AddForce(Vector3.up * _bouncePower, ForceMode.Impulse);
            foreach(var obj in _upDownCheckBox.detectedObjs)
            {
                obj.gameObject.GetComponent<Player_EatTheLand_DefaultState>().ChangeState(ETL_State.SPRING);
            }            
            _anim.SetBool("isSpring", isSpring);
            isSpring = false;
            _time = 0;
            Debug.Log("플레이어 닿음");
        }    
        else 
        {
            _anim.SetBool("isSpring", false);
        }
    }
   
    void ChangeState()
    {
        _time += Time.deltaTime;
        if(_time > _durationTime)
        {
            _time = 0;
            int _percent = Random.Range(0, 9);
            if(_percent < _changePercent)
            {
                isSpring = true;                
            }
            else 
            {
                isSpring = false;
            }
        }
    }
}
