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

    public void GameStart()
    {
        // hide gameover panel
        DeathOverlay.SetActive(false);
        Score.SetActive(true);
        RestartButton.SetActive(true);
    }

    public void SetScore(int score)
    {
        ScoreText.GetComponent<TextMeshProUGUI>().text = "Score : " + score.ToString();
        FinalScoreText.GetComponent<TextMeshProUGUI>().text = "Score : " + score.ToString();
    }

    public void GameOver()
    {
        // hide gameover panel
        DeathOverlay.SetActive(true);
        Score.SetActive(false);
        RestartButton.SetActive(false);
    }
}
