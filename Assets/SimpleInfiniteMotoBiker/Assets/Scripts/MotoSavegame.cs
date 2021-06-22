using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotoSavegame : MonoBehaviour {

    //Name of the save keys
    private string currentScoreKey = "MotoCurrentScore";
    private string highScoreKey = "MotoHighScore";

    public string prefixScore = "Score: ";
    public string prefixHighScore = "Highscore: ";


    public void saveScore(int score)
    {
        //Save Current player score
        PlayerPrefs.SetInt(currentScoreKey, score);

        if (PlayerPrefs.HasKey(highScoreKey))
        {
            int highscore = PlayerPrefs.GetInt(highScoreKey);

            //Check the highscore to be less than score to renew the Highscore
            if (highscore < score)
                PlayerPrefs.SetInt(highScoreKey, score);
        }
        else
        {
            PlayerPrefs.SetInt(highScoreKey, score);
        }
    }

    public int loadScore()
    {
        if (PlayerPrefs.HasKey(currentScoreKey))
        {
            return PlayerPrefs.GetInt(currentScoreKey);
        }
        else
        {
            return 0;
        }
    }

    public int loadHighScore()
    {
        if (PlayerPrefs.HasKey(highScoreKey))
        {
            return PlayerPrefs.GetInt(highScoreKey);
        }
        else
        {
            return 0;
        }
    }
}
