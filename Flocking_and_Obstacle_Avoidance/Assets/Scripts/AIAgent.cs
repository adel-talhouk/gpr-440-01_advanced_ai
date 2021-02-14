using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AIAgent : MonoBehaviour
{
    //The neighbourhood
    Collider2D col;
    public Collider2D Neighbourhood { get { return col; } }

    void Start()
    {
        //Get the collider 
        col = GetComponent<Collider2D>();

        //Make it a random colour
        Color randomColour = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        GetComponent<SpriteRenderer>().color = randomColour;
    }

    public void Move(Vector2 direction)
    {
        //Move the in the right direction
        transform.up = direction;
        transform.position += (Vector3)direction * Time.deltaTime;
    }
}
