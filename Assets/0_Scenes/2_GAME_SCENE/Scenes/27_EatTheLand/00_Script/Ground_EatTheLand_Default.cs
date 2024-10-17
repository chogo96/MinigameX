using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground_EatTheLand_Default : MonoBehaviour
{
    public Material[] materials;    
    private MeshRenderer _meshRender;
    private UpDownBoxCheck _upDownCheck;

    [SerializeField] private float _explsionForwardPower;
    [SerializeField] private float _explsionUpPower;


    private void Awake()
    {
        _meshRender = GetComponent<MeshRenderer>();
        _upDownCheck = GetComponent<UpDownBoxCheck>();
        
    }

    private void Update()
    {
        if(_upDownCheck.detatchedObj)
        {
            int index = (int)_upDownCheck.detatchedObj.GetComponent<PlayerControl>().team;
            _meshRender.material = materials[index];
            //if (this.gameObject.CompareTag("EXPLOSION"))
            //{
            //    foreach (var obj in _upDownCheck.detatchedObjs)
            //    {
            //        PlayerControl _control = obj.GetComponent<PlayerControl>();
            //        //_control.explosionDir =
            //        _control.ChangeState(EPlayer.EXPLOSION);
            //        this.gameObject.tag = "Untagged";
                 
            //    }
            //}
            
        }
        else
        {
            return;
        }
    }
}
