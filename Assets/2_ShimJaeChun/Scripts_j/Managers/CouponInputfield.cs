using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CouponInputfield : MonoBehaviour
{
    public TMP_InputField inputF;

    public TextMeshProUGUI warning;

    [Header("����S2")]
    [SerializeField]
    private string coupon1;
    public GameObject coupon1PopUp;

    [Header("SS�����մϴ�22")]
    [SerializeField]
    private string coupon2;
    public GameObject coupon2PopUp;

    [Header("SS��������22")]
    [SerializeField]
    private string coupon3;
    public GameObject coupon3PopUp;

    [Header("������ ���� ����")]
    [SerializeField]
    private string couponDevelop;

    public void OnClickClearButton()
    {
        if(inputF.text == coupon1
            && !StatManager.Instance.usedCoupon1)
        {
            // �Է¼���
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
            // �Է¼���
            SoundManager.Instance.PlayEffectSound(EffectSoundType.PlayerLevelUpSound);
            StatManager.Instance.AddDia(50);
            StatManager.Instance.usedCoupon2 = true;
            this.gameObject.SetActive(false);
            coupon2PopUp.SetActive(true);
            inputF.text = "";
        }
        else if (inputF.text == coupon3)
        {
            // �Է¼���
            SoundManager.Instance.PlayEffectSound(EffectSoundType.PlayerLevelUpSound);
            StatManager.Instance.AddDia(100);
            StatManager.Instance.usedCoupon3 = true;
            this.gameObject.SetActive(false);
            coupon3PopUp.SetActive(true);
            inputF.text = "";
        }
        else if (inputF.text == couponDevelop)
        {
            // �Է¼���
            SoundManager.Instance.PlayEffectSound(EffectSoundType.PlayerLevelUpSound);
            StatManager.Instance.AddMineral(999999999);
            StatManager.Instance.AddDia(999999999);
            StatManager.Instance.AddExtraEnergy(300);
            this.gameObject.SetActive(false);
            inputF.text = "";
        }
        else
        {
            // ����
            warning.text = "�߸��� �ڵ��Դϴ�.";
            inputF.text = "";
        }
    }
}
