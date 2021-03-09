using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Heavily inspired by: Turbo Makes Games: Tutorial - Flow Field Pathfinding in Unity https://www.youtube.com/watch?v=tSe6ZqDKB0Y
public class FlowField
{
    public Cell[,] grid { get; private set; }
    public Vector2Int gridSize { get; private set; }
    public float cellRadius { get; private set; }
    public byte gravelCost { get; set; }
    public byte mountainCost { get; set; }

    //The target location
    public Cell destinationCell;

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

    public void ResetCosts()
    {
        foreach (Cell currentCell in grid)
        {
            currentCell.ResetCosts();
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

    //Basically dijkstra, with no early exit
    public void CreateIntegrationField(Cell destination)
    {
        destinationCell = destination;

        //Set costs of destination cell
        destinationCell.cost = 0;
        destinationCell.bestCost = 0;

        //Open list of all neighbouring cells
        Queue<Cell> frontier = new Queue<Cell>();

        //Add destination cell to the queue
        frontier.Enqueue(destinationCell);

        //While the frontier is NOT empty
        while (frontier.Count > 0)
        {
            //Get the current cell (the first thing in the frontier)
            Cell currentCell = frontier.Dequeue();

            //Get the neighbours
            List<Cell> neighbours = GetNeighbouringCells(currentCell.gridPosition, GridDirection.CardinalDirections);

            //Iterate through the neighbours
            foreach (Cell currentNeighbour in neighbours)
            {
                //Skip over any impassable cells
                if (currentNeighbour.cost == byte.MaxValue)
                    continue;

                //Check if the cost of this path to the neighbour is less that what was previously stored as the best cost
                if (currentCell.bestCost + currentNeighbour.cost < currentNeighbour.bestCost)
                {
                    //Update the best cost
                    currentNeighbour.bestCost = (ushort)(currentCell.bestCost + currentNeighbour.cost);

                    //Add the neighbour to the frontier
                    frontier.Enqueue(currentNeighbour);
                }
            }
        }
    }

    List<Cell> GetNeighbouringCells(Vector2Int cellGridIndex, List<GridDirection> directions)
    {
        List<Cell> neighbours = new List<Cell>();

        //GO through all the directions we want
        foreach (Vector2Int currentDirection in directions)
        {
            //Get the current neighbour in the currentDirection
            Cell currentNeighbour = GetCellInDirection(cellGridIndex, currentDirection);

            //If it is valid
            if (currentNeighbour != null)
            {
                //Add it to the neighbours list
                neighbours.Add(currentNeighbour);
            }
        }

        //Return the list
        return neighbours;
    }

    Cell GetCellInDirection(Vector2Int cellIndex, Vector2Int relativeDirection)
    {
        Vector2Int finalPosition = cellIndex + relativeDirection;

        //Return nothing for anything outside the grid
        if (finalPosition.x < 0 || finalPosition.x >= gridSize.x || finalPosition.y < 0 || finalPosition.y >= gridSize.y)
        {
            return null;
        }

        //Return the correct cell
        return grid[finalPosition.x, finalPosition.y];
    }

    public Cell GetCellAtWorldPosition(Vector3 worldPosition)
    {
        float percentX = worldPosition.x / (gridSize.x * cellDiameter);
        float percentY = worldPosition.z / (gridSize.y * cellDiameter);

        //Clamp values from 0 to 1
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.Clamp(Mathf.FloorToInt((gridSize.x) * percentX), 0, gridSize.x - 1);
        int y = Mathf.Clamp(Mathf.FloorToInt((gridSize.y) * percentY), 0, gridSize.y - 1);

        return grid[x, y];
    }
}
