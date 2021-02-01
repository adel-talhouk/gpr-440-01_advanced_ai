using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base of the scriptable object and composite steering is inspired by: "Flocking Algorithm in Unity" - Board To Bits Games
//https://www.youtube.com/playlist?list=PL5KbKbJ6Gf99UlyIqzV1UpOzseyRn5H1d

//This allows us to right-click to create the behaviour as a scriptable object
[CreateAssetMenu(menuName = "Steering/Behaviours/Wander")]

public class WanderSteering : Steering
{
    //Wander variables
    [Header("Wander Circle Properties")]
    [Range(1.0f, 20.0f)] public float circleCenterDistance = 4.0f;
    [Range(0.1f, 2.0f)] public float circleRadius = 1.0f;

    //Private data to calculate circle and point
    Vector2 circleCentre;
    Vector2 pointOnCircle = Vector2.zero;
    Vector2 headingVector = Vector2.zero;
    float theta;

    //Private variables to make it choose a new direction less often
    int numOfFrames = 0;
    int framesToRecalculate = 10;

    public override Vector2 GetSteering(AIAgent agent)
    {
        numOfFrames++;

        //Make it choose a new direction less often (reduce jitter)
        if (numOfFrames % framesToRecalculate == 0)
        {
            //Calculate the centre of the circle
            circleCentre = agent.transform.position + (agent.transform.up.normalized * circleCenterDistance);

            //Create random angle
            theta = Random.Range(0.0f, 360);

            //X pos on circle	https://stackoverflow.com/questions/14096138/find-the-point-on-a-circle-with-given-center-point-radius-and-degree
            pointOnCircle.x = circleCentre.x + (circleRadius * Mathf.Cos(theta * Mathf.PI / 180.0f));
            //Y pos on circle	https://stackoverflow.com/questions/14096138/find-the-point-on-a-circle-with-given-center-point-radius-and-degree
            pointOnCircle.y = circleCentre.y + (circleRadius * Mathf.Sin(theta * Mathf.PI / 180.0f));

            //Go towards the point
            headingVector = pointOnCircle - (Vector2)agent.transform.position;
            headingVector.Normalize();
            headingVector *= agent.moveSpeed;
        }

        return headingVector;
    }
}
