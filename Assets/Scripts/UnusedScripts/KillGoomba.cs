using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillGoomba : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy") && col.contacts[0].normal.y < -0.5f)
        {
            Debug.Log("Goomba died");
            GameManager.instance.IncreaseScore(1);
        }
    }
}
