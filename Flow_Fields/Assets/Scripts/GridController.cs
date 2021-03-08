using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Update()
    {
        //KEY DOWN - RETURN     - Init flow field and cost field
        if (Input.GetKeyDown(KeyCode.Return))
        {
            InitFlowField();
            currentFlowField.CreateCostField();
        }

        //KEY DOWN - R         - Reload cost field
        if (Input.GetKeyDown(KeyCode.R) && currentFlowField != null)
        {
            //Reset costs
            currentFlowField.ResetCosts();

            //Set costs
            currentFlowField.gravelCost = gravelCost;
            currentFlowField.mountainCost = mountainCost;

            //Create new cost field
            currentFlowField.CreateCostField();
        }
    }
}
