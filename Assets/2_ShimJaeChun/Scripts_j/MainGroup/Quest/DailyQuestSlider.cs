using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyQuestSlider : MonoBehaviour
{
    [Header("���� ����Ʈ ���൵ ��")]
    public Slider dailyQuestProceedBar;

    [Header("������ �����ߴ���?")]
    public bool isGetReward;

    [Header("�⺻ ���̾�")]
    public GameObject normalDia;

    [Header("���� ���̾�")]
    public GameObject rewardDia;

    [Header("���� ���̾� ���޷�")]
    public int diaAmount;

    private void Start()
    {
        isGetReward = false;
    }

    private void OnEnable()
    {
        isGetReward = QuestManager.Instance.isGetDailyAllReward;

        rewardDia.SetActive(false);
        normalDia.SetActive(true);

        // ������ ������ ���
        if (isGetReward == true)
        {
            // ����ȹ�� ���̾� �÷� ����
            normalDia.GetComponent<Image>().color = new Color(255, 255, 255, 255);

            dailyQuestProceedBar.value = 1.0f;
        }
        // ������ �������� ���� ���
        else
        {
            // ���� ������ ������ ����
            if(dailyQuestProceedBar.value >= 0.9f)
            {
                rewardDia.SetActive(true);
                normalDia.SetActive(false);
            }
            else
            {
                rewardDia.SetActive(false);
                normalDia.SetActive(true);

                // ����Ʈ ���� �� ���̾� �÷� ����
                normalDia.GetComponent<Image>().color = new Color(135, 135, 135, 255);
            }
        }
    }
    
    // ���� ���̾� ��ư Ŭ�� �� 
    public void OnClickRewardButton()
    {
        SoundManager.Instance.PlayEffectSound(EffectSoundType.HarvestPlantSound);

        QuestManager.Instance.isGetDailyAllReward = true;

        StatManager.Instance.AddDia(diaAmount);
        rewardDia.SetActive(false);
        normalDia.SetActive(true);
        
        // ����ȹ�� �� ���̾� �÷� ����
        normalDia.GetComponent<Image>().color = new Color(255,255,255,255);

        isGetReward = true;
    }

    public void ResetSlider()
    {
        dailyQuestProceedBar.value = 0;

        rewardDia.SetActive(false);
        normalDia.SetActive(true);

        // ����Ʈ ���� �� ���̾� �÷� ����
        normalDia.GetComponent<Image>().color = new Color(135, 135, 135, 255);
    }

    public void UpdateDailyQuestSlider()
    {
        Debug.Log("�����̴� ������Ʈ");

        if (dailyQuestProceedBar.value < 0.9f)
        {
            dailyQuestProceedBar.value += 0.2f;

            // ���� ����Ʈ�� ��� �Ϸ��� ���
            if(dailyQuestProceedBar.value >= 0.9f)
            {
                rewardDia.SetActive(true);
                normalDia.SetActive(false);
            }
        }
    }
}
