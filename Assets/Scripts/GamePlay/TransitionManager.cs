using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    public float scaler;
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        DontDestroyOnLoad(gameObject);

    }


    private void Start()
    {
        StartCoroutine(Fade(0));
    }
    private IEnumerator Fade(int amount)
    {
        _canvasGroup.blocksRaycasts = true;

        while(_canvasGroup.alpha!=amount)
        {
            switch(amount)
            {
                case 1:
                    _canvasGroup.alpha += Time.deltaTime*scaler;
                    break;
                case 0:
                    _canvasGroup.alpha -= Time.deltaTime * scaler;
                    break;
            }
        yield return null;
        }
        _canvasGroup.blocksRaycasts = false;
    }
}
