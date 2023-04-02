using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace Modules.Ads
{ 
    public class AdsPlayButton : MonoBehaviour
    {
        public RewardAdsType myAdsType;

        public TextMeshProUGUI countingText;

        private void Start()
        {
        }

        private void OnEnable()
        {
            UpdateUI();
        }

        public void UpdateUI()
        {
            switch (myAdsType)
            {
                case RewardAdsType.Ads_Dia:
                    {
                        countingText.text = "(" + StatManager.Instance.adsDiaCount.ToString() + "/3)";
                        break;
                    }
                case RewardAdsType.Ads_Energy:
                    {
                        countingText.text = "(" + StatManager.Instance.adsEnergyCount.ToString() + "/3)";
                        break;
                    }
                case RewardAdsType.Ads_Battle:
                    {
                        countingText.text = "(" + StatManager.Instance.adsEnergyCount.ToString() + "/3)";
                        break;
                    }
                default: break;
            }
        }

        public void OnClickAdsPlayButton()
        {
            switch (myAdsType)
            {
                case RewardAdsType.Ads_Dia:
                    {
                        if (StatManager.Instance.adsDiaCount > 0)
                        {
                            Debug.Log("광고 버튼 클릭");
                            //StatManager.Instance.AddDia(20);
                            //StatManager.Instance.AdsCounting(RewardAdsType.Ads_Dia);
                            AdmobManager.Instance.ShowRewardVideo((int)myAdsType, null);
                        }
                        break;
                    }
                case RewardAdsType.Ads_Energy:
                    {
                        if (StatManager.Instance.adsEnergyCount > 0)
                        {
                            Debug.Log("광고 버튼 클릭");
                            //SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponLevelUpSound);
                            //StatManager.Instance.AddExtraEnergy(5);
                            //StatManager.Instance.AdsCounting(RewardAdsType.Ads_Energy);
                            AdmobManager.Instance.ShowRewardVideo((int)myAdsType, null);
                        }
                        break;
                    }
                case RewardAdsType.Ads_Revival:
                    {
                        Debug.Log("광고 버튼 클릭");
                        // 부활
                        //if (StatManager.Instance.adsRevivalCount > 0)
                        //{
                        //    GameManager_E.Instance.Revival(false);
                        //    StatManager.Instance.AdsCounting(RewardAdsType.Ads_Revival);
                        //}
                        AdmobManager.Instance.ShowRewardVideo((int)myAdsType, null);
                        break;
                    }
                case RewardAdsType.Ads_Battle:
                    {
                        if (StatManager.Instance.adsEnergyCount > 0)
                        {
                            Debug.Log("광고 버튼 클릭");
                            //StatManager.Instance.AdsCounting(RewardAdsType.Ads_Energy);
                            //SceneManager.LoadScene("InGame_E");
                            //Time.timeScale = 1f;
                            AdmobManager.Instance.ShowRewardVideo((int)myAdsType, null);
                        }
                        break;
                    }

                default: break;
            }
        }
    }
}
