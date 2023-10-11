using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetHighScore : MonoBehaviour, IInteractiveButton
{
    //public GameConstants gameConstants;
    public IntVariable gameScore;

    public void ButtonClick()
    {
        //gameConstants.topScore = 0;
        gameScore.ResetHighestValue();
    }
}
