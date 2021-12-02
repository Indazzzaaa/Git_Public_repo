using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameInfoUI : MonoBehaviour
{
    [SerializeField] private Slider _mySlider;
    [SerializeField] private TMP_Text _currentLevelInfo;
    [SerializeField] private TMP_Text _nextLevelInfo;

    private float _playerEndCordinate;

    // Start is called before the first frame update
    private void Start()
    {
        _currentLevelInfo.SetText(GameData.Instance.GetGameLevel.ToString());
        var nextLevel = GameData.Instance.GetGameLevel + 1;
        _nextLevelInfo.SetText(nextLevel.ToString());

        _playerEndCordinate = SpawnObjectSystem.Instance.GetEndCordinate;
    }

    // Update is called once per frame
    private void Update()
    {
        _mySlider.value = GameManager.Instance.PlayerLastCollidingCordinate.z / _playerEndCordinate;
    }
}