using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Steering : ScriptableObject
{
    //GetSteering method will be implemented by the derived classes
    public abstract Vector2 GetSteering(AIAgent agent);
}
