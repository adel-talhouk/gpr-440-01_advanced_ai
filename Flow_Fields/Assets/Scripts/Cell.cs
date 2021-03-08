using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    //Positions
    public Vector3 worldPosition;
    public Vector2Int gridPosition;

    //Costs
    

    //Constructor
    public Cell(Vector3 worldPos, Vector2Int gridPos)
    {
        worldPosition = worldPos;
        gridPosition = gridPos;
    }
}
