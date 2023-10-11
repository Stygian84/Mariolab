using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinConsumable : MonoBehaviour
{
   
    [SerializeField]
    private AudioSource coinAudioSource;
    public AudioClip coinSound;
    private Transform coinTransform;
    private Animator coinAnimator;
    private Vector3 initialPos;
    void Start()
    {
        coinAnimator = GetComponent<Animator>();
        coinTransform = GetComponent<Transform>();
        coinAudioSource = GetComponent<AudioSource>();
        initialPos = coinTransform.position;
        GameManager.instance.gameRestart.AddListener(gameRestart);
    }

    void Update(){
        
    }
     void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            coinAudioSource.PlayOneShot(coinSound);
            gameObject.SetActive(false);
            GameManager.instance.IncreaseScore(1);
        }
    }

     void gameRestart()
    {
        gameObject.SetActive(true);
        coinAnimator.enabled = true;
        coinTransform.position = initialPos;
    }
}
