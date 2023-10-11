using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : Singleton<MusicManager>
{
    public static MusicManager Instance;
    public AudioMixer audioMixer;
    private float pitchValue;
    private int deathCount;

    // Start is called before the first frame update
    void Awake()
    {
        deathCount = PlayerPrefs.GetInt("Death");
    }

    void Start()
    {
        audioMixer.SetFloat("Pitch", 1 - deathCount * 0.05f);
    }

    // Update is called once per frame
    void Update() { }
}
