using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MushroomController : MonoBehaviour
{
    private Vector2 velocity;
    private Vector2 currentDirection;
    private Rigidbody2D mushroomBody;
    private Transform mushroomTransform;
    private int moveRight = 5;
    private float time = 0f;

    void Start()
    {
        mushroomBody = GetComponent<Rigidbody2D>();
        mushroomTransform = GetComponent<Transform>();
        //mushroomBody.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        // ComputeVelocity();
    }

    // void ComputeVelocity()
    // {
    //     velocity = new Vector2((moveRight), 0);
    // }

    // Update is called once per frame
    void Update()
    {
        if (time < 1f)
        {
            time += Time.deltaTime;
            //mushroomTransform.Translate(0, Time.deltaTime*(1), 0);
            mushroomTransform.position = Vector3.MoveTowards(
                mushroomTransform.position,
                mushroomTransform.position + new Vector3(0, 1, 0),
                Time.deltaTime)
            ;
        }
        else
        {
            mushroomBody.velocity = new Vector2(moveRight, mushroomBody.velocity.y);
        }
        // moveMushroom();
    }

    // called when the cube hits the floor

    void OnCollisionEnter2D(Collision2D col)
    {
        if (Mathf.Abs(col.contacts[0].normal.y) < 0.3f && col.gameObject.CompareTag("Obstacles"))
        {
            moveRight *= -1;
        }
        if (Mathf.Abs(col.contacts[0].normal.y) < 0.3f && col.gameObject.CompareTag("Enemy"))
        {
            moveRight *= -1;
        }
        // if (col.contacts[0].normal.y > 0.5f)
        // {
        //     // Collision from the top
        //     Debug.Log("Collision from the top");
        // }
        // if (Mathf.Abs(col.contacts[0].normal.y) < 0.3f)
        // {
        //     // Collision from the side
        //     Debug.Log("Collision from the side");
        //     if (col.gameObject.CompareTag("Obstacles"))
        //     {
        //         moveRight *= -1;
        //         // ComputeVelocity();
        //         // moveMushroom();
        //     }
        // }
        if (col.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
