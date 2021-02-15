using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AIAgent : MonoBehaviour
{
    //Components
    Collider2D col;
    LineRenderer lineRenderer;

    //Accessors
    public Collider2D Neighbourhood { get { return col; } }
    public LineRenderer LineRend { get { return lineRenderer; } }

    void Awake()
    {
        //Get the components
        col = GetComponent<Collider2D>();
        lineRenderer = GetComponent<LineRenderer>();

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
