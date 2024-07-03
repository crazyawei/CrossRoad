using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class AdsManager :MonoBehaviour,IUnityAdsInitializationListener,IUnityAdsLoadListener,IUnityAdsShowListener
{
    public static AdsManager instance;

    private string gameID = "5645874";

    private string rewardPlacementID = "Rewarded_Android";

    private string interPlacementID = "Interstitial_Android";



    private void Awake()
    {
        if(instance==null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        Advertisement.Initialize(gameID, false, this);  //����ʼ��
    }

    public void ShowRewardAds()
    {
        Advertisement.Show(rewardPlacementID, this);
    }

    public void ShowInterAds()
    {
        Advertisement.Show(interPlacementID, this);
    }
    #region ��ʼ��
    public void OnInitializationComplete()
    {
        Debug.Log("����ʼ���ɹ�");
        Advertisement.Load(rewardPlacementID,this);  //Ԥ�ȼ��ع��
        Advertisement.Load(interPlacementID,this);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log("����ʼ��ʧ��"+message);
    }
    #endregion
    #region �����ʾ
    public void OnUnityAdsShowClick(string placementId)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        SceneManager.LoadScene("Gameplay");
        AudioManager.instance.bgmMusic.Play();
    }
   
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        throw new System.NotImplementedException();
    }
    public void OnUnityAdsShowStart(string placementId)
    {
        
        AudioManager.instance.bgmMusic.Stop();  //ֹͣ����
    }
    #endregion
    #region ������
    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("��棺" + placementId + "���سɹ�");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log("��棺" + placementId + "����ʧ��");
    }

    #endregion
}
