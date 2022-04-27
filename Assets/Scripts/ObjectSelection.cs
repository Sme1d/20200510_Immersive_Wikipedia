using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelection : MonoBehaviour
{
    [Header("PROPERTIES TO SET")]
        [Tooltip("Length of your raycast line")]
        [SerializeField] float _rayCaseLength = 100.0f;






    //Private Variables section
        Ray _ray;
        RaycastHit _hit;
        GameObject _selectedObject;

    #region UNITY METHODS

    void Start()
    {
        
    }

    void Update()
    {
        RayCastingObjects(); //Raycast to the object - Either by looking or on mouse click!!!
    }

    #endregion




    #region CUSTOM METHOS
    void RayCastingObjects()  //Raycast to the object - Either by looking or on mouse click!!!
    {

           
        _ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(_ray, out _hit))
        {
            if (_hit.transform.gameObject.tag == "Selectable")
            {
                if (Input.GetMouseButtonDown(0)) //on mouse click
                {
                    //Debug.Log(_hit.transform.gameObject);
                    _selectedObject = _hit.transform.gameObject;
                    ObjectSelectionEffects(_hit.transform.gameObject);
                }
            }
        }
        else
        {
            _selectedObject.transform.GetChild(0).gameObject.SetActive(false);
            _selectedObject.transform.GetChild(1).gameObject.SetActive(false);
        }

            
        

    }



    void ObjectSelectionEffects(GameObject _selectedObject)
    {
        _selectedObject.transform.GetChild(0).gameObject.SetActive(true);
        _selectedObject.transform.GetChild(1).gameObject.SetActive(true);
    }
    #endregion
}
