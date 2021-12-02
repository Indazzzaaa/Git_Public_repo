using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using managers;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    #region Variables

    [Header("Assign in Inspector")] [Space(10)] [SerializeField]
    private Animator _uiAnimator;

    [Header("-----------Only For visualization-------------")] [Space(10)] [SerializeField]
    private bool _isPlayerDied;

    [SerializeField] private GameObject _playerObject;

    [Space(20)] [SerializeField] private GameObject _destroyingParticlesObject;

    [SerializeField] private GameObject _connectionInfo;

    public Vector3
        PlayerLastCollidingCordinate; //-- this is required , so that when we resume game player can place there

    [Space(10)] [Header("Game color use")] [SerializeField]
    private Color _gameColorInUse;

    [SerializeField] private ParticleSystem _collidingParticleColor;
    [SerializeField] private Material _ballMaterials;
    [SerializeField] private GameObject _victoryPaticles;
    [Space(20)] [SerializeField] private bool _isNetworkAvailable;

    public bool IsNetworkAvailable
    {
        get => _isNetworkAvailable;
    }

    public Material GetBallMeterial
    {
        get => _ballMaterials;
    }

    public static GameManager Instance;
    private static readonly int PlayUI = Animator.StringToHash("play_ui");

    public bool IsPlayerDied
    {
        set => _isPlayerDied = value;
    }

    private bool _isCoroutineFinised;
    [SerializeField] private float _timeoutTime;
    private float _currentTime;

    #endregion Variables

    private void Start()
    {
        // Make it singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }

        _playerObject = GameObject.FindGameObjectWithTag(Constants.consts.playerTag);
        AudioListener.pause = !GameData.Instance.GetOnOff;
        InitializeColors();
        _isCoroutineFinised = true;
        if (!_isCoroutineFinised) return;
        StartCoroutine(CheckNetworkInStart());
        AdManager2.SingleTon?.Hide_OR_ShowBanner(1);
    }

    private void Update()
    {
       
            // if (_isNetworkAvailable) return;
            // time check of 1-minute for the internet connectivity if internet is disabled in the device
            if (_currentTime < _timeoutTime)
            {
                _currentTime += Time.deltaTime;
            }
            else
            {
                if (_isCoroutineFinised) StartCoroutine(CheckNetworkInStart() );
                _currentTime = 0;
            }

      
        
    }


    public void ShowMeResumePanel()
    {
        _uiAnimator.gameObject.SetActive(true);
        _uiAnimator.SetBool(PlayUI, true);
    }

    public async void ResumeGame()
    {
        if (!_isNetworkAvailable || _isNetworkAvailable && !AdManager2.SingleTon.GetInterstitialCachedState)
        {
            ShowConnectionError();
            return;
        }
        

        await PlayWithResumePanel(false);
        // StartCoroutine(CheckInternetConnectivity( AdManager.Instance.RequestRewardBasedVideoAd));
        AdManager2.SingleTon.ShowAd(AdType.REWARDED_VIDEO);
    }


    private async Task PlayWithResumePanel(bool whatYouWant)
    {
        // close the resume menu  is passed false, open if passed true
        _uiAnimator.SetBool(PlayUI, whatYouWant);
        await Task.Delay(2000);
        _uiAnimator.gameObject.SetActive(whatYouWant);
    }


    // this is to call when we exit the ads
    public void StartPlay()
    {
        _playerObject.SetActive(true);
        _playerObject.transform.position = PlayerLastCollidingCordinate;
    }

    public async void RestartTheGame()
    {
        await PlayWithResumePanel(false);
        if (AdManager2.SingleTon.StartAgainCount == 0)
        {
            AdManager2.SingleTon.StartAgainCount = 3;
            AdManager2.SingleTon.ShowAd(AdType.INTERSTITIAL);
            
        }
        else
        {
            AdManager2.SingleTon.StartAgainCount -=1;
            ChangeScene(1);
        }
        // StartCoroutine(CheckInternetConnectivity(AdManager.Instance.RequestInterstitial, true));
        /*if (_uiAnimator.GetBool("play_ui"))
        {
            _uiAnimator.SetBool("play_ui", false);
            await Task.Delay(2000);
            _uiAnimator.gameObject.SetActive(false);

            if (!IsConnected())
            {
                ChangeScene(1);
            }
            else
            {
                AdManager.Instance?.RequestInterstitial();
                //ChangeScene(1);
            }
        }
        else
        {
            return;
        }*/
    }

    public async void ChangeScene(int sceneIndex)
    {
        AdManager2.SingleTon.Hide_OR_ShowBanner(sceneIndex);
        await Task.Delay(100);

        SceneManager.LoadScene(sceneIndex);
    }

    public void PlayDestroyingParticles(Vector3 position)
    {
        if (_destroyingParticlesObject.activeSelf)
        {
            _destroyingParticlesObject.transform.position = position;
            var particle = _destroyingParticlesObject.GetComponent<ParticleSystem>();

            var main = particle.main;
            main.startColor = _gameColorInUse;
            particle.Play();
        }
        else
        {
            _destroyingParticlesObject.transform.position = position;
            _destroyingParticlesObject.SetActive(true);
            var particle = _destroyingParticlesObject.GetComponent<ParticleSystem>();
            var main = particle.main;
            main.startColor = _gameColorInUse;
            particle.Play();
        }
    }

    private void ChooseRandomColor()
    {
        var colorChoose = Random.Range(1, 14);
        switch (colorChoose)
        {
            case 1:
                _gameColorInUse = new Color(243.0f / 255, 122.0f / 255, 71.0f / 255);
                break;

            case 2:
                _gameColorInUse = new Color(254.0f / 255, 138.0f / 255, 126.0f / 255);
                break;

            case 3:
                _gameColorInUse = new Color(207.0f / 255, 60.0f / 255, 26.0f / 255);
                break;

            case 4:
                _gameColorInUse = new Color(8.0f / 255, 170.0f / 255, 146.0f / 255);
                break;

            case 5:
                _gameColorInUse = new Color(184.0f / 255, 76.0f / 255, 0.0f / 255);
                break;

            case 6:
                _gameColorInUse = new Color(250.0f / 255, 169.0f / 255, 77.0f / 255);
                break;

            case 7:
                _gameColorInUse = new Color(34.0f / 255, 240.0f / 255, 95.0f / 255);
                break;

            case 8:
                _gameColorInUse = new Color(207.0f / 255, 243.0f / 255, 21.0f / 255);
                break;

            case 9:
                _gameColorInUse = new Color(227.0f / 255, 170.0f / 255, 23.0f / 255);
                break;

            case 10:
                _gameColorInUse = new Color(222.0f / 255, 32.0f / 255, 219.0f / 255);
                break;

            case 11:
                _gameColorInUse = new Color(58.0f / 255, 15.0f / 255, 181.0f / 255);
                break;

            case 12:
                _gameColorInUse = new Color(242.0f / 255, 38.0f / 255, 25.0f / 255);
                break;

            case 13:
                _gameColorInUse = new Color(168.0f / 255, 208.0f / 255, 11.0f / 255);
                break;

            default:
                _gameColorInUse = new Color(8.0f / 255, 170.0f / 255, 146.0f / 255);
                break;
        }
    }

    private void InitializeColors()
    {
        ChooseRandomColor();
        var trailRender = _playerObject?.GetComponent<TrailRenderer>();
        _ballMaterials.color = _gameColorInUse;
        var particle = _collidingParticleColor.main;
        particle.startColor = _gameColorInUse;

        trailRender.startColor = _gameColorInUse;
    }

    public void PlayTheVictoryShow()
    {
        var angle = Quaternion.Euler(0, 90, 0);
        var instance = Instantiate(_victoryPaticles, _playerObject.transform.position, angle);
        var particles = instance.GetComponent<ParticleSystem>();
        particles.Play();
        AudioManager.Instance.PlaySound(AudioManager.Instance?.GetVicotrySound);
    }


// check the connectivity of the app
    IEnumerator CheckInternetConnectivity(Action perfromTask, bool shouldPassEitherConnectedOrNot = false)
    {
        var request = new UnityWebRequest("www.google.com");
        request.timeout = 3;
        yield return request.SendWebRequest();
        if (request.error != null)
        {
            _isNetworkAvailable = false;
            if (shouldPassEitherConnectedOrNot)
            {
                // if request for the start again the level , then if not connected then also move to play the game
                ChangeScene(1);
            }
            else
            {
                _connectionInfo.SetActive(true);
            }
        }
        else
        {
            _isNetworkAvailable = true;

            PlayWithResumePanel(false);
            perfromTask();
        }
    }

    public void ShowConnectionError()
    {
        _connectionInfo.SetActive(true);
    }

    IEnumerator CheckNetworkInStart()
    {
        _isCoroutineFinised = false;
        var request = new UnityWebRequest("www.google.com");
        request.timeout = 3;
        
        yield return request.SendWebRequest();
        // print("Web request error : " + request.error);
        _isNetworkAvailable = request.error == null;
        _isCoroutineFinised = true;
    }
}