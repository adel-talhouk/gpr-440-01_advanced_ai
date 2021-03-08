using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowField
{
    public Cell[,] grid { get; private set; }
    public Vector2Int gridSize { get; private set; }
    public float cellRadius { get; private set; }
    public byte gravelCost { get; set; }
    public byte mountainCost { get; set; }

    //Utility variable
    float cellDiameter;

    //Constructor
    public FlowField(float gridCellRadius, Vector2Int gridDimensions)
    {
        cellRadius = gridCellRadius;
        cellDiameter = 2f * cellRadius;
        gridSize = gridDimensions;
    }

    public void CreateGrid()
    {
        //Create a new grid
        grid = new Cell[gridSize.x, gridSize.y];

        //Iterate through the grid (2D array)
        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                //Initialise the cells
                Vector3 worldPos = new Vector3(cellDiameter * i + cellRadius, 0f, cellDiameter * j + cellRadius);   //Add radius for offset
                grid[i, j] = new Cell(worldPos, new Vector2Int(i, j));
            }
        }
    }

    public void CreateCostField()
    {
        //Get data for overlap box
        Vector3 halfCellWidth = Vector3.one * cellRadius;
        int terrainLayerMask = LayerMask.GetMask("Gravel", "Mountain", "Impassable");

        //Go through the grid
        foreach (Cell currentCell in grid)
        {
            //Get the obstacles on the terrainLayerMask
            Collider[] obstacles = Physics.OverlapBox(currentCell.worldPosition, halfCellWidth, Quaternion.identity, terrainLayerMask);
            bool hasIncreasedCost = false;

            //Go through all the obstacles and increase their costs
            foreach (Collider col in obstacles)
            {
                if (!hasIncreasedCost && col.gameObject.layer == 6)  //Gravel
                {
                    currentCell.IncreaseCost(gravelCost);
                    hasIncreasedCost = true;
                }
                else if (!hasIncreasedCost && col.gameObject.layer == 7)    //Mountain
                {
                    currentCell.IncreaseCost(mountainCost);
                    hasIncreasedCost = true;
                }
                else if (col.gameObject.layer == 8) //Impassable
                {
                    currentCell.IncreaseCost(byte.MaxValue);
                    continue;
                }
            }
        }
    }
}
