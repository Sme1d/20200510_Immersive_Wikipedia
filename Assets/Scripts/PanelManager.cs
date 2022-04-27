using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class PanelManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public string objectName;
    public Camera UserCam;
    public Animator menuAnim;
    public bool alwaysRotate = false;
    public bool check = false;
    public Dictionary<Player, GameObject> player_dict = new Dictionary<Player, GameObject>();
    private bool active = false;
    private float rotation =  0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        menuAnim = GetComponent<Animator>();
    }

    void Update()
    {
        

        if (Input.GetKeyUp(KeyCode.P))
        {
            if (alwaysRotate == true)
            {
                alwaysRotate = false;
            }
            else
            {
                alwaysRotate = true;

            }
        }
        if (alwaysRotate)
        {
            UpdatePlayers();
            Camera cam = GameObject.Find("Camera").GetComponent<Camera>();
            SetUserCamera(cam);
            RotateToUser();

        }

    }

    public void switchon(bool checking) //Function to Set "Check"
    {
        if (checking == true)
        {
            this.gameObject.SetActive(true);
            //check = true;
        }
        else
        {
            this.gameObject.SetActive(false);
            //check = false;
        }
        
    }


    public void SetUserCamera(Camera cam) //Sets camera to set panel rotation
    {
       
        UserCam = cam;
    }
    void UpdatePlayers()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players != null && PhotonNetwork.PlayerList != null)
        {
            foreach (GameObject p in players)
            {
                foreach (Player player in PhotonNetwork.PlayerList)
                {
                    if (p.GetPhotonView().Owner == player && !player_dict.ContainsKey(player) && p.GetPhotonView().ViewID / 1000 == player.ActorNumber)
                    {
                        player_dict.Add(player, p);
                    }
                }
            }
        }
    }

    public void RotateToUser() //rotates to user position
    {
        Debug.Log("Rotate to user activated");

        if (UserCam != null)
        {
            var PlayrPos = UserCam.transform.position;
            var i = 1;
            List<GameObject> temp = GameObject.FindGameObjectsWithTag("Player").ToList();


            foreach (GameObject player in temp)
            {
                PhotonView view = player.GetPhotonView();
                var pos = player.transform.position;
                var lookPos = transform.position - pos;
                lookPos.y = 0;
                PointingRay pointScript = player.transform.Find("Camera").gameObject.GetComponentInChildren<PointingRay>();
                
                var targetPosition = pos;
                
                var localTarget = this.gameObject.transform.parent.gameObject.transform.InverseTransformPoint(targetPosition);

                float angle = Mathf.Atan2(-lookPos.x, -lookPos.z) * Mathf.Rad2Deg - 180;

                if (pointScript.lookcheck)
                {
                    rotation = rotation + angle;
                    i++;
                }
                var ang = transform.eulerAngles.y;

            }
            //Debug.Log("Total Rotation : " + rotation + " with number of players :" + temp.Count);
            rotation = rotation / i;
            if (rotation > 360)
            {
                rotation = rotation - 360;
            }
            var damping = 1f;
            
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(rotation, Vector3.up), Time.deltaTime * damping);

        }
        else
        {
            Debug.Log("No Camera Found");
        }
    }
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
        if (stream.IsWriting)
        {
            //stream.SendNext(check);
            stream.SendNext(alwaysRotate);
            //stream.SendNext(gameObject.activeSelf);
            
        }
        else
        {
            
            //check = (bool)stream.ReceiveNext();
            alwaysRotate = (bool)stream.ReceiveNext();
            //this.gameObject.SetActive((bool)stream.ReceiveNext());

        }
    }
}
