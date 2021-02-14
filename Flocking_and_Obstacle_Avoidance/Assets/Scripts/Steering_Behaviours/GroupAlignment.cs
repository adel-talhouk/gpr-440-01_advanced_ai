using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Steering/GroupAlignment")]
public class GroupAlignment : Steering
{
    //Utility
    Vector2 averageHeading;

    public override Vector2 GetSteering(AIAgent agent, List<Transform> neighbours, FlockManager flock)
    {
        //If there are no neighbours, keep heading in the same direction
        if (neighbours.Count == 0)
            return agent.transform.up;

        //Get the average of all neighbour directions
        averageHeading = Vector2.zero;

        foreach (Transform item in neighbours)
        {
            averageHeading += (Vector2)item.transform.up;
        }

        if (neighbours.Count > 0)
            averageHeading /= neighbours.Count;

        return averageHeading;
    }
}
