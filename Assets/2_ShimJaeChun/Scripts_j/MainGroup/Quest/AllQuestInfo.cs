using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AllQuestInfo : MonoBehaviour
{
    [Header("����Ʈ Ÿ��")]
    public AllQuestType myType;

    [Header("����Ʈ ���� Ÿ��")]
    public QuestRewardType myRewardType;

    [Header("���� ���޷�")]
    public float rewardPerCount;

    [Header("����Ʈ Ŭ���� ��ư")]
    public Button clearButton;

    [Header("����Ʈ ������ ����")]
    public GameObject proceedBadge;

    [Header("����Ʈ Ŭ���� ����")]
    public GameObject clearBadge;

    [Header("����Ʈ ���")]
    public GameObject questLock;

    [Header("����Ʈ ���� �ؽ�Ʈ")]
    public TextMeshProUGUI conditionText;

    [Header("����Ʈ ���൵ �ؽ�Ʈ")]
    public TextMeshProUGUI proceedText;

    [Header("����Ʈ ���� �ؽ�Ʈ")]
    public TextMeshProUGUI rewardText;

    [Header("����Ʈ ���� ����")]
    public int questReq;

    [Header("����Ʈ Ŭ���� Ƚ��")]
    public int questClearCount;

    [Header("����Ʈ Ŭ��� ������ ��������?")]
    public bool isClearEnable;

    [Header("����Ʈ ���� (�б� ����)")]
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
                    // �ִ� ������������ Ŭ���� �Ϸ��� ���
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
                    // �ִ� ������������ Ŭ���� �Ϸ��� ���
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
                    // ������ �Ĺ����� �ر� �Ϸ��� ���
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

                    // Ŭ��� ������ ���¶��
                    if(isClearEnable)
                    {
                        isClearEnable = true;

                        // Ŭ���� ��ư Ȱ��ȭ
                        clearButton.enabled = true;
                        // Ŭ���� ���� Ȱ��ȭ
                        proceedBadge.SetActive(false);
                        clearBadge.SetActive(true);
                    }
                    else
                    {
                        isClearEnable = false;

                        // Ŭ���� ��ư ��Ȱ��ȭ
                        clearButton.enabled = false;
                        // ���� ���� Ȱ��ȭ
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

                    // Ŭ��� ������ ���¶��
                    if (isClearEnable)
                    {
                        isClearEnable = true;

                        // Ŭ���� ��ư Ȱ��ȭ
                        clearButton.enabled = true;
                        // Ŭ���� ���� Ȱ��ȭ
                        proceedBadge.SetActive(false);
                        clearBadge.SetActive(true);
                    }
                    else
                    {
                        isClearEnable = false;

                        // Ŭ���� ��ư ��Ȱ��ȭ
                        clearButton.enabled = false;
                        // ���� ���� Ȱ��ȭ
                        proceedBadge.SetActive(true);
                        clearBadge.SetActive(false);
                    }
                    break;
                }
            case AllQuestType.All_ClearEasyStage:
                {
                    conditionText.text = StageManager.Instance.stageMainTextList[questReq-1];
                    rewardText.text = "+" + rewardPerCount.ToString();

                    // Ŭ��� ������ ���¶��
                    if (isClearEnable)
                    {
                        isClearEnable = true;

                        // Ŭ���� ��ư Ȱ��ȭ
                        clearButton.enabled = true;
                        // Ŭ���� ���� Ȱ��ȭ
                        proceedBadge.SetActive(false);
                        clearBadge.SetActive(true);
                    }
                    else
                    {
                        isClearEnable = false;

                        // Ŭ���� ��ư ��Ȱ��ȭ
                        clearButton.enabled = false;
                        // ���� ���� Ȱ��ȭ
                        proceedBadge.SetActive(true);
                        clearBadge.SetActive(false);
                    }

                    break;
                }
            case AllQuestType.All_ClearHardStage:
                {
                    conditionText.text = StageManager.Instance.stageMainTextList[questReq - 1];
                    rewardText.text = "+" + rewardPerCount.ToString();

                    // Ŭ��� ������ ���¶��
                    if (isClearEnable)
                    {
                        isClearEnable = true;

                        // Ŭ���� ��ư Ȱ��ȭ
                        clearButton.enabled = true;
                        // Ŭ���� ���� Ȱ��ȭ
                        proceedBadge.SetActive(false);
                        clearBadge.SetActive(true);
                    }
                    else
                    {
                        isClearEnable = false;

                        // Ŭ���� ��ư ��Ȱ��ȭ
                        clearButton.enabled = false;
                        // ���� ���� Ȱ��ȭ
                        proceedBadge.SetActive(true);
                        clearBadge.SetActive(false);
                    }

                    break;
                }
            case AllQuestType.All_UnLockPlant:
                {
                    conditionText.text = SlotManager.Instance.plantDataList[questReq].PlantName;
                    rewardText.text = "+" + rewardPerCount.ToString();

                    // Ŭ��� ������ ���¶��
                    if (isClearEnable)
                    {
                        isClearEnable = true;

                        // Ŭ���� ��ư Ȱ��ȭ
                        clearButton.enabled = true;
                        // Ŭ���� ���� Ȱ��ȭ
                        proceedBadge.SetActive(false);
                        clearBadge.SetActive(true);
                    }
                    else
                    {
                        isClearEnable = false;

                        // Ŭ���� ��ư ��Ȱ��ȭ
                        clearButton.enabled = false;
                        // ���� ���� Ȱ��ȭ
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


    // Ŭ���� ��ư Ŭ�� �� 
    public void OnClickClearButton()
    {
        questClearCount++;

        QuestManager.Instance.allQuestClearCountList[(int)myType] = questClearCount;

        // ���� ���
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


    // ����Ʈ ���� ����
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

        // ���� ���� �ٽ� ����Ʈ ���� üũ
        CheckQuestCondition();
    }

    public void SetQuestState(QuestButtonState _state)
    {
        switch (_state)
        {
            // ������
            case QuestButtonState.Proceed:
                {
                    isClearEnable = false;

                    // Ŭ���� ��ư ��Ȱ��ȭ
                    clearButton.enabled = false;
                    // ���� ���� Ȱ��ȭ
                    proceedBadge.SetActive(true);
                    clearBadge.SetActive(false);

                    myState = QuestButtonState.Proceed;
                    break;
                }
            // ���� ȹ�� ����
            case QuestButtonState.Enable:
                {
                    isClearEnable = true;

                    // Ŭ���� ��ưȰ��ȭ
                    clearButton.enabled = true;

                    // Ŭ���� ���� Ȱ��ȭ
                    proceedBadge.SetActive(false);
                    clearBadge.SetActive(true);

                    myState = QuestButtonState.Enable;
                    break;
                }

            // ��� ����Ʈ Ŭ����
            case QuestButtonState.Clear:
                {
                    isClearEnable = false;

                    // ����Ʈ Ŭ���� �� Ȱ��ȭ
                    questLock.SetActive(true);

                    // ���� ��� ��Ȱ��ȭ
                    proceedBadge.SetActive(false);
                    clearBadge.SetActive(false);
                    // ��ư ��Ȱ��ȭ
                    clearButton.enabled = false;
                    myState = QuestButtonState.Clear;
                    break;
                }
            default: break;
        }

    }

}
