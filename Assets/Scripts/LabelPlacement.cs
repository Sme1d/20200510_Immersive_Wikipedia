using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelPlacement : MonoBehaviour
{
    public LookSelection _LookSelection;
    [SerializeField] List<GameObject> _selectedObjects_lp;

    [SerializeField] float _angle;
    [SerializeField] float _axisPerpendicular;
    [SerializeField] float _objectPointRadius;
    //[SerializeField] float _tempangle;
    [SerializeField] float dot;

    [SerializeField] float _xoffset;
    [SerializeField] float _yoffset;
    float _zoffset = 1.5f;

    bool once = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _selectedObjects_lp = _LookSelection._selectedObjects;


        foreach (var item in _selectedObjects_lp)
        {
            if (item.transform.GetChild(1).GetComponent<TextLookAtScript>().inview)
            {

                _angle = Vector3.Angle(transform.forward, (-transform.position + item.transform.position));
                //_tempangle = Vector3.SignedAngle(transform.forward, (-transform.position + item.transform.position), transform.forward);
                _axisPerpendicular = Mathf.Abs(Mathf.Cos(_angle) * (-transform.position + item.transform.position).magnitude);
                _objectPointRadius = Mathf.Abs(Mathf.Tan((_LookSelection._coneApexAngle / 2) * Mathf.Deg2Rad) * _axisPerpendicular);

                dot = Vector3.Dot(transform.right, (-transform.position + item.transform.position));

                /*if (dot >= 0)
                {
                    item.transform.GetChild(1).transform.position = item.transform.position + new Vector3(_objectPointRadius + _xoffset, _yoffset, _zoffset);
                }
                else {
                    item.transform.GetChild(1).transform.position = item.transform.position + new Vector3(_objectPointRadius + _xoffset, _yoffset, -_zoffset);
                }*/

                item.transform.GetChild(1).transform.position = item.transform.position + new Vector3(_objectPointRadius + _xoffset, _yoffset, -_zoffset);

                //item.transform.GetChild(1).transform.position = item.transform.position + new Vector3(_objectPointRadius + _xoffset, _yoffset, _zoffset);
                item.transform.GetChild(1).GetComponent<TextLookAtScript>().inview = false;
            }

        }

    }
}



