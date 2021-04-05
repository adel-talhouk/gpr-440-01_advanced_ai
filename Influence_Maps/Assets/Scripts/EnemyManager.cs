using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    public GameObject zombiePrefab;
    [Range(1.5f, 5f)] public float zombieMinSpeed = 1.5f;
    [Range(1.5f, 5f)] public float zombieMaxSpeed = 5f;
    [Range(5f, 20f)] public float zombieMaxHealth = 10f;
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
    bool shouldStartWave;
    TowerManager towerManager;
    GridManager gridManager;


    // Start is called before the first frame update
    void Start()
    {
        currentWave = 0;
        currentWaveText.text = "Current Wave: " + currentWave;

        numOfZombiesPrevWave = (int)(zombiesPerWave / waveSpawnMultiplier);

        shouldStartWave = false;

        towerManager = FindObjectOfType<TowerManager>();
        gridManager = FindObjectOfType<GridManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //If there are towers in the game and no zombies
        if (towerManager.hasSpawnedTowers && numOfZombiesAlive <= 0)
        {
            shouldStartWave = true;
        }

        if (shouldStartWave)
        {
            shouldStartWave = false;

            //Start the next wave
            //StartCoroutine(StartNextWave());

            //Bonus cash after the first round
            if (currentWave > 1)
                towerManager.IncreaseCash(profitPerWave);

            //Start next wave
            currentWave++;
            currentWaveText.text = "Current Wave: " + currentWave;
            numOfZombiesToSpawn = (int)(numOfZombiesPrevWave * waveSpawnMultiplier);
            SpawnZombies();
        }
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
            numOfZombiesAlive++;

            //Set data
            zombie.GetComponent<ZombieBehaviour>().moveSpeed = moveSpeed;
            zombie.GetComponent<ZombieBehaviour>().seekDestination = zombieSeekLocation;
            zombie.GetComponent<ZombieBehaviour>().maxHealth = zombieMaxHealth;
            zombie.GetComponent<ZombieBehaviour>().enemyManager = this;

            //Colour destination cell
            gridManager.GetCellAt(zombieSeekLocation).GetComponent<SpriteRenderer>().color = Color.yellow;

            //Find path
            zombie.GetComponent<ZombieBehaviour>().SetAStarData(gridManager.GetCellAt(spawnPos), gridManager.GetCellAt(zombieSeekLocation));
        }

        numOfZombiesPrevWave = numOfZombiesAlive;
    }

    public void MarkZombieDead()
    {
        numOfZombiesAlive--;
        towerManager.IncreaseCash(profitPerKill);
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
        ScoreManager.waveNumber = currentWave;
        ScoreManager.cashNumber = towerManager.currentCash;
    }

    //IEnumerator StartNextWave()
    //{
    //    //Bonus cash after the first round
    //    if (currentWave > 1)
    //        towerManager.IncreaseCash(profitPerWave);

    //    //Wait
    //    yield return new WaitForSeconds(timeToStartWave);

    //    //Start next wave
    //    currentWave++;
    //    currentWaveText.text = "Current Wave: " + currentWave;
    //    numOfZombiesToSpawn = (int)(numOfZombiesPrevWave * waveSpawnMultiplier);
    //    SpawnZombies();
    //}
}
