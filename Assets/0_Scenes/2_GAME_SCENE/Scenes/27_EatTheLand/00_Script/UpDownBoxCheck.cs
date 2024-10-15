using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpDownBoxCheck : MonoBehaviour
{
    [SerializeField] private bool _isDownCast;

    [SerializeField] private float _xzBoxSize;
    [SerializeField] private float _yBoxSize;
    [SerializeField] private float _lengthFromTransform;
    
    private Vector3 _checkBox;
    private Vector3 _checkBoxCenter;

    // public Transform isLandObj;

    [SerializeField] private LayerMask checkLandMasks;
    [SerializeField] private Collider[] detatchedObj;


    private void UpdateCheckBoxParameter()
    {
        _checkBox = new Vector3(_xzBoxSize, _yBoxSize, _xzBoxSize);
        Vector3 castDir;
        if(_isDownCast)
        {
            castDir = Vector3.down;
        }
        else 
        {
            castDir = Vector3.up;
        }
        _checkBoxCenter = transform.position + castDir * _lengthFromTransform;
    }

    public bool CheckBox()
    {
        UpdateCheckBoxParameter();
        detatchedObj = Physics.OverlapBox(_checkBoxCenter, _checkBox * 0.5f, transform.rotation, checkLandMasks);

        // if(detatchedObj.Length ==0)
        // {
        //     isLandObj = null;
        // }
        // else
        // {
        //     isLandObj = detatchedObj[0].transform;
        // }
        if(detatchedObj.Length >0)
        {
            return true;
        }
        else return false;
    }
    
    void Update()
    {
        CheckBox();
    }
    void OnDrawGizmos()
    {
        UpdateCheckBoxParameter();
        Gizmos.color = Color.red;
        Gizmos.DrawCube(_checkBoxCenter, _checkBox /2);
    }
}
