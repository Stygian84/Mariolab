using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomSpawner : MonoBehaviour
{
    public Transform initialPosition;
    public GameObject consumablePrefab;
    public int lives;
    private Rigidbody2D rigidBody;

    private Animator qbAnimator;
    private SpringJoint2D springJoint2D;
    private bool active = true;

    private float initialDistance;
    private float initialDamping;
    private float initialFrequency;
    private float time = 0;

    private AudioSource mushroomAudio;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        qbAnimator = GetComponent<Animator>();
        springJoint2D = GetComponent<SpringJoint2D>();
        mushroomAudio = GetComponent<AudioSource>();
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
            qbAnimator.SetTrigger("death");
            killSpringJoint2D();
            active = false;
        }
    }

    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.GetType() == typeof(BoxCollider2D))
        {
            if (col.gameObject.CompareTag("Player") && col.contacts[0].normal.y > 0.5f && active)
            {
                lives -= 1;
                mushroomAudio.PlayOneShot(mushroomAudio.clip);
                shootMushroom();
                qbAnimator.SetTrigger("death");
                killSpringJoint2D();
                active = false;
            }
        }
    }

    void shootMushroom()
    {
        // spawn the mushroom prefab slightly above the box
        Instantiate(
            consumablePrefab,
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
        qbAnimator.ResetTrigger("death");
        lives = 1;
        unkillSpringJoint2D();
        qbAnimator.SetTrigger("gameRestart");
    }
}
