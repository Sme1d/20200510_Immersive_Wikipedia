using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelVisualisation : MonoBehaviour
{
    [SerializeField] public GameObject[] _labels;

    //private
    LookSelection _lookselection;
    List<GameObject> _selectedObjects = new List<GameObject>();
    GameObject[] _allSelectableObjects;

    // Start is called before the first frame update
    void Start()
    {
        _allSelectableObjects = GameObject.FindGameObjectsWithTag("SelectableObject");
        _lookselection = transform.GetComponent<LookSelection>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ConeModeToggle._instance._mode == Mode.CircleSelection)
        {

            _selectedObjects = _lookselection._selectedObjects;
            foreach (var item in _selectedObjects)
            {
                switch (item.name)
                {
                    case "Chair1":
                        _labels[0].SetActive(true);
                        _labels[0].GetComponent<LineRenderer>().SetPosition(0, _labels[0].transform.position);
                        _labels[0].GetComponent<LineRenderer>().SetPosition(1, item.transform.position);
                        /*item.GetComponent<LineRenderer>().SetPosition(0, item.transform.position);
                        item.GetComponent<LineRenderer>().SetPosition(1, _labels[0].transform.position);*/
                        break;
                    case "Chair2":
                        _labels[1].SetActive(true);
                        _labels[1].GetComponent<LineRenderer>().SetPosition(0, _labels[1].transform.position);
                        _labels[1].GetComponent<LineRenderer>().SetPosition(1, item.transform.position);
                        /*item.GetComponent<LineRenderer>().SetPosition(0, item.transform.position);
                        item.GetComponent<LineRenderer>().SetPosition(1, _labels[1].transform.position);*/
                        break;
                    case "Carpet":
                        _labels[2].SetActive(true);
                        _labels[2].GetComponent<LineRenderer>().SetPosition(0, _labels[2].transform.position);
                        _labels[2].GetComponent<LineRenderer>().SetPosition(1, item.transform.position);
                        /*item.GetComponent<LineRenderer>().SetPosition(0, item.transform.position);
                        item.GetComponent<LineRenderer>().SetPosition(1, _labels[2].transform.position);*/
                        break;
                    case "box":
                        _labels[3].SetActive(true);
                        _labels[3].GetComponent<LineRenderer>().SetPosition(0, _labels[3].transform.position);
                        _labels[3].GetComponent<LineRenderer>().SetPosition(1, item.transform.position);
                        break;
                    case "Mesanova":
                        _labels[4].SetActive(true);
                        _labels[4].GetComponent<LineRenderer>().SetPosition(0, _labels[4].transform.position);
                        _labels[4].GetComponent<LineRenderer>().SetPosition(1, item.transform.position);
                        break;
                    case "Tub":
                        _labels[5].SetActive(true);
                        _labels[5].GetComponent<LineRenderer>().SetPosition(0, _labels[5].transform.position);
                        _labels[5].GetComponent<LineRenderer>().SetPosition(1, item.transform.position);
                        break;
                    case "Wardrobe":
                        _labels[6].SetActive(true);
                        _labels[6].GetComponent<LineRenderer>().SetPosition(0, _labels[6].transform.position);
                        _labels[6].GetComponent<LineRenderer>().SetPosition(1, item.transform.position);
                        break;
                    case "BauhausWiege":
                        _labels[7].SetActive(true);
                        _labels[7].GetComponent<LineRenderer>().SetPosition(0, _labels[7].transform.position);
                        _labels[7].GetComponent<LineRenderer>().SetPosition(1, item.transform.position);
                        break;
                    default:
                        break;
                }
            }

            foreach (var item in _allSelectableObjects)
            {
                if (!(_selectedObjects.Contains(item)))
                {
                    switch (item.name)
                    {
                        case "Chair1":
                            _labels[0].SetActive(false);
                            _labels[0].GetComponent<LineRenderer>().SetPosition(0, new Vector3(0, 0, 0));
                            _labels[0].GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, 0, 0));
                            /*item.GetComponent<LineRenderer>().SetPosition(0, new Vector3(0, 0, 0));
                            item.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, 0, 0));*/
                            break;
                        case "Chair2":
                            _labels[1].SetActive(false);
                            _labels[1].GetComponent<LineRenderer>().SetPosition(0, new Vector3(0, 0, 0));
                            _labels[1].GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, 0, 0));
                            /*item.GetComponent<LineRenderer>().SetPosition(0, new Vector3(0, 0, 0));
                            item.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, 0, 0));*/
                            break;
                        case "Carpet":
                            _labels[2].SetActive(false);
                            _labels[2].GetComponent<LineRenderer>().SetPosition(0, new Vector3(0, 0, 0));
                            _labels[2].GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, 0, 0));
                            /*item.GetComponent<LineRenderer>().SetPosition(0, new Vector3(0, 0, 0));
                            item.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, 0, 0));*/
                            break;
                        case "box":
                            _labels[3].SetActive(false);
                            _labels[3].GetComponent<LineRenderer>().SetPosition(0, new Vector3(0, 0, 0));
                            _labels[3].GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, 0, 0));
                            break;
                        case "Mesanova":
                            _labels[4].SetActive(false);
                            _labels[4].GetComponent<LineRenderer>().SetPosition(0, new Vector3(0, 0, 0));
                            _labels[4].GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, 0, 0));
                            break;
                        case "Tub":
                            _labels[5].SetActive(false);
                            _labels[5].GetComponent<LineRenderer>().SetPosition(0, new Vector3(0, 0, 0));
                            _labels[5].GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, 0, 0));
                            break;
                        case "Wardrobe":
                            _labels[6].SetActive(false);
                            _labels[6].GetComponent<LineRenderer>().SetPosition(0, new Vector3(0, 0, 0));
                            _labels[6].GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, 0, 0));
                            break;
                        case "BauhausWiege":
                            _labels[7].SetActive(false);
                            _labels[7].GetComponent<LineRenderer>().SetPosition(0, new Vector3(0, 0, 0));
                            _labels[7].GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, 0, 0));
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}



