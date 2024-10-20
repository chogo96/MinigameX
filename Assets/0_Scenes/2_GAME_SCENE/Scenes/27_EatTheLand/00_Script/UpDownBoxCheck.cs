using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpDownBoxCheck : MonoBehaviour
{
    [SerializeField] private Color _boxColor;
    [SerializeField] private bool _isDownCast;

    [SerializeField] private float _xzBoxSize;
    [SerializeField] private float _yBoxSize;
    [SerializeField] private float _lengthFromTransform;
    
    private Vector3 _checkBox;
    private Vector3 _checkBoxCenter;

    public Transform detectedObj;

    [SerializeField] private LayerMask checkLandMasks;
    public Collider[] detectedObjs;


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
    public Ray hit;
    
    public bool CheckBox()
    {
        UpdateCheckBoxParameter();
        detectedObjs = Physics.OverlapBox(_checkBoxCenter, _checkBox * 0.25f, transform.rotation,  checkLandMasks);

        if (detectedObjs.Length == 0)
        {
            detectedObj = null;
        }
        else
        {
            detectedObj = detectedObjs[0].transform;
        }
        if (detectedObjs.Length >0)
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
        Gizmos.color = _boxColor;
        Gizmos.DrawCube(_checkBoxCenter, _checkBox /2);
    }

    
}
