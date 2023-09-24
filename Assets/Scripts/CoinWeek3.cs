using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CoinWeek3 : MonoBehaviour
{
    public Transform initialPosition;
    public GameObject coinPrefab;
    public int lives;
    public int coin;

    private Animator qbAnimator;
    private SpringJoint2D springJoint2D;
    private bool active = true;
    public GameObject coinObject;

    private float initialDistance;
    private float initialDamping;
    private float initialFrequency;
    private float time = 0;

    void Start()
    {
        qbAnimator = GetComponent<Animator>();
        springJoint2D = GetComponent<SpringJoint2D>();
        initialDistance = springJoint2D.distance;
        initialDamping = springJoint2D.dampingRatio;
        initialFrequency = springJoint2D.frequency;
        time = 0f;
    }

    void Update()
    {
        if (lives < 1)
        {
            time += Time.deltaTime;
            if (time > 0.1f)
            {
                qbAnimator.SetTrigger("death");
                killSpringJoint2D();
                active = false;
            }
        }
    }

    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.GetType() == typeof(BoxCollider2D))
        {
            if (col.gameObject.CompareTag("Player") && col.contacts[0].normal.y > 0.5f)
            {
                if (active == true && coin > 0)
                {
                    ShootCoin();
                    coin -= 1;
                    lives -= 1;
                }
            }
        }
    }

    void ShootCoin()
    {
        coinObject.SetActive(true);
    }

    void killSpringJoint2D()
    {
        springJoint2D.distance = 0;
        springJoint2D.dampingRatio = 10;
        springJoint2D.frequency = 0;
    }
}
