using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    //Boids info
    [Header("Boids Information")]
    public AIAgent agentPrefab;
    public CompositeSteering steering;
    [Range(1.0f, 20.0f)] public float agentSpeed = 5.0f;

    //Boids spawning
    [Header("Boids Spawning Data")]
    [Range(1, 250)] public int numOfInitialAgents = 50;
    [Range(1, 10)] public int agentSpawnMultiplier = 1;

    //Neighbours
    [Header("Neighbourhood Information")]
    [Range(1.0f, 5.0f)] public float neighbourhoodRadius = 1.5f;
    [Range(0.0f, 1.0f)] public float avoidanceRadiusMultiplier = 0.5f;

    [Header("Misc.")]
    public GameObject foodPrefab;

    //Utility variables
    Vector2 foodPosition;
    bool isFoodSpawned = false;
    float squaredNeighbourhoodRadius;
    float squaredAvoidanceRadius;
    int numOfAgents = 0;

    //Accessors
    public Vector3 FoodPosition { get { return foodPosition; } }
    public bool IsFoodSpawned {  get { return isFoodSpawned; } set { isFoodSpawned = value; } }
    public float SquaredAvoidanceRadius { get { return squaredAvoidanceRadius; } }
    public int NumOfAgents { get { return numOfAgents; } }

    //Utility variable(s)
    const float AGENTSPAWNDENSITY = 0.08f;
    List<AIAgent> agentsList = new List<AIAgent>();

    // Start is called before the first frame update
    void Start()
    {
        //For efficiency, we use the squared values
        squaredNeighbourhoodRadius = neighbourhoodRadius * neighbourhoodRadius;
        squaredAvoidanceRadius = squaredNeighbourhoodRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        //Spawn the agents
        SpawnAgents(numOfInitialAgents);
    }

    // Update is called once per frame
    void Update()
    {
        //If the player presses the Space bar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnAgents(numOfInitialAgents * agentSpawnMultiplier);
        }

        //Spawning food (only 1 at a time)
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isFoodSpawned)
        {
            //Spawn the food at the click location
            foodPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(foodPrefab, foodPosition, Quaternion.identity);

            isFoodSpawned = true;
        }

        //Iterate through all the boids
        foreach (AIAgent agent in agentsList)
        {
            //Context of the boid's neighbourhood
            List<Transform> context = GetNearbyObjects(agent);
            Vector2 move = steering.GetSteering(agent, context, this);
            move *= agentSpeed;

            agent.Move(move);
        }
    }

    List<Transform> GetNearbyObjects(AIAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighbourhoodRadius);

        //Go through all the contextColliders
        foreach (Collider2D col in contextColliders)
        {
            //As long as it's not this boid
            if (col != agent.Neighbourhood)
            {
                //Add the transform to the context
                context.Add(col.transform);
            }
        }

        return context;
    }

    void SpawnAgents(int numToSpawn)
    {
        for (int i = 0; i < numToSpawn; i++)
        {
            AIAgent newAgent = Instantiate(
                agentPrefab,
                Random.insideUnitCircle * numOfInitialAgents * AGENTSPAWNDENSITY,     //Spawn inside circle (scales with num of initial boids)
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),      //Random rotation
                transform   //Child of this Flock Manager
                );
            newAgent.name = "Agent " + i;
            agentsList.Add(newAgent);
            numOfAgents++;
        }
    }
}
