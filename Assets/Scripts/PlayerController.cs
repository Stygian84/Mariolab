using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
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

    [Header("RestartConfig")]
    public AudioClip marioDeath;
    public AudioSource bgMusic;
    public float deathImpulse = 7;
    public TextMeshProUGUI scoreText;
    public JumpOverGoomba jumpOverGoomba;
    public GameObject DeathOverlay;
    public TextMeshProUGUI finalScoreText;
    public GameObject RestartButton;
    public GameObject enemies;
    public GameObject mainCamera;

    // state
    [System.NonSerialized]
    public bool alive = true;
    int collisionLayerMask = (1 << 6) | (1 << 9) | (1 << 10);
    private float dirX;

    // Start is called before the first frame update
    void Start()
    {
        DeathOverlay.gameObject.SetActive(false);
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

    void Update()
    {
        //WallSlide();
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
            marioAnimator.SetTrigger("die");
            bgMusic.enabled = false;
            marioAudioSource.PlayOneShot(marioDeath);
            PlayDeathImpulse();
            alive = false;
        }
    }

    void Die()
    {
        Time.timeScale = 0.0f;
        DeathOverlay.gameObject.SetActive(true);
        finalScoreText.text = "Score: " + jumpOverGoomba.score.ToString();
        scoreText.enabled = false;
        RestartButton.SetActive(false);
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

    void PlayDeathImpulse()
    {
        marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
    }

    public void RestartButtonCallback(int input)
    {
        Debug.Log("Restart!");
        // reset everything
        // ResetGame();
        RestartLevel();
        // resume time
        Time.timeScale = 1.0f;
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ResetGame()
    {
        // reset position
        marioBody.transform.position = new Vector3(-4.37f, -1.39f, 0.0f);
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;
        // reset score
        scoreText.enabled = true;
        scoreText.text = "Score : 0";
        RestartButton.SetActive(true);
        // reset Goomba
        foreach (Transform eachChild in enemies.transform)
        {
            eachChild.transform.localPosition = eachChild
                .GetComponent<EnemyController>()
                .startPosition;
        }
        mainCamera.transform.localPosition = new Vector3(0f, 0f, -10f);
        // reset animation
        alive = true;
        marioAnimator.ResetTrigger("die");
        marioAnimator.Play("idle");
        // reset score
        jumpOverGoomba.score = 0;
        DeathOverlay.gameObject.SetActive(false);
        bgMusic.enabled = true;
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
