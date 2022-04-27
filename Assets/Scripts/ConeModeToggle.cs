using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Mode
{
    SolidConeSelection, CircleSelection
}

public class ConeModeToggle : MonoBehaviour
{

    [SerializeField] public Mode _mode;
    public static ConeModeToggle _instance;

    //Solid cone mode related
    [SerializeField] GameObject[] _solidlabels;

    //Circle Selection mode related
    [SerializeField] GameObject[] _circlelabels;

    // Start is called before the first frame update
    void Start()
    {
        _instance = this;
        Invoke("invokeStart", 3);

    }

    void invokeStart()
    {
        if (_mode == Mode.CircleSelection)
        {
            /*SpaceLogic.Instance.myPlayerGO.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
            SpaceLogic.Instance.myPlayerGO.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);*/
            foreach (var item in _solidlabels)
            {
                item.SetActive(false);
            }

        }
        else 
        {
            /*SpaceLogic.Instance.myPlayerGO.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
            SpaceLogic.Instance.myPlayerGO.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);*/
            foreach (var item in _solidlabels)
            {
                item.SetActive(true);
            }
            foreach (var item in SpaceLogic.Instance.myPlayerGO.GetComponent<LabelVisualisation>()._labels)
            {
                item.SetActive(false);
            }
        }
        
        
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) {
            if (_mode == Mode.CircleSelection)
            {
                _mode = Mode.SolidConeSelection;
                /*SpaceLogic.Instance.myPlayerGO.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
                SpaceLogic.Instance.myPlayerGO.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);*/
            }
            else {
                /*SpaceLogic.Instance.myPlayerGO.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
                SpaceLogic.Instance.myPlayerGO.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);*/
                _mode = Mode.CircleSelection;
            }
        }
        invokeStart();
    }
}


