using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class Ads_Initializer : MonoBehaviour , IUnityAdsInitializationListener , IUnityAdsLoadListener , IUnityAdsShowListener

{
    [SerializeField] private string _androidGameId;
    [SerializeField] private string _iosGameId;
    private string gameId;
    [SerializeField] private bool testMode = true;

    private void Awake()
    {
        if(!Advertisement.isInitialized)
        {
            InitializeAds();
        }
    }

    //Public Methods
    public void InitializeAds()
    {
        // Checking the platform that the game is currently running
        gameId = (Application.platform == RuntimePlatform.IPhonePlayer) ? _iosGameId : _androidGameId;
        Advertisement.Initialize(gameId, testMode, this);
    }
    public void LoadRewarededAd()
    {
        Advertisement.Load("Rewarded_Android", this);
    }

    public void LoadBannerAd()
    {
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Load("Banner_Android",
          new BannerLoadOptions
          {
              loadCallback = OnBannerLoaded,
              errorCallback = OnBannerLoadError
          }) ;
    }

    //Banner Load Delegate Methods
    private void OnBannerLoaded()
    {
        Advertisement.Banner.Show("Banner_Android");
    }

    private void OnBannerLoadError(string message)
    {

    }

    #region Intialization Interface Methods
    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads Initialization Complete");
        LoadBannerAd();
    }
    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    #endregion

    #region Load Interface Methods

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("UnityAdsLoaded");
        Advertisement.Show(placementId ,this);
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error Showing Ad Unit { placementId } :{ error.ToString() } - { message }");
    }

    #endregion

    #region Show Interface Methods

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log("UnityAdsShowfailed");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("UnityAdsShowStart");
        Time.timeScale = 0;
        Advertisement.Banner.Hide();
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("UnityAdsShowClick");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log("UnityAdsShowComplete" + " " + showCompletionState);
        if(placementId.Equals("Rewarded_Android") && UnityAdsShowCompletionState.COMPLETED.Equals(showCompletionState))
        {
            PlayerPrefs.SetInt("StarCount", PlayerPrefs.GetInt("StarCount") + 10);
        }
        Time.timeScale = 1;
        Advertisement.Banner.Show("Banner_Android");
    }
    #endregion
}
