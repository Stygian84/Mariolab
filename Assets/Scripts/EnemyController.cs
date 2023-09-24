using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private float originalX;
    private float maxOffset = 5.0f;
    private float enemyPatroltime = 2.0f;
    private bool alive=true;
    private float lifetime = 0;
    private int moveRight = -1;
    private Vector2 velocity;
    private Animator goombaAnimator;

    private Rigidbody2D enemyBody;
    public Vector3 startPosition = new Vector3(0.0f, 0.0f, 0.0f);
    [Header("Stomp")]
    [SerializeField]
    private AudioSource goombaAudioSource;
    public AudioClip stompSound;
    void Start()
    {
        goombaAnimator =GetComponent<Animator>();
        enemyBody = GetComponent<Rigidbody2D>();
        // get the starting position
        originalX = transform.position.x;
        ComputeVelocity();
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
        if (alive==false){
            lifetime+=Time.deltaTime;
            if (lifetime>=1.5f){
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.contacts[0].normal.y < -0.5f)
        {
            Debug.Log("a");
            if (col.gameObject.CompareTag("Player"))
            {
                
                goombaAudioSource.PlayOneShot(stompSound);
                goombaAnimator.SetTrigger("die");
                enemyBody.constraints = RigidbodyConstraints2D.FreezeAll;
                alive=false;
                
                //gameObject.transform.localScale -= new Vector3(0f, 1f, 0f);
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
        transform.localPosition = startPosition;
        originalX = transform.position.x;
        moveRight = -1;
        ComputeVelocity();
    }
}
