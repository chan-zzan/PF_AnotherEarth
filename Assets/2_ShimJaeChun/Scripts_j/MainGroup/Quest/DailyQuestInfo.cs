using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class DailyQuestInfo : MonoBehaviour
{
    [Header("퀘스트 타입")]
    public DailyQuestType myType;

    [Header("퀘스트 보상 타입")]
    public QuestRewardType myRewardType;

    [Header("퀘스트 상태")]
    public QuestButtonState myState;

    [Header("퀘스트 보상량")]
    public int rewardAmount;

    [Header("퀘스트 성공 여부")]
    public bool isClear;

    [Header("퀘스트 진행중 여부")]
    public bool isProceed;

    [Header("퀘스트 진행중 배지")]
    public GameObject questProceedBadge;

    [Header("퀘스트 클리어 배지")]
    public GameObject questClearBadge;

    [Header("클리어 한 퀘스트 락 (퀘스트 클리어 시 활성화)")]
    public GameObject QuestClearLock;

    [Header("퀘스트 클리어 버튼")]
    public Button clearButton; 

    public void SetQuestState(QuestButtonState _state)
    {
        switch(_state)
        {
            // 진행중
            case QuestButtonState.Proceed:
                {
                    isClear = false;
                    isProceed = true;

                    // 퀘스트 클리어 락 비활성화
                    QuestClearLock.SetActive(false);

                    // 진행 배지 활성화
                    questProceedBadge.SetActive(true);
                    questClearBadge.SetActive(false);
                    // 버튼 비활성화
                    clearButton.enabled = false;

                    myState = QuestButtonState.Proceed;
                    break;
                }
            // 보상 획득 가능
            case QuestButtonState.Enable:
                {
                    isClear = false;
                    isProceed = false;

                    // 퀘스트 클리어 락 비활성화
                    QuestClearLock.SetActive(false);

                    // 클리어 배지 활성화
                    questClearBadge.SetActive(true);
                    questProceedBadge.SetActive(false);

                    // 버튼 활성화
                    clearButton.enabled = true;
                    myState = QuestButtonState.Enable;
                    break;
                }
            // 클리어
            case QuestButtonState.Clear:
                {
                    isClear = true;
                    isProceed = false;

                    // 퀘스트 클리어 락 활성화
                    QuestClearLock.SetActive(true);

                    // 배지 모두 비활성화
                    questProceedBadge.SetActive(false);
                    questClearBadge.SetActive(false);
                    // 버튼 비활성화
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
