using System.Collections;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.XR;
using System.Linq;

public class SpaceLogic : MonoBehaviourPunCallbacks
{
    public static SpaceLogic Instance;

    [Tooltip("The prefab to use for representing the player")]
    public GameObject desktopUserPrefab;
    public GameObject hmdUserPrefab;
    public GameObject myPlayerGO;

    public Camera standbyCamera;
    public GameObject connectionMenu;
    private Dictionary<int, GameObject> otherPlayers = new Dictionary<int, GameObject>();

    string gameVersion = "1";

    private void Awake()
    {
    }

    void Start()
    {
        // Check if started through lobby or through the scene.
        if (PhotonNetwork.CurrentRoom != null)
        {
            // A photon room exists -> init users.
            SetupUsersWithPhoton();
        } else
        {
            // No room exists -> setup photon and room.
            connectionMenu.SetActive(true);
            SetupPhotonNetwork();   
        }
    }

    // Update is called once per frame
    void Update()
    {
        updateOtherPlayers();
    }

    public float getDistanceToOtherPlayer(Vector3 ownPlayerPos, Vector3 otherPlayerPos)
    {
        return Vector3.Distance(ownPlayerPos, otherPlayerPos); ;
    }

    private void SetupPhotonNetwork()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Starting without Lobby. Connecting to Photon ...");
        StartCoroutine(SwitchToOffline(5)); // Async function that checks if network is running after 5 sec.
    }

    public override void OnConnectedToMaster()
    {
        //After we connected to Master server create a default room
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("Conntected to Photon. Creating Room ...");
            connectionMenu.transform.Find("ConnectionText").GetComponent<Text>().text = "Connected.";
            PhotonNetwork.LocalPlayer.NickName = "DefaultUser";
            RoomOptions roomOptions = new RoomOptions();
            TypedLobby sqlLobby = new TypedLobby("immersive", LobbyType.SqlLobby);

            // Create room. Once the room is created the PUN function OnJoinedRoom() is called.
            PhotonNetwork.JoinOrCreateRoom("DefaultRoom", roomOptions, sqlLobby, null);
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");

        //PhotonNetwork.LoadLevel("InformationSpace1");
        SetupUsersWithPhoton();
        connectionMenu.SetActive(false);
    }

    private void SetupUsersWithPhoton()
    {
        Instance = this;
        if (PlayerMovementManager.localPlayerInstance == null)
        {
            float randomX = Random.Range(-20.0f, -5.0f);
            float randomZ = Random.Range(-20.0f, -38.0f);
            Vector3 spawnPos = new Vector3(randomX, 2f, randomZ);

            Debug.Log("Init Desktop user");
            // Check if HMD is connected
            if (!XRDevice.isPresent) // no HMD -> create Desktop user
            {
                
                myPlayerGO = (GameObject)PhotonNetwork.Instantiate(this.desktopUserPrefab.name, spawnPos, Quaternion.identity, 0);
                if (!myPlayerGO.GetPhotonView().IsMine)
                {
                    return;
                }
                else
                {
                    // Turn on scripts if player GameObject is controlled by this instance.
                    myPlayerGO.GetComponent<MouseCameraLook>().enabled = true;
                    myPlayerGO.transform.Find("Camera").GetComponent<Camera>().enabled = true;
                    myPlayerGO.GetComponent<PlayerMovementManager>().enabled = true;
                    myPlayerGO.GetComponent<LookSelection>().enabled = true;
                    myPlayerGO.GetComponent<PeripheralPresenceScript>().enabled = true;
                    myPlayerGO.transform.Find("Camera").Find("Canvas").Find("Panel").GetComponent<Image>().enabled = true;
                    myPlayerGO.transform.Find("Camera").Find("Canvas").Find("Panel").Find("GroupLabel").GetComponent<Text>().enabled = true;
                    myPlayerGO.transform.GetChild(0).gameObject.layer = 22;
                    myPlayerGO.transform.GetChild(0).GetChild(0).gameObject.layer = 22;
                    myPlayerGO.transform.GetChild(0).GetChild(1).gameObject.layer = 22;
                    myPlayerGO.transform.GetChild(0).GetChild(2).gameObject.layer = 22;
                    myPlayerGO.transform.GetChild(0).GetChild(3).gameObject.layer = 22;
                    myPlayerGO.GetComponent<Grouping_R>().enabled = true;
                    myPlayerGO.transform.Find("Label").GetComponent<TextMesh>().text = PhotonNetwork.LocalPlayer.NickName;
                    // Disable Scene Camera
                    standbyCamera.GetComponent<Camera>().enabled = false;
                }
            }
            else // HMD -> create VR user
            {
                myPlayerGO = (GameObject)PhotonNetwork.Instantiate(this.hmdUserPrefab.name, spawnPos, Quaternion.identity, 0);
                if (!myPlayerGO.GetPhotonView().IsMine)
                {
                    return;
                }
                else
                {
                    // Setup all VR scripts
                    myPlayerGO.GetComponent<PlayerMovementManagerHMD>().enabled = true;
                    myPlayerGO.GetComponent<PeripheralPresenceScript>().enabled = true;
                    myPlayerGO.GetComponent<Grouping_R>().enabled = true;
                    myPlayerGO.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.layer = 22;
                    myPlayerGO.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject.layer = 22;
                    myPlayerGO.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).gameObject.layer = 22;
                    myPlayerGO.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).gameObject.layer = 22;
                    myPlayerGO.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(3).gameObject.layer = 22;
                    myPlayerGO.transform.Find("Label").GetComponent<TextMesh>().text = PhotonNetwork.LocalPlayer.NickName;
                    standbyCamera.GetComponent<Camera>().enabled = false;
                }
            }
        }
        else
        {
            Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
        }
        connectionMenu.SetActive(false);
    }

    // This function starts an Offline application if not network is available.
    IEnumerator SwitchToOffline(float time)
    {
        yield return new WaitForSeconds(time);

        // Code to execute after the delay
        Debug.Log("Check if network is online.");
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.OfflineMode = true;
            Debug.Log("Switching to OFFLINE MODE");
            SetupUsersWithPhoton();
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }


    public void QuitRoom()
    {
        Application.Quit();
    }


    public override void OnLeftRoom()
    {
        //SceneManager.LoadScene("Lobby");
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("User ", other.NickName, " entered the Room."); // not seen if you're the player connecting
  
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("User ", other.NickName, " left the Room."); // seen when other disconnects
    }
    public Dictionary<int, GameObject> getOtherPlayers()
    {
        return otherPlayers;
    }
    private void updateOtherPlayers()
    {
        if (otherPlayers.Count != (PhotonNetwork.PlayerList.Length - 1))
        {
            List<GameObject> temp = GameObject.FindGameObjectsWithTag("Player").ToList();
            otherPlayers = new Dictionary<int, GameObject>();

            foreach (GameObject player in temp)
            {
                PhotonView view = player.GetPhotonView();
                if (view.Owner.ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
                {
                    otherPlayers.Add(view.Owner.ActorNumber, player);
                }
            }
        }
    }

}
