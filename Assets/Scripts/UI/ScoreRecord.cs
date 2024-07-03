using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreRecord : MonoBehaviour
{
    public Text scoreText;

    public Text playerName;
    public void SetScoreText(int point)
    {
        scoreText.text = point.ToString();
    }

    public void SetPlayerName(string name) 
    {
        playerName.text = name.ToString();  
    }
}
