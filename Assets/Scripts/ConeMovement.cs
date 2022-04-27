using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeMovement : MonoBehaviour
{

    [SerializeField] float sensitivity = 3f;
    public float xRot, yRot;
    public bool temp = true;
    Quaternion _coneInitialTransform;

    // Start is called before the first frame update
    void Start()
    {
        _coneInitialTransform = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (ConeModeToggle._instance._mode == Mode.SolidConeSelection)
        {

            if (Input.GetKey(KeyCode.M))
            {
                temp = false;
                xRot += Input.GetAxis("Mouse Y") * sensitivity;
                yRot += Input.GetAxis("Mouse X") * sensitivity;
                transform.rotation = Quaternion.Euler(0, yRot, 0) * Quaternion.Euler(-xRot, 0, 0);


                //transform.parent.parent.GetComponent<MouseCameraLook>().enabled = false;
                FindObjectOfType<MouseCameraLook>().enabled = false;
            }
            else
            {
                temp = true;
                //transform.parent.parent.GetComponent<MouseCameraLook>().enabled = true;
                FindObjectOfType<MouseCameraLook>().enabled = true;
                //transform.position = _coneInitialTransform;
                //transform.rotation = _coneInitialTransform;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                //transform.position = _coneInitialTransform.position;
                //transform.rotation.SetEulerAngles(new Vector3(18.887f, -20.478f, 0));

                transform.localRotation = _coneInitialTransform;
            }
        }
    }
}



