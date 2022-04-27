using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class PeripheralPresenceScript : MonoBehaviour
{
    public GameObject circle;
    public GameObject canvas;
    public PhotonView photonView;
    public Transform camera;
    public GameObject ellipse;
    public GameObject text;
    public GameObject blackout;

    private Dictionary<int, GameObject> circles = new Dictionary<int, GameObject>(); // <key, circle>
    private Vector3 scaleHMD =  new Vector3(0.1f,0.1f,0.1f);
    private Vector3 scaleDesktop = new Vector3(0.2f, 0.2f, 0.2f);
    private Vector3 usedScale;

    private Boolean active = true;
    private Dictionary<String, Color> colorsDict;
    private int status = 0;
  

    // Start is called before the first frame update
    void Start()
    {
        colorsDict = this.GetComponent<Grouping_R>().getColorsDict();
        if (!XRDevice.isPresent)
        {
            usedScale = scaleDesktop;
        }
        else
        {
            usedScale = scaleHMD;
        }
    }

    void switchPresenceIndication()
    {
        active = !active;
        if (photonView.IsMine)
        {
            canvas.SetActive(active);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown("e") || OVRInput.GetDown(OVRInput.Button.Two))
        {
            switchPresenceIndication();
        }*/

        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick))
        {
            status = (status + 1) % 4;
            switch (status)
            {
                case 0:
                    text.GetComponent<Text>().text = "";
                    ellipse.SetActive(false);
                    text.SetActive(false);
                    break;
                case 1:
                    ellipse.SetActive(true);
                    text.SetActive(true);
                    text.GetComponent<Text>().text = "Eye Distance";
                    break;
                case 2:
                    text.GetComponent<Text>().text = "Ellipse Form";
                    break;
                case 3:
                    text.GetComponent<Text>().text = "Circle Form";
                    break;
            }
        }

        if (status != 0)
        {
            adjustPresenceIndication();
        }
        updatePresenceIndication();

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            blackout.SetActive(!blackout.activeSelf);
        }
    }

    void adjustPresenceIndication()
    {
        float x = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x;
        float y = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y;

        RectTransform ellipseTrans = ellipse.GetComponent<RectTransform>();
        Vector3 ellipsePos = ellipseTrans.localPosition;

        switch (status)
        {
            case 1:     // Distance to Eye
                ellipseTrans.localPosition = new Vector3(ellipsePos.x, ellipsePos.y, ellipsePos.z + y);
                break;
            case 2:     // Ellipse Form
                ellipseTrans.localScale = new Vector2(ellipseTrans.localScale.x + (x * 0.05f), ellipseTrans.localScale.y + (y * 0.05f));
                break;
            case 3:
                scaleHMD = new Vector2(usedScale.x + (x * 0.0001f), usedScale.y + (y * 0.0001f));
                usedScale = scaleHMD;
                break;
        }
    }

   void clearAllCircles()
    {
        foreach (KeyValuePair<int,GameObject> activeCircle in circles)
        {
            GameObject.Destroy(activeCircle.Value);
        }
        circles.Clear();
    }

    void updatePresenceIndication()
    {
        Dictionary<int, GameObject> otherPlayers = GameObject.Find("SpaceLogic").GetComponent<SpaceLogic>().getOtherPlayers();

        // Check if players have left -> destroy their circles
        int index = 0;
        while (circles.Count > otherPlayers.Count)
        {
            int key = circles.ElementAt(index).Key;
            if (!otherPlayers.ContainsKey(key))
            {
                GameObject oldCircle = circles[key];
                GameObject.Destroy(oldCircle);
                circles.Remove(key);
            }
            index++;
        }

        foreach (int key in otherPlayers.Keys)
        {
            GameObject otherPlayer = otherPlayers[key];
            // Check if new player have joined -> add new circles
            if (!circles.ContainsKey(key))
            {
                GameObject newCircle = GameObject.Instantiate(circle, canvas.transform);
                circles.Add(key, newCircle);
            }

            updateCircleSize(key, otherPlayer.transform.position);
            updateCircleColor(key, otherPlayer.GetComponent<Grouping_R>().group);
            if (!XRDevice.isPresent)
            {
                updateCirclePosition(key, otherPlayer.transform.position);
            }
            else
            {
                updateCirclePositionHMD(key, otherPlayer.transform.position);
            }
        }
    }
    void updateCircleSize(int key, Vector3 otherPlayerPos)
    {
        float dist = getDistance(otherPlayerPos);
        if (dist > 8)
        {
            circles[key].SetActive(false);
        }
        else
        {
            circles[key].SetActive(true);
            if (dist > 4)
            {
                circles[key].GetComponent<Mask>().enabled = false;
                circles[key].GetComponent<RectTransform>().localScale = usedScale;
            }
            else
            {
                if (!XRDevice.isPresent)
                {
                    circles[key].GetComponent<Mask>().enabled = true;
                    circles[key].GetComponent<RectTransform>().localScale = usedScale * (1 + (4 - dist) * 1.5f);
                }
            }
        }
    }

    void updateCirclePosition(int key, Vector3 otherPos)
    {
        float h = canvas.GetComponent<RectTransform>().rect.height;// Screen.height;
        float w = canvas.GetComponent<RectTransform>().rect.width;// Screen.height;
        float canvasX = 0;
        float canvasY = 0;
        int fov = 100;                  // FoV of the camera
        int sideRange = (180 - fov) / 2;// degrees for left/right area
        int rightBorder = fov / 2;      // Most right point in FoV (in degree)
        int leftBorder = -(fov / 2);    // Most left point in FoV (in degree)
        float angle = getAngle(otherPos);

        if (90 > angle && angle > rightBorder)                  //right
        {
            canvasX = 1;
            canvasY = (90 - angle) / sideRange;
        }
        else if (rightBorder > angle && angle > leftBorder)     // in front
        {
            canvasX = (angle + rightBorder) / fov;
            canvasY = 1;
        }
        else if (leftBorder > angle && angle > -90)              // left
        {
            canvasY = (90 - (-angle)) / sideRange;
        }
        else                                                   // behind
        {
            if (angle >= 90)                             // right behind
            {
                canvasX = (-angle + 270) / 180;
            }
            else                                        // left behind
            {
                canvasX = (-angle - 90) / 180;
            }
        }

        Vector2 circlePos = new Vector2(canvasX * w, canvasY * h);
        circles[key].GetComponent<RectTransform>().anchoredPosition = circlePos;
    }

    /* Alternative Mapping for Desktop. Instead of mapping FoV to upper border, this
     * approach just uses the relative position to the viewer.*/
    void updateCirclePositionAlternative(int key, Vector3 otherPos)
    {
        // Geradegleichung: y = mx
        // Horizontalengleichung: y = t (t=Schnittpunkt mit y-Achse)
        // Vertikalengleichung: x = c ( (c=Schnittpunkt mit x-Achse))
        // => y = m*c && x = x = y/c
        float t = 0.5f * canvas.GetComponent<RectTransform>().rect.height;
        float c = 0.5f * canvas.GetComponent<RectTransform>().rect.width;
        Vector3 otherLocalPos = camera.InverseTransformPoint(otherPos);
        float m = otherLocalPos.z / otherLocalPos.x;
        float x;
        float y;
        if (Math.Abs(otherLocalPos.z) >= Math.Abs(otherLocalPos.x))
        {
            if(otherLocalPos.z >= 0)    // Front
            {
                y = t;
            }
            else                         // Back
            {
                y = -t;                
            }
            x = y / m;
        }
        else
        {
            if (otherLocalPos.x >= 0)   // Right
            {
                x = c;
            }
            else                        // Left
            {
                x = -c;
            }
            y = m * x;
        }
        Vector3 circlePos = new Vector3(x, y, 0);
        circles[key].GetComponent<RectTransform>().localPosition = circlePos;
    }

    void updateCirclePositionHMD(int key, Vector3 otherPos)
    {
        // Geradegleichung:  y = mx
        // Ellipsengleichung: (x^2)/(a^2) + (y^2)/(b^2) = 1
        // => x^2 = ((a^2)*(b^2))/((b^2)+((m^2)*(a^2)))
        Vector3 otherLocalPos = camera.InverseTransformPoint(otherPos);
        float m = otherLocalPos.z / otherLocalPos.x;
        float a = 50 * ellipse.GetComponent<RectTransform>().localScale.x;
        float b = 50 * ellipse.GetComponent<RectTransform>().localScale.y;
        float x1 = (float)Math.Sqrt((Math.Pow(a, 2) * Math.Pow(b, 2)) / (Math.Pow(b, 2) + (Math.Pow(m, 2) * Math.Pow(a, 2))));
        float x2 = -x1;
        float y1 = m * x1;
        float y2 = m * x2;
        Vector3 circlePos = new Vector3(x1, y1, ellipse.GetComponent<RectTransform>().localPosition.z);

        if (otherLocalPos.x < 0)
        {
            circlePos.x = x2;
            circlePos.y = y2;
        }
        circles[key].GetComponent<RectTransform>().localPosition = circlePos;
        float angle = Vector3.Angle(Vector3.left, new Vector3(circlePos.x,0,circlePos.y));
        if (circlePos.y > 0)
        {
            angle *= -1;
        }
        circles[key].GetComponent<RectTransform>().localRotation = Quaternion.Euler(0.0f,0.0f,angle);
    }

    void updateCircleColor(int key, String color)
    {
        if (colorsDict.ContainsKey(color))
        {
            circles[key].transform.GetChild(0).GetChild(0).GetComponent<Image>().color = colorsDict[color];
        }
    }

    float getDistance(Vector3 otherPlayerPos)
    {
        return Vector3.Distance(camera.position, otherPlayerPos); ;
    }

    float getAngle(Vector3 otherPlayerPos)
    {
        Vector3 otherPlayerLocalPos = camera.InverseTransformPoint(otherPlayerPos);
        otherPlayerLocalPos.y = 0;
        float angle = Vector3.Angle(Vector3.forward, otherPlayerLocalPos);
        if (otherPlayerLocalPos.x < 0)
        {
            angle *= -1;
        }

        return angle;
    }
}
