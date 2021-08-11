using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using System;
using TMPro;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
#if UNITY_IOS
    string gameId = "4257780";
#else
    string gameId = "4257781";
#endif
    Action onRewardedAddSuccess;
    private static bool isInitialized = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!isInitialized) {
            isInitialized = true;
            Advertisement.AddListener(this);
            Advertisement.Initialize(gameId);
        }
    }

    public void PlayAd()
    {
        if (Advertisement.IsReady("Interstitial_Android")) {
            Advertisement.Show("Interstitial_Android");
        }
    }

    public void PlayRewardedAd(Action onSuccess)
    {
        onRewardedAddSuccess = onSuccess;
        if (Advertisement.IsReady("Rewarded_Android")) {
            Advertisement.Show("Rewarded_Android");
        } else {
            Debug.Log("Rewarded ad is not ready!");
        }
    }


    public void OnUnityAdsReady(string placementId)
    {
        //Debug.Log("Ads Ready");
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.Log("ERROR: " + message);
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log("Video started");
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if(placementId == "Rewarded_Android" && showResult == ShowResult.Finished)
        {
            onRewardedAddSuccess.Invoke();
            Debug.Log("REWARD");
        }
    }
}
