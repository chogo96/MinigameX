using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground_EatTheLand : MonoBehaviour
{
    UpDownBoxCheck _upDownCheckBox => GetComponent<UpDownBoxCheck>();
    Rigidbody _rb => GetComponent<Rigidbody>();
    
    [SerializeField] private float _bouncePower;
    [SerializeField] private float _time;
    [SerializeField] private float _durationTime;
    [Range(0, 5)]
    [SerializeField] private int _changePercent;
    [SerializeField] bool isExplosion = false;
    // Update is called once per frame
    void Update()
    {
        ChangeState();
        if(_upDownCheckBox.CheckBox() && isExplosion)
        {
            _rb.AddForce(Vector3.up * _bouncePower, ForceMode.Impulse);
            isExplosion = false;
            Debug.Log("플레이어 닿음");
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
                isExplosion = true;                
            }
            else 
            {
                isExplosion = false;
            }

        }
    }
}
