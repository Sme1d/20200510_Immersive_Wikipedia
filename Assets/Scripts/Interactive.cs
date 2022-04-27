using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

public class Interactive : MonoBehaviourPunCallbacks
{
    public GameObject Panel;
    public string objectName;
    public string objectInformation;
    private static string panelLocation;
    public bool panelIsActive = false;
    // Start is called before the first frame update
    void Awake()
    {
        //Text text = Panel.GetComponent<Transform>().Find("Title").GetComponent<InputField>().GetComponent<Transform>().Find("Title").GetComponent<Text>();
        //text.text = objectName;

        //panelLocation = "Assets/Prefab/Panel";
        //Panel = Resources.Load(panelLocation) as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreatePanel(GameObject obj, Camera cam)
    {
        //GameObject Panel = PhotonNetwork.Instantiate("Panel", obj.transform.position + new Vector3(0, 4, 0), Quaternion.Euler(0.0f, cam.transform.rotation.eulerAngles.y, 0.0f));
        
        //Debug.Log("PARENT: " + selectedObject.name);
        //this.name = obj.name;
        //        //selecObjPan.transform.SetParent(selectedObject.transform, true);
        //        //selecObjPan.transform.localScale = new Vector3(selecObjPan.transform.localScale.x / selectedObject.transform.lossyScale.x, selecObjPan.transform.localScale.y / selectedObject.transform.lossyScale.y, selecObjPan.transform.localScale.z / selectedObject.transform.lossyScale.z);
        //panelScript.Toggle(true);
        //panelScript.SetUserCamera(cam);
        //panelScript.RotateToUser();
    }
}
