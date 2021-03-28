using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerManager : MonoBehaviour
{
    enum TowerType { cannon, minigun }
    TowerType currentTower;

    [Header("Costs")]
    public int cannonCost = 500;
    public int minigunCost = 1500;
    public int startingCash = 5000;

    [Header("Prefabs")]
    public GameObject cannonTower;
    public GameObject minigunTower;

    [Header("UI")]
    public TextMeshProUGUI currentTowerText;
    public TextMeshProUGUI cashAmountText;

    //Private data
    int currentCash;
    GridManager gridManager;

    // Start is called before the first frame update
    void Start()
    {
        currentCash = startingCash;
        cashAmountText.text = "$" + currentCash;

        //Current tower
        currentTower = TowerType.cannon;
        currentTowerText.text = "Current Tower: Cannon";

        gridManager = FindObjectOfType<GridManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //GET KEY DOWN - KEYCODE TAB - Change tower type
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (currentTower == TowerType.cannon)
            {
                currentTower = TowerType.minigun;
                currentTowerText.text = "Current Tower: Minigun";
            }
            else if (currentTower == TowerType.minigun)
            {
                currentTower = TowerType.cannon;
                currentTowerText.text = "Current Tower: Cannon";
            }
        }

        //GET MOUSE BUTTON DOWN - 0 - Place tower
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 spawnPos = gridManager.GetCellWorldPos(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            PlaceTower(spawnPos);
        }
    }

    void PlaceTower(Vector2 pos)
    {
        //Check current tower and cost
        int cost;

        switch (currentTower)
        {
            case TowerType.cannon:
                cost = cannonCost;
                break;

            case TowerType.minigun:
                cost = minigunCost;
                break;
            default:
                cost = 99999999;
                break;
        }

        //If they have enough to place a tower
        if (currentCash >= cost)
        {
            //Check which tower
            switch (currentTower)
            {
                case TowerType.cannon:
                {
                    //Place it here
                    GameObject cannon = Instantiate(cannonTower, pos, Quaternion.identity, transform);

                    //Update influence map


                    //Pay for it you greedy mf
                    currentCash -= cost;
                    cashAmountText.text = "$" + currentCash;
                }
                    break;
                case TowerType.minigun:
                {
                    //Place it here
                    GameObject minigun = Instantiate(minigunTower, pos, Quaternion.identity, transform);

                    //Update influence map


                    //Pay for it you greedy mf
                    currentCash -= cost;
                    cashAmountText.text = "$" + currentCash;
                }
                    break;
                default:
                    Debug.LogError("No such tower!");
                    break;
            }
        }
    }

    void SellTower(Vector2 pos)
    {

    }
}
