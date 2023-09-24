using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameManager gameManager;

    [Header("Layer")]
    [SerializeField]
    private LayerMask jumpableGround;

    [Header("PlayerConfig")]
    public float speed = 7f;
    private Rigidbody2D marioBody;
    private BoxCollider2D coll;
    private Animator marioAnimator;

    public float jumpForce = 14f;
    public float maxSpeed = 10f;
    private bool onGroundState = true;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;

    [Header("JumpSound")]
    [SerializeField]
    private AudioSource marioAudioSource;

    [Header("RestartConfig")]
    public AudioClip marioDeath;
    public AudioSource bgMusic;
    public float deathImpulse = 7;
    public JumpOverGoomba jumpOverGoomba;
    public GameObject mainCamera;

    // state
    [System.NonSerialized]
    public bool alive = true;
    int collisionLayerMask = (1 << 6) | (1 << 9) | (1 << 10);
    private float dirX;

    // Start is called before the first frame update
    void Start()
    {
        // Set to be 30 FPS
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        marioAnimator = GetComponent<Animator>();
        marioAudioSource = GetComponent<AudioSource>();
        marioAnimator.SetBool("onGround", onGroundState);
    }

    // FixedUpdate may be called once per frame. See documentation for details.
    void FixedUpdate()
    {
        if (alive)
        {
            float dirX = Input.GetAxisRaw("Horizontal");
            marioBody.velocity = new Vector2(dirX * speed, marioBody.velocity.y);
            if (dirX > 0.01f)
            {
                transform.localScale = Vector3.one;
            }
            else if (dirX < -0.01f)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            if (Input.GetButtonDown("Jump") && onGroundState)
            {
                PlayJumpSound();
                marioBody.velocity = new Vector2(marioBody.velocity.x, jumpForce);
                onGroundState = false;
                marioAnimator.SetBool("onGround", onGroundState);
            }

            if (Input.GetKeyDown("a") && faceRightState)
            {
                faceRightState = false;
                if (marioBody.velocity.x < 0.5f)
                {
                    marioAnimator.SetTrigger("onSkid");
                }
            }

            if (Input.GetKeyDown("d") && !faceRightState)
            {
                faceRightState = true;
                if (marioBody.velocity.x > -0.5f)
                {
                    marioAnimator.SetTrigger("onSkid");
                }
            }

            marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));
        }
    }

    void Update() { }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (((collisionLayerMask & (1 << col.transform.gameObject.layer)) > 0) & !onGroundState)
            if (col.contacts[0].normal.y > 0.5f)
            {
                onGroundState = true;
                // update animator state
                marioAnimator.SetBool("onGround", onGroundState);
            }
        if (col.contacts[0].normal.y > 0.5f)
        {
            if (col.gameObject.CompareTag("Enemy") && alive)
            {
                marioBody.AddForce(Vector2.up * (jumpForce-2), ForceMode2D.Impulse);
            }
        }
        else
        {
            if (col.gameObject.CompareTag("Enemy") && alive)
            {
                marioAnimator.SetTrigger("die");
                bgMusic.enabled = false;
                marioAudioSource.PlayOneShot(marioDeath);
                PlayDeathImpulse();
                alive = false;

                gameManager.gameOver.Invoke();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D Other) { }

    void Die()
    {
        Time.timeScale = 0.0f;
    }

    void PlayJumpSound()
    {
        marioAudioSource.PlayOneShot(marioAudioSource.clip);
    }

    void PlayDeathImpulse()
    {
        marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameRestart()
    {
        // reset position
        marioBody.transform.position = new Vector3(-4.37f, -1.5f, 0.0f);
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;

        // reset animation
        marioAnimator.SetTrigger("gameRestart");
        alive = true;

        // reset camera position
        mainCamera.transform.position = new Vector3(0, 0, -10);
    }
}
