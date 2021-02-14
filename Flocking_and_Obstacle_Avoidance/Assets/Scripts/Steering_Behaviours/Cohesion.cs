using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Steering/Cohesion")]
public class Cohesion : Steering
{
    public float smoothTime = 0.5f;

    //Utilities
    Vector2 currentVelocity;
    Vector2 averagePos;

    public override Vector2 GetSteering(AIAgent agent, List<Transform> neighbours, FlockManager flock)
    {
        //If there are no neighbours, return zero vector2
        if (neighbours.Count == 0)
            return Vector2.zero;

        //Reset each call
        averagePos = Vector2.zero;

        foreach (Transform item in neighbours)
        {
            averagePos += (Vector2)item.position;
        }

        if (neighbours.Count > 0)
            averagePos /= neighbours.Count;

        //Create the difference vector
        averagePos -= (Vector2)agent.transform.position;
        averagePos = Vector2.SmoothDamp(agent.transform.up, averagePos, ref currentVelocity, smoothTime);

        return averagePos;
    }
}
