using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public InputField fields;
    public string playname;

    public void GetPlayName()
    {
        playname=fields.text;
        Debug.Log(playname);
        EventHandler.CallGetNameEvent(playname);
        SceneManager.LoadScene("Gameplay");     //启动游戏加载游戏场景
        
    }

}
