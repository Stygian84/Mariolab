using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public GameObject Score;
    public GameObject RestartButton;
    public GameObject DeathOverlay;
    public GameObject ScoreText;
    public GameObject FinalScoreText;

    public GameObject highscoreText;
    public IntVariable gameScore;

    void Awake()
    {
        // other instructions
        // subscribe to events
        GameManager.instance.gameStart.AddListener(GameStart);
        GameManager.instance.gameOver.AddListener(GameOver);
        GameManager.instance.gameRestart.AddListener(GameStart);
        GameManager.instance.scoreChange.AddListener(SetScore);
    }

    public void GameStart()
    {
        // hide gameover panel
        DeathOverlay.SetActive(false);
        Score.SetActive(true);
        RestartButton.SetActive(true);
        ScoreText.GetComponent<TextMeshProUGUI>().text = "Score : 0";
    }

    public void SetScore(int score)
    {
        ScoreText.GetComponent<TextMeshProUGUI>().text = "Score : " + gameScore.Value.ToString();
        FinalScoreText.GetComponent<TextMeshProUGUI>().text = "Score : " + gameScore.Value.ToString();
    }

    public void GameOver()
    {
        // hide gameover panel
        DeathOverlay.SetActive(true);
        Score.SetActive(false);
        RestartButton.SetActive(false);

        // set highscore
        highscoreText.GetComponent<TextMeshProUGUI>().text =
            "TOP- " + gameScore.previousHighestValue.ToString("D6");
        // show
        highscoreText.SetActive(true);
    }
}
