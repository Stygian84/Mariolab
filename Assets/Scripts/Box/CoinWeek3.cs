using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CoinWeek3 : MonoBehaviour
{
    public GameObject coinPrefab;
    public Transform initialPosition;
    public int lives;
    public int coin;

    private Animator qbAnimator;
    private SpringJoint2D springJoint2D;
    private bool active = true;

    private float initialDistance;
    private float initialDamping;
    private float initialFrequency;
    private float time = 0;

    private bool coinIsShot = false;
    private float timetoincreasescore = 0;

    void Start()
    {
        qbAnimator = GetComponent<Animator>();
        springJoint2D = GetComponent<SpringJoint2D>();
        initialDistance = springJoint2D.distance;
        initialDamping = springJoint2D.dampingRatio;
        initialFrequency = springJoint2D.frequency;
        time = 0f;

        GameManager.instance.gameRestart.AddListener(gameRestart);
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
        if (coinIsShot)
        {
            timetoincreasescore += Time.deltaTime;
            if (timetoincreasescore>1){
                GameManager.instance.IncreaseScore(1);
                timetoincreasescore=0;
                coinIsShot=false;
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
                    coinIsShot = true;
                }
            }
        }
    }

    void ShootCoin()
    {
        //coinObject.SetActive(true);
        Instantiate(
            coinPrefab,
            new Vector3(
                this.transform.position.x,
                this.transform.position.y,
                this.transform.position.z
            ),
            Quaternion.identity
        );
    }

    void killSpringJoint2D()
    {
        springJoint2D.distance = 0;
        springJoint2D.dampingRatio = 10;
        springJoint2D.frequency = 0;
    }

    void unkillSpringJoint2D()
    {
        springJoint2D.distance = initialDistance;
        springJoint2D.dampingRatio = initialDamping;
        springJoint2D.frequency = initialFrequency;
    }

    void gameRestart()
    {
        time = 0f;
        active = true;
        coin = 1;
        coinIsShot = false;
        timetoincreasescore = 0;
        if (gameObject.name != "GameObject")
        {
            qbAnimator.ResetTrigger("death");
            lives = 1;
            unkillSpringJoint2D();
            qbAnimator.SetTrigger("gameRestart");
        }
    }
}
