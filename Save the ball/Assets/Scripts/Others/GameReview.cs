using System;
using Google.Play.Review;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameReview : MonoBehaviour
{
    // Create instance of ReviewManager
    private ReviewManager _reviewManager;

    private PlayReviewInfo _playReviewInfo;

    private void Start()
    {
        // GiveReivew();
        
    }

    public void GiveReivew()
    {
        // print("We are launching the in app review");
        // StartCoroutine(ReviewTheGame());
        Application.OpenURL($"market://details?id={Application.identifier}");
    }

    private IEnumerator ReviewTheGame()
    {
        
        // print("Review is starting");
        _reviewManager = new ReviewManager();

        // request review info object
        var requestFlowOperation = _reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;
        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            // print(requestFlowOperation.Error.ToString());
            yield break;
        }

        _playReviewInfo = requestFlowOperation.GetResult();


        var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
        yield return launchFlowOperation;
        _playReviewInfo = null; // Reset the object
        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            yield break;
        }
        // The flow has finished. The API does not indicate whether the user
        // reviewed or not, or even whether the review dialog was shown. Thus, no
        // matter the result, we continue our app flow.
    }
    
    
}