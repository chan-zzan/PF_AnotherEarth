using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EncyclopediaInfo : MonoBehaviour
{
    [Header("ÆË¾÷")]
    public GameObject infoPopUp;

    public TextMeshProUGUI curCount;
    public TextMeshProUGUI maxCount;

    private void OnEnable()
    {
        UpdateUI();
    }

    private void OnDisable()
    {
        if(infoPopUp.activeSelf)
        {
            infoPopUp.SetActive(false); 
        }
    }

    public void UpdateUI()
    {
        curCount.text = SlotManager.Instance.countingReleaseAnimal.ToString();
        maxCount.text = SlotManager.Instance.maxReleaseAnimal.ToString();

        if (SlotManager.Instance.countingReleaseAnimal >= SlotManager.Instance.maxReleaseAnimal)
        {
            curCount.color = new Color(255, 0, 0, 255);
            maxCount.color = new Color(255, 0, 0, 255);
        }
        else
        {
            curCount.color = new Color(255, 255, 255, 255);
            maxCount.color = new Color(255, 255, 255, 255);
        }

    }
}
