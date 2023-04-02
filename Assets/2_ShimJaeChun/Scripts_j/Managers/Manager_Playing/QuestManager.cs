using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

// 전체 퀘스트 타입
public enum AllQuestType
{
    All_Attend = 0,
    All_DailyQuestCount,
    All_ClearEasyStage,
    All_ClearHardStage,
    All_UnLockPlant
}

// 일일 퀘스트 타입
public enum DailyQuestType
{
    AttendGame = 0,     // 출석
    HarvestPlant,       // 식물 생산 
    WeaponLevelUp,      // 무기 레벨업
    StageBattle,        // 전투
    WatchingAds,        // 광고 시청
}

// 퀘스트 상태 타입
public enum QuestButtonState
{
    Proceed = 0,
    Enable,
    Clear
}
// 퀘스트 보상 타입
public enum QuestRewardType
{
    Coin = 0,   // 코인
    Diamond     // 다이아
}

public class QuestManager : MonoBehaviour
{
    #region 싱글톤
    private static QuestManager instance;
    public static QuestManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<QuestManager>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<QuestManager>();
                    instance = newObj;
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        var objs = FindObjectsOfType<QuestManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        // 씬 변경시에도 파괴되지 않는다.
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    [Header("일일 퀘스트 리스트")]
    public DailyQuestInfo[] dailyQuestList;

    [Header("일일 퀘스트 클리어 상태 리스트")]
    public bool[] dailyQuestClearList;

    [Header("일일 퀘스트 진행 상태 리스트")]
    public bool[] dailyQuestProceedList;

    [Header("일일 퀘스트 총 클리어 횟수")]
    public int dailyQuestClearCount;

    [Header("일일 퀘스트 진행도 슬라이더")]
    public DailyQuestSlider dqSlider;

    [Header("전체 퀘스트 리스트")]
    public AllQuestInfo[] allQuestList;

    [Header("전체 퀘스트 보상량 리스트")]
    [Header("0:출석 1:일일미션 2:이지클리어 3:하드클리어 4:식물해금")]
    public int[] allQuestRewardList;

    [Header("전체 퀘스트 클리어 횟수 리스트")]
    [Header("0:출석 1:일일미션 2:이지클리어 3:하드클리어 4:식물해금")]
    public int[] allQuestClearCountList;

    [Header("일일 퀘스트 모두 완료 보상 수령 여부")]
    public bool isGetDailyAllReward;


    public void InitialSetting(bool isNextDay)
    {
        // 데이터 로드 
        QuestData_Json data = GameDataManager.Instance.LoadQuestData();

        // 로드 된 데이터가 없을 경우 초기 세팅
        if (data == null)
        {
            // 일일 퀘스트 클리어 상태 리스트 초기화
            dailyQuestClearList = Enumerable.Repeat(false, 5).ToArray<bool>();
            dailyQuestProceedList = Enumerable.Repeat(true, 5).ToArray<bool>();
            // 출석하기 퀘스트는 항상 완료 가능한 상태로 초기화
            dailyQuestProceedList[(int)DailyQuestType.AttendGame] = false;

            // 일일 퀘스트 클리어 횟수 초기화
            dailyQuestClearCount = 0;

            allQuestClearCountList = Enumerable.Repeat(0,5).ToArray<int>();
        }
        // 로드 된 데이터가 있을 경우 
        else
        {
            dailyQuestClearCount = data.iDailyQuestClearCount;
            allQuestClearCountList = data.iAllQuestClearCountList;
            // 하루가 지난 경우
            if (isNextDay)
            {
                isGetDailyAllReward = false;

                // 일일 퀘스트 클리어 상태 리스트 초기화
                dailyQuestClearList = Enumerable.Repeat(false, 5).ToArray<bool>();
                dailyQuestProceedList = Enumerable.Repeat(true, 5).ToArray<bool>();

                // 출석하기 퀘스트는 항상 완료 가능한 상태로 초기화
                dailyQuestProceedList[(int)DailyQuestType.AttendGame] = false;
            }
            // 같은 날짜에 재접속한 경우
            else
            {
                isGetDailyAllReward = data.bIsGetDailyAllReward;

                // 일일 퀘스트 클리어 상태 리스트 로드
                dailyQuestClearList = data.bDailyQuestClearList;
                // 일일 퀘스트 진행 상태 리스트 로드
                dailyQuestProceedList = data.bDailyQuestProceedList;

                // 출석하기 퀘스트는 항상 완료 가능한 상태로 초기화
                dailyQuestProceedList[(int)DailyQuestType.AttendGame] = false;
            }

            // 전체 퀘스트 로드 세팅
            AllQuestLoadSetting();
        }

        // 퀘스트 상태 업데이트 
        UpdateDailyQuestState();
    }

