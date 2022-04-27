using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Photon.Pun;
using Photon.Realtime;


public class MouseCameraLook : MonoBehaviourPunCallbacks
{

    public float mouseSensitivity = 100f;

    public Transform playerBody;
    public Camera userCamera;

    float xRotation = 0f;

    // Start is called before the first frame update

    void Awake()
    {
        if (playerBody == null) // Set body to rotate it into the viewing direction.
        {
            playerBody = this.transform.Find("Body");
        }
    }

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        userCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        if (playerBody != null)
        {
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}
