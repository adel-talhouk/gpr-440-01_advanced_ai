using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base of the scriptable object and composite steering is inspired by: "Flocking Algorithm in Unity" - Board To Bits Games
//https://www.youtube.com/playlist?list=PL5KbKbJ6Gf99UlyIqzV1UpOzseyRn5H1d

//This allows us to right-click to create the behaviour as a scriptable object
[CreateAssetMenu(menuName = "Steering/Behaviours/ObstacleAvoidance")]

public class ObstacleAvoidanceSteering : Steering
{
    //Obstacle avoidance information
    [Range(1.0f, 10.0f)] public float detectionDistance = 3.0f;
    [Range(0.01f, 0.5f)] public float avoidanceStrength = 0.05f;
    Vector2 headingVector = Vector2.zero;

    public override Vector2 GetSteering(AIAgent agent)
    {
        //Cast a ray
        Vector3 startPos = agent.transform.GetChild(0).transform.position;
        RaycastHit2D obstacleDetected = Physics2D.Raycast(startPos, agent.transform.up, detectionDistance);
        Debug.DrawRay(startPos, agent.transform.up.normalized * detectionDistance, Color.green, 0.1f, true);

        //Check if the ray hit another agent or an obstacle
        if (obstacleDetected && (obstacleDetected.collider.CompareTag("AI_Agent") || obstacleDetected.collider.CompareTag("Obstacle")))
        {
            //If the normal is pointing to the left         | Inspired by https://youtu.be/PiYffouHvuk?t=567
            if (obstacleDetected.normal.x < 0)
            {
                //Steer left
                headingVector = new Vector2(-agent.moveSpeed * avoidanceStrength, agent.moveSpeed * avoidanceStrength);
                Debug.Log("Veering Left");
            }
            else    //Otherwise
            {
                //Steer right
                headingVector = new Vector2(agent.moveSpeed * avoidanceStrength, agent.moveSpeed * avoidanceStrength);
                Debug.Log("Veering Right");
            }
        }

        return headingVector;
    }
}
