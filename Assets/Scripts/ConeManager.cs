using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeManager : MonoBehaviour
{
    [SerializeField] LookSelection _lookselection;

    [SerializeField] float _coneHeight; //Z axis
    [SerializeField] float _coneRadius; //x/y 
    [SerializeField] float _apexAngle; //tan
    // Start is called before the first frame update

    //private

    float _prevHeight;

    void Start()
    {
        Invoke("startFunction", 2.0f);
    }

    /*void startFunction()
    {
        _slantHeight = _lookselection._coneMaxDistance;
        _apexAngle = _lookselection._coneApexAngle;

    }

    private void Update()
    {
        _slantHeight = _lookselection._coneMaxDistance;
        _apexAngle = _lookselection._coneApexAngle;

        _coneRadius = 2 * _slantHeigt * Mathf.Sin((_apexAngle/2) * Mathf.Deg2Rad);
        //_coneRadius = (Mathf.Abs(_coneRadius))/2;

        _coneHeight = _coneRadius / (2 * Mathf.Tan((_apexAngle/2) * Mathf.Deg2Rad));

        transform.localScale = new Vector3(_coneRadius, _coneRadius, _coneHeight);


    }*/


    void startFunction()
    {
        _prevHeight = _lookselection._coneMaxDistance;
    }

    private void Update()
    {
        _coneHeight = _lookselection._coneMaxDistance;
        _apexAngle = _lookselection._coneApexAngle;

        if (_prevHeight == _coneHeight)
        {
            _coneRadius = 2 * _coneHeight * Mathf.Tan((_apexAngle / 2) * Mathf.Deg2Rad);
        }

        transform.localScale = new Vector3(_coneRadius, _coneRadius, _coneHeight);

        _prevHeight = _coneHeight;
    }
}



