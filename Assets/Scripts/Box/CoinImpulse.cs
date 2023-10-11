using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoinImpulse : MonoBehaviour
{
    [SerializeField]
    private AudioSource coinAudioSource;
    public AudioClip coinSound;
    public float coinImpulse = 7;
    public float maxLifetime = 1.5f;
    public float delay = 0.5f;
    private float lifetime = 0;
    private Rigidbody2D coinBody;
    private Transform coinTransform;
    private bool coinSpawned = false;
    private Animator coinAnimator;

    void Start()
    {
        coinAnimator = GetComponent<Animator>();
        coinTransform = GetComponent<Transform>();
        coinBody = GetComponent<Rigidbody2D>();
        coinAudioSource = GetComponent<AudioSource>();
        coinBody.AddForce(Vector2.up * coinImpulse, ForceMode2D.Impulse);

        GameManager.instance.gameRestart.AddListener(gameRestart);
    }

    void Update()
    {
        lifetime += Time.deltaTime;

        if (lifetime > maxLifetime)
        {
            coinBody.constraints = RigidbodyConstraints2D.FreezeAll;
            coinTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            coinAnimator.enabled = false;
            if (coinSpawned == false)
            {
                coinAudioSource.PlayOneShot(coinSound);
                coinSpawned = true;
            }
            if ((lifetime > maxLifetime + delay) & coinSpawned == true)
            {
                Destroy(gameObject);
            }
        }
    }

    void gameRestart()
    {
        coinSpawned = false;
        lifetime = 0;
        coinTransform.localScale = new Vector3(1, 1, 1);
        coinBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        coinBody.constraints = RigidbodyConstraints2D.FreezePositionX;
        coinAnimator.enabled = true;
        coinTransform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
    }
}
