using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Advertisements;
using GoogleMobileAds.Api;

namespace Modules.Ads
{
    public class PlayAdsButton : MonoBehaviour
    {
        public RewardAdsType myAdsType;

        [Header("Layout_BuyEnergyGroup")]
        public GameObject layout_BuyEnergyGroup;

        private void Start()
        {
        }

        public void OnClickAdsPlayButton()
        {
            switch(myAdsType)
            {
                case RewardAdsType.Ads_Dia:
                    {
                        if(StatManager.Instance.adsDiaCount < 3)
                        {
                            Debug.Log("광고 버튼 클릭");
                            AdmobManager.Instance.ShowRewardVideo((int)myAdsType, null);

                            layout_BuyEnergyGroup.SetActive(false);
                        }
                        break;
                    }
                case RewardAdsType.Ads_Energy:
                    {
                        if (StatManager.Instance.adsEnergyCount < 3)
                        {
                            Debug.Log("광고 버튼 클릭");
                            AdmobManager.Instance.ShowRewardVideo((int)myAdsType, null);

                            layout_BuyEnergyGroup.SetActive(false);
                        }
                        break;
                    }
                default: break;
            }
        }
    }
}
