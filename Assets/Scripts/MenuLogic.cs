using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class MenuLogic : MonoBehaviourPunCallbacks
{
    string _gameVersion = "1";
    public string lobbyName = "immersive";

    [Space(5)]
    public GameObject mainMenu;

    public GameObject createMenu;

    public GameObject joinMenu;

    public List<RoomInfo> roomList;

    [Space(5)]
    public Text userStatus;

    public Text connectionStatus;

    public UnityEngine.UI.VerticalLayoutGroup _roomListView;

    public GameObject _roomListEntryPrefab;

    private string _userName = "";
    public string UserName
    {
        get { return _userName; }
        set { _userName = value; }
    }
    private string _roomName = "";
    public string RoomName
    {
        get { return _roomName; }
        set
        {
            foreach (Transform entry in _roomListView.transform)
            {
                _roomName = value;
                //if (_roomName == entry.GetComponent<LobbyRoomButtonHandler>().ButtonRoomName)
                //{
                //    entry.GetComponent<Image>().color = new Vector4(1, 1, 1, 1);
                //}
                //else
               // {
                 //   entry.GetComponent<Image>().color = new Vector4(0.8f, 0.8f, 0.8f, 1);
               // }
            }

            UpdateStartButtonColor();
        }
    }

    void Awake()
    {
        // make sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        userStatus.text = "Status: No Space";
        connectionStatus.text = "Connection: No Connection";
        ConnectToPhoton();
        
    }

    void Update()
    {
        
    }

    void ConnectToPhoton()
    {
        //_connectionStatus.text = "Connection: Connecting...";
        PhotonNetwork.GameVersion = _gameVersion;
        Debug.Log("PhotonNetwork.IsConnected! | Trying to join room " + _roomName);
        PhotonNetwork.ConnectUsingSettings();
    }

    public void JoinRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.LocalPlayer.NickName = _userName;
            PhotonNetwork.JoinRoom(_roomName);
        }
    }

    public void CreateRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.LocalPlayer.NickName = _userName;
            Debug.Log("PhotonNetwork.IsConnected! | Trying to create room " + _roomName);
            RoomOptions roomOptions = new RoomOptions();
            TypedLobby sqlLobby = new TypedLobby(lobbyName, LobbyType.SqlLobby);
            bool _result = PhotonNetwork.CreateRoom(_roomName, roomOptions, sqlLobby, null);
            Debug.Log(_result + "created room");
        }
    }

    public void UpdateStartCreateButton()
    {
        InputField input = createMenu.transform.Find("Space Name Input").GetComponent<InputField>();
        _roomName = input.text;
        UpdateStartButtonColor();
    }
    public void UpdateUserName()
    {
        InputField input = mainMenu.transform.Find("User Name Input").GetComponent<InputField>();
        _userName = input.text;
    }

    public void UpdateStartButtonColor()
    {
        if (_roomName == "" || _userName == "")
        {
            Debug.Log( _userName + " red / " + _roomName);
            createMenu.transform.Find("Start Button").GetComponent<Image>().color = new Vector4(1.0f, 0.75f, 0.64f, 1.0f);
        }
        else
        {
            Debug.Log(_userName + " green/ " + _roomName);
            createMenu.transform.Find("Start Button").GetComponent<Image>().color = new Vector4(0.65f, 1.0f, 0.65f, 1.0f);
        }
    }

    // Photon Methods
    public override void OnConnected()
    {
        base.OnConnected();
        //_connectionStatus.text = "Connection: <Color=Green><a>Connected</a></Color>";
        //_connectionStatus.color = Color.green;
        Debug.Log("connected");
    }

    public override void OnJoinedLobby()
    {
        connectionStatus.text = "Connection: <Color=Green><a>In Lobby</a></Color>";
        Debug.Log("joined lobby");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        TypedLobby sqlLobby = new TypedLobby(lobbyName, LobbyType.SqlLobby);
        PhotonNetwork.JoinLobby(sqlLobby);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("TESST JOINED RRROm");
        if (PhotonNetwork.IsMasterClient)
        {
            userStatus.text = "Status: Host";
        }
        else
        {
            userStatus.text = "Status: Client";
        }
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("TESST Created RRROm");
        base.OnCreatedRoom();
        createMenu.SetActive(false);
        mainMenu.transform.Find("Enter Button").gameObject.SetActive(true);
        //LoadSpace();
    }

    public override void OnRoomListUpdate(List<RoomInfo> inRoomList)
    {
        Debug.Log("We have received the Room list with size " + roomList.Count);
        this.roomList = inRoomList;
        RectTransform parent = _roomListView.GetComponent<RectTransform>();
        Debug.Log(_roomListView.transform.childCount.ToString() + " and roomList " + roomList.Count.ToString());
        
        // remove old room entries from roomListView
        /*for (int i = 0; i < _roomListView.transform.childCount; i++)
        {
            int idx = _roomList.FindIndex(item => item.Name == _roomListView.transform.GetChild(i).name);
            if (idx >= 0) {
                Debug.Log("Space " + _roomListView.transform.GetChild(i).name + " still exists.");
            } else
            {
            if (_roomListView.transform.GetChild(i).GetComponent<RoomButtonHandler>().isSelected)
            {
                _roomName = "";
            }
                Destroy(_roomListView.transform.GetChild(i));
            }
        }*/

        // instantiate room entries
        for (int i = 0; i < roomList.Count; i++)
        {
            Debug.Log(i.ToString() + " " + roomList[i].Name);
            GameObject entry = Instantiate(_roomListEntryPrefab);
            entry.GetComponent<Image>().color = new Vector4(0.2f, 0.2f, 0.2f, 1);
            Button roomListEntryButton = entry.GetComponent<Button>();
            RoomButtonHandler rbh = entry.GetComponent<RoomButtonHandler>();

            Text roomButtonText = entry.transform.Find("Text").GetComponent<Text>();
            roomButtonText.text = roomList[i].Name;
            entry.transform.SetParent(parent);
            roomListEntryButton.onClick.AddListener(delegate { ClickedRoomButton(roomButtonText.text); });
        }
    }

    private void ClickedRoomButton(string message)
    {
        _roomName = message;
        Debug.Log("Room " + _roomName + "clicked");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError("Disconnected. Please check your Internet connection.");
    }

    public void RefreshRoomListings()
    {
    }

    public void EnableJoinMenu()
    {
        joinMenu.SetActive(true);
        UpdateStartButtonColor();
    }

    public void DisableJoinMenu()
    {
        joinMenu.SetActive(false);
    }

    public void EnableCreateMenu()
    {
        createMenu.SetActive(true);
        UpdateStartButtonColor();
    }

    public void DisableCreateMenu()
    {
        createMenu.SetActive(false);
    }

    public void LoadSpace()
    {
        if (_roomName != "")
        {
            Debug.Log("Load Information Space");
            PhotonNetwork.LoadLevel("InformationSpace1");
        }
    }
}
