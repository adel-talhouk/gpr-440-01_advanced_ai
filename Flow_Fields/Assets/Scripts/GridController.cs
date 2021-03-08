using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Vector2Int gridSize;
    [Range(0.1f, 1.0f)] public float cellRadius = 0.5f;
    public FlowField currentFlowField;
    public GridDebug gridDebug;

    void InitFlowField()
    {
        currentFlowField = new FlowField(cellRadius, gridSize);
        currentFlowField.CreateGrid();
        gridDebug.SetFlowField(currentFlowField);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            InitFlowField();
        }
    }
}
