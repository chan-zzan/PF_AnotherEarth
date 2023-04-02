using UnityEngine;

// Adbase 추상 클래스 
namespace Modules.Ads
{
    // 현재는 리워드 광고만 포함
    public abstract class Adbase : MonoBehaviour
    {
        [SerializeField]
        protected bool isRewardUse = true;  // 리워드 광고

        protected System.Action<AdResultType> OnRewardResult = null;

        [SerializeField]
        protected Advertiser advertiser;

        public Advertiser Type
        {
            get { return advertiser; }
        }

        // 광고 모듈 초기화
        public abstract void OnInitialize();

        // 동영상 광고 시청
        public abstract void ShowRewardVideo(System.Action<AdResultType> result);
    }
}