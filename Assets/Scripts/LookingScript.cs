using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        look();
    }

    void look()
    {
        Vector3 playerposition = transform.position;
        Vector3 forwardDirection = transform.forward;
        Ray lookray = new Ray(playerposition, forwardDirection);
        RaycastHit LookObj;
        float lookDist = 5.0f;

        bool hitfound = Physics.Raycast(lookray, out LookObj, lookDist);

        if (hitfound)
        {
            GameObject hitObj = LookObj.transform.gameObject;
            Debug.Log("Looking at " + hitObj.name);
        }
    }
}
