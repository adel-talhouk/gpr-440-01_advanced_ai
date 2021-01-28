using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base of the scriptable object and composite steering is inspired by: "Flocking Algorithm in Unity" - Board To Bits Games
//https://www.youtube.com/playlist?list=PL5KbKbJ6Gf99UlyIqzV1UpOzseyRn5H1d

//This allows us to right-click to create the behaviour as a scriptable object
[CreateAssetMenu(menuName = "Steering/CompositeSteering")]
public class CompositeSteering : Steering
{
    //The steering behaviours and their associated weights
    public Steering[] steeringBehaviours;
    public float[] correspondingWeights;

    public override Vector2 GetSteering(AIAgent agent)
    {
        //Safety check To ensure that the arrays are of the same size
        if (correspondingWeights.Length != steeringBehaviours.Length)
        {
            Debug.LogError("Data mismatch in " + name, this);
            return Vector2.zero;
        }

        //The end trajectory
        Vector2 finalHeading = Vector2.zero;

        //Iterate through the steering behaviours
        for (int i = 0; i < steeringBehaviours.Length; i++)
        {
            //The current heading trajectory
            Vector2 currentHeading = steeringBehaviours[i].GetSteering(agent) * correspondingWeights[i];

            //Make sure the weight is respected
            if (currentHeading != Vector2.zero)
            {
                if (currentHeading.sqrMagnitude > correspondingWeights[i] * correspondingWeights[i])
                {
                    currentHeading.Normalize();
                    currentHeading *= correspondingWeights[i];
                }

                //Add it to the combined movement vector
                finalHeading += currentHeading;
            }
        }

        return finalHeading;
    }
}
