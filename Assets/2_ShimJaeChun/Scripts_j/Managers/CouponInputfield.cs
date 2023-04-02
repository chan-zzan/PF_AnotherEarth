using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CouponInputfield : MonoBehaviour
{
    public TMP_InputField inputF;

    public TextMeshProUGUI warning;

    [Header("어나더어스S2")]
    [SerializeField]
    private string coupon1;
    public GameObject coupon1PopUp;

    [Header("SS감사합니다22")]
    [SerializeField]
    private string coupon2;
    public GameObject coupon2PopUp;

    [Header("SS설문보상22")]
    [SerializeField]
    private string coupon3;
    public GameObject coupon3PopUp;

    [Header("개발자 전용 쿠폰")]
    [SerializeField]
    private string couponDevelop;

    public void OnClickClearButton()
    {
        if(inputF.text == coupon1
            && !StatManager.Instance.usedCoupon1)
        {
            // 입력성공
            SoundManager.Instance.PlayEffectSound(EffectSoundType.PlayerLevelUpSound);
            UserProductManager.Instance.BuyProduct(ProductType.Animal, 1);
            StatManager.Instance.AddDia(100);
            StatManager.Instance.usedCoupon1 = true;
            this.gameObject.SetActive(false);
            coupon1PopUp.SetActive(true);
            inputF.text = "";
        }
        else if(inputF.text == coupon2
            && !StatManager.Instance.usedCoupon2)
        {
            // 입력성공
            SoundManager.Instance.PlayEffectSound(EffectSoundType.PlayerLevelUpSound);
            StatManager.Instance.AddDia(50);
            StatManager.Instance.usedCoupon2 = true;
            this.gameObject.SetActive(false);
            coupon2PopUp.SetActive(true);
            inputF.text = "";
        }
        else if (inputF.text == coupon3)
        {
            // 입력성공
            SoundManager.Instance.PlayEffectSound(EffectSoundType.PlayerLevelUpSound);
            StatManager.Instance.AddDia(100);
            StatManager.Instance.usedCoupon3 = true;
            this.gameObject.SetActive(false);
            coupon3PopUp.SetActive(true);
            inputF.text = "";
        }
        else if (inputF.text == couponDevelop)
        {
            // 입력성공
            SoundManager.Instance.PlayEffectSound(EffectSoundType.PlayerLevelUpSound);
            StatManager.Instance.AddMineral(999999999);
            StatManager.Instance.AddDia(999999999);
            StatManager.Instance.AddExtraEnergy(300);
            this.gameObject.SetActive(false);
            inputF.text = "";
        }
        else
        {
            // 실패
            warning.text = "잘못된 코드입니다.";
            inputF.text = "";
        }
    }
}
