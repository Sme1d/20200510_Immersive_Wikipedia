using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class PlayerMovementManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GameObject localPlayerInstance;
    public CharacterController controller;
    public GameObject pointer;
    public List<GameObject> _selectedObjects_pm;
    public GameObject _player;
    //public GameObject PlayerUiPrefab;

    public float speed = 12f;

    public bool coneToggle = true;
    public bool coneToggleTemp = false;

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
        Invoke("startFunction", 1.5f);
    }

    void startFunction()
    {

        _player = SpaceLogic.Instance.myPlayerGO;
    }

    // Update is called once per frame
    void Update()
    {

        _selectedObjects_pm = GetComponent<LookSelection>()._selectedObjects;
        foreach (var item in _selectedObjects_pm)
        {
            item.transform.GetChild(0).gameObject.SetActive(!_player.GetComponent<PlayerMovementManager>().coneToggle);
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetKeyDown("q"))
        {
            pointer.GetComponent<PointingRay>().Toggle();
        }
        if (photonView.IsMine)
        {
            ConeToggle();
        }


    }

    void ConeToggle()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {
            coneToggleTemp = true;
            if (coneToggle)
            {

                if (ConeModeToggle._instance._mode == Mode.SolidConeSelection)
                {
                    SpaceLogic.Instance.myPlayerGO.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
                    SpaceLogic.Instance.myPlayerGO.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
                }
                else
                {
                    SpaceLogic.Instance.myPlayerGO.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
                    SpaceLogic.Instance.myPlayerGO.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
                }

            }
            else {
                SpaceLogic.Instance.myPlayerGO.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
                SpaceLogic.Instance.myPlayerGO.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
            }
            //_player.transform.GetChild(1).GetChild(1).gameObject.SetActive(coneToggle);
            _player.GetComponent<LookSelection>().enabled = coneToggle;
            coneToggle = !coneToggle;
        }

        if (coneToggle && coneToggleTemp)
        {
            coneToggleTemp = false;
            StartCoroutine(CT());
        }


    }

    IEnumerator CT()
    {
        foreach (var item in GetComponent<LookSelection>()._selectedObjects)
        {
            item.transform.GetChild(1).gameObject.SetActive(false);
        }
        GetComponent<LookSelection>()._selectedObjects.Clear();
        _player.GetComponent<LookSelection>().enabled = true;
        yield return new WaitForSeconds(1.0f);
        _player.GetComponent<LookSelection>().enabled = false;
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



