using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIToggleInstructions : MonoBehaviour
{
    public GameObject instructionsText;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ToggleInstructions();
        }
    }

    void ToggleInstructions()
    {
        instructionsText.SetActive(!instructionsText.activeSelf);
    }
}
