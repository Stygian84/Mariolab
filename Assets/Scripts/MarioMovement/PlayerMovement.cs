using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [Header("Layer")]
    [SerializeField]
    private LayerMask jumpableGround;

    [SerializeField]
    private LayerMask wallLayer;

    [SerializeField]
    private LayerMask obstaclesLayer;

    [Header("PlayerConfig")]
    public float speed = 7f;
    public float wallSlidingSpeed = 2f;
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


    // state
    [System.NonSerialized]
    public bool alive = true;
    private bool moving = false;
    private bool jumpedState = false;
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

    void FlipMarioSprite(int value)
    {
        if (value == -1 && faceRightState)
        {
            faceRightState = false;
            transform.localScale = Vector3.one;
            if (marioBody.velocity.x < 0.5f)
                marioAnimator.SetTrigger("onSkid");
        }
        else if (value == 1 && !faceRightState)
        {
            faceRightState = true;
            transform.localScale = new Vector3(-1, 1, 1);
            if (marioBody.velocity.x > -0.5f)
                marioAnimator.SetTrigger("onSkid");
        }
    }

    // FixedUpdate may be called once per frame. See documentation for details.
    void FixedUpdate()
    {
        if (alive && moving)
        {
            Move(faceRightState == true ? 1 : -1);
        }
    }

    void Update()
    {
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));
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

    void OnCollisionEnter2D(Collision2D col)
    {
        if (((collisionLayerMask & (1 << col.transform.gameObject.layer)) > 0) & !onGroundState)
            if (col.contacts[0].normal.y > 0.5f)
            {
                onGroundState = true;
                // update animator state
                marioAnimator.SetBool("onGround", onGroundState);
            }
    }

    void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.gameObject.CompareTag("Enemy") && alive)
        {
            Debug.Log("Collided with goomba!");
            marioAnimator.SetTrigger("die");
            alive = false;
        }
    }

    void Die()
    {
        Time.timeScale = 0.0f;
    }

    void PlayJumpSound()
    {
        marioAudioSource.PlayOneShot(marioAudioSource.clip);
    }

    private bool onObstacles()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            coll.bounds.center,
            coll.bounds.size,
            0f,
            new Vector2(transform.localScale.x, 0),
            .1f,
            obstaclesLayer
        );
        return raycastHit.collider != null;
    }

    public void RestartButtonCallback(int input)
    {
        Debug.Log("Restart!");
        // reset everything
        ResetGame();
        // resume time
        Time.timeScale = 1.0f;
    }

    private void ResetGame()
    {
        // reset position
        marioBody.transform.position = new Vector3(-4.37f, -1.39f, 0.0f);
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;
        
    }

    // void WallSlide()
    // {
    //     if (onObstacles() && !onGroundState)
    //     {
    //         marioBody.velocity = new Vector2(
    //             marioBody.velocity.x,
    //             Mathf.Clamp(marioBody.velocity.y, -wallSlidingSpeed, float.MaxValue)
    //         );
    //     }
    // }

    // private bool IsGrounded()
    // {
    //     RaycastHit2D raycastHit = Physics2D.BoxCast(
    //         coll.bounds.center,
    //         coll.bounds.size,
    //         0f,
    //         Vector2.down,
    //         .1f,
    //         collisionLayerMask
    //     );
    //     return raycastHit.collider != null;
    // }
}
