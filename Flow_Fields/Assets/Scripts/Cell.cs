using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Heavily inspired by: Turbo Makes Games: Tutorial - Flow Field Pathfinding in Unity https://www.youtube.com/watch?v=tSe6ZqDKB0Y
public class Cell
{
    //Positions
    public Vector3 worldPosition;
    public Vector2Int gridPosition;

    //Cost
    public byte cost;   //Int from 0 to 255, makes it easier to scope out terrain costs
    public ushort bestCost; //Unsigned short int for the best cost

    //Constructor
    public Cell(Vector3 worldPos, Vector2Int gridPos)
    {
        worldPosition = worldPos;
        gridPosition = gridPos;
        cost = 1;   //Default value
        bestCost = ushort.MaxValue; //Default value is max, because we want to save whatever is the smallest value (basis for comparison is the largest possible num)
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

    public void ResetCosts()
    {
        cost = 1;
        bestCost = ushort.MaxValue;
    }
}
