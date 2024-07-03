using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Mixer")]
    public AudioMixer mixer;

    [Header("Audio Clips")]
    public AudioClip bgmClip;

    public AudioClip jumpClip;

    public AudioClip longJumpClip;

    public AudioClip deadClip;

    [Header("Audio Source")]
    public AudioSource bgmMusic;
    public AudioSource fx;
    private void Awake()
    {
        if(instance==null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        bgmMusic.clip = bgmClip;
        PlayMusic();
    }

    private void OnEnable()
    {
        EventHandler.GameOverEvent += OnGameOverEvent;
    }
    private void OnDisable()
    {
        EventHandler.GameOverEvent -= OnGameOverEvent;
    }

    private void OnGameOverEvent()
    {
        fx.clip=deadClip;
        fx.Play();
    }

    /// <summary>
    /// ������Ծ����ЧƬ��
    /// </summary>
    /// <param name="type">0С����1����</param>
    public void SetJumpClip(int type)
    {
        switch (type) 
        {
            case 0:
                fx.clip = jumpClip; 
                break;
            case 1:
                fx.clip = longJumpClip; 
                break;
        }
        //fx.clip = type == 0 ? jumpClip : longJumpClip;
    }

    public void PlayJumpFX()
    {
        fx.Play();
    }

    public void PlayMusic()
    {
        if(!bgmMusic.isPlaying)
        {
            bgmMusic.Play();
        }
    }

    public void ToggleAudio(bool isOn)
    {
        if(isOn)
        {
            mixer.SetFloat("masterVolume", 0);
        }
        else
        {
            mixer.SetFloat("masterVolume", -80);
        }
    }
}
