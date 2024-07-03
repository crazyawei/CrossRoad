using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.InputSystem.Utilities;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; 

    public List<int> scoreList;

    public List<string> nameList;

    private int score;

    private string playername;

    private string dataPath;

    private string dataPath2;

    private void Awake()
    {
        dataPath = Application.persistentDataPath + "/leaderboard.json";
        dataPath2 = Application.persistentDataPath + "/playername.json";
        scoreList =GetScoreListData();
        nameList=GetNameListData();
        if (instance==null)
            instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        EventHandler.GameOverEvent += OnGameOverEvent;
        EventHandler.GetPointEvent += OnGetPointEvent;
        EventHandler.GetNameEvent += OngetNameEvent;
    }

    private void OnDisable()
    {
        EventHandler.GameOverEvent -= OnGameOverEvent;
        EventHandler.GetPointEvent -= OnGetPointEvent;
        EventHandler.GetNameEvent -= OngetNameEvent;
    }

    private void OngetNameEvent(string name)
    {
        playername = name;
    }

    private void OnGetPointEvent(int point)
    {
        score=point;
    }
    private void OnGameOverEvent()
    {

        //todo:在list中添加新的分数，排序
        if(!scoreList.Contains(score))
        {
            scoreList.Add(score);
            nameList.Add(playername);
        }

        scoreList.Sort();
        scoreList.Reverse();
   
        File.WriteAllText(dataPath,JsonConvert.SerializeObject(scoreList));
        File.WriteAllText(dataPath2, JsonConvert.SerializeObject(nameList));
    }

    //读取保存数据的记录
    public List<int> GetScoreListData()
    {
        if(File.Exists(dataPath))
        {
            string jsonData=File.ReadAllText(dataPath);
            return JsonConvert.DeserializeObject<List<int>>(jsonData);
        }
        return new List<int>();
    }

    public List<string> GetNameListData()
    {
        if (File.Exists(dataPath2))
        {
            string jsonData = File.ReadAllText(dataPath2);
            return JsonConvert.DeserializeObject<List<string>>(jsonData);
        }
        return new List<string>();
    }
}
