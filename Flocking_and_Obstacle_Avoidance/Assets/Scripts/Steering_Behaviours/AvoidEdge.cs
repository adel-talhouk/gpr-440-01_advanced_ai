using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Steering/AvoidEdge")]
public class AvoidEdge : Steering
{
    public Vector2 center;
    public float radius = 10.0f;

    //Utilities
    Vector2 centerOffset;
    float portionAway;

    public override Vector2 GetSteering(AIAgent agent, List<Transform> neighbours, FlockManager flock)
    {
        //How far away the boid is from the center
        centerOffset = center - (Vector2)agent.transform.position;
        portionAway = centerOffset.magnitude / radius;

        //If the boid is very close to the edge of the circle
        if (portionAway >= 0.9f)
        {
            return centerOffset * portionAway * portionAway;
        }

        return Vector2.zero;
    }
}
