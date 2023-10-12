
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButtonController : MonoBehaviour, IInteractiveButton
{
    private bool isPaused = false;
    public Sprite pauseIcon;
    public Sprite playIcon;
    public GameObject pauseOverlay;
    private AudioSource pauseSource;
    private Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        pauseSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ButtonClick()
    {
        Time.timeScale = isPaused ? 1.0f : 0.0f;
        isPaused = !isPaused;
        pauseSource.PlayOneShot(pauseSource.clip);
        if (isPaused)
        {
            image.sprite = playIcon;
            pauseOverlay.SetActive(true);
        }
        else
        {
            image.sprite = pauseIcon;
            pauseOverlay.SetActive(false);
        }
    }
}
