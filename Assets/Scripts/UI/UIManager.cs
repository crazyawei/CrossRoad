using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Text scoreText;

    public GameObject gameOverPanel;

    public GameObject leaderboardPanel;

    private void OnEnable()
    {
        Time.timeScale = 1f;
        EventHandler.GetPointEvent += OnGetPointEvent;
        EventHandler.GameOverEvent += OnGameOverEvnet;
    }

    private void OnDisable()
    {
        EventHandler.GetPointEvent -= OnGetPointEvent;
        EventHandler.GameOverEvent -= OnGameOverEvnet;
    }

   

    private void Start()
    {
        scoreText.text = "00";
    }

    private void OnGetPointEvent(int point)
    {
        scoreText.text=point.ToString();
    }

    private void OnGameOverEvnet()
    {
        gameOverPanel.SetActive(true);
        if(gameOverPanel.activeInHierarchy)
        {
            Time.timeScale = 0;
        }
    }


    #region 按钮添加方法
    public void RestartGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void BackMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void OpenLeaderBoard()
    {
        leaderboardPanel.SetActive(true);
    }
    #endregion
}
