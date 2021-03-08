using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowField
{
    public Cell[,] grid { get; private set; }
    public Vector2Int gridSize { get; private set; }
    public float cellRadius { get; private set; }

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
}
