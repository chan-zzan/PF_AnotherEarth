using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public class AdsManager_E : MonoBehaviour
{
    public bool isTestMode;
    //public Text LogText;
    //public Button FrontAdsBtn;
    public Button RewardAdsBtn;


    void Start()
    {
        var requestConfiguration = new RequestConfiguration
           .Builder()
           .SetTestDeviceIds(new List<string>() { "1DF7B7CC05014E8" }) // test Device ID
           .build();

        MobileAds.SetRequestConfiguration(requestConfiguration);

        //LoadBannerAd();
        //LoadFrontAd();
        LoadRewardAd();
    }

    void Update()
    {
        //FrontAdsBtn.interactable = frontAd.IsLoaded();
        //RewardAdsBtn.interactable = rewardAd.IsLoaded();

        //FrontAdsBtn.interactable = frontAd.CanShowAd();
        RewardAdsBtn.interactable = rewardAd.CanShowAd();
    }

    AdRequest GetAdRequest()
    {
        return new AdRequest.Builder().Build();
    }



    #region 배너 광고
    //const string bannerTestID = "ca-app-pub-3940256099942544/6300978111";
    //const string bannerID = "";
    //BannerView bannerAd;


    //void LoadBannerAd()
    //{
    //    bannerAd = new BannerView(isTestMode ? bannerTestID : bannerID,
    //        AdSize.Banner, AdPosition.Bottom);
    //    bannerAd.LoadAd(GetAdRequest());
    //    ToggleBannerAd(false);
    //}

    //public void ToggleBannerAd(bool b)
    //{
    //    if (b) bannerAd.Show();
    //    else bannerAd.Hide();
    //}
    #endregion



    #region 전면 광고
    //const string frontTestID = "ca-app-pub-3940256099942544/8691691433";
    //const string frontID = "ca-app-pub-1268568640559310/1784514480";
    //InterstitialAd frontAd;


    //void LoadFrontAd()
    //{
    //    frontAd = new InterstitialAd(isTestMode ? frontTestID : frontID);
    //    frontAd.LoadAd(GetAdRequest());
    //    frontAd.OnAdClosed += (sender, e) =>
    //    {
    //        LogText.text = "전면광고 성공";
    //    };

    //    //InterstitialAd.Load(isTestMode ? frontTestID : frontID, GetAdRequest(), 
    //    //    (ad, error) =>
    //    //    {
    //    //        ad.OnAdFullScreenContentClosed += () => { LogText.text = "전면광고 성공"; };                
    //    //    }
    //    //    );
    //}

    //public void ShowFrontAd()
    //{
    //    frontAd.Show();
    //    LoadFrontAd();
    //}
    #endregion



    #region 리워드 광고
    const string rewardTestID = "ca-app-pub-3940256099942544/5224354917";
    const string rewardID = "ca-app-pub-1268568640559310/1134596038";
    RewardedAd rewardAd;


    void LoadRewardAd()
    {
        rewardAd = new RewardedAd(isTestMode ? rewardTestID : rewardID);
        rewardAd.LoadAd(GetAdRequest());
        rewardAd.OnUserEarnedReward += (sender, e) =>
        {
            //LogText.text = "리워드 광고 성공";
            Debug.Log("리워드 광고 성공");
        };
    }

    public void ShowRewardAd()
    {
        rewardAd.Show();
        LoadRewardAd();
    }
    #endregion
}
