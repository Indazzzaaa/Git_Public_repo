using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveAndFade : MonoBehaviour
{
    [SerializeField] private float _deactivationTime;
    [SerializeField] private TMPro.TMP_Text _infoText;
    private float _currentTime;

    private IEnumerator FadeAndDeactivate()
    {
        yield return new WaitForSeconds(_deactivationTime);
        var time = 2.0f;
        while (time > 0)
        {
            var color = _infoText.color;
            color.a = time;
            _infoText.color = color;
            time -= Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(FadeAndDeactivate());
    }
}