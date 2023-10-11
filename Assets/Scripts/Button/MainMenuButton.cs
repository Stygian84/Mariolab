using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour, IInteractiveButton
{
    public IntVariable gameScore;
    public GameObject mario;
    public GameManager gameManager;

    // implements the interface
    public void ButtonClick()
    {
        SceneManager.LoadScene("StartGame");
        Destroy(mario);
        Destroy(gameManager);
        Time.timeScale = 1.0f;
        gameScore.Value = 0;
    }
}