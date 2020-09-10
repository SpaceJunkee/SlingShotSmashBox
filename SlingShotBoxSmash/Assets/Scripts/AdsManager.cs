﻿using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    string placement = "rewardedVideo";
    int moneyEarned;
    int normalCurrency;

    IEnumerator Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize("3815483", true);

        while (!Advertisement.IsReady(placement))
        {
            yield return null;
        }

        
    }

    public void ShowRewardAd()
    {
        moneyEarned = PlayerPrefs.GetInt("MoneyEarned");
        normalCurrency = PlayerPrefs.GetInt("NormalCurrency");
        Advertisement.Show(placement);
    }

   
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if(showResult == ShowResult.Finished)
        {          
            GameObject.Find("MoneyEarnedText").GetComponent<Text>().text = "〄" + PlayerPrefs.GetInt("MoneyEarned") * 2 + "";
            PlayerPrefs.SetInt("NormalCurrency", normalCurrency + moneyEarned);
        }
        else if (showResult == ShowResult.Failed)
        {
            // :(
        }

        RestartScreenManager.playerRewarded = false;
    }

    public void OnUnityAdsDidError(string message)
    {

    }

    public void OnUnityAdsDidStart(string placementId)
    {

    }

    public void OnUnityAdsReady(string placementId)
    {

    }
}
