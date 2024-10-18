using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] int _changeTagPercent;
    [SerializeField] int _staticTagPercent;

    [SerializeField] bool _isExplosion;
   
    private GameObject _lastDetetedObj;

    public float time;
    public float _durationTime;

    private void Awake()
    {
        _meshRender = GetComponent<MeshRenderer>();
        _upDownCheck = GetComponent<UpDownBoxCheck>();        
        _rb= GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //ChangeTag();
        time += Time.deltaTime;
        Debug.Log(time);

        if (time > 5)
        {
            time = 0;
            ChangeTag();
            Debug.Log(time);
        }

        //  올라가 있고
        if (_upDownCheck.detectedObj)
        {
            if (_upDownCheck.detectedObj && _upDownCheck.detectedObj != _lastDetetedObj)
            {
                _lastDetetedObj = _upDownCheck.detectedObj.gameObject;
                Debug.Log("새로운 플레이어가 올라옴");
                //  색상 변경
                int index = (int)_upDownCheck.detectedObj.GetComponent<PlayerControl>().team;
                _meshRender.material = materials[index]; 
            }
            if(this.gameObject.tag == "EXPLOSION")
            {
                _rb.AddForce(Vector3.up * _bounceForce, ForceMode.Impulse);
                this.gameObject.tag = "Untagged";
            }
        }
        else
        {
            return;
        }

      
    }

    //  15퍼센트의 확률로 태그를 바꾼다
    //  그리고 그것은 몇초 동안만 유지가 된다
    //  그 유지되는 시간이 지단다면 (20초)

    //  다시 확률을 돌려 태그를 바꿔준다.
    public void ChangeTag()
    {
        _changeTagPercent = Random.Range(0, 99);

        //Debug.Log("퍼센트는 " + _changeTagPercent);

        // 15퍼센트 확률로
        if(_changeTagPercent < _staticTagPercent)
        {
            this.gameObject.tag = "EXPLOSION";
            Debug.Log("익스프로전");
        }
        else
        {
            this.gameObject.tag = "Untagged";
            Debug.Log("일반");
        }
        time = 0;
    }
}
