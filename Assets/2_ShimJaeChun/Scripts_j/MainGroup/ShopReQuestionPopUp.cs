using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopReQuestionPopUp : MonoBehaviour
{

    public BuyButton buyInfo;

    [Header("üũ�ڽ� üũ �̹���")]
    public GameObject checkImage;

    public void UpdateBuyInfo(BuyButton _buyInfo)
    {
        buyInfo = _buyInfo;
    }

    public void OnClickYesButton()
    {
        buyInfo.BuyProduct();
    }

    public void OnClickCheckBox()
    {
        if(SlotManager.Instance.isRequestOn)
        {
            checkImage.SetActive(true);
            SlotManager.Instance.isRequestOn = false;      
        }
        else
        {
            checkImage.SetActive(false);
            SlotManager.Instance.isRequestOn = true;
        }
    }
}
