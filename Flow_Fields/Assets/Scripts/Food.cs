using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public int maxHealth = 20;
    public int damageTaken = 1;

    int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Unit"))
        {
            currentHealth -= damageTaken;

            if (currentHealth <= 0)
            {
                GridController.canSpawnFood = true;
                Destroy(gameObject);
            }
        }
    }
}
