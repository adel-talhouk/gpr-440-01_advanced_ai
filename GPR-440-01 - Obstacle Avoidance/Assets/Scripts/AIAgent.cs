using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAgent : MonoBehaviour
{
    //Steering information
    [Header("Steering Information")]
    [Range(1.0f, 10.0f)] public float moveSpeed = 5f;
    public CompositeSteering compositeSteering;
    Vector2 headingVector;

    // Update is called once per frame
    void Update()
    {
        //Move the agent
        headingVector = compositeSteering.GetSteering(this);
        transform.up = headingVector;
        transform.position += (Vector3)headingVector * moveSpeed * Time.deltaTime;
    }
}
