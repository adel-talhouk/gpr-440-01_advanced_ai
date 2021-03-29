using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject zombiePrefab;
    [Range(1.5f, 5f)] public float zombieMinSpeed = 1.5f;
    [Range(1.5f, 5f)] public float zombieMaxSpeed = 5f;
    public Vector2 zombieSeekLocation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnZombies()
    {
        //Data for the zombie
        float moveSpeed = Random.Range(zombieMinSpeed, zombieMaxSpeed);
    }
}
