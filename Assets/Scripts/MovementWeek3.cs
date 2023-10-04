using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementWeek3 : MonoBehaviour
{
    public GameManager gameManager;
    public MusicManager musicManager;

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

    private int deathCount;

    private bool moving = false;
    private bool jumpedState = false;

    // Start is called before the first frame update
    void Start()
    { // Set to be 30 FPS
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        marioAnimator = GetComponent<Animator>();
        marioAudioSource = GetComponent<AudioSource>();
        marioAnimator.SetBool("onGround", onGroundState);
    }

    void Update()
    {
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));
    }

    void FixedUpdate()
    {
        if (alive && moving)
        {
            Move(faceRightState == true ? 1 : -1);
        }
    }

    void Move(int value)
    {
        Vector2 movement = new Vector2(value, 0);
        // check if it doesn't go beyond maxSpeed
        if (marioBody.velocity.magnitude < maxSpeed)
            marioBody.AddForce(movement * speed);
    }

    public void MoveCheck(int value)
    {
        if (value == 0)
        {
            moving = false;
        }
        else
        {
            FlipMarioSprite(value);
            moving = true;
            Move(value);
        }
    }

    void FlipMarioSprite(int value)
    {
        if (value == -1 && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;
            if (marioBody.velocity.x > 0.05f)
                marioAnimator.SetTrigger("onSkid");
        }
        else if (value == 1 && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;
            if (marioBody.velocity.x < -0.05f)
                marioAnimator.SetTrigger("onSkid");
        }
    }

    public void Jump()
    {
        if (alive && onGroundState)
        {
            // jump
            marioBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            onGroundState = false;
            jumpedState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
        }
    }

    public void JumpHold()
    {
        if (alive && jumpedState)
        {
            // jump higher
            marioBody.AddForce(Vector2.up * jumpForce * 30, ForceMode2D.Force);
            jumpedState = false;
        }
    }
}
