using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillGoomba : MonoBehaviour
{
    public GameManager gameManager;

    [System.NonSerialized]
    public int score = 0; // we don't want this to show up in the inspector

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    void FixedUpdate() { }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy") && col.contacts[0].normal.y < -0.5f)
        {
            gameManager.IncreaseScore(1);
        }
    }
}
