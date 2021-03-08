using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    //Positions
    public Vector3 worldPosition;
    public Vector2Int gridPosition;

    //Cost
    public byte cost;   //Int from 0 to 255, makes it easier to scope out terrain costs

    //Constructor
    public Cell(Vector3 worldPos, Vector2Int gridPos)
    {
        worldPosition = worldPos;
        gridPosition = gridPos;
        cost = 1;   //Default value
    }

    public void IncreaseCost(int increment)
    {
        //Do not go over the max value of a byte
        if (cost == byte.MaxValue)
            return;
        if (cost + increment >= byte.MaxValue)
            cost = byte.MaxValue;
        else
            cost += (byte)increment;
    }
}
