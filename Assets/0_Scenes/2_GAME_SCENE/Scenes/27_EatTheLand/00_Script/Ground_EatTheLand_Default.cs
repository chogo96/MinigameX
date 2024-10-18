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

        //  �ö� �ְ�
        if (_upDownCheck.detectedObj)
        {
            if (_upDownCheck.detectedObj && _upDownCheck.detectedObj != _lastDetetedObj)
            {
                _lastDetetedObj = _upDownCheck.detectedObj.gameObject;
                Debug.Log("���ο� �÷��̾ �ö��");
                //  ���� ����
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

    //  15�ۼ�Ʈ�� Ȯ���� �±׸� �ٲ۴�
    //  �׸��� �װ��� ���� ���ȸ� ������ �ȴ�
    //  �� �����Ǵ� �ð��� ���ܴٸ� (20��)

    //  �ٽ� Ȯ���� ���� �±׸� �ٲ��ش�.
    public void ChangeTag()
    {
        _changeTagPercent = Random.Range(0, 99);

        //Debug.Log("�ۼ�Ʈ�� " + _changeTagPercent);

        // 15�ۼ�Ʈ Ȯ����
        if(_changeTagPercent < _staticTagPercent)
        {
            this.gameObject.tag = "EXPLOSION";
            Debug.Log("�ͽ�������");
        }
        else
        {
            this.gameObject.tag = "Untagged";
            Debug.Log("�Ϲ�");
        }
        time = 0;
    }
}
