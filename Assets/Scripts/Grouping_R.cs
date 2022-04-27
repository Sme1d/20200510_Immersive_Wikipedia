using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Chat;
using Photon.Pun.UtilityScripts;
using ExitGames.Client.Photon;
using static SpaceLogic;
using UnityEngine.UI;
using System.Linq;
using System;

public class Grouping_R : MonoBehaviourPunCallbacks, IPunObservable
{
    private List<string> groupnames = new List<string>{"Red", "Blue", "Green", "Black", "Yellow", "Orange", "Purple", "Pink", "White", "Brown"};
    private Dictionary<string, Color> groups = new Dictionary<string, Color> { { "Red", Color.red }, { "Blue", Color.blue }, { "Green", Color.green }, {"Black", Color.black},
        {"Yellow", Color.yellow }, {"Orange", new Color(1.0f, 0.64f, 0.0f)}, {"Purple", new Color(0.5f, 0.0f, 0.5f) }, {"Pink", new Color(0.65f, 0.16f, 0.16f) }, 
        {"White", Color.white }, {"Brown", new Color(0.095f, 0.74f, 0.54f) } };
    private bool active = true;
    private bool exit = false;
    //private Dictionary<Player, GameObject> player_dict = new Dictionary<Player, GameObject>();
    private Vector3 prevposition;
    GameObject[] players;
    public string group;
    Vector3 oldraystart;
    Vector3 oldrayend;

    // Start is called before the first frame update
    void Start()
    {
        OnJoinedRoom();
        prevposition = this.gameObject.transform.position;
        oldraystart = this.GetComponentInChildren<PointingRay>().lRenderer.GetPosition(0);
        oldrayend = this.GetComponentInChildren<PointingRay>().lRenderer.GetPosition(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.transform.position != prevposition)
        {
            Group();
        }
        prevposition = this.gameObject.transform.position;
        addLines();
    }
    void UpdatePlayers ()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    bool Crossing(GameObject other)
    {
        LineRenderer myray = this.GetComponentInChildren<PointingRay>().lRenderer;
        LineRenderer otherray;
        if (other.GetComponentInChildren<PointingRay>() == null) 
        {
            otherray = other.GetComponent<LineRenderer>(); 
        }
        else if (other.transform.Find("Camera") != null) {
            otherray = other.transform.Find("Camera").GetComponentInChildren<PointingRay>().lRenderer; 
        }
        else
        {
            otherray = other.transform.Find("Offset").Find("RightHand").GetComponentInChildren<PointingRay>().lRenderer;
        }
        Vector3 start =  myray.GetPosition(0);
        Vector3 end = myray.GetPosition(1);
        List<Vector3> Triangle1 = new List<Vector3> { start, end, oldraystart };
        List<Vector3> Triangle2 = new List<Vector3> { oldraystart, oldrayend, end };
        Ray ray = new Ray { };
        ray.origin = otherray.GetPosition(0);
        ray.direction = otherray.GetPosition(1) - otherray.GetPosition(0);
        oldraystart = start;
        oldrayend = end;
        return Intersect(Triangle1[0], Triangle1[1], Triangle1[2], ray) || Intersect(Triangle2[0], Triangle2[1], Triangle2[2], ray);
    }

    void Group()
    {
        UpdatePlayers();
        GameObject ownPlayer = this.gameObject;
        Vector3 ownPlayerPos = ownPlayer.transform.position;

        if (this.transform.Find("Label") != null)
        {
            this.transform.Find("Label").GetComponent<TextMesh>().text = this.GetComponent<PhotonView>().Owner.NickName;
        }

        foreach (GameObject player in players)
        {
            if (player != ownPlayer)
            {
                Vector3 otherPlayerPos = player.transform.position;
                if (this.GetComponentInChildren<PointingRay>().lRenderer.enabled == true && player.GetComponentInChildren<PointingRay>().lRenderer.enabled == true && Vector3.Distance(ownPlayerPos, otherPlayerPos) <= 5)
                {
                    active = Crossing(player);
                }
                else
                {
                    active = false;
                }
                if (active)
                {
                    string othergroup = player.GetComponent<Grouping_R>().group;
                    if (othergroup != group)
                    {
                        group = othergroup;
                    }
                }
                if (this.GetComponentInChildren<PointingRay>().lRenderer.enabled == true && this.transform.Find("Line " + player.GetComponent<PhotonView>().ViewID) != null)
                {
                    exit = Crossing(this.transform.Find("Line " + player.GetComponent<PhotonView>().ViewID).gameObject);                    
                }
                else
                {
                    exit = false;
                }
                if (exit)
                {
                    string othergroup = player.GetComponent<Grouping_R>().group;
                    if (othergroup == group)
                    {
                        group = getgroup();
                    }
                }
            }
        }
        if (this.transform.Find("Camera") != null)
        {
            this.transform.Find("Camera").Find("Canvas").Find("Panel").Find("GroupLabel").GetComponent<Text>().text = "Group: " + this.GetComponent<Grouping_R>().group;
        }
        HighlightGroup();
        viewPanels();
    }

