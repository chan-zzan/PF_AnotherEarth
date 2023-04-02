using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AllQuestInfo : MonoBehaviour
{
    [Header("퀘스트 타입")]
    public AllQuestType myType;

    [Header("퀘스트 보상 타입")]
    public QuestRewardType myRewardType;

    [Header("보상 지급량")]
    public float rewardPerCount;

    [Header("퀘스트 클리어 버튼")]
    public Button clearButton;

    [Header("퀘스트 진행중 배지")]
    public GameObject proceedBadge;

    [Header("퀘스트 클리어 배지")]
    public GameObject clearBadge;

    [Header("퀘스트 잠금")]
    public GameObject questLock;

    [Header("퀘스트 조건 텍스트")]
    public TextMeshProUGUI conditionText;

    [Header("퀘스트 진행도 텍스트")]
    public TextMeshProUGUI proceedText;

    [Header("퀘스트 보상 텍스트")]
    public TextMeshProUGUI rewardText;

    [Header("퀘스트 조건 변수")]
    public int questReq;

    [Header("퀘스트 클리어 횟수")]
    public int questClearCount;

    [Header("퀘스트 클리어가 가능한 상태인지?")]
    public bool isClearEnable;

    [Header("퀘스트 상태 (읽기 전용)")]
    public QuestButtonState myState;

    // Start is called before the first frame update
    void Awake()
    {
    }

    private void OnEnable()
    {
        CheckQuestCondition();
    }

    public void CheckQuestCondition()
    {
        switch (myType)
        {
            case AllQuestType.All_Attend:
                {
                    if (GameTimeManager.Instance.dayCounting >= questReq)
                    {
                        SetQuestState(QuestButtonState.Enable);
                    }
                    else
                    {
                        SetQuestState(QuestButtonState.Proceed);
                    }
                    UpdateUI();
                    break;
                }
            case AllQuestType.All_DailyQuestCount:
                {
                    if(QuestManager.Instance.dailyQuestClearCount >= questReq)
                    {
                        SetQuestState(QuestButtonState.Enable);
                    }
                    else
                    {
                        SetQuestState(QuestButtonState.Proceed);
                    }
                    UpdateUI();
                    break;
                }
            case AllQuestType.All_ClearEasyStage:
                {
                    // 최대 스테이지까지 클리어 완료한 경우
                    if(questReq > 10)
                    {
                        conditionText.text = StageManager.Instance.stageMainTextList[9];
                        SetQuestState(QuestButtonState.Clear);
                        break;
                    }
                    if(StageManager.Instance.EasyClearList[questReq-1])
                    {
                        SetQuestState(QuestButtonState.Enable);
                    }
                    else
                    {
                        SetQuestState(QuestButtonState.Proceed);
                    }
                    UpdateUI();
                    break;
                }
            case AllQuestType.All_ClearHardStage:
                {
                    // 최대 스테이지까지 클리어 완료한 경우
                    if (questReq > 10)
                    {
                        conditionText.text = StageManager.Instance.stageMainTextList[9];
                        SetQuestState(QuestButtonState.Clear);
                        break;
                    }
                    if (StageManager.Instance.HardClearList[questReq - 1])
                    {
                        SetQuestState(QuestButtonState.Enable);
                    }
                    else
                    {
                        SetQuestState(QuestButtonState.Proceed);
                    }
                    UpdateUI();
                    break;
                }
            case AllQuestType.All_UnLockPlant:
                {
                    // 마지막 식물까지 해금 완료한 경우
                    if (questReq > 19)
                    {
                        conditionText.text = SlotManager.Instance.plantDataList[19].PlantName;
                        SetQuestState(QuestButtonState.Clear);
                        break;
                    }
                    if (SlotManager.Instance.harvestCount[questReq-1] >= 5)
                    {
                        SetQuestState(QuestButtonState.Enable);
                    }
                    else
                    {
                        SetQuestState(QuestButtonState.Proceed);
                    }
                    UpdateUI();


                    break;
                }
            default: break;
        }

    }

    public void UpdateUI()
    {
        switch (myType)
        {
            case AllQuestType.All_Attend:
                {
                    conditionText.text = questReq.ToString();
                    rewardText.text = "+"+rewardPerCount.ToString();
                    proceedText.text = GameTimeManager.Instance.dayCounting.ToString(); 

                    // 클리어가 가능한 상태라면
                    if(isClearEnable)
                    {
                        isClearEnable = true;

                        // 클리어 버튼 활성화
                        clearButton.enabled = true;
                        // 클리어 배지 활성화
                        proceedBadge.SetActive(false);
                        clearBadge.SetActive(true);
                    }
                    else
                    {
                        isClearEnable = false;

                        // 클리어 버튼 비활성화
                        clearButton.enabled = false;
                        // 진행 배지 활성화
                        proceedBadge.SetActive(true);
                        clearBadge.SetActive(false);
                    }
                    break;
                }
            case AllQuestType.All_DailyQuestCount:
                {
                    conditionText.text = questReq.ToString();
                    rewardText.text = "+" + rewardPerCount.ToString();
                    proceedText.text = QuestManager.Instance.dailyQuestClearCount.ToString();

                    // 클리어가 가능한 상태라면
                    if (isClearEnable)
                    {
                        isClearEnable = true;

                        // 클리어 버튼 활성화
                        clearButton.enabled = true;
                        // 클리어 배지 활성화
                        proceedBadge.SetActive(false);
                        clearBadge.SetActive(true);
                    }
                    else
                    {
                        isClearEnable = false;

                        // 클리어 버튼 비활성화
                        clearButton.enabled = false;
                        // 진행 배지 활성화
                        proceedBadge.SetActive(true);
                        clearBadge.SetActive(false);
                    }
                    break;
                }
            case AllQuestType.All_ClearEasyStage:
                {
                    conditionText.text = StageManager.Instance.stageMainTextList[questReq-1];
                    rewardText.text = "+" + rewardPerCount.ToString();

                    // 클리어가 가능한 상태라면
                    if (isClearEnable)
                    {
                        isClearEnable = true;

                        // 클리어 버튼 활성화
                        clearButton.enabled = true;
                        // 클리어 배지 활성화
                        proceedBadge.SetActive(false);
                        clearBadge.SetActive(true);
                    }
                    else
                    {
                        isClearEnable = false;

                        // 클리어 버튼 비활성화
                        clearButton.enabled = false;
                        // 진행 배지 활성화
                        proceedBadge.SetActive(true);
                        clearBadge.SetActive(false);
                    }

                    break;
                }
            case AllQuestType.All_ClearHardStage:
                {
                    conditionText.text = StageManager.Instance.stageMainTextList[questReq - 1];
                    rewardText.text = "+" + rewardPerCount.ToString();

                    // 클리어가 가능한 상태라면
                    if (isClearEnable)
                    {
                        isClearEnable = true;

                        // 클리어 버튼 활성화
                        clearButton.enabled = true;
                        // 클리어 배지 활성화
                        proceedBadge.SetActive(false);
                        clearBadge.SetActive(true);
                    }
                    else
                    {
                        isClearEnable = false;

                        // 클리어 버튼 비활성화
                        clearButton.enabled = false;
                        // 진행 배지 활성화
                        proceedBadge.SetActive(true);
                        clearBadge.SetActive(false);
                    }

                    break;
                }
            case AllQuestType.All_UnLockPlant:
                {
                    conditionText.text = SlotManager.Instance.plantDataList[questReq].PlantName;
                    rewardText.text = "+" + rewardPerCount.ToString();

                    // 클리어가 가능한 상태라면
                    if (isClearEnable)
                    {
                        isClearEnable = true;

                        // 클리어 버튼 활성화
                        clearButton.enabled = true;
                        // 클리어 배지 활성화
                        proceedBadge.SetActive(false);
                        clearBadge.SetActive(true);
                    }
                    else
                    {
                        isClearEnable = false;

                        // 클리어 버튼 비활성화
                        clearButton.enabled = false;
                        // 진행 배지 활성화
                        proceedBadge.SetActive(true);
                        clearBadge.SetActive(false);
                    }

                    break;
                }
            default: break;
        }

    }

    public void LoadSetting(int clearCount)
    {
        if(clearCount == 0)
        {
            return;
        }

        switch (myType)
        {
            case AllQuestType.All_Attend:
                {
                    questClearCount = clearCount;
                    questReq += questReq* questClearCount;
                    break;
                }
            case AllQuestType.All_DailyQuestCount:
                {
                    questClearCount = clearCount;
                    questReq += questReq * questClearCount;
                    break;
                }
            case AllQuestType.All_ClearEasyStage:
                {
                    questClearCount = clearCount;
                    questReq += questReq * questClearCount;
                    rewardPerCount += (questClearCount / 3) * QuestManager.Instance.allQuestRewardList[(int)AllQuestType.All_ClearEasyStage];
                    break;
                }
            case AllQuestType.All_ClearHardStage:
                {
                    questClearCount = clearCount;
                    questReq += questReq * questClearCount;
                    rewardPerCount += (questClearCount / 3) * QuestManager.Instance.allQuestRewardList[(int)AllQuestType.All_ClearHardStage];
                    break;
                }
            case AllQuestType.All_UnLockPlant:
                {
                    questClearCount = clearCount;
                    questReq += questReq * questClearCount;
                    rewardPerCount += (questClearCount / 5) * QuestManager.Instance.allQuestRewardList[(int)AllQuestType.All_UnLockPlant];
                    break;
                }
            default: break;
        }

    }


    // 클리어 버튼 클릭 시 
    public void OnClickClearButton()
    {
        questClearCount++;

        QuestManager.Instance.allQuestClearCountList[(int)myType] = questClearCount;

        // 사운드 출력
        SoundManager.Instance.PlayEffectSound(EffectSoundType.QuestClearSound);

        switch (myRewardType)
        {
            case QuestRewardType.Coin:
                {
                    StatManager.Instance.AddMineral(rewardPerCount);
                    RenewQuestCondition();
                    break;
                }
            case QuestRewardType.Diamond:
                {
                    StatManager.Instance.AddDia(rewardPerCount);
                    RenewQuestCondition();
                    break;
                }
        }

    }


    // 퀘스트 조건 갱신
    public void RenewQuestCondition()
    {
        switch(myType)
        {
            case AllQuestType.All_Attend:
                {
                    questReq += 5;
                    break;
                }
            case AllQuestType.All_DailyQuestCount:
                {
                    questReq += 10;
                    break;
                }
            case AllQuestType.All_ClearEasyStage:
                {
                    questReq += 1;
                    rewardPerCount = 10 + (questClearCount / 3) * QuestManager.Instance.allQuestRewardList[(int)AllQuestType.All_ClearEasyStage];
                    break;
                }
            case AllQuestType.All_ClearHardStage:
                {
                    questReq += 1;
                    rewardPerCount = 15 + (questClearCount / 3) * QuestManager.Instance.allQuestRewardList[(int)AllQuestType.All_ClearHardStage];
                    break;
                }
            case AllQuestType.All_UnLockPlant:
                {
                    questReq += 1;
                    rewardPerCount = 5 + (questClearCount / 5) * QuestManager.Instance.allQuestRewardList[(int)AllQuestType.All_UnLockPlant];
                    break;
                }
            default: break;
        }

        // 갱신 이후 다시 퀘스트 조건 체크
        CheckQuestCondition();
    }

    public void SetQuestState(QuestButtonState _state)
    {
        switch (_state)
        {
            // 진행중
            case QuestButtonState.Proceed:
                {
                    isClearEnable = false;

                    // 클리어 버튼 비활성화
                    clearButton.enabled = false;
                    // 진행 배지 활성화
                    proceedBadge.SetActive(true);
                    clearBadge.SetActive(false);

                    myState = QuestButtonState.Proceed;
                    break;
                }
            // 보상 획득 가능
            case QuestButtonState.Enable:
                {
                    isClearEnable = true;

                    // 클리어 버튼활성화
                    clearButton.enabled = true;

                    // 클리어 배지 활성화
                    proceedBadge.SetActive(false);
                    clearBadge.SetActive(true);

                    myState = QuestButtonState.Enable;
                    break;
                }

            // 모든 퀘스트 클리어
            case QuestButtonState.Clear:
                {
                    isClearEnable = false;

                    // 퀘스트 클리어 락 활성화
                    questLock.SetActive(true);

                    // 배지 모두 비활성화
                    proceedBadge.SetActive(false);
                    clearBadge.SetActive(false);
                    // 버튼 비활성화
                    clearButton.enabled = false;
                    myState = QuestButtonState.Clear;
                    break;
                }
            default: break;
        }

    }

}
