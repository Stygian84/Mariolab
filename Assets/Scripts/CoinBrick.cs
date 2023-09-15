using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBrick : MonoBehaviour
{
    public Transform initialPosition;
    public GameObject coinPrefab;
    public int coin;

    void Start()
    {
    }

    void Update() { }

    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.GetType() == typeof(BoxCollider2D))
        {
            if (col.gameObject.CompareTag("Player") && col.contacts[0].normal.y > 0.5f)
            {
                if (coin < 1)
                {
                    ShootCoin();
                    coin -= 1;
                }
            }
        }
    }

    void ShootCoin()
    {
        Instantiate(coinPrefab, initialPosition.position, initialPosition.rotation);
    }
}
