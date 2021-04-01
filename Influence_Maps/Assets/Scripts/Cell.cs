using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class Cell : MonoBehaviour
{
    public float cost = 0f;

    //Zombies in the cell
    int zombiesInCell = 0;
    public int ZombiesInCell {  get { return zombiesInCell; } }

    public GridManager gridManager;

    public void IncreaseCost(float increaseAmount)
    {
        cost += increaseAmount;

        //Update colour
        GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 0f, Mathf.Min(1f, cost));
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Zombie"))
        {
            zombiesInCell++;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Zombie"))
            zombiesInCell--;
    }

    void OnDrawGizmos()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.MiddleCenter;
        Handles.Label(transform.position, cost.ToString(), style);
    }

    public List<Cell> GetNeighbouringCells()
    {
        List<Cell> neighbours = new List<Cell>();
        Vector2 cellPosition2D = new Vector2(transform.position.x, transform.position.y);
        Vector2 dir;

        //Go through all the directions
        dir = new Vector2(0f, 1f);
        Cell up = gridManager.GetCellAt(cellPosition2D + dir);

        dir = new Vector2(0f, -1f);
        Cell down = gridManager.GetCellAt(cellPosition2D + dir);

        dir = new Vector2(-1f, 0f);
        Cell left = gridManager.GetCellAt(cellPosition2D + dir);

        dir = new Vector2(1f, 0f);
        Cell right = gridManager.GetCellAt(cellPosition2D + dir);

        dir = new Vector2(-1f, 1f);
        Cell topLeft = gridManager.GetCellAt(cellPosition2D + dir);

        dir = new Vector2(1f, 1f);
        Cell topRight = gridManager.GetCellAt(cellPosition2D + dir);

        dir = new Vector2(-1f, -1f);
        Cell bottomLeft = gridManager.GetCellAt(cellPosition2D + dir);

        dir = new Vector2(1f, -1f);
        Cell bottomRight = gridManager.GetCellAt(cellPosition2D + dir);

        //Add the cells to the list
        neighbours.Add(up);
        neighbours.Add(down);
        neighbours.Add(left);
        neighbours.Add(right);
        neighbours.Add(topLeft);
        neighbours.Add(topRight);
        neighbours.Add(bottomLeft);
        neighbours.Add(bottomRight);

        //Return the list
        return neighbours;
    }
}
