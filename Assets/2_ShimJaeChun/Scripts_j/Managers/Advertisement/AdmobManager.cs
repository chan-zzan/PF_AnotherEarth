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
        #region �̱���
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
            // �� ����ÿ��� �ı����� �ʴ´�.
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(AdsList);
        }

        #endregion

        [Header("���� ���� ����Ʈ")]
        [Header("0:����(���̾�) 1:����(������)")]
        public List<Adbase> rewardAdList = new List<Adbase>();

        [Header("���� ���� Ÿ�� ����Ʈ")]
        [Header("0:����(���̾�) 1:����(������)")]
        public List<GetRewardType> getRewardTypeList = new List<GetRewardType>();

        [Header("���� �������޷� ����Ʈ")]
        [Header("0:����(���̾�) 1:����(������)")]
        public List<int> getAmountList = new List<int>();

        
        public GameObject AdsList;

        private void Start()
        {
            AdInitialize();
        }

        // ���� �ʱ�ȭ
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
