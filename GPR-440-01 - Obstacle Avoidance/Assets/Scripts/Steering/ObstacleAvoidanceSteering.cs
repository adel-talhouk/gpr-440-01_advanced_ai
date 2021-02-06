using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base of the scriptable object and composite steering is inspired by: "Flocking Algorithm in Unity" - Board To Bits Games
//https://www.youtube.com/playlist?list=PL5KbKbJ6Gf99UlyIqzV1UpOzseyRn5H1d

//This allows us to right-click to create the behaviour as a scriptable object
[CreateAssetMenu(menuName = "Steering/Behaviours/ObstacleAvoidance")]

//public class ObstacleAvoidanceSteering : Steering
//{
//    //Obstacle avoidance information
//    [Range(1.0f, 10.0f)] public float detectionDistance = 3.0f;
//    //[Range(0.01f, 0.5f)] public float avoidanceStrength = 0.05f;
//    [Range(0.1f, 5.0f)] public float avoidanceStrength = 1.0f;
//    [Range(10, 180)] public int raysSeparatingAngle = 45;
//    Vector2 headingVector = Vector2.zero;

//    public override Vector2 GetSteering(AIAgent agent)
//    {
//        //Data for the rays
//        Transform firePointTransform = agent.transform.GetChild(0).transform;
//        Vector3 startPos;
//        Vector3 direction;

//        //Cast a ray (straight)
//        startPos = firePointTransform.position;
//        direction = firePointTransform.up;
//        RaycastHit2D obstacleDetected = Physics2D.Raycast(startPos, direction, detectionDistance);
//        Debug.DrawRay(startPos, direction.normalized * detectionDistance, Color.green, 0.1f, true);

//        //For the left and right rays
//        Vector3 angleDiff = new Vector3((raysSeparatingAngle / 2.0f * Mathf.Deg2Rad), 0f, 0f);

//        //Cast a ray (left)
//        direction = firePointTransform.up - angleDiff;
//        RaycastHit2D obstacleDetectedLeft = Physics2D.Raycast(startPos, direction, detectionDistance);
//        Debug.DrawRay(startPos, direction.normalized * detectionDistance, Color.red, 0.1f, true);

//        //Cast a ray (right)
//        direction = firePointTransform.up + angleDiff;                                                                  //BUG HERE, NOT CHANGING LIKE RED
//        RaycastHit2D obstacleDetectedRight = Physics2D.Raycast(startPos, direction, detectionDistance);
//        Debug.DrawRay(startPos, direction.normalized * detectionDistance, Color.blue, 0.1f, true);

//        //Check if the straight ray hit another agent or an obstacle
//        if (obstacleDetected && (obstacleDetected.collider.CompareTag("AI_Agent") || obstacleDetected.collider.CompareTag("Obstacle")))
//        {
//            //If the normal is pointing to the left         | Inspired by https://youtu.be/PiYffouHvuk?t=567
//            if (obstacleDetected.normal.x < 0)
//            {
//                //Steer left
//                headingVector = new Vector2(-agent.moveSpeed * avoidanceStrength, agent.moveSpeed * avoidanceStrength);
//                Debug.Log("Dodging Left");
//            }
//            else    //Otherwise
//            {
//                //Steer right
//                headingVector = new Vector2(agent.moveSpeed * avoidanceStrength, agent.moveSpeed * avoidanceStrength);
//                Debug.Log("Dodging Right");
//            }
//        }

//        //Check if the left ray hit another agent or an obstacle
//        else if (obstacleDetectedLeft && (obstacleDetectedLeft.collider.CompareTag("AI_Agent") || obstacleDetectedLeft.collider.CompareTag("Obstacle")))
//        {
//            //Steer right
//            headingVector = new Vector2(agent.moveSpeed * avoidanceStrength, agent.moveSpeed * avoidanceStrength);
//            Debug.Log("Veering Right");
//        }

//        //Check if the right ray hit another agent or an obstacle
//        else if (obstacleDetectedRight && (obstacleDetectedRight.collider.CompareTag("AI_Agent") || obstacleDetectedRight.collider.CompareTag("Obstacle")))
//        {
//            //Steer left
//            headingVector = new Vector2(-agent.moveSpeed * avoidanceStrength, agent.moveSpeed * avoidanceStrength);
//            Debug.Log("Veering Left");
//        }
//        return headingVector;
//    }
//}

