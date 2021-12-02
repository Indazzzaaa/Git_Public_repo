using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
 
    [SerializeField] private int _playerLevel;
    [SerializeField] private int _numberOfAttempts;
    [SerializeField] private bool _audioOn;

    public static GameData Instance;

    #region Properties

    public int GetGameLevel
    {
        get => _playerLevel;
    }

    public int IncrementLevel() => _playerLevel++;

    public int GetAttempts
    {
        get => _numberOfAttempts;
    }

    public int IncrementAttempts() => _numberOfAttempts++;

    public bool SetOnOff
    {
        set => _audioOn = value;
    }

    public bool GetOnOff
    {
        get => _audioOn;
    }

    #endregion Properties

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(gameObject);
        InitializeData(); // start from the level 1 and attempt 0
        Application.targetFrameRate = 60; // we want that our game runs at 60 fps on all devices.
        _audioOn = true;
    }

    // this will collect the data or create the new one for us
    private void InitializeData()
    {
        if (PlayerPrefs.HasKey(Constants.consts.GamePlayed) && PlayerPrefs.HasKey(Constants.consts.PlayerLevel))
        {
            _playerLevel = PlayerPrefs.GetInt(Constants.consts.PlayerLevel);
            _numberOfAttempts = PlayerPrefs.GetInt(Constants.consts.GamePlayed);
            //print("Data has been loaded");
        }
        else
        {
            SaveData();
        }
    }

    public void SaveData()
    {
        _playerLevel = Mathf.Clamp(_playerLevel, 1, 100);
        PlayerPrefs.SetInt(Constants.consts.GamePlayed, _numberOfAttempts);
        PlayerPrefs.SetInt(Constants.consts.PlayerLevel, _playerLevel);
        PlayerPrefs.Save();
    }

    // --------------------------------------------- ONLY for testing -----------------------------------
    [ContextMenu("Override the game data")]
    private void OverrideTheGameData()
    {
        SaveData();
    }

    [ContextMenu("Delete All data")]
    private void DeleteData()
    {
        PlayerPrefs.DeleteKey(Constants.consts.GamePlayed);
        PlayerPrefs.DeleteKey(Constants.consts.PlayerLevel);
        print("All data deleted");
    }
}