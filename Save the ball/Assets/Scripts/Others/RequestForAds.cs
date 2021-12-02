using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestForAds : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        //AdManager.Instance?.RequestInterstitial();
        AdManager.Instance?.RequestBanner();
    }
}