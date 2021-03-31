using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    public GameObject zombiePrefab;
    [Range(1.5f, 5f)] public float zombieMinSpeed = 1.5f;
    [Range(1.5f, 5f)] public float zombieMaxSpeed = 5f;
    [Range(15f, 50f)] public float zombieMaxHealth = 20f;
    public Vector2 zombieSeekLocation;
    public Vector2 spawnBoundsX;
    public Vector2 spawnBoundsY;

    [Header("Zombie Waves")]
    public int zombiesPerWave = 20;
    public float waveSpawnMultiplier = 1.25f;
    public float timeToStartWave = 2f;
    public TextMeshProUGUI currentWaveText;

    [Header("Misc.")]
    public int profitPerKill = 250;
    public int profitPerWave = 1500;

    //Helper data
    int currentWave;
    int numOfZombiesAlive;
    int numOfZombiesToSpawn;
    int numOfZombiesPrevWave;
    TowerManager towerManager;

    // Start is called before the first frame update
    void Start()
    {
        currentWave = 0;
        currentWaveText.text = "Current Wave: " + currentWave;

        numOfZombiesPrevWave = (int)(zombiesPerWave / waveSpawnMultiplier);

        towerManager = FindObjectOfType<TowerManager>();

        StartCoroutine(StartNextWave());
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnZombies()
    {
        for (int i = 0; i < numOfZombiesToSpawn; i++)
        {
            //Data for the zombie
            float moveSpeed = Random.Range(zombieMinSpeed, zombieMaxSpeed);
            Vector2 spawnPos = new Vector2(Random.Range(spawnBoundsX.x, spawnBoundsX.y),
                Random.Range(spawnBoundsY.x, spawnBoundsY.y));

            //Spawn zombie
            GameObject zombie = Instantiate(zombiePrefab, spawnPos, Quaternion.identity, transform);

            //Set data
            zombie.GetComponent<ZombieBehaviour>().moveSpeed = moveSpeed;
            zombie.GetComponent<ZombieBehaviour>().seekDestination = zombieSeekLocation;
            zombie.GetComponent<ZombieBehaviour>().maxHealth = zombieMaxHealth;
            zombie.GetComponent<ZombieBehaviour>().enemyManager = this;

            numOfZombiesAlive++;
        }

        numOfZombiesPrevWave = numOfZombiesAlive;
    }

    public void MarkZombieDead()
    {
        numOfZombiesAlive--;
        towerManager.currentCash += profitPerKill;

        //Check if there are no zombies left
        if (numOfZombiesAlive <= 0)
        {
            //Start next wave
            StartCoroutine(StartNextWave());
        }
    }

    IEnumerator StartNextWave()
    {
        towerManager.currentCash += profitPerWave;

        //Wait
        yield return new WaitForSeconds(timeToStartWave);

        //Start next wave
        currentWave++;
        currentWaveText.text = "Current Wave: " + currentWave;
        numOfZombiesToSpawn = (int)(numOfZombiesPrevWave * waveSpawnMultiplier);
        SpawnZombies();
    }

}
