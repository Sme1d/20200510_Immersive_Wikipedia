using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      float moveHorizontal = Input.GetAxis ("Horizontal");
      float moveVertical = Input.GetAxis ("Vertical");

      Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
      transform.position += movement * speed;

      if (Input.GetKeyDown(KeyCode.Q))
      {
          rotateY(90);
      }

      if (Input.GetKeyDown(KeyCode.E))
      {
          rotateY(-90);
      }
    }

    void rotateY(float degree){
        Vector3 rotVec = new Vector3(0.0f, degree, 0.0f);
        transform.Rotate(rotVec);
    }
}
