using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlantItemInfo : MonoBehaviour
{
    [Header("������ ���� ���� �ؽ�Ʈ")]
    public TextMeshProUGUI remainText;

    [Header("������ ���� �˾�")]
    public GameObject itemInfoPopUp;

    [Header("��ư ���� ����� ��ư �̹���")]
    public Image[] buttonImage;

    private Color onColor = new Color(255, 0, 0, 255);
    private Color offColor = new Color(130, 130, 130, 255);

    // �������� Ȱ��ȭ �Ǿ�����
    private bool isEnableItem = false;

    private void OnEnable()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        remainText.text = "("+ SlotManager.Instance.plantItem.ToString() + ")";
        
        // �������� ��� ������ ���
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
