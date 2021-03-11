using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Heavily inspired by: Turbo Makes Games: Tutorial - Flow Field Pathfinding in Unity https://www.youtube.com/watch?v=tSe6ZqDKB0Y
public class UnitManager : MonoBehaviour
{
    public GridController gridController;
    public GameObject unitPrefab;
    [Range(10, 50)] public int numOfUnitsToSpawn = 20;
    [Range(1.0f, 5.0f)] public float unitMoveSpeed = 2.5f;
    public TextMeshProUGUI numOfUnitsText;

    //Units list
    List<GameObject> unitsList;

    int numOfUnits = 0;

    // Start is called before the first frame update
    void Start()
    {
        unitsList = new List<GameObject>();
        numOfUnitsText.text = "Num of Units: " + numOfUnits;
    }

    // Update is called once per frame
    void Update()
    {
        //KEY DOWN - SPACE     - Spawn Units
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnUnits(numOfUnitsToSpawn);
        }
    }

    void FixedUpdate()
    {
        //If there is a flow field
        if (gridController.hasFlowField)
        {
            //Iterate through the units list
            foreach (GameObject unit in unitsList)
            {
                Cell cellBelow = gridController.currentFlowField.GetCellAtWorldPosition(unit.transform.position);
                Vector3 moveDirection = new Vector3(cellBelow.bestDirection.Vector.x, 0, cellBelow.bestDirection.Vector.y);
                Rigidbody unitRb = unit.GetComponent<Rigidbody>();
                unitRb.velocity = moveDirection * unitMoveSpeed;
            }
        }
    }

    void SpawnUnits(int numToSpawn)
    {
        Vector2Int gridSize = gridController.gridSize;
        float cellRadius = gridController.cellRadius;
        Vector2 maxSpawnPos = new Vector2(gridSize.x * cellRadius * 2 + cellRadius, gridSize.y * cellRadius * 2 + cellRadius);
        int colMask = LayerMask.GetMask("Gravel", "Mountain", "Impassable");
        Vector3 newPos;

        for (int i = 0; i < numToSpawn; i++)
        {
            GameObject unit = Instantiate(unitPrefab);
            unit.transform.parent = transform;
            unitsList.Add(unit);
            numOfUnits++;
            numOfUnitsText.text = "Num of Units: " + numOfUnits;

            do
            {
                newPos = new Vector3(Random.Range(0, maxSpawnPos.x), 1.0f, Random.Range(0, maxSpawnPos.y));
                unit.transform.position = newPos;
            } while (Physics.OverlapSphere(newPos, 0.25f, colMask).Length > 0);
        }
    }
}
