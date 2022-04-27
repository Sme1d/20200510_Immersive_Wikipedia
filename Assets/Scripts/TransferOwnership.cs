using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TransferOwnership : MonoBehaviourPun, IPunOwnershipCallbacks
{
    GameObject cylinder;
    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        if (targetView != base.photonView)
            return;
        base.photonView.TransferOwnership(requestingPlayer);
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        if (targetView != base.photonView)
            return;
    }

    // Start is called before the first frame update
    void Start()
    {
        cylinder = GameObject.Find("Cylindertest");
    }

    private void Awake()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    void master()
    {
        if (Input.GetKey(KeyCode.V))
        {
            base.photonView.RequestOwnership();
            cylinder.transform.position += Vector3.up * 2 * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.B))
        {
            base.photonView.RequestOwnership();
            cylinder.transform.position += Vector3.down * 2 * Time.deltaTime;
        }
    }


    // Update is called once per frame
    void Update()
    {
        master();
    }
}
