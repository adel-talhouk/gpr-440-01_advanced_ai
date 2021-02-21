using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Steering/Swarm")]
public class Swarm : Steering
{
    [Range(1.0f, 10.0f)] public float maxDetectionRange = 2.5f;
    Vector3 direction;

    public override Vector2 GetSteering(AIAgent agent, List<Transform> neighbours, FlockManager flock)
    {
        //If there is food
        if (flock.IsFoodSpawned)
        {
            //If the boid is close enough to the food
            if (Vector3.Distance(agent.transform.position, flock.FoodPosition) <= maxDetectionRange)
            {
                //Calculate the vector from the agent to the food
                direction = flock.FoodPosition - agent.transform.position;

                //Go there
                return direction;
            }
        }
        return Vector2.zero;
    }
}
