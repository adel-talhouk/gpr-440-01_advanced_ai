using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FrameRate : MonoBehaviour
{
    public TextMeshProUGUI frameRateDisplay;
    public TextMeshProUGUI numOfAgentsDisplay;
    FlockManager flock;

    private void Start()
    {
        //Find the flock manager
        flock = FindObjectOfType<FlockManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Update UI texts
        frameRateDisplay.text = "FPS: " + (int)(1 / Time.deltaTime);
        numOfAgentsDisplay.text = "Agent Count: " + flock.NumOfAgents;
    }
}
