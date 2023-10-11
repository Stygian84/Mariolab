using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour, IInteractiveButton
{
    public IntVariable gameScore;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ButtonClick()
    {
        QuitGame();
    }

    public void QuitGame()
    {
        Debug.Log("Application Quit");
    }

    public void ReturntoMainMenu()
    {
        SceneManager.LoadScene("StartGame");
        Time.timeScale = 1.0f;
        gameScore.Value = 0;
    }
}
