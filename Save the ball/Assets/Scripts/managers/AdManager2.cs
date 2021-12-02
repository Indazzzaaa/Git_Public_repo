using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace managers
{
    public enum AdType
    {
        BANNER,
        INTERSTITIAL,
        REWARDED_VIDEO
    }

    public class AdManager2 : MonoBehaviour
    {
        #region variables

        #region constants

        private const string APP_ID = "ca-app-pub-7521894813354632~9063354925";
        private const string BANNER_ID = "ca-app-pub-7521894813354632/3851406756";
        private const string INTERSTITIAL_ID = "ca-app-pub-7521894813354632/5511617562";
        private const string REWARDED_ID = "ca-app-pub-7521894813354632/1001866221";

        #endregion constants

        #region Ads variables

        private BannerView _bannerView;
        private InterstitialAd _interstitial;
        private RewardedAd _rewardedAd;

        /// <summary>
        /// We should go for requesting for ads if it failed to load then only
        /// </summary>
        [Header("Request checking variables")]
        [Space(20)]
        [SerializeField]
        private bool _shouldRequestBanner;

        [SerializeField] private bool _shouldRequestInterstitial;
        [SerializeField] private bool _shouldRequestRewardedVideoAd;

        #endregion Ads variables

        #region ad cached telling varialbes

        [Header("Ad loaded or not")]
        [Space(20)]
        [SerializeField]
        private bool _isBannerCached;

        [SerializeField] private bool _isInterstitialCached;

        public bool GetInterstitialCachedState
        {
            get => _isInterstitialCached;
        }

        [SerializeField] private bool _isRewardedCached;

        #endregion ad cached telling varialbes

        public static AdManager2 SingleTon;
        private readonly Queue<Action> _actionQueue = new Queue<Action>();
        private readonly Queue<Action<int>> _actionQueForSceneChange = new Queue<Action<int>>();

        [SerializeField] public int StartAgainCount;

        #endregion variables

        private void Awake()
        {
            PerfromSingletonCheck();
            DontDestroyOnLoad(gameObject);
            StartAgainCount = 3;
        }

        private void Start()
        {
            // Initialize the Google Mobile Ads SDK
            MobileAds.Initialize(status =>
            {
                // SDK initialization is completed
            });
            _shouldRequestBanner = true;
            _shouldRequestInterstitial = true;
            _shouldRequestRewardedVideoAd = true;

            CreateAdObjects();
            // FormatedPrint(AdManager2.SingleTon.ToString(), "My start method runs");
        }

        private void CreateAdObjects()
        {
            #region Banner

            // Create a 320x50 banner at the adPosition.
            _bannerView = new BannerView(BANNER_ID, AdSize.SmartBanner, AdPosition.Bottom);

            // subscribing to the events
            _bannerView.OnAdLoaded += BannerHandleOnAdLoad;
            _bannerView.OnAdFailedToLoad += BannerHandleOnAdFailedToLoad;

            #endregion Banner

            #region Interstitial

            // Initialize an Interstitial Ad
            _interstitial = new InterstitialAd(INTERSTITIAL_ID);

            // subscribing to events
            _interstitial.OnAdLoaded += Interstitial_HandleOn_AdLoaded;
            _interstitial.OnAdFailedToLoad += Interstitial_HandleOnAd_FailedToLoad;
            _interstitial.OnAdClosed += Interstitial_HandleOn_AdClosed;
            _interstitial.OnAdFailedToShow += Interstitial_HandleOn_FailedToShow;

            #endregion Interstitial

            #region Rewarded Video Ad

            // Initialize the rewarded ad
            _rewardedAd = new RewardedAd(REWARDED_ID);

            // subscribing the events
            _rewardedAd.OnAdLoaded += Rewarded_HandleOn_AdLoaded;
            _rewardedAd.OnAdFailedToLoad += Rewarded_HandleOn_AdFailedToLoad;
            _rewardedAd.OnUserEarnedReward += Rewarded_HandleOn_UserEarnReward;
            _rewardedAd.OnAdClosed += Rewarded_HandleOn_AdClosed;
            _rewardedAd.OnAdFailedToShow += Rewarded_HandleOn_AdFailedToShow;

            #endregion Rewarded Video Ad
        }

        // making script singleton
        private void PerfromSingletonCheck()
        {
            if (SingleTon == null)
            {
                SingleTon = this;
            }
            else if (SingleTon != this)
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            #region Some region

            while (_actionQueue.Count > 0)
            {
                _actionQueue.Dequeue().Invoke();
            }

            while (_actionQueForSceneChange.Count > 0)
            {
                _actionQueForSceneChange.Dequeue().Invoke(1);
            }

            // if there is no any network available then return from here
            if (!GameManager.Instance.IsNetworkAvailable) return;
            // FormatedPrint(AdManager2.SingleTon.ToString(),"Connection Established");

            // if banner is not loaded and we are allowed to request banner then do that
            if (!_isBannerCached && _shouldRequestBanner)
            {
                // print("[Update] Banner Requested");
                RequestBanner();
                _shouldRequestBanner = false;
            }

            // if interstitial is not loaded and we are allowed to reques interstitial then do that
            if (!_isInterstitialCached && _shouldRequestInterstitial)
            {
                // print("[Update] Interstitial Requested");
                RequestInterstitial();
                _shouldRequestInterstitial = false;
            }

            if (!_isRewardedCached && _shouldRequestRewardedVideoAd)
            {
                // print("[Update] Rewarded Requested");
                RequestRewardedVideoAd(); // request for rewarded if ad is not loaded
                _shouldRequestRewardedVideoAd = false;
            }

            #endregion Some region
        }

        /*private void OnDestroy()
        {
            var currentScene = SceneManager.GetActiveScene().buildIndex;
            if (currentScene != 0 || !_isBannerCached) return;
            // print("It's time to hide the banner ");
            if (!_isBannerCached) return;
            // print("Hiding and caching the banner");
            _bannerView.Hide();

            _bannerView?.Destroy();
            _isBannerCached = false;
            _shouldRequestBanner = true;
        }*/

        #region Banner Ads

        // banner aas refereshed automatically
        private void RequestBanner()
        {
         
            // create empty ad request
            AdRequest adRequest = new AdRequest.Builder().Build();

            //load the banner with the request
            _bannerView.LoadAd(adRequest);
            // _bannerView.Destroy(); no need to destroy the banner ads they always refresh themselves
            // _bannerView.Hide(); but we can hide though.

            // FormatedPrint(_bannerView.ToString(), "Requesting banner");
        }

        private void BannerHandleOnAdLoad(object sender, EventArgs e)
        {
            // code when our ad is loaded and waiting to display
            _isBannerCached = true;
            // FormatedPrint(_bannerView.ToString(), "Ad loaded");
        }

        private void BannerHandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
        {
            // code when our ad is failed to load
            // WaitForSomeTimeBeforBannerAgainRequesting();
            // FormatedPrint(_bannerView.ToString(), "Ad failed to load");
            // _actionQueue.Enqueue(WaitForSomeTimeBeforBannerAgainRequesting);
            WaitBeforeNextRequesting();
        }

        private async void WaitBeforeNextRequesting()
        {
            await Task.Delay(60000);
            _isBannerCached = false;
            _shouldRequestBanner = true;
            // FormatedPrint(_bannerView.ToString(), "Ad failed to load");
        }

        #endregion Banner Ads

        #region Interstitial Ads

        private void RequestInterstitial()
        {
            // create an empty ad request
            AdRequest request = new AdRequest.Builder().Build();

            // load the interstitial
            _interstitial.LoadAd(request);
            // FormatedPrint(_interstitial.ToString(), "Requesting interstitial");
        }

        private void Interstitial_HandleOn_AdLoaded(object sender, EventArgs e)
        {
            // code to run when our ad is loaded
            _isInterstitialCached = true;
            // FormatedPrint(_interstitial.ToString(), "Ad loaded");
        }

        private void Interstitial_HandleOnAd_FailedToLoad(object sender, AdFailedToLoadEventArgs e)
        {
            // code to run when our ad is failed to load

            WaitInterstitialBeforeNextRequest();
            // FormatedPrint(_interstitial.ToString(), "Ad failed to load");
        }

        private async void WaitInterstitialBeforeNextRequest()
        {
            if (!GameManager.Instance.IsNetworkAvailable) return;
            await Task.Delay(60000);
            _isInterstitialCached = false;
            _shouldRequestInterstitial = true;
        }

        private void Interstitial_HandleOn_AdClosed(object sender, EventArgs e)
        {
            // code to run when player closed the ad manually
            // destroy the intersitial object because it is one time object
            _interstitial.Destroy();
            _isInterstitialCached = false;
            _shouldRequestInterstitial = true;
            // FormatedPrint(_interstitial.ToString(), "Ad closed");
            _actionQueForSceneChange.Enqueue(GameManager.Instance.ChangeScene);
            // GameManager.Instance.ChangeScene(1);

            // transition to the next scene
        }

        private void Interstitial_HandleOn_FailedToShow(object sender, AdErrorEventArgs e)
        {
            _interstitial.Destroy();
            _isInterstitialCached = false;
            _shouldRequestInterstitial = true;
            // FormatedPrint(_interstitial.ToString(), "Ad closed");
            _actionQueForSceneChange.Enqueue(GameManager.Instance.ChangeScene);
        }

        #endregion Interstitial Ads

        #region Rewarded Ads

        private void RequestRewardedVideoAd()
        {
            //create an emptly ad request
            AdRequest request = new AdRequest.Builder().Build();

            // load the rewarded ad with the request
            _rewardedAd.LoadAd(request);

            // FormatedPrint(_interstitial.ToString(), "Requesting Rewarded");
        }

        private void Rewarded_HandleOn_AdLoaded(object sender, EventArgs e)
        {
            // code to run when our ad is loaded
            _isRewardedCached = true;
            // FormatedPrint(_rewardedAd.ToString(), "Ad loaded");
        }

        private void Rewarded_HandleOn_AdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
        {
            // code to run when our ad is failed to load

            WaitRewardedBeforeNextRequest();
            // GameManager.Instance.ShowConnectionError();
            // FormatedPrint(_rewardedAd.ToString(), "Ad failed to load");
        }

        private async void WaitRewardedBeforeNextRequest()
        {
            if (!GameManager.Instance.IsNetworkAvailable) return;
            await Task.Delay(20000);
            _isRewardedCached = false;
            _shouldRequestRewardedVideoAd = true;
        }

        private void Rewarded_HandleOn_UserEarnReward(object sender, Reward e)
        {
            // code to run when the user should be rewarded for interacting with the ad
            // FormatedPrint(_rewardedAd.ToString(), "Your player have earned reward");
            _actionQueue.Enqueue(GameManager.Instance.StartPlay);
            // GameManager.Instance.StartPlay();
        }

        private void Rewarded_HandleOn_AdClosed(object sender, EventArgs e)
        {
            // code to run when user closes the ad
            // FormatedPrint(_rewardedAd.ToString(), "your player just closed the ad");
            _rewardedAd.Destroy();
            _isRewardedCached = false;
            _shouldRequestRewardedVideoAd = true;
            GameManager.Instance.StartPlay();
        }

        private void Rewarded_HandleOn_AdFailedToShow(object sender, AdErrorEventArgs e)
        {
            // code to run when our ads is loaded but failed to show
            // FormatedPrint(_rewardedAd.ToString(), "Ad loaded but failed to show");
            // ShowAd(AdType.REWARDED_VIDEO);
            // It's our fault so that player should start playing
            // GameManager.Instance.StartPlay();
        }

        #endregion Rewarded Ads

        #region Ads showing  methods

        private void ShowBannerAds()
        {
            if (!GameManager.Instance.IsNetworkAvailable) return;
            if (!_isBannerCached) return;
            _bannerView.Show();
        }

        private void ShowInterstitialAd()
        {
            if (!GameManager.Instance.IsNetworkAvailable ||
                (GameManager.Instance.IsNetworkAvailable && !_isInterstitialCached))
            {
                _actionQueForSceneChange.Enqueue(GameManager.Instance.ChangeScene);
                return;
            }

            _interstitial.Show();
        }

        private void ShowRewardedVideoAd()
        {
            if (!GameManager.Instance.IsNetworkAvailable ||
                (GameManager.Instance.IsNetworkAvailable && !_isRewardedCached))
            {
                GameManager.Instance.ShowConnectionError();
                return;
            }

            _rewardedAd.Show();
        }

        #endregion Ads showing  methods

        public void Hide_OR_ShowBanner(int buildIndex)
        {
            if (buildIndex == 1)
            {
                if (!_isBannerCached) return;

                // print("Showing the banner from Hide_or_show_method");
                _bannerView.Show();
            }

            if (buildIndex == 0)
            {
                // if (!GameManager.Instance.IsNetworkAvailable) return;
                // FormatedPrint(AdManager2.SingleTon.ToString(), "Hiding banner");
                _bannerView.Hide();
            }
        }

        public void ShowAd(AdType adType)
        {
            switch (adType)
            {
                case AdType.BANNER:
                    ShowBannerAds();
                    break;

                case AdType.INTERSTITIAL:
                    ShowInterstitialAd();
                    break;

                case AdType.REWARDED_VIDEO:
                    ShowRewardedVideoAd();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(adType), adType,
                        "This argument is not present in the enum list of AdType");
            }
        }

        #region Test Code

        private void FormatedPrint(string className, string msg)
        {
            print($"[{className} , {msg}]");
        }

        #endregion Test Code
    }
}