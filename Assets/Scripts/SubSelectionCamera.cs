using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SubSelectionCamera : MonoBehaviourPunCallbacks, IPunObservable
{
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }

    [SerializeField] GameObject _cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //_players = GameObject.FindGameObjectsWithTag("Player");
        //toggleSubSlectionCamera();
        if (photonView.IsMine)
        {
            _cam.SetActive(true);
           // Debug.Log("in if - is my player");
        }
        else
        {
            _cam.SetActive(false);
            //Debug.Log("in else - is other player");
        }
    }



}

