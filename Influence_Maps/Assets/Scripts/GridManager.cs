using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject cellPrefab;

    [Header("Grid Data")]
    public Vector2Int gridSize;
    [Range(0.1f, 0.5f)] public float cellSpacing = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateGrid()
    {
        //For through the 2D array
        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                //Spawn cell at (i, j) with offset
                Vector2 spawnPos = new Vector2(i + cellSpacing, j + cellSpacing);
                GameObject cell = Instantiate(cellPrefab, spawnPos, Quaternion.identity, transform);
                cell.GetComponent<Cell>().cost = 0;
            }
        }
    }
}
