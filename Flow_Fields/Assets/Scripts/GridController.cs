using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Inspired by: Turbo Makes Games: Tutorial - Flow Field Pathfinding in Unity https://www.youtube.com/watch?v=tSe6ZqDKB0Y
public class GridController : MonoBehaviour
{
    public Vector2Int gridSize;
    [Range(0.5f, 2.0f)] public float cellRadius = 1.5f;
    public FlowField currentFlowField;
    public GridDebug gridDebug;

    [Header("Terrain Costs")]
    public byte gravelCost = 3;
    public byte mountainCost = 8;

    void InitFlowField()
    {
        currentFlowField = new FlowField(cellRadius, gridSize);
        currentFlowField.CreateGrid();
        gridDebug.SetFlowField(currentFlowField);

        //Set costs
        currentFlowField.gravelCost = gravelCost;
        currentFlowField.mountainCost = mountainCost;
    }

    void Update()
    {
        //KEY DOWN - RETURN     - Init flow field and cost field
        if (Input.GetKeyDown(KeyCode.Return))
        {
            InitFlowField();
            currentFlowField.CreateCostField();
        }

        //MOUSE BUTTON DOWN - 0     - Create cost field, integration field, and flow field
        if (Input.GetMouseButtonDown(0) && currentFlowField != null)
        {
            //Reset costs
            currentFlowField.ResetCosts();
            currentFlowField.gravelCost = gravelCost;
            currentFlowField.mountainCost = mountainCost;

            //Create cost field
            currentFlowField.CreateCostField();

            //Get the mouse click position
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f);
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

            //Destination cell and integration field creation
            Cell destinationCell = currentFlowField.GetCellAtWorldPosition(mouseWorldPos);
            currentFlowField.CreateIntegrationField(destinationCell);

            //Flow field
            currentFlowField.CreateFlowField();
            gridDebug.DrawFlowField();
        }
    }
}
