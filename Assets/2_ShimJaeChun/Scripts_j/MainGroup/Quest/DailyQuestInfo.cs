using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class DailyQuestInfo : MonoBehaviour
{
    [Header("����Ʈ Ÿ��")]
    public DailyQuestType myType;

    [Header("����Ʈ ���� Ÿ��")]
    public QuestRewardType myRewardType;

    [Header("����Ʈ ����")]
    public QuestButtonState myState;

    [Header("����Ʈ ����")]
    public int rewardAmount;

    [Header("����Ʈ ���� ����")]
    public bool isClear;

    [Header("����Ʈ ������ ����")]
    public bool isProceed;

    [Header("����Ʈ ������ ����")]
    public GameObject questProceedBadge;

    [Header("����Ʈ Ŭ���� ����")]
    public GameObject questClearBadge;

    [Header("Ŭ���� �� ����Ʈ �� (����Ʈ Ŭ���� �� Ȱ��ȭ)")]
    public GameObject QuestClearLock;

    [Header("����Ʈ Ŭ���� ��ư")]
    public Button clearButton; 

    public void SetQuestState(QuestButtonState _state)
    {
        switch(_state)
        {
            // ������
            case QuestButtonState.Proceed:
                {
                    isClear = false;
                    isProceed = true;

                    // ����Ʈ Ŭ���� �� ��Ȱ��ȭ
                    QuestClearLock.SetActive(false);

                    // ���� ���� Ȱ��ȭ
                    questProceedBadge.SetActive(true);
                    questClearBadge.SetActive(false);
                    // ��ư ��Ȱ��ȭ
                    clearButton.enabled = false;

                    myState = QuestButtonState.Proceed;
                    break;
                }
            // ���� ȹ�� ����
            case QuestButtonState.Enable:
                {
                    isClear = false;
                    isProceed = false;

                    // ����Ʈ Ŭ���� �� ��Ȱ��ȭ
                    QuestClearLock.SetActive(false);

                    // Ŭ���� ���� Ȱ��ȭ
                    questClearBadge.SetActive(true);
                    questProceedBadge.SetActive(false);

                    // ��ư Ȱ��ȭ
                    clearButton.enabled = true;
                    myState = QuestButtonState.Enable;
                    break;
                }
            // Ŭ����
            case QuestButtonState.Clear:
                {
                    isClear = true;
                    isProceed = false;

                    // ����Ʈ Ŭ���� �� Ȱ��ȭ
                    QuestClearLock.SetActive(true);

                    // ���� ��� ��Ȱ��ȭ
                    questProceedBadge.SetActive(false);
                    questClearBadge.SetActive(false);
                    // ��ư ��Ȱ��ȭ
                    clearButton.enabled = false;
                    myState = QuestButtonState.Clear;
                    break;
                }
            default: break;
        }

    }

    public void OnClickClearButton()
    {
        QuestManager.Instance.ClearDailyQuest(myType);

        switch(myRewardType)
        {
            case QuestRewardType.Coin:
                {
                    StatManager.Instance.AddMineral(rewardAmount);
                    break;
                }
            case QuestRewardType.Diamond:
                {
                    StatManager.Instance.AddDia(rewardAmount);
                    break;
                }
            default: break;
        }
    }
}
