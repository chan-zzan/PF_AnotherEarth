using UnityEngine;

// Adbase �߻� Ŭ���� 
namespace Modules.Ads
{
    // ����� ������ ���� ����
    public abstract class Adbase : MonoBehaviour
    {
        [SerializeField]
        protected bool isRewardUse = true;  // ������ ����

        protected System.Action<AdResultType> OnRewardResult = null;

        [SerializeField]
        protected Advertiser advertiser;

        public Advertiser Type
        {
            get { return advertiser; }
        }

        // ���� ��� �ʱ�ȭ
        public abstract void OnInitialize();

        // ������ ���� ��û
        public abstract void ShowRewardVideo(System.Action<AdResultType> result);
    }
}