using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;


public class LobbyLogic : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private GameObject singleUserMenu;

    [SerializeField]
    private GameObject multiUserMenu;

    [SerializeField]
    private byte maxPlayersPerRoom = 4;

    [SerializeField]
    private List<RoomInfo> roomList;

    string gameVersion = "1";
    bool isConnecting;

    [Space(5)]
    public Text playerStatus;
    public Text connectionStatus;

    public InputField userNameField;
    public InputField roomNameField;
    public GameObject roomListEntryButton;
    public GameObject startButton;

    public UnityEngine.UI.VerticalLayoutGroup listView;

    public string lobbyName = "immersive";

    string playerName = "";
    string roomName = "";
    public string userType = "Desktop";
    private TypedLobby sqlLobby;



    void Awake()
    {
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        lobbyName = "immersive";
        sqlLobby = new TypedLobby(lobbyName, LobbyType.SqlLobby);
        ConnectToPhoton();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Helper Methods
    public void SetPlayerName(string name)
    {
        if (name == "")
        {
            playerName = userNameField.text;
        } else
        {
            playerName = name;
        }
    }

    public void SetRoomName(string name)
    {
        if (name == "")
        {
            roomName = roomNameField.text;
        }
        else
        {
            roomName = name;
        }
    }

    void ConnectToPhoton()
    {
        connectionStatus.text = "Connection Status: Connecting...";
        PhotonNetwork.GameVersion = gameVersion; 
        PhotonNetwork.ConnectUsingSettings(); 
    }

    public void JoinRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.LocalPlayer.NickName = playerName; 
            PhotonNetwork.JoinRoom(roomName); 
        }
    }

    public void CreateRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.LocalPlayer.NickName = playerName; 
            Debug.Log("Creating room " + roomNameField.text);
            RoomOptions roomOptions = new RoomOptions();
            bool _result = PhotonNetwork.CreateRoom(roomName, roomOptions, sqlLobby, null);
        }
    }

    // Photon Methods
    public override void OnConnected()
    {
        base.OnConnected();
        
        connectionStatus.text = "Connection Status: Connected";
        connectionStatus.color = Color.green;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to MasterServer. Joining lobby...");
        PhotonNetwork.JoinLobby(sqlLobby);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("We have received the Room list with size " + roomList.Count);
        //After this callback, update the room list
        this.roomList = roomList;
        Debug.Log("Rooms: " + this.roomList.Count);
        RectTransform parent = listView.GetComponent<RectTransform>();
        for (int i = 0; i < this.roomList.Count; i++)
        {
            
            GameObject roomListEntry = Instantiate(roomListEntryButton);
            Button roomListButton = roomListEntry.GetComponent<Button>();
            RoomButtonHandler rbh= roomListEntry.GetComponent<RoomButtonHandler>();
            rbh.roomNameInput = roomNameField;

            Text roomButtonText = roomListEntry.transform.Find("Text").GetComponent<Text>();
            roomButtonText.text = this.roomList[i].Name;

            roomListEntry.transform.SetParent(parent);
            roomListButton.onClick.AddListener(delegate { ClickedRoomButton(roomButtonText.text); });

            //Instantiate(roomListEntryButton);
        }
        for (int i = 0; i < this.roomList.Count; i++)
        {
           Debug.Log("Room " + i + ": " + roomList[i].Name);
        }
    }

    private void ClickedRoomButton(string message)
    {
        roomNameField.text = message;
        roomName = message;
        Debug.Log("Room " + roomName + "clicked");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        isConnecting = false;
        Debug.LogError("Disconnected. Please check your Internet connection.");
    }

    public void RefreshRoomListings()
    {

    }

    public override void OnJoinedLobby()
    {
        connectionStatus.text = "Connection Status: In Lobby";

        Debug.Log("Joined Lobby " + PhotonNetwork.CurrentLobby.Name.ToString());
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            playerStatus.text = "User Status: Host";
            startButton.SetActive(true);
        }
        else
        {
            playerStatus.text = "User Status: Client";
            startButton.SetActive(false);
        }
        
    }


    public void LoadSimulation()
    {
        playerStatus.text = "User Status: Single User Mode";

        PhotonNetwork.LoadLevel("InformationSpace1");
    }
}
