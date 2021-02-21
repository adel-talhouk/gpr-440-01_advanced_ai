using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public int maxHealth = 20;
    public int individualBoidDamage = 2;

    FlockManager flock;
    int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        flock = FindObjectOfType<FlockManager>();
        currentHealth = maxHealth;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Boid"))
        {
            currentHealth -= individualBoidDamage;

            //If dead
            if (currentHealth <= 0)
            {
                Debug.Log("Food Consumed");

                //Food is no longer spawned, destroy this gameObject
                flock.IsFoodSpawned = false;
                Destroy(gameObject);
            }
        }
    }
}
