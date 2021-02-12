using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAgent : MonoBehaviour
{
    //Steering information
    [Header("Steering Information")]
    public CompositeSteering compositeSteering;

    [HideInInspector] public float moveSpeed = 1f;

    //For calculations
    Vector2 headingVector;

    void Update()
    {
        //Move the agent
        headingVector = compositeSteering.GetSteering(this);
        transform.up = headingVector;
        transform.position += (Vector3)headingVector * moveSpeed * Time.deltaTime;
    }
}