public class ObstacleAvoidanceSteering : Steering
{
    //Obstacle avoidance information
    [Range(1.0f, 10.0f)] public float detectionDistance = 3.0f;
    [Range(0.1f, 5.0f)] public float avoidanceStrength = 1.0f;
    [Range(0.1f, 2.0f)] public float sideRaysLengthMultiplier = 0.75f;
    [Range(10, 180)] public int raysSeparatingAngle = 45;
    public LayerMask obstaclesLayer;

    //Ray origins
    Transform midFirePoint;
    Transform leftFirePoint;
    Transform rightFirePoint;
    LineRenderer midLineRenderer;
    LineRenderer leftLineRenderer;
    LineRenderer rightLineRenderer;
    bool hasFoundFirePoints = false;        //For efficiency

    Vector2 headingVector = Vector2.zero;

    public override Vector2 GetSteering(AIAgent agent)
    {
        //Find the fire points
        if (!hasFoundFirePoints)
        {
            //Fire points
            midFirePoint = agent.transform.Find("FirePoint_Centre");
            leftFirePoint = agent.transform.Find("FirePoint_Left");
            rightFirePoint = agent.transform.Find("FirePoint_Right");

            //Line renderers
            midLineRenderer = midFirePoint.GetComponent<LineRenderer>();
            leftLineRenderer = leftFirePoint.GetComponent<LineRenderer>();
            rightLineRenderer = rightFirePoint.GetComponent<LineRenderer>();

            //Null check on the transforms
            if (midFirePoint && leftFirePoint && rightFirePoint)
                hasFoundFirePoints = true;  //Efficiency
        }

        //Ray direction and visualisation
        Vector3 rayDirection;
        Vector3[] rayPoints = new Vector3[2];

        //Cast a ray (straight)
        rayDirection = midFirePoint.up * detectionDistance;
        RaycastHit2D obstacleDetected = Physics2D.Raycast(midFirePoint.position, midFirePoint.up, detectionDistance, obstaclesLayer);
        Debug.DrawRay(midFirePoint.position, rayDirection, Color.green, 0.01f, true);
        rayPoints[0] = midFirePoint.position;
        rayPoints[1] = midFirePoint.position + rayDirection;
        midLineRenderer.SetPositions(rayPoints);

        //Set the rotations
        float angleOffset = raysSeparatingAngle / 2.0f;
        leftFirePoint.localRotation = Quaternion.Euler(0f, 0f, angleOffset);
        rightFirePoint.localRotation = Quaternion.Euler(0f, 0f, -angleOffset);

        //Cast a ray (left)
        rayDirection = leftFirePoint.up * detectionDistance;
        RaycastHit2D obstacleDetectedLeft = Physics2D.Raycast(leftFirePoint.position, leftFirePoint.up, detectionDistance * sideRaysLengthMultiplier, obstaclesLayer);
        rayPoints[0] = leftFirePoint.position;
        rayPoints[1] = leftFirePoint.position + rayDirection * sideRaysLengthMultiplier;
        leftLineRenderer.SetPositions(rayPoints);

        //Cast a ray (right)
        rayDirection = rightFirePoint.up * detectionDistance;
        RaycastHit2D obstacleDetectedRight = Physics2D.Raycast(rightFirePoint.position, rightFirePoint.up, detectionDistance * sideRaysLengthMultiplier, obstaclesLayer);
        rayPoints[0] = rightFirePoint.position;
        rayPoints[1] = rightFirePoint.position + rayDirection * sideRaysLengthMultiplier;
        rightLineRenderer.SetPositions(rayPoints);

        //For avoidance
        headingVector = Vector2.zero;

        if (obstacleDetected)
        {
            if (obstacleDetected.normal.x < 0)
            {
                //Steer left
                headingVector = new Vector2(-avoidanceStrength, 0);
            }
            else    //Otherwise
            {
                //Steer right
                headingVector = new Vector2(avoidanceStrength, 0);
            }
        }

        //Check if the left ray hit another agent or an obstacle
        else if (obstacleDetectedLeft)
        {
            //Steer right
            headingVector = new Vector2(agent.moveSpeed * avoidanceStrength, 0);
        }

        //Check if the right ray hit another agent or an obstacle
        else if (obstacleDetectedRight)
        {
            //Steer left
            headingVector = new Vector2(-agent.moveSpeed * avoidanceStrength, 0);
        }

        return headingVector;
    }
}