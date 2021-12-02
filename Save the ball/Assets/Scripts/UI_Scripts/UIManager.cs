using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _loadingObject;
    [SerializeField] private Slider _loadingIndicator;

    // ----------------------- Game Info----------------------------------

    [Space(20)]
    [SerializeField] private TMP_Text _levelInfo;

    [SerializeField] private TMP_Text _attemptsInfo;

    [Header("Sound Component Only")]
    [SerializeField] private Image _soundIcon;

    [SerializeField] private TMP_Text _soundText;
    [SerializeField] private Sprite _onSprite;
    [SerializeField] private Sprite _offSprite;
    //---Y

    private void Start()
    {
        _attemptsInfo.text = $"Attempts {GameData.Instance.GetAttempts}";
        _levelInfo.text = $"Level {GameData.Instance.GetGameLevel}/100";
    }

    public void LoadGamePlayLevel(int sceneIndex)
    {
        StartCoroutine(LoadTheSceneLevel(sceneIndex));
    }

    private IEnumerator LoadTheSceneLevel(int sceneIndex)
    {
        //stage1:operation 0 - 0.9 , stage2:activation 0.9-1.0
        //AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        var time = 0f;
        _loadingObject.SetActive(true);
        // AdManager.Instance?.RequestBanner(GoogleMobileAds.Api.AdPosition.Center);
        while (time < 3)
        {
            _loadingIndicator.value = time / 3;
            time += Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene(sceneIndex);
    }

    public void AudioOff()
    {
        if (GameData.Instance.GetOnOff)
        {
            GameData.Instance.SetOnOff = false;
            _soundIcon.sprite = _offSprite;
            _soundText.SetText("Audio Off");
            AudioListener.pause = true;
        }
        else if (!GameData.Instance.GetOnOff)
        {
            GameData.Instance.SetOnOff = true;
            _soundIcon.sprite = _onSprite;
            _soundText.SetText("Audio On");
            AudioListener.pause = false;
        }
    }
}