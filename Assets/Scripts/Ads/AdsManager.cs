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

        Advertisement.Initialize(gameID, false, this);  //广告初始化
    }

    public void ShowRewardAds()
    {
        Advertisement.Show(rewardPlacementID, this);
    }

    public void ShowInterAds()
    {
        Advertisement.Show(interPlacementID, this);
    }
    #region 初始化
    public void OnInitializationComplete()
    {
        Debug.Log("广告初始化成功");
        Advertisement.Load(rewardPlacementID,this);  //预先加载广告
        Advertisement.Load(interPlacementID,this);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log("广告初始化失败"+message);
    }
    #endregion
    #region 广告显示
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
        
        AudioManager.instance.bgmMusic.Stop();  //停止音乐
    }
    #endregion
    #region 广告加载
    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("广告：" + placementId + "加载成功");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log("广告：" + placementId + "加载失败");
    }

    #endregion
}
