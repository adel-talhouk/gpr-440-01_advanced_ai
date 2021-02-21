using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Food : MonoBehaviour
{
    [Header("Health Values")]
    public int maxHealth = 20;
    public int individualBoidDamage = 2;

    [Header("Health Bar")]
    public Slider healthBar;
    public Gradient healthGradient;
    public Image healthBarFill;

    //Utility variables
    FlockManager flock;
    int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        //Find the flock manager
        flock = FindObjectOfType<FlockManager>();

        //Health-related values
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
        healthBarFill.color = healthGradient.Evaluate(1f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //If a boid enters the trigger
        if (col.CompareTag("Boid"))
        {
            //Decrease health and update health bar
            currentHealth -= individualBoidDamage;
            healthBar.value = currentHealth;
            healthBarFill.color = healthGradient.Evaluate(healthBar.normalizedValue);

            //If dead
            if (currentHealth <= 0)
            {
                //Food is no longer spawned, destroy this gameObject
                flock.IsFoodSpawned = false;
                Destroy(gameObject);
            }
        }
    }
}
