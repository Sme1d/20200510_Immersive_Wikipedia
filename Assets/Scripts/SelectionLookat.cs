using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionLookat : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] GameObject _textmesh;


    RaycastHit hit1;
    RaycastHit hit2;
    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<PlayerMovementManager>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        #region DEBUGS
        Debug.DrawRay(transform.position, transform.right * 50, Color.blue);
        Debug.DrawRay(transform.position, -transform.right * 50, Color.green);
        #endregion

        transform.LookAt(target.transform);

        if (Physics.Raycast(transform.position, transform.right * 500, out hit1)) {
            if (hit1.transform.tag == "Cone") {
                Debug.LogError("Casted on right : " +hit1.distance);
            }
        }
        if (Physics.Raycast(transform.position, -transform.right * 500, out hit2))
        {
            if (hit2.transform.tag == "Cone")
            {
                Debug.LogError("Casted on left : " +hit2.distance);
            }
        }


        if (hit1.distance < hit2.distance) {
            Instantiate(_textmesh, hit1.transform.position, Quaternion.identity);
        }
        else  if(hit1.distance > hit2.distance)
            Instantiate(_textmesh, hit2.transform.position, Quaternion.identity);
    }
}
