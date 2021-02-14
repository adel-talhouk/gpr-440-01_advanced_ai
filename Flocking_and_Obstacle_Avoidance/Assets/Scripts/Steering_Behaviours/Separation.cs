using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Steering/Separation")]
public class Separation : Steering
{
    //Utilities
    Vector2 avoidanceMove;
    int numOfBoidsToAvoid;

    public override Vector2 GetSteering(AIAgent agent, List<Transform> neighbours, FlockManager flock)
    {
        //If there are no neighbours, return zero vector2
        if (neighbours.Count == 0)
            return Vector2.zero;

        //Move to avoid
        avoidanceMove = Vector2.zero;
        numOfBoidsToAvoid = 0;

        foreach (Transform item in neighbours)
        {
            //Make sure the neighbour is in the avoidance radius
            if (Vector2.SqrMagnitude(item.position - agent.transform.position) < flock.SquaredAvoidanceRadius)
            {
                //Move away from the neighbour
                numOfBoidsToAvoid++;
                avoidanceMove += (Vector2)(agent.transform.position - item.position);
            }
        }

        if (numOfBoidsToAvoid > 0)
            avoidanceMove /= numOfBoidsToAvoid;

        return avoidanceMove;
    }
}
