using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground_EatTheLand_Default : MonoBehaviour
{
    public Material[] materials;    
    private MeshRenderer _meshRender;
    private UpDownBoxCheck _upDownCheck;

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
        }
        else
        {
            return;
        }
    }
    
}
