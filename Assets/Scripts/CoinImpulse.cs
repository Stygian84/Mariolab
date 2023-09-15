using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinImpulse : MonoBehaviour
{
    [SerializeField]
    private AudioSource coinAudioSource;
    public AudioClip coinSound;
    public float coinImpulse = 7;
    public float maxLifetime = 2;
    private float lifetime = 0;
    private Rigidbody2D coinBody;

    void Start()
    {
        coinBody = GetComponent<Rigidbody2D>();
        coinAudioSource = GetComponent<AudioSource>();
        coinBody.AddForce(Vector2.up * coinImpulse, ForceMode2D.Impulse);
    }

    void Update()
    {
        lifetime += Time.deltaTime;
        if (lifetime > maxLifetime)
        {
            coinAudioSource.PlayOneShot(coinSound);
            Destroy(gameObject);
        }
    }
}
