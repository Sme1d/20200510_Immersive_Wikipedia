using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class LobbyRoomButtonHandler : MonoBehaviour
{
    public InputField roomNameInput;

    public void SetText()
    {
        Debug.Log("set text");
        Text buttonText = transform.Find("Text").GetComponent<Text>();
        roomNameInput.text = buttonText.text;
    }
}
