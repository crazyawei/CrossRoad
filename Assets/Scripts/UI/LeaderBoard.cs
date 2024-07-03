using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{
    public List<ScoreRecord> scoreRecords;

    private List<int> scoreList;

    private List<string> playerList;

    [Header("¹ã¸æ°´Å¥")]
    public Button adsButton;


    private void OnEnable()
    {
        scoreList=GameManager.instance.GetScoreListData();
        playerList=GameManager.instance.GetNameListData();

        adsButton.onClick.AddListener(AdsManager.instance.ShowRewardAds);
    }
    private void Start()
    {
        SetLeaderboardData();
    }

    public void SetLeaderboardData()
    {
        for (int i = 0; i < scoreRecords.Count; i++) 
        {
            if(i<scoreList.Count)
            {
                scoreRecords[i].SetScoreText(scoreList[i]);
                scoreRecords[i].SetPlayerName(playerList[i]);
                Debug.Log(playerList[i]);
                scoreRecords[i].gameObject.SetActive(true);
            }
            else
            {
                scoreRecords[i].gameObject.SetActive(false);
            }
        }
    }

    public void GameChange()
    {
        gameObject.SetActive(false);
    }
}
