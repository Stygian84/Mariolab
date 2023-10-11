using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MushroomController : MonoBehaviour
{
    private Rigidbody2D mushroomBody;
    private Transform mushroomTransform;
    private int moveRight = 5;
    private float time = 0f;

    void Start()
    {
        mushroomBody = GetComponent<Rigidbody2D>();
        mushroomTransform = GetComponent<Transform>();
        GameManager.instance.gameRestart.AddListener(gameRestart);
    }

    // Update is called once per frame
    void Update()
    {
        if (time < 1f)
        {
            time += Time.deltaTime;
            mushroomTransform.position = Vector3.MoveTowards(
                mushroomTransform.position,
                mushroomTransform.position + new Vector3(0, 1, 0),
                Time.deltaTime
            );
        }
        else
        {
            mushroomBody.velocity = new Vector2(moveRight, mushroomBody.velocity.y);
        }
    }

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

        if (col.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    void gameRestart()
    {
        time = 0f;
        Destroy(gameObject);
        mushroomBody.velocity = new Vector2(0, 0);
        moveRight = 5;
    }
}
