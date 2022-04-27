using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

//using UnityEngine.XR.Interaction.Toolkit;


public class VRActive : MonoBehaviourPunCallbacks, IPunObservable
{
  public Camera userCamera;
  //public XRController rightController;
  //public XRController leftController;
  public static GameObject localPlayerInstance;
    public GameObject rightHand;
    public GameObject leftHand;

    void Awake()
  {
    Debug.Log("MOUSE AWAKES");
        if (photonView.IsMine)
        {
          VRActive.localPlayerInstance = this.gameObject;
          userCamera = transform.GetChild(0).GetChild(0).GetComponent<Camera>();
          //rightController = transform.GetChild(0).GetChild(1).GetComponent<XRController>();
          //leftController = transform.GetChild(0).GetChild(2).GetComponent<XRController>();

          GameObject.Find("StandbyCamera").GetComponent<Camera>().enabled = false;
          userCamera.enabled = true;
          //rightController.enabled = true;
          //leftController.enabled = true;
        }
       DontDestroyOnLoad(this.gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }
}
