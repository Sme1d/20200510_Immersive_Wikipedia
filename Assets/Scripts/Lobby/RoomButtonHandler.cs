using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class RoomButtonHandler : MonoBehaviour
{
    public InputField roomNameInput;

    public void SetText()
    {
        Text buttonText = transform.Find("Text").GetComponent<Text>();
        roomNameInput.text = buttonText.text;
    }
}
