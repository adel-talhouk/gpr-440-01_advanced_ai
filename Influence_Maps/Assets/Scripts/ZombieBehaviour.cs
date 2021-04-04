using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBehaviour : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector2 seekDestination;
    public float maxHealth = 20f;
    public EnemyManager enemyManager;

    //Private data
    float currentHealth;
    List<Cell> bestPath;
    Vector2 position2D;
    Cell currentSeekNode;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        bestPath = new List<Cell>();
        position2D = new Vector2(transform.position.x, transform.position.y);
        currentSeekNode = null;
    }

    // Update is called once per frame
    void Update()
    {
        FollowPath();
    }

    public void DealDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0f)
        {
            enemyManager.MarkZombieDead();
            Destroy(gameObject);
        }
    }

    public void FindPathAStar(Cell startingNode, Cell destinationNode)
    {
        //Open and closed lists
        List<Cell> openList = new List<Cell>();
        //List<Cell> closedList = new List<Cell>();
        bestPath = new List<Cell>();
        Dictionary<Cell, Cell> cameFrom = new Dictionary<Cell, Cell>();
        Dictionary<Cell, float> costSoFar = new Dictionary<Cell, float>();

        //Starting node on the open list
        openList.Add(startingNode);
        Cell currentNode = null;
        cameFrom[currentNode] = null;
        costSoFar[currentNode] = 0;

        while (openList.Count > 0)
        {
            //Starting node (front of openList)
            currentNode = openList[0];
            openList.RemoveAt(0);

            //If we found the end node
            if (currentNode == destinationNode)
            {
                //Temp node
                Cell tempNode = currentNode;

                //"Trace" the path
                while (tempNode != startingNode)
                {
                    //Add it to the path
                    bestPath.Add(tempNode);

                    //Get where the temp node came from
                    tempNode = cameFrom[tempNode];
                }

                //Early exit
                return;
            }

            float cheapestCost = float.MaxValue;
            Cell cheapestNode = null;

            //Iterate through the open list
            foreach (Cell node in openList)
            {
                //Find the cheapest node
                float nodeCost = node.cost;

                //Check if we have a cheaper cost
                if (nodeCost < cheapestCost)
                {
                    cheapestCost = nodeCost;

                    //Manhattan distance
                    float heuristicCost = Mathf.Abs(currentNode.transform.position.x - destinationNode.transform.position.x)
                        + Mathf.Abs(currentNode.transform.position.y - destinationNode.transform.position.y);
                    float estimatedCost = cheapestCost + heuristicCost;

                    //Set this node as the cheapest
                    cheapestNode = node;
                    costSoFar[node] = estimatedCost;
                }
            }

            //Set the currentNode to the cheapest one we found
            currentNode = cheapestNode;

            //Get the neighbours and add them to the cameFrom dictionary
            List<Cell> neighbouringNodes = currentNode.GetNeighbouringCells();
            foreach (Cell neighbour in neighbouringNodes)
            {
                //Calculate cost - Manhattan distance + node cost + cost so far
                float heuristicCost = Mathf.Abs(neighbour.transform.position.x - destinationNode.transform.position.x)
                    + Mathf.Abs(neighbour.transform.position.y - destinationNode.transform.position.y);
                float estimatedCost = costSoFar[currentNode] + neighbour.cost + heuristicCost;

                //If there is not cost for the neighbour or we found a cheaper one
                if (!costSoFar.ContainsKey(neighbour) || estimatedCost < costSoFar[neighbour])
                {
                    //Set the new cost
                    costSoFar[neighbour] = estimatedCost;

                    //Add the node to the open list
                    openList.Add(neighbour);

                    //Add the neighbour to the openList
                    openList.Add(neighbour);

                    //Set where it came from
                    cameFrom[neighbour] = currentNode;
                }
            }
        }
    }

    void FollowPath()
    {
        //Set the current seek node
        currentSeekNode = bestPath[0];

        //Get the position
        Vector2 nodePos = currentSeekNode.transform.position;

        //Move towards it
        Vector3.MoveTowards(position2D, nodePos, moveSpeed);

        //If close enough, continue
        if (Vector2.Distance(position2D, nodePos) <= 0.1f)
        {
            //Go to the next node
            bestPath.RemoveAt(0);
            currentSeekNode = bestPath[0];
        }
    }
}
