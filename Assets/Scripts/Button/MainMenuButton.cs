using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour, IInteractiveButton
{
    public IntVariable gameScore;

    // implements the interface
    public void ButtonClick()
    {
        SceneManager.LoadScene("StartGame");
        Destroy(GameObject.Find("Mario"));
        Destroy(GameObject.Find("GameManager"));
        Time.timeScale = 1.0f;
        gameScore.Value = 0;
    }
}
