using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Steering/ObstacleAvoidance")]
public class ObstacleAvoidanceSteering : Steering
{
    //Obstacle avoidance information
    [Range(1.0f, 10.0f)] public float detectionDistance = 3.0f;
    [Range(0.1f, 5.0f)] public float avoidanceStrength = 1.0f;
    public LayerMask obstaclesLayer;

    //Raycasting and visualisation
    Transform firePoint;
    LineRenderer lineRenderer;

    Vector2 headingVector = Vector2.zero;

    public override Vector2 GetSteering(AIAgent agent, List<Transform> neighbours, FlockManager flock)
    {
        //Fire point
        firePoint = agent.transform;

        //Line renderer
        lineRenderer = firePoint.GetComponent<LineRenderer>();  //Getting the line renderer each time is not efficient, need a better way to do this

        //Ray direction and visualisation
        Vector3 rayDirection;
        Vector3[] rayPoints = new Vector3[2];

        //Cast a ray
        rayDirection = firePoint.up * detectionDistance;
        RaycastHit2D obstacleDetected = Physics2D.Raycast(firePoint.position, firePoint.up, detectionDistance, obstaclesLayer);
        rayPoints[0] = firePoint.position;
        rayPoints[1] = firePoint.position + rayDirection;
        lineRenderer.SetPositions(rayPoints);

        //For avoidance
        headingVector = Vector2.zero;

        if (obstacleDetected)
        {
            if (obstacleDetected.normal.x < 0)
            {
                //Steer left
                headingVector = new Vector2(-avoidanceStrength, 0f);
            }
            else    //Otherwise
            {
                //Steer right
                headingVector = new Vector2(avoidanceStrength, 0f);
            }
        }

        return headingVector;
    }
}
