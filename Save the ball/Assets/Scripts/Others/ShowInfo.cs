using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TO show the game info to the player and greetings
/// </summary>
public class ShowInfo : MonoBehaviour
{
    private const string _info1 = "Good Luck";
    private const string _info2 = "Tap on screen  for moving the ball";
    [SerializeField] private TMPro.TMP_Text _infoText;
    [SerializeField] private int _waitTime;

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(DisplayMsg("Level : " + GameData.Instance.GetGameLevel.ToString()));
    }

    private IEnumerator DisplayMsg(string msg)
    {
        _infoText.text = msg;

        yield return new WaitForSeconds(_waitTime);
        yield return StartCoroutine(FadeInElement(2));
        if (GameData.Instance.GetGameLevel < 6)
        {
            _infoText.text = _info2;
            yield return StartCoroutine(FadeOut(2));
            yield return new WaitForSeconds(_waitTime);
            yield return StartCoroutine(FadeInElement(2));
            _infoText.text = _info1;
            yield return StartCoroutine(FadeOut(2));
            yield return StartCoroutine(FadeInElement(2));
        }

        Destroy(gameObject);
    }

    private IEnumerator FadeInElement(float timeForFading)
    {
        var time = timeForFading;// this is fading time of the text;
        while (time > 0)
        {
            var color = _infoText.color;
            color.a = time;
            _infoText.color = color;
            time -= Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator FadeOut(float timeForFading)
    {
        var time = 0.0f;// this is fading time of the text;
        while (time < timeForFading)
        {
            var color = _infoText.color;
            color.a = time;
            _infoText.color = color;
            time += Time.deltaTime;
            yield return null;
        }
    }
}