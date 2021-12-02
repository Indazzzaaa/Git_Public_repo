using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdManager : MonoBehaviour
{
    #region original

    private const string App_ID = "ca-app-pub-7521894813354632~9063354925";
    private const string o_banner_1 = "ca-app-pub-7521894813354632/3851406756";
    private const string o_interstitial_1 = "ca-app-pub-7521894813354632/5511617562";
    private const string o_rewarded_1 = "ca-app-pub-7521894813354632/1001866221";

    //---Y

    #endregion original

    //#region Testing

    //private const string t_banner = "ca-app-pub-3940256099942544/6300978111";
    //private const string t_interstitial = "ca-app-pub-3940256099942544/1033173712";
    //private const string t_rewarded = "ca-app-pub-3940256099942544/5224354917";

    //#endregion Testing

    //[SerializeField] private bool isTesting;

    private BannerView _bannerView;
    private InterstitialAd _interstitial;
    private RewardedAd _rewardedAd;

    public static AdManager Instance;
    //---R

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
      

        // Initialize the Google Mobile Ads SDK.initStatus.getAdapterStatusForClassName(App_ID);
        MobileAds.Initialize(initStatus =>
        {
            // SDK initialization is completed
            
        });
    }

    // -------------------------- BANNER ADS--------------------------------------------------------

    #region BannerADs

    [ContextMenu("Banner")]
    public void RequestBanner(AdPosition adPosition = AdPosition.Top)
    {
        var bannerIdUse = o_banner_1;
        _bannerView = new BannerView(bannerIdUse, AdSize.Banner, adPosition);

        ShowBannerAd();
    }

    private void ShowBannerAd()
    {
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        _bannerView.LoadAd(request);
    }

    #endregion BannerADs

    // -------------------------- Interstitial ADS--------------------------------------------------------

    #region Interstitial

    [ContextMenu("Show interstitial")]
    public void RequestInterstitial()
    {
        var interstitlaUseId = o_interstitial_1;
        // Initialize an InterstitialAd.
        _interstitial = new InterstitialAd(interstitlaUseId);
        _interstitial.OnAdClosed += HandleOnInterstialClose;
        _interstitial.OnAdFailedToLoad += HandleOnInterstitialFailedToLoad;
        _interstitial.OnAdLoaded += HandleOnAdLoad;
        _interstitial.OnAdFailedToShow += HandleOnAdFailedToShow;
        
        
        ShowInterstitial();
    }
    private void ShowInterstitial()
    {
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        _interstitial.LoadAd(request);
        if (_interstitial.IsLoaded())
        {
            _interstitial.Show();
        }
    }

    private void HandleOnAdFailedToShow(object sender, AdErrorEventArgs e)
    {
        print("[Interstitial] your ad is failed to show");
    }

    private void HandleOnAdLoad(object sender, EventArgs e)
    {
        print("[Interstitial ] your ad is loaded");
        _interstitial.Show();
    }

  

    private void HandleOnInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs evntArgs)
    {
        // print("[Interstitial] your ad is failed to load");
        LoadAdError loadAdError = evntArgs.LoadAdError;

        // Gets the domain from which the error came.
        string domain = loadAdError.GetDomain();

        // Gets the error code. See
        // https://developers.google.com/android/reference/com/google/android/gms/ads/AdRequest
        // and https://developers.google.com/admob/ios/api/reference/Enums/GADErrorCode
        // for a list of possible codes.
        int code = loadAdError.GetCode();

        // Gets an error message.
        // For example "Account not approved yet". See
        // https://support.google.com/admob/answer/9905175 for explanations of
        // common errors.
        string message = loadAdError.GetMessage();

        // Gets the cause of the error, if available.
        AdError underlyingError = loadAdError.GetCause();

        // All of this information is available via the error's toString() method.
        Debug.Log("Load error string: " + loadAdError.ToString());

        // Get response information, which may include results of mediation requests.
        ResponseInfo responseInfo = loadAdError.GetResponseInfo();
        Debug.Log("Response info: " + responseInfo.ToString());
        // GameManager.Instance.ChangeScene(1);
    }

    private void HandleOnInterstialClose(object sender, EventArgs eventArgs)
    {
        GameManager.Instance.ChangeScene(1);
    }

    #endregion Interstitial

    //++ ---------------------- Reward Based Video Ad------------

    #region Video Ad

    [ContextMenu("Show rewarded video ad")]
    public void RequestRewardBasedVideoAd()
    {
        var rewardedVideoId = o_rewarded_1;
        this._rewardedAd = new RewardedAd(rewardedVideoId);

        #region Events subscribe

        // Called when an ad request has successfully loaded.
        this._rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this._rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this._rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this._rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this._rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this._rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        #endregion Events subscribe

        ShowRewardedVideoAd();
    }

    private void ShowRewardedVideoAd()
    {
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this._rewardedAd.LoadAd(request);
        if (this._rewardedAd.IsLoaded())
        {
            _rewardedAd.Show();
        }
    }

    //+ Events and delegate for the rewarded ads

    #region Events functions

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        print("[Rewarded ad] your ad has been loaded success");
        _rewardedAd.Show();
        //var info = " Rewarded ad loaded";
        //print(info);
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        //var info = " Rewarded ad failed";
        //print(info);
        GameManager.Instance.StartPlay();
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        //MonoBehaviour.print(     "HandleRewardedAdFailedToShow event received with message: " + args.Message);
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        GameManager.Instance.StartPlay();
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        // number of reward we set for the rewarded video
    }

    #endregion Events functions

    #endregion Video Ad
}