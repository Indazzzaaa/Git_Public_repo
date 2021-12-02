using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBackgroundManager : MonoBehaviour
{
    [SerializeField] private Sprite[] _spritesArray;
    [SerializeField] private Image _imagePlaceHolder;

    private void Start()
    {
        var ranNo = Random.Range(0, 10);
        _imagePlaceHolder.sprite = _spritesArray[ranNo];
    }
}