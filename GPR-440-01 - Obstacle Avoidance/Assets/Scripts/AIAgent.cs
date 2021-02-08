using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AIAgent : MonoBehaviour
{
    //Steering information
    [Header("Steering Information")]
    [Range(1.0f, 10.0f)] public float moveSpeed = 5f;
    public CompositeSteering compositeSteering;

    //Health
    [Header("Agent Health Info")]
    public int startingHealth = 20;
    public int hitDamageTaken = 5;
    public TextMeshProUGUI healthDisplayText;
    public TextMeshProUGUI deathCountDisplayText;

    //For calculations
    Vector2 headingVector;
    int currentHealth;
    int numOfDeaths;
    Vector3 startingPosition;

    //Damage-related
    SpriteRenderer spriteRenderer;
    Color originalColour;
    Animator cameraAnimator;

    void Start()
    {
        //Start with the max health
        currentHealth = startingHealth;

        //No deaths
        numOfDeaths = 0;

        //Display starting health and death count
        healthDisplayText.text = "Health: " + currentHealth;
        deathCountDisplayText.text = "Death count: " + numOfDeaths;

        //Starting position
        startingPosition = transform.position;

        //Sprite renderer and original colour
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColour = spriteRenderer.color;

        //Camera animator
        cameraAnimator = FindObjectOfType<Camera>().GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Move the agent
        headingVector = compositeSteering.GetSteering(this);
        transform.up = headingVector;
        transform.position += (Vector3)headingVector * moveSpeed * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Take damage
        currentHealth -= hitDamageTaken;

        //Update damage text
        healthDisplayText.text = "Health: " + currentHealth;

        //Show that damage was taken
        StartCoroutine(IndicateDamage());

        //Camera shake
        int randomCameraShake = Random.Range(0, 2);
        if (randomCameraShake == 0)
            cameraAnimator.SetTrigger("cameraShake0");
        else
            cameraAnimator.SetTrigger("cameraShake1");

        //If they die
        if (currentHealth <= 0)
        {
            //Reset position
            transform.position = startingPosition;

            //Random rotation
            transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

            //Camera reset animation
            cameraAnimator.SetTrigger("cameraReset");

            //Reset health
            currentHealth = startingHealth;

            //Update death count
            numOfDeaths++;

            //Update texts
            healthDisplayText.text = "Health: " + currentHealth;
            deathCountDisplayText.text = "Death count: " + numOfDeaths;
        }
    }

    IEnumerator IndicateDamage()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColour;
    }
}
