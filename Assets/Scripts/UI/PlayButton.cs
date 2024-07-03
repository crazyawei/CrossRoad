using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    private Button button;

    public GameObject nameInputPanel;

    public Button confirmButton;

    public InputField inputfield;

    private void Awake()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(StartGame); 
    }

    private void StartGame()
    {
       nameInputPanel.SetActive(true);
    }
}
