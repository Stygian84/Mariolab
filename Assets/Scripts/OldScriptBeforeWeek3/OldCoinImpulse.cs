using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OldCoinImpulse : MonoBehaviour
{
    [SerializeField]
    private AudioSource coinAudioSource;
    public AudioClip coinSound;
    public float coinImpulse = 7;
    public float maxLifetime = 2;
    public float delay = 3;
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
}