    // 전체 퀘스트 로드 세팅
    public void AllQuestLoadSetting()
    {
        for(int i=0; i<allQuestList.Length;i++)
        {
            allQuestList[i].LoadSetting(allQuestClearCountList[i]);
        }
    }

    // 일일 퀘스트 클리어상태 업데이트
    public void UpdateDailyQuestState()
    {
        for(int i=0; i< dailyQuestList.Length; i++)
        {
            // 클리어한 퀘스트
            if(dailyQuestClearList[i] == true)
            {
                // 클리어 상태
                dailyQuestList[i].SetQuestState(QuestButtonState.Clear);
                dqSlider.UpdateDailyQuestSlider();
            }
            // 클리어하지 못한 퀘스트
            else
            {
                // 퀘스트가 진행중인지? (보상수령이 가능한 상태인지)
                if (dailyQuestProceedList[i] == true)
                {
                    // 진행중 상태
                    dailyQuestList[i].SetQuestState(QuestButtonState.Proceed);
                }
                else
                {
                    // 보상 수령 가능 상태
                    dailyQuestList[i].SetQuestState(QuestButtonState.Enable);
                    Debug.Log(dailyQuestList[i] + " 는 보상수령이 가능한 상태입니다.");
                }
            }
        }
    }

    // 일일 퀘스트 갱신 (접속 날짜가 변경되면 호출)
    public void RenewDayilyQuest()
    {
        // 일일 퀘스트 클리어 상태 리스트 초기화
        dailyQuestClearList = Enumerable.Repeat(false, 5).ToArray<bool>();
    
        // 일일 퀘스트 진행 상태 리스트 초기화
        dailyQuestProceedList = Enumerable.Repeat(true, 5).ToArray<bool>();

        // 출석하기 퀘스트는 항상 완료 가능한 상태로 초기화
        dailyQuestProceedList[(int)DailyQuestType.AttendGame] = false;

        // 퀘스트 진행상태 업데이트
        UpdateDailyQuestState();

        // 보상 슬라이더 초기화
        dqSlider.ResetSlider();
    }

    // 일일 퀘스트 클리어 (퀘스트 버튼에서 호출)
    public void ClearDailyQuest(DailyQuestType _quest)
    {
        dailyQuestList[(int)_quest].SetQuestState(QuestButtonState.Clear);

        dailyQuestClearList[(int)_quest] = true;
        dailyQuestClearCount++;

        SoundManager.Instance.PlayEffectSound(EffectSoundType.QuestClearSound);

        dqSlider.UpdateDailyQuestSlider();
    }
    // 일일 퀘스트 상태 변경 : 진행중 -> 보상수령 가능 (퀘스트가 진행되는 부분에서 호출)
    public void UpdateDailyQuest(DailyQuestType _quest)
    {
        if (dailyQuestList[(int)_quest].myState == QuestButtonState.Proceed)
        {
            // 유저 리액션 활성화
            UserReactionManager.Instance.OnReactObject(ReactionType.Quest, true);

            dailyQuestList[(int)_quest].SetQuestState(QuestButtonState.Enable);
            // false = 진행완료
            dailyQuestProceedList[(int)_quest] = false;
        }
    }
}

public class QuestData_Json
{
    public bool[] bDailyQuestClearList;
    public bool[] bDailyQuestProceedList;
    public int iDailyQuestClearCount;
    public int[] iAllQuestClearCountList;
    public bool bIsGetDailyAllReward;

    public QuestData_Json()
    {
        bDailyQuestClearList = QuestManager.Instance.dailyQuestClearList;
        bDailyQuestProceedList = QuestManager.Instance.dailyQuestProceedList;
        iDailyQuestClearCount = QuestManager.Instance.dailyQuestClearCount;
        iAllQuestClearCountList = QuestManager.Instance.allQuestClearCountList;
        bIsGetDailyAllReward = QuestManager.Instance.isGetDailyAllReward;
    }
}