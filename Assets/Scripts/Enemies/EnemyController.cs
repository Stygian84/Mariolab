using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private float originalX;
    private float maxOffset = 5.0f;
    private float enemyPatroltime = 2.0f;
    private bool alive = true;
    private float lifetime = 0;
    private int moveRight = -1;
    private Vector2 velocity;
    private Animator goombaAnimator;

    private Rigidbody2D enemyBody;
    private BoxCollider2D boxCollider2D;
    public Vector3 startPosition = new Vector3(0.0f, 0.0f, 0.0f);

    [Header("Stomp")]
    [SerializeField]
    private AudioSource goombaAudioSource;
    public AudioClip stompSound;

    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        goombaAnimator = GetComponent<Animator>();
        enemyBody = GetComponent<Rigidbody2D>();

        // get the starting position
        originalX = transform.position.x;
        ComputeVelocity();
        GameManager.instance.gameRestart.AddListener(GameRestart);
    }

    void ComputeVelocity()
    {
        velocity = new Vector2((moveRight) * maxOffset / enemyPatroltime, 0);
    }

    void Movegoomba()
    {
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(enemyBody.position.x - originalX) < maxOffset)
        { // move goomba
            Movegoomba();
        }
        else
        {
            // change direction
            moveRight *= -1;
            ComputeVelocity();
            Movegoomba();
        }
        if (alive == false)
        {
            lifetime += Time.deltaTime;
            if (lifetime >= 1.5f)
            {
                gameObject.SetActive(false);
                //Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) { }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.contacts[0].normal.y < -0.5f)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                goombaAudioSource.PlayOneShot(stompSound);
                goombaAnimator.SetTrigger("die");
                this.transform.Translate(0, -0.3f, 0);

                enemyBody.constraints = RigidbodyConstraints2D.FreezeAll;
                boxCollider2D.enabled = false;

                alive = false;
                GameManager.instance.IncreaseScore(1);
            }
        }
        if (Mathf.Abs(col.contacts[0].normal.y) < 0.2f)
        { //collision from side
            if (col.gameObject.CompareTag("Obstacles"))
            {
                moveRight *= -1;
                // ComputeVelocity();
                // moveMushroom();
            }
        }
    }

    public void GameRestart()
    {
        gameObject.SetActive(true);
        transform.localPosition = startPosition;
        boxCollider2D.enabled = true;
        originalX = transform.position.x;
        moveRight = -1;
        enemyBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        ComputeVelocity();
        alive = true;
        lifetime = 0;
        goombaAnimator.SetTrigger("gameRestart");
    }
}
