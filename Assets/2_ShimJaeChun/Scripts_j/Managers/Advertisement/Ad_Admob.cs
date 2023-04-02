using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;


namespace Modules.Ads
{
    // 에드몹형 광고
    public class Ad_Admob : Adbase
    {
        [SerializeField]
        protected string rewardID = "ca-app-pub-3940256099942544/5224354917"; // 테스트 아이디

        protected RewardedAd rewardAd = null;

        protected bool IsRewared { get; set; }

        [Header("보상 타입")]
        public GetRewardType getRewardType;

        [Header("보상 지급량")]
        public int rewardAmount;

        public override void OnInitialize()
        {
            MobileAds.Initialize((initStatus) =>
            {
                if (isRewardUse)
                {
                    rewardAd = new RewardedAd(rewardID);
                    LoadRewardBasedVideo();
                    rewardAd.OnAdClosed += HandleRewardBasedVideoClosed;
                    rewardAd.OnUserEarnedReward += HandleRewardBasedVideoRewarded;
                    rewardAd.OnAdFailedToLoad += RewardAd_OnAdFailedToLoad;
                }
            });
        }
        public void OnDestroy()
        {
            if(isRewardUse && rewardAd!= null)
            {
                rewardAd.OnAdClosed -= HandleRewardBasedVideoClosed;
                rewardAd.OnUserEarnedReward -= HandleRewardBasedVideoRewarded;
                rewardAd.OnAdFailedToLoad -= RewardAd_OnAdFailedToLoad;
            }
        }

        // 리워드 광고 함수
        public override void ShowRewardVideo(Action<AdResultType> result)
        {
            OnRewardResult = result;
            ShowRewardVideo();
        }

        public void ShowRewardVideo()
        {
            // 광고가 로드된 경우
            if(rewardAd.CanShowAd())
            {
                IsRewared = false;
                rewardAd.Show();
            }
            else
            {
                // 광고 보상획득 실패
                if(OnRewardResult !=null)
                {
                    OnRewardResult(AdResultType.Fail);
                    OnRewardResult = null;
                }

                LoadRewardBasedVideo();
            }
        }

        protected void LoadRewardBasedVideo()
        {
            AdRequest request = new AdRequest.Builder().Build();

            rewardAd.LoadAd(request);
        }

        protected virtual void HandleRewardBasedVideoRewarded(object sender, Reward e)
        {
            Debug.Log("[동영상광고 보상지급]"+"보상타입 : "+e.Type+" / " +"보상개수 : " + e.Amount);

            // 퀘스트 적용
            if(QuestManager.Instance.dailyQuestList[(int)DailyQuestType.WatchingAds].myState == QuestButtonState.Proceed)
            {
                QuestManager.Instance.UpdateDailyQuest(DailyQuestType.WatchingAds);
            }

            switch (getRewardType)
            {
                case GetRewardType.Coin:
                    {
                        StatManager.Instance.AddMineral(rewardAmount);
                        break;
                    }
                case GetRewardType.Dia:
                    {
                        StatManager.Instance.AddDia(rewardAmount);
                        StatManager.Instance.AdsCounting(RewardAdsType.Ads_Dia);
                        break;
                    }
                case GetRewardType.Energy:
                    {
                        StatManager.Instance.AddExtraEnergy(rewardAmount);
                        StatManager.Instance.AdsCounting(RewardAdsType.Ads_Energy);
                        break;
                    }
                case GetRewardType.Revival:
                    {
                        // 부활
                        GameManager_E.Instance.Revival(false);
                        //StatManager.Instance.AdsCounting(RewardAdsType.Ads_Revival);
                        break;
                    }
                case GetRewardType.Battle:
                    {
                        // 씬로드
                        SceneManager.LoadScene("InGame_E");
                        StatManager.Instance.AdsCounting(RewardAdsType.Ads_Energy);
                        Time.timeScale = 1f;
                        break;
                    }

            }
            IsRewared = true;
        }

        protected void HandleRewardBasedVideoClosed(object sender, EventArgs e)
        {
            Debug.Log("비디오 종료 : " + e.ToString());

            if(OnRewardResult != null)
            {
                if(IsRewared)
                {
                    OnRewardResult(AdResultType.Sucess);
                }
                else
                {
                    //#if UNITY_EDITOR
                    //OnRewardResult(AdResultType.Sucess);
                    //#else
                    OnRewardResult(AdResultType.Fail);
                    //#endif
                }
                OnRewardResult = null;
            }
            IsRewared = false;
            LoadRewardBasedVideo();
        }

        protected void RewardAd_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
        {
            Debug.Log(e.ToString());
        }
    }
}
