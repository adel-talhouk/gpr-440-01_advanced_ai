using UnityEditor;
using UnityEngine;

//Link: Turbo Makes Games: Tutorial - Flow Field Pathfinding in Unity https://www.youtube.com/watch?v=tSe6ZqDKB0Y
public enum FlowFieldDisplayType { None, AllIcons, DestinationIcon, CostField, IntegrationField };

public class GridDebug : MonoBehaviour
{
	public GridController gridController;
	public bool displayGrid;

	public FlowFieldDisplayType curDisplayType;

    //For drawing
	private Vector2Int gridSize;
    private float cellRadius;
    private FlowField curFlowField;

    public Sprite arrowIcon;
    public Sprite destinationIcon;
    public Sprite impassableIcon;

    public void SetFlowField(FlowField newFlowField)
    {
        curFlowField = newFlowField;
        cellRadius = newFlowField.cellRadius;
        gridSize = newFlowField.gridSize;
    }

    public void DrawFlowField()
    {
        ClearCellDisplay();

        switch (curDisplayType)
        {
            case FlowFieldDisplayType.AllIcons:
                DisplayAllCells();
                break;

            case FlowFieldDisplayType.DestinationIcon:
                DisplayDestinationCell();
                break;

            default:
                break;
        }
    }

    private void DisplayAllCells()
    {
        if (curFlowField == null) { return; }
        foreach (Cell curCell in curFlowField.grid)
        {
            DisplayCell(curCell);
        }
    }

    private void DisplayDestinationCell()
    {
        if (curFlowField == null) { return; }
        DisplayCell(curFlowField.destinationCell);
    }

    private void DisplayCell(Cell cell)
    {
        GameObject iconGO = new GameObject();
        SpriteRenderer iconSR = iconGO.AddComponent<SpriteRenderer>();
        iconGO.transform.parent = transform;
        iconGO.transform.position = cell.worldPosition + new Vector3(0.0f, cellRadius, 0.0f);

        //Destination
        if (cell.cost == 0)
        {
            iconSR.sprite = destinationIcon;
            Quaternion newRot = Quaternion.Euler(90, 0, 0);
            iconGO.transform.rotation = newRot;
            iconGO.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else if (cell.cost == byte.MaxValue)    //Impassable
        {
            iconSR.sprite = impassableIcon;
            Quaternion newRot = Quaternion.Euler(90, 0, 0);
            iconGO.transform.rotation = newRot;
            iconGO.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
        else if (cell.bestDirection == GridDirection.North) //Just arrows from here on out
        {
            iconSR.sprite = arrowIcon;
            Quaternion newRot = Quaternion.Euler(90, 0, 0);
            iconGO.transform.rotation = newRot;
            iconGO.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
        else if (cell.bestDirection == GridDirection.South)
        {
            iconSR.sprite = arrowIcon;
            Quaternion newRot = Quaternion.Euler(90, 180, 0);
            iconGO.transform.rotation = newRot;
            iconGO.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
        else if (cell.bestDirection == GridDirection.East)
        {
            iconSR.sprite = arrowIcon;
            Quaternion newRot = Quaternion.Euler(90, 90, 0);
            iconGO.transform.rotation = newRot;
            iconGO.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
        else if (cell.bestDirection == GridDirection.West)
        {
            iconSR.sprite = arrowIcon;
            Quaternion newRot = Quaternion.Euler(90, 270, 0);
            iconGO.transform.rotation = newRot;
            iconGO.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
        else if (cell.bestDirection == GridDirection.NorthEast)
        {
            iconSR.sprite = arrowIcon;
            Quaternion newRot = Quaternion.Euler(90, 0, 0);
            iconGO.transform.rotation = newRot;
            iconGO.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
        else if (cell.bestDirection == GridDirection.NorthWest)
        {
            iconSR.sprite = arrowIcon;
            Quaternion newRot = Quaternion.Euler(90, 270, 0);
            iconGO.transform.rotation = newRot;
            iconGO.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
        else if (cell.bestDirection == GridDirection.SouthEast)
        {
            iconSR.sprite = arrowIcon;
            Quaternion newRot = Quaternion.Euler(90, 90, 0);
            iconGO.transform.rotation = newRot;
            iconGO.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
        else if (cell.bestDirection == GridDirection.SouthWest)
        {
            iconSR.sprite = arrowIcon;
            Quaternion newRot = Quaternion.Euler(90, 180, 0);
            iconGO.transform.rotation = newRot;
            iconGO.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
        else
        {
            iconSR.sprite = arrowIcon;
            iconGO.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
    }

    public void ClearCellDisplay()
    {
        foreach (Transform t in transform)
        {
            GameObject.Destroy(t.gameObject);
        }
    }

    private void OnDrawGizmos()
	{
		if (displayGrid)
		{
			if (curFlowField == null)
			{
				DrawGrid(gridController.gridSize, Color.red, gridController.cellRadius);
			}
			else
			{
				DrawGrid(gridSize, Color.green, cellRadius);
			}
		}

        if (curFlowField == null) { return; }

        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.MiddleCenter;

        switch (curDisplayType)
        {
            case FlowFieldDisplayType.CostField:
            {
                foreach (Cell curCell in curFlowField.grid)
                {
                    Handles.Label(curCell.worldPosition, curCell.cost.ToString(), style);
                }
                break;
            }
            case FlowFieldDisplayType.IntegrationField:
            {
                foreach (Cell curCell in curFlowField.grid)
                {
                    Handles.Label(curCell.worldPosition, curCell.bestCost.ToString(), style);
                }
                break;
            }
            default:
                break;
        }

    }

	private void DrawGrid(Vector2Int drawGridSize, Color drawColor, float drawCellRadius)
	{
		Gizmos.color = drawColor;
		for (int x = 0; x < drawGridSize.x; x++)
		{
			for (int y = 0; y < drawGridSize.y; y++)
			{
				Vector3 center = new Vector3(drawCellRadius * 2 * x + drawCellRadius, 0, drawCellRadius * 2 * y + drawCellRadius);
				Vector3 size = Vector3.one * drawCellRadius * 2;
				Gizmos.DrawWireCube(center, size);
			}
		}
	}
}
