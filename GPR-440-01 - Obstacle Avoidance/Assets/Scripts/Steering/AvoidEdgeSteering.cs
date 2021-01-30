using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base of the scriptable object and composite steering is inspired by: "Flocking Algorithm in Unity" - Board To Bits Games
//https://www.youtube.com/playlist?list=PL5KbKbJ6Gf99UlyIqzV1UpOzseyRn5H1d

//This allows us to right-click to create the behaviour as a scriptable object
[CreateAssetMenu(menuName = "Steering/Behaviours/AvoidEdge")]

public class AvoidEdgeSteering : Steering
{
    public Vector2 circleCentre;
    [Range(5.0f, 25.0f)] public float circleRadius = 15.0f;

    //Inspired by: https://www.youtube.com/watch?v=kmwVDSUivkg&list=PL5KbKbJ6Gf99UlyIqzV1UpOzseyRn5H1d&index=8
    public override Vector2 GetSteering(AIAgent agent)
    {
        //Calculate the distance to the centre of the circle
        Vector2 centerOffset = circleCentre - (Vector2)agent.transform.position;
        float portionAway = centerOffset.magnitude / circleRadius;

        //If the boid is very close to the edge of the circle
        if (portionAway >= 0.9f)
        {
            return centerOffset * portionAway * portionAway;
        }

        return Vector2.zero;
    }
}
