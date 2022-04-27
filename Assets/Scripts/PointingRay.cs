using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using UnityEditor;

public class PointingRay : MonoBehaviourPunCallbacks, IPunObservable
{

    public LineRenderer lRenderer;
    private bool isActive = false;
    private bool isOwned = false;
    public bool lookcheck = false;
    int layerMask = 1 << 8; // Layer 8 = Interactive
    GameObject selectedObject;
    GameObject selecObjPan;
    GameObject hitObj;

    RaycastHit hit;
    Interactive script;
    Camera cam;
    PanelManager panelScript;

    // Start is called before the first frame update
    void Start()
    {
        //lRenderer = transform.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRay();
        look();
    }

    public void look()
    {
        Vector3 playerposition = transform.position;
        Vector3 forwardDirection = transform.forward;
        Ray lookray = new Ray(playerposition, forwardDirection);
        RaycastHit LookObj;
        float lookDist = 15.0f;

        bool hitfound = Physics.Raycast(lookray,out LookObj, lookDist);

        if (hitfound)
        {   
            hitObj = LookObj.transform.gameObject;
            if (hitObj.tag == "InteractivePanel")
            {
                lookcheck = true;
                //Debug.Log("This is a Panel");
            }
            else
            {
                lookcheck = false;
            }
            //Debug.Log("Looking at " + hitObj.name);
        }
        
    }

    public void UpdateRay()
    {
        if (isActive)
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
            Vector3 end = transform.position + transform.TransformDirection(Vector3.forward) * 10;

            Debug.DrawRay(transform.position, forward, Color.green);

            lRenderer.SetPosition(0, transform.position);
            lRenderer.SetPosition(1, end);

            Color color = this.GetComponentInParent<Grouping_R>().getColorsDict()[this.GetComponentInParent<Grouping_R>().group];
            lRenderer.startColor = color;
            lRenderer.endColor = color;

            CheckClick();
        }
    }

    public void CheckClick()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            script = hit.transform.gameObject.GetComponent<Interactive>();
            if (isOwned)
            {
                if (Input.GetMouseButtonUp(0) || OVRInput.Get(OVRInput.Button.Two))
                {

                    Panel();
                    lRenderer.SetPosition(0, transform.position);
                    lRenderer.SetPosition(1, hit.point);
                }
            }
        }
    }
    [PunRPC]
    public void PanelRPC(string name)
    {
        //print(name);
        PanelAdd(name);
        //PanelAdd(name);
    }

    public void PanelAdd(string name)
    {
        //print(name);
        print(gameObject.GetComponentInParent<Grouping_R>().group);
        selectedObject = GameObject.Find(name);
        selecObjPan = selectedObject.transform.Find(selectedObject.name + " Panel").gameObject;
        selecObjPan.layer = LayerMask.NameToLayer(gameObject.GetComponentInParent<Grouping_R>().group);
        //print(selecObjPan.name);
        panelScript = selecObjPan.GetComponent<PanelManager>();
        if (selecObjPan.activeSelf == true) //checks if object has panel open
        {
            //panelScript.Toggle(false);
            panelScript.switchon(false);
        }
        else
        {
            panelScript.switchon(true);
            //panelScript.Toggle(true);
            panelScript.SetUserCamera(cam);
            panelScript.RotateToUser();
        }
    }
    public void Panel()
    {
        if (script != null)
        {
            selectedObject = hit.transform.gameObject; //Selected object
            cam = this.transform.parent.GetComponent<Camera>();
            photonView.RPC("PanelRPC", RpcTarget.All, selectedObject.name);

        }
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
 
        if (stream.IsWriting)
        {
                stream.SendNext(isActive);
        }
        else
        {
            isActive = (bool)stream.ReceiveNext();
            lRenderer.enabled = isActive;

        }
    }

    public void SetOwned(bool flag)
    {
        isOwned = flag;
    }

    public void Toggle()
    {
        isActive = !isActive;
        lRenderer.enabled = isActive;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.DrawRay(collision.contacts[0].point, collision.contacts[0].normal, Color.green, 2, false);
    }
}
