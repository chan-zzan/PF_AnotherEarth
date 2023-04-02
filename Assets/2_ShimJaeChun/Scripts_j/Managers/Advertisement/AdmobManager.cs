using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public enum RewardAdsType
{
    Ads_Dia,
    Ads_Energy,
    Ads_Revival,
    Ads_Battle
}

namespace Modules.Ads
{
    public class AdmobManager : MonoBehaviour
    {
        #region 싱글톤
        private static AdmobManager instance;
        public static AdmobManager Instance
        {
            get
            {
                if (instance == null)
                {
                    var obj = FindObjectOfType<AdmobManager>();
                    if (obj != null)
                    {
                        instance = obj;
                    }
                    else
                    {
                        var newObj = new GameObject().AddComponent<AdmobManager>();
                        instance = newObj;
                    }
                }
                return instance;
            }
        }
        private void Awake()
        {
            var objs = FindObjectsOfType<AdmobManager>();
            if (objs.Length != 1)
            {
                Destroy(gameObject);
                return;
            }
            // 씬 변경시에도 파괴되지 않는다.
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(AdsList);
        }

        #endregion

        [Header("보상 광고 리스트")]
        [Header("0:상점(다이아) 1:상점(에너지)")]
        public List<Adbase> rewardAdList = new List<Adbase>();

        [Header("광고 보상 타입 리스트")]
        [Header("0:상점(다이아) 1:상점(에너지)")]
        public List<GetRewardType> getRewardTypeList = new List<GetRewardType>();

        [Header("광고 보상지급량 리스트")]
        [Header("0:상점(다이아) 1:상점(에너지)")]
        public List<int> getAmountList = new List<int>();

        
        public GameObject AdsList;

        private void Start()
        {
            AdInitialize();
        }

        // 광고 초기화
        public void AdInitialize()
        {
            for(int i = 0; i< rewardAdList.Count; i++)
            {
                rewardAdList[i].OnInitialize();
            }
        }

        public void ShowRewardVideo(int adsNum, System.Action<AdResultType> result)
        {
            if(rewardAdList[adsNum] != null)
            {
                rewardAdList[adsNum].ShowRewardVideo(result);
                Debug.Log(result);
            }
        }
    }
}
