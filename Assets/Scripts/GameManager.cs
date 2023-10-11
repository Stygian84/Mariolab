using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    // [Serializable]
    // public class IntUnityEvent : UnityEvent<int> { }
    //public GameConstants gameConstants;
    public IntVariable gameScore;

    // events
    public UnityEvent gameStart;
    public UnityEvent gameRestart;
    public UnityEvent<int> scoreChange;

    // public IntUnityEvent scoreChange;
    public UnityEvent gameOver;

    private int score = 0;

    void Start()
    {
        gameStart.Invoke();
        Time.timeScale = 1.0f;
        // subscribe to scene manager scene change
        SceneManager.activeSceneChanged += SceneSetup;
    }

    public void SceneSetup(Scene current, Scene next)
    {
        gameStart.Invoke();
        SetScore(score);
    }

    public void GameRestart()
    {
        // reset score
        score = 0;
        SetScore(score);
        gameScore.Value = 0;

        //gameConstants.currentScore = 0;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameRestart.Invoke();
        Time.timeScale = 1.0f;
    }

    public void IncreaseScore(int increment)
    {
        score += increment;
        gameScore.ApplyChange(1);
        SetScore(score);
    }

    private void SetScore(int score)
    {
        scoreChange.Invoke(gameScore.Value);
        
    }

    public void GameOver()
    {
        Time.timeScale = 0.0f;
        gameOver.Invoke();
    }
}
