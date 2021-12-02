using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdMobTest : MonoBehaviour
{

    #region variables
    private const string App_ID = "ca-app-pub-7521894813354632~9063354925";
    private const string o_banner_1 = "ca-app-pub-7521894813354632/3851406756";
    private const string o_interstitial_1 = "ca-app-pub-7521894813354632/5511617562";
    private const string o_rewarded_1 = "ca-app-pub-7521894813354632/1001866221";


    private BannerView _bannerView;
    private InterstitialAd _interstitial;
    private RewardedAd _rewardedAd;

    #endregion

    private void Start()
    {
        MobileAds.Initialize(status =>
        {
            // SDK initialization is completed
        } );
        RequestInterstialAd();
        
    }


    #region Interstitial ad

    

    private void RequestInterstialAd()
    {
        // initializing the interstitial ad
        _interstitial = new InterstitialAd(o_interstitial_1);
        // create and emptly ad request
        var adRequest = new AdRequest.Builder().Build();
        // load the interstitial with the request
        this._interstitial.LoadAd(adRequest);
        
        
        
        // Key Point: On iOS, InterstitialAd objects are one time use objects.
        // That means once an interstitial is shown, the InterstitialAd object can't be used to load another ad. To request another interstitial, you'll need to create a new InterstitialAd object.
        
        
        // subscribing to the events
        this._interstitial.OnAdClosed += HandleOnAdClosed;


    }

    private void HandleOnAdClosed(object sender, EventArgs e)
    {
        print("Ad has been closed");
    }

    [ContextMenu("Load the interstial ad")]
    private void GameOver()
    {

        if (this._interstitial.IsLoaded())
        {
            print("Ad is loaded");
            this._interstitial.Show();
        }
    }
    
    #endregion

    
    
    
    
    
    
    
    
    

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}
