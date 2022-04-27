using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRTeleportation : MonoBehaviour
{
  public GameObject player;
  public UnityEngine.XR.InputDevice rightController;
  int layerMask = 1 << 9;
  bool triggerPressed = false;
  public LineRenderer lRenderer;

  void Start()
  {
    lRenderer = transform.GetComponent<LineRenderer>();

    var rightHandDevices = new List<UnityEngine.XR.InputDevice>();
    UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.RightHand, rightHandDevices);

    if (rightHandDevices.Count == 1)
    {
      rightController = rightHandDevices[0];
      Debug.Log(string.Format("Device name '{0}' with role '{1}'", rightController.name, rightController.role.ToString()));
    }
    else if (rightHandDevices.Count != 1)
    {
      Debug.Log("Found more than one right hand!");
    }
  }
  

  public void teleport()
  {
    RaycastHit hit;

    if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
    {
      Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 5.0f, Color.yellow);
      lRenderer.enabled = true;
      lRenderer.SetPosition(0, transform.position);
      lRenderer.SetPosition(1, hit.point);

      bool triggerValue;

      if (rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerValue) && triggerValue && !triggerPressed)
      {
        Debug.Log("Trigger button is pressed");

        player.transform.position = hit.point;

        triggerPressed = true;
        Debug.Log("trigger value " + triggerValue);
      }

      else if(rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerValue) && !triggerValue)
      {
        triggerPressed = false;
      }
    }
    else
    {
      lRenderer.enabled = false;
    }
  }

  void Update()
  {
    teleport();
  }
}
