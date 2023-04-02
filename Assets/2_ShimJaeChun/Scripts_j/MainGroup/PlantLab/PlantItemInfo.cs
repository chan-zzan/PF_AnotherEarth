using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlantItemInfo : MonoBehaviour
{
    [Header("아이템 남은 개수 텍스트")]
    public TextMeshProUGUI remainText;

    [Header("아이템 정보 팝업")]
    public GameObject itemInfoPopUp;

    [Header("버튼 색상 변경용 버튼 이미지")]
    public Image[] buttonImage;

    private Color onColor = new Color(255, 0, 0, 255);
    private Color offColor = new Color(130, 130, 130, 255);

    // 아이템이 활성화 되었는지
    private bool isEnableItem = false;

    private void OnEnable()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        remainText.text = "("+ SlotManager.Instance.plantItem.ToString() + ")";
        
        // 아이템이 모두 소진된 경우
        if(SlotManager.Instance.plantItem <= 0)
        {
            OffItem();
        }
    }

    public void OnClickPlantItemButton()
    {
        if(SlotManager.Instance.plantItem > 0)
        {
            if(isEnableItem)
            {
                SoundManager.Instance.PlayOpenSound();
                OffItem();
            }
            else
            {
                SoundManager.Instance.PlayOpenSound();
                OnItem();
            }
        }
        else
        {
            SoundManager.Instance.PlayOpenSound();
            itemInfoPopUp.SetActive(true);
        }
    }

    public bool GetItemState()
    {
        return isEnableItem;
    }

    public void OnItem()
    {
        isEnableItem = true;

        for(int i=0; i<buttonImage.Length; i++)
        {
            buttonImage[i].color = onColor;
        }
    }
    public void OffItem()
    {
        isEnableItem = false;

        for (int i = 0; i < buttonImage.Length; i++)
        {
            buttonImage[i].color = offColor;
        }

    }
}
