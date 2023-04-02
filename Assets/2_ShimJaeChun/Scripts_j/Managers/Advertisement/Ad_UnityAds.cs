using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Modules.Ads
{
    public class Ad_UnityAds : Adbase, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        protected string placement_reward_Id = "rewardedVideo";

        [SerializeField]
        protected string android_game_id = "5141064";

        private const string REWARDED_VIDEO_PLACEMENT = "rewardedVideo";

        private bool testMode = true;

        public override void OnInitialize()
        {
            Advertisement.Initialize(android_game_id, testMode, this);
        
            if(isRewardUse)
            {
                Advertisement.Load(REWARDED_VIDEO_PLACEMENT, this);
            }
        }

        #region 인터페이스 구현목록
        public void OnInitializationComplete()
        {
            Debug.Log("Init Success");
        }
        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log($"Init Failed: [{error}]: {message}");
        }
        public void OnUnityAdsAdLoaded(string placementId)
        {
            Debug.Log($"Load Success: {placementId}");
        }
        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"Load Failed: [{error}:{placementId}] {message}");
        }
        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            Debug.Log($"OnUnityAdsShowFailure: [{error}]: {message}");
        }
        public void OnUnityAdsShowStart(string placementId)
        {
            Debug.Log($"OnUnityAdsShowStart: {placementId}");
        }
        public void OnUnityAdsShowClick(string placementId)
        {
            Debug.Log($"OnUnityAdsShowClick: {placementId}");
        }


        // 보상형 광고 콜백
        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            switch(showCompletionState)
            {
                case UnityAdsShowCompletionState.COMPLETED:
                    {
                        if(OnRewardResult != null)
                        {
                            OnRewardResult(AdResultType.Sucess);
                        }
                    }
                    break;
                case UnityAdsShowCompletionState.SKIPPED:
                case UnityAdsShowCompletionState.UNKNOWN:
                    {
                        if (OnRewardResult != null)
                        {
                            OnRewardResult(AdResultType.Fail);
                        }
                    }
                    break;
            }
            OnRewardResult = null;
        }

        // 보상형 광고 show
        public override void ShowRewardVideo(Action<AdResultType> result)
        {
            Debug.Log("광고 출력");
            OnRewardResult = result;
            Advertisement.Show(REWARDED_VIDEO_PLACEMENT, this);
        }

        #endregion
    }
}
