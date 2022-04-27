using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class PlayerMovementManagerHMD : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GameObject localPlayerInstance;
    public GameObject pointer;
    public Transform camera;
    private CharacterController controller;

    public float speed = 12f;

    void Awake()
    {
        // #Important
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (photonView.IsMine)
        {
            PlayerMovementManager.localPlayerInstance = this.gameObject;
            pointer.GetComponent<PointingRay>().SetOwned(true);

        }
        else
        {
            pointer.GetComponent<PointingRay>().SetOwned(false);

        }
        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        controller = this.GetComponent<CharacterController>();
    }

    void Update()
    {
        float thumbStickX = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x;
        float thumbStickY = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y;

        Vector3 rotate = Vector3.up * thumbStickX;
        Vector3 move = new Vector3(camera.forward.x, 0.0f, camera.forward.z) * thumbStickY;

        this.transform.Rotate(rotate);
        controller.Move(move * speed * Time.deltaTime);

        if (OVRInput.GetDown(OVRInput.Button.One)) 
        {
            pointer.GetComponent<PointingRay>().Toggle();
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
        }
        else
        {
            // Network player, receive data
        }
    }
}
