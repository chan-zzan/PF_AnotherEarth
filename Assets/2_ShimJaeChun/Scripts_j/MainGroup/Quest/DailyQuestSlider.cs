using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyQuestSlider : MonoBehaviour
{
    [Header("일일 퀘스트 진행도 바")]
    public Slider dailyQuestProceedBar;

    [Header("보상을 수령했는지?")]
    public bool isGetReward;

    [Header("기본 다이아")]
    public GameObject normalDia;

    [Header("보상 다이아")]
    public GameObject rewardDia;

    [Header("보상 다이아 지급량")]
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

        // 보상을 수령한 경우
        if (isGetReward == true)
        {
            // 보상획득 다이아 컬러 적용
            normalDia.GetComponent<Image>().color = new Color(255, 255, 255, 255);

            dailyQuestProceedBar.value = 1.0f;
        }
        // 보상을 수령하지 않은 경우
        else
        {
            // 보상 수령이 가능한 상태
            if(dailyQuestProceedBar.value >= 0.9f)
            {
                rewardDia.SetActive(true);
                normalDia.SetActive(false);
            }
            else
            {
                rewardDia.SetActive(false);
                normalDia.SetActive(true);

                // 퀘스트 진행 중 다이아 컬러 적용
                normalDia.GetComponent<Image>().color = new Color(135, 135, 135, 255);
            }
        }
    }
    
    // 보상 다이아 버튼 클릭 시 
    public void OnClickRewardButton()
    {
        SoundManager.Instance.PlayEffectSound(EffectSoundType.HarvestPlantSound);

        QuestManager.Instance.isGetDailyAllReward = true;

        StatManager.Instance.AddDia(diaAmount);
        rewardDia.SetActive(false);
        normalDia.SetActive(true);
        
        // 보상획득 시 다이아 컬러 적용
        normalDia.GetComponent<Image>().color = new Color(255,255,255,255);

        isGetReward = true;
    }

    public void ResetSlider()
    {
        dailyQuestProceedBar.value = 0;

        rewardDia.SetActive(false);
        normalDia.SetActive(true);

        // 퀘스트 진행 중 다이아 컬러 적용
        normalDia.GetComponent<Image>().color = new Color(135, 135, 135, 255);
    }

    public void UpdateDailyQuestSlider()
    {
        Debug.Log("슬라이더 업데이트");

        if (dailyQuestProceedBar.value < 0.9f)
        {
            dailyQuestProceedBar.value += 0.2f;

            // 일일 퀘스트를 모두 완료한 경우
            if(dailyQuestProceedBar.value >= 0.9f)
            {
                rewardDia.SetActive(true);
                normalDia.SetActive(false);
            }
        }
    }
}
