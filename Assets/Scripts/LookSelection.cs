using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Specialized;

public class LookSelection : MonoBehaviourPunCallbacks, IPunObservable
{


    [Header("***************PRESENT SELECTED OBEJCTS***********")]
    [SerializeField] public List<GameObject> _selectedObjects;

    [Header("*****************CONE PROPERTIES****************")]
    [SerializeField] public float _coneMaxDistance;
    [SerializeField] public float _coneApexAngle;

    public GameObject cone;

    //Private variables
    Vector3 lookDirection;
    float _angleBtw;
    float _distanceBtw;
    float _slantHeight;
    GameObject _player;
    int count = -1;
    bool temp = false;

    void Start()
    {
        Invoke("startFunction", 2.5f);
        _selectedObjects.Clear();
        //_pointLight.GetComponent<Light>().range = _coneMaxDistance;
        //_pointLight.GetComponent<Light>().spotAngle = _coneAppexAngle;
        //cone = GameObject.Find("ConeController");

    }

    void startFunction()
    {

        _player = SpaceLogic.Instance.myPlayerGO;
    }

    void Update()
    {
        resizeCone();
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("SelectableObject"))
        {
            Debug.DrawLine(cone.transform.position, (-cone.transform.position + item.transform.position) * 10.0f, Color.red);
            _angleBtw = Vector3.Angle(cone.transform.forward, (-cone.transform.position + item.transform.position));
            _distanceBtw = Vector3.Distance(cone.transform.forward, (-cone.transform.position + item.transform.position));

            _slantHeight = _coneMaxDistance / (Mathf.Cos((_coneApexAngle / 2) * Mathf.Deg2Rad));

            //Debug.Log("BEFORE CHECKING INSIDE VIEW CONE : Distance of " + item.name + "is" + _distanceBtw);

            if (_angleBtw < (_coneApexAngle / 2) && _distanceBtw < _slantHeight)
            {
                //Debug.Log("Angle between " + _angleBtw);
                //Debug.Log("AFTER CHECKING INSIDE VIEW CONE : Distance of " + item.name + "is" + _distanceBtw);
                // item.GetComponent<Renderer>().material.color = new Color(0, 0, 0);

                /*if (item.gameObject.name == "Carpet.001") {
                    item.GetComponent<Renderer>().material.SetFloat("_OutlineWidth", 10f);
                }
                else
                    item.GetComponent<Renderer>().material.SetFloat("_OutlineWidth", 1.76f);*/
                if (GetComponent<PlayerMovementManager>().coneToggleTemp)
                {
                    item.transform.GetChild(1).gameObject.SetActive(true);
                    item.transform.GetChild(1).gameObject.transform.LookAt(this.transform, Vector3.up);
                }


                if (!_selectedObjects.Contains(item) && GetComponent<PlayerMovementManager>().coneToggleTemp)
                {
                    _selectedObjects.Add(item);
                }

            }
            else
            {
                //item.GetComponent<Renderer>().material.SetFloat("_OutlineWidth", 0);
                //  item.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                _selectedObjects.Remove(item);

                item.transform.GetChild(1).gameObject.SetActive(false);
                item.transform.GetChild(1).GetComponent<TextLookAtScript>().inview = true;

            }

        }

        if (photonView.IsMine)
        {
            if (_selectedObjects.Count == 0)
            {
                CameraScript._instance.activateFocusEffect = false;
            }
            else
            {
                CameraScript._instance.activateFocusEffect = true;
            }

            addSelectableLayer();
        }

        //singleObjectSelection();
    }

    void addSelectableLayer()
    {

        foreach (var item in _selectedObjects)
        {
            item.gameObject.layer = 23;
            foreach (Transform child in item.transform)
            {
                child.gameObject.layer = 23;
            }
            item.gameObject.transform.GetChild(1).GetChild(0).gameObject.layer = 23;
        }

        foreach (GameObject item in GameObject.FindGameObjectsWithTag("SelectableObject"))
        {
            if (!(_selectedObjects.Contains(item)))
            {
                item.gameObject.layer = 8;
                foreach (Transform child in item.transform)
                {
                    child.gameObject.layer = 8;
                }
                item.gameObject.transform.GetChild(1).GetChild(0).gameObject.layer = 8;
            }
        }
    }

    public void resizeCone() {
        if (Input.GetKey(KeyCode.H)) {
            _coneApexAngle += Time.deltaTime *10;
        }
        if (Input.GetKey(KeyCode.L)) {
            _coneApexAngle -= Time.deltaTime *10;
        }

        if (_coneApexAngle >= 70) {
            _coneApexAngle = 70;
        }
        if (_coneApexAngle <= 50) {
            _coneApexAngle = 50;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }
    /*void singleObjectSelection() {
   int n = _selectedObjects.Count-1;
   if (Input.mouseScrollDelta.y > 0)
   {
       temp = true;
       count++;
       if (count > n)
       {
           count = 0;
       }
       //_selectedObjects[count].GetComponent<Renderer>().material.SetColor("_OutlineColor", Color.red);
   }
   else if (Input.mouseScrollDelta.y < 0)
   {
       temp = true;
       count--;
       if (count < 0)
       {
           count = n;
       }
       //_selectedObjects[count].GetComponent<Renderer>().material.SetColor("_OutlineColor", Color.red);
   }
   else {
       temp = false;
   }
   if (temp)
   {
       for (int i = 0; i <= n; i++)
       {
           if (i == count)
           {
               _selectedObjects[count].GetComponent<Renderer>().material.SetColor("_OutlineColor", Color.red);
           }
           else
           {
               _selectedObjects[count].GetComponent<Renderer>().material.SetColor("_OutlineColor", Color.green);
           }
       }
       temp = false;
   }

}*/
}