    void HighlightGroup()
    {
        GameObject ownPlayer = this.gameObject;
        
        if (group != "")
        {
            if (ownPlayer.transform.GetChild(0).name == "Offset")
            {
                addShader(ownPlayer.transform.GetChild(0).GetChild(0).GetChild(0).gameObject, ownPlayer, 0.3f);
                addShader(ownPlayer.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject, ownPlayer);
                addShader(ownPlayer.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).gameObject, ownPlayer);
                addShader(ownPlayer.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).gameObject, ownPlayer);
                addShader(ownPlayer.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(3).gameObject, ownPlayer, 0.3f);
            }
            else
            {
                addShader(ownPlayer.transform.GetChild(0).gameObject, ownPlayer, 0.3f);
                addShader(ownPlayer.transform.GetChild(0).GetChild(0).gameObject, ownPlayer);
                addShader(ownPlayer.transform.GetChild(0).GetChild(1).gameObject, ownPlayer);
                addShader(ownPlayer.transform.GetChild(0).GetChild(2).gameObject, ownPlayer);
                addShader(ownPlayer.transform.GetChild(0).GetChild(3).gameObject, ownPlayer, 0.3f);
            }
        }
    }

    string getgroup()
    {
        string availablegroup = groupnames[groupnames.Count - 1];
        bool available;
        foreach (string g in groupnames)
        {
            available = true;
            foreach (GameObject player in players)        
            {
                if (player != this.gameObject)
                {
                    if (player.GetComponent<Grouping_R>().group == g)
                    {
                        available = false;
                    }
                }
            }
            if (available)
            {
                return g;
            }
        }
        return availablegroup;
    }

    void addShader(GameObject p, GameObject player, float outline = 0.03f)
    {
        MeshRenderer renderer = p.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            Material m = new Material(Shader.Find("Outlined/Highlight"));
            m.SetFloat("_Outline", outline);
            m.SetColor("_OutlineColor", groups[player.GetComponent<Grouping_R>().group]);
            Material[] materials = new Material[] { renderer.materials[0], m };
            renderer.materials = materials;
        }
    }

    void addLines()
    {
        foreach (GameObject player in players)
        {
            if (player != this.gameObject)
            {
                string other = player.GetComponent<Grouping_R>().group;
                string linename = "Line " + player.GetComponent<PhotonView>().ViewID;
                if (other == group)
                {
                    GameObject myLine;
                    LineRenderer lr;
                    Color color = groups[group];
                    Vector3 start = this.transform.position;
                    Vector3 end = player.transform.position;
                    if ((this.transform.Find(linename) == null && player.transform.Find(linename) == null))
                    {
                        myLine = new GameObject(linename);
                        myLine.transform.parent = this.transform;
                        myLine.AddComponent<LineRenderer>();

                    }
                    else if (player.transform.Find(linename) != null && this.transform.Find(linename) == null)
                    {
                        myLine = player.transform.Find(linename).gameObject;
                    }
                    else
                    {
                        myLine = this.transform.Find(linename).gameObject;
                    }
                    myLine.transform.position = start;
                    myLine.layer = LayerMask.NameToLayer(group);
                    lr = myLine.GetComponent<LineRenderer>();
                    lr.material = new Material(Shader.Find("Sprites/RayShader"));
                    lr.startColor = color;
                    lr.endColor = color;
                    lr.startWidth = 0.1f;
                    lr.endWidth = 0.1f;
                    lr.SetPosition(0, new Vector3(start.x,0.4f,start.z));
                    lr.SetPosition(1, new Vector3(end.x,0.4f,end.z));
                }
                else
                {
                    if (this.transform.Find(linename) != null)
                    {
                        GameObject.Destroy(this.transform.Find(linename).gameObject);
                    }
                    if (player.transform.Find(linename) != null)
                    {
                        GameObject.Destroy(player.transform.Find(linename).gameObject);
                    }
                }
            }
        }
    }

    void viewPanels()
    {
        GameObject ownPlayer = this.gameObject;
        Camera cam;
        if (ownPlayer.transform.GetChild(0).gameObject.name == "Offset")
        {
            cam = ownPlayer.transform.GetChild(0).gameObject.GetComponentInChildren<Camera>();
        }
        else
        {
            cam = ownPlayer.GetComponentInChildren<Camera>();
        }
        foreach (string g in groupnames)
        {
            if (g != group)
            {
                cam.cullingMask &= ~(1 << LayerMask.NameToLayer(g));
            }
            else
            {
                cam.cullingMask |= 1 << LayerMask.NameToLayer(g);
            }
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer,changedProps);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        UpdatePlayers();
        group = getgroup();
        Group();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Group();
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Group();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Group();
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

    public Dictionary<string, Color> getColorsDict()
    {
        return groups;
    }

    /// Möller–Trumbore ray-triangle intersection algorithm implementation.
    public static bool Intersect(Vector3 p1, Vector3 p2, Vector3 p3, Ray ray)
    {
        // Vectors from p1 to p2/p3 (edges)
        Vector3 e1, e2;

        Vector3 p, q, t;
        float det, invDet, u, v;


        //Find vectors for two edges sharing vertex/point p1
        e1 = p2 - p1;
        e2 = p3 - p1;

        // calculating determinant 
        p = Vector3.Cross(ray.direction, e2);

        //Calculate determinat
        det = Vector3.Dot(e1, p);

        //if determinant is near zero, ray lies in plane of triangle otherwise not
        if (det > -Mathf.Epsilon && det < Mathf.Epsilon) { return false; }
        invDet = 1.0f / det;

        //calculate distance from p1 to ray origin
        t = ray.origin - p1;

        //Calculate u parameter
        u = Vector3.Dot(t, p) * invDet;

        //Check for ray hit
        if (u < 0 || u > 1) { return false; }

        //Prepare to test v parameter
        q = Vector3.Cross(t, e1);

        //Calculate v parameter
        v = Vector3.Dot(ray.direction, q) * invDet;

        //Check for ray hit
        if (v < 0 || u + v > 1) { return false; }

        if ((Vector3.Dot(e2, q) * invDet) > Mathf.Epsilon)
        {
            //ray does intersect
            return true;
        }

        // No hit at all
        return false;
    }
}