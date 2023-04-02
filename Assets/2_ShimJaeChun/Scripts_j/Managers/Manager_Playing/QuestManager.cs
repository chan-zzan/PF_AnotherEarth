using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

// ��ü ����Ʈ Ÿ��
public enum AllQuestType
{
    All_Attend = 0,
    All_DailyQuestCount,
    All_ClearEasyStage,
    All_ClearHardStage,
    All_UnLockPlant
}

// ���� ����Ʈ Ÿ��
public enum DailyQuestType
{
    AttendGame = 0,     // �⼮
    HarvestPlant,       // �Ĺ� ���� 
    WeaponLevelUp,      // ���� ������
    StageBattle,        // ����
    WatchingAds,        // ���� ��û
}

// ����Ʈ ���� Ÿ��
public enum QuestButtonState
{
    Proceed = 0,
    Enable,
    Clear
}
// ����Ʈ ���� Ÿ��
public enum QuestRewardType
{
    Coin = 0,   // ����
    Diamond     // ���̾�
}

public class QuestManager : MonoBehaviour
{
    #region �̱���
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
        // �� ����ÿ��� �ı����� �ʴ´�.
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    [Header("���� ����Ʈ ����Ʈ")]
    public DailyQuestInfo[] dailyQuestList;

    [Header("���� ����Ʈ Ŭ���� ���� ����Ʈ")]
    public bool[] dailyQuestClearList;

    [Header("���� ����Ʈ ���� ���� ����Ʈ")]
    public bool[] dailyQuestProceedList;

    [Header("���� ����Ʈ �� Ŭ���� Ƚ��")]
    public int dailyQuestClearCount;

    [Header("���� ����Ʈ ���൵ �����̴�")]
    public DailyQuestSlider dqSlider;

    [Header("��ü ����Ʈ ����Ʈ")]
    public AllQuestInfo[] allQuestList;

    [Header("��ü ����Ʈ ���� ����Ʈ")]
    [Header("0:�⼮ 1:���Ϲ̼� 2:����Ŭ���� 3:�ϵ�Ŭ���� 4:�Ĺ��ر�")]
    public int[] allQuestRewardList;

    [Header("��ü ����Ʈ Ŭ���� Ƚ�� ����Ʈ")]
    [Header("0:�⼮ 1:���Ϲ̼� 2:����Ŭ���� 3:�ϵ�Ŭ���� 4:�Ĺ��ر�")]
    public int[] allQuestClearCountList;

    [Header("���� ����Ʈ ��� �Ϸ� ���� ���� ����")]
    public bool isGetDailyAllReward;


    public void InitialSetting(bool isNextDay)
    {
        // ������ �ε� 
        QuestData_Json data = GameDataManager.Instance.LoadQuestData();

        // �ε� �� �����Ͱ� ���� ��� �ʱ� ����
        if (data == null)
        {
            // ���� ����Ʈ Ŭ���� ���� ����Ʈ �ʱ�ȭ
            dailyQuestClearList = Enumerable.Repeat(false, 5).ToArray<bool>();
            dailyQuestProceedList = Enumerable.Repeat(true, 5).ToArray<bool>();
            // �⼮�ϱ� ����Ʈ�� �׻� �Ϸ� ������ ���·� �ʱ�ȭ
            dailyQuestProceedList[(int)DailyQuestType.AttendGame] = false;

            // ���� ����Ʈ Ŭ���� Ƚ�� �ʱ�ȭ
            dailyQuestClearCount = 0;

            allQuestClearCountList = Enumerable.Repeat(0,5).ToArray<int>();
        }
        // �ε� �� �����Ͱ� ���� ��� 
        else
        {
            dailyQuestClearCount = data.iDailyQuestClearCount;
            allQuestClearCountList = data.iAllQuestClearCountList;
            // �Ϸ簡 ���� ���
            if (isNextDay)
            {
                isGetDailyAllReward = false;

                // ���� ����Ʈ Ŭ���� ���� ����Ʈ �ʱ�ȭ
                dailyQuestClearList = Enumerable.Repeat(false, 5).ToArray<bool>();
                dailyQuestProceedList = Enumerable.Repeat(true, 5).ToArray<bool>();

                // �⼮�ϱ� ����Ʈ�� �׻� �Ϸ� ������ ���·� �ʱ�ȭ
                dailyQuestProceedList[(int)DailyQuestType.AttendGame] = false;
            }
            // ���� ��¥�� �������� ���
            else
            {
                isGetDailyAllReward = data.bIsGetDailyAllReward;

                // ���� ����Ʈ Ŭ���� ���� ����Ʈ �ε�
                dailyQuestClearList = data.bDailyQuestClearList;
                // ���� ����Ʈ ���� ���� ����Ʈ �ε�
                dailyQuestProceedList = data.bDailyQuestProceedList;

                // �⼮�ϱ� ����Ʈ�� �׻� �Ϸ� ������ ���·� �ʱ�ȭ
                dailyQuestProceedList[(int)DailyQuestType.AttendGame] = false;
            }

            // ��ü ����Ʈ �ε� ����
            AllQuestLoadSetting();
        }

        // ����Ʈ ���� ������Ʈ 
        UpdateDailyQuestState();
    }

    // ��ü ����Ʈ �ε� ����
    public void AllQuestLoadSetting()
    {
        for(int i=0; i<allQuestList.Length;i++)
        {
            allQuestList[i].LoadSetting(allQuestClearCountList[i]);
        }
    }

    // ���� ����Ʈ Ŭ������� ������Ʈ
    public void UpdateDailyQuestState()
    {
        for(int i=0; i< dailyQuestList.Length; i++)
        {
            // Ŭ������ ����Ʈ
            if(dailyQuestClearList[i] == true)
            {
                // Ŭ���� ����
                dailyQuestList[i].SetQuestState(QuestButtonState.Clear);
                dqSlider.UpdateDailyQuestSlider();
            }
            // Ŭ�������� ���� ����Ʈ
            else
            {
                // ����Ʈ�� ����������? (��������� ������ ��������)
                if (dailyQuestProceedList[i] == true)
                {
                    // ������ ����
                    dailyQuestList[i].SetQuestState(QuestButtonState.Proceed);
                }
                else
                {
                    // ���� ���� ���� ����
                    dailyQuestList[i].SetQuestState(QuestButtonState.Enable);
                    Debug.Log(dailyQuestList[i] + " �� ��������� ������ �����Դϴ�.");
                }
            }
        }
    }

    // ���� ����Ʈ ���� (���� ��¥�� ����Ǹ� ȣ��)
    public void RenewDayilyQuest()
    {
        // ���� ����Ʈ Ŭ���� ���� ����Ʈ �ʱ�ȭ
        dailyQuestClearList = Enumerable.Repeat(false, 5).ToArray<bool>();
    
        // ���� ����Ʈ ���� ���� ����Ʈ �ʱ�ȭ
        dailyQuestProceedList = Enumerable.Repeat(true, 5).ToArray<bool>();

        // �⼮�ϱ� ����Ʈ�� �׻� �Ϸ� ������ ���·� �ʱ�ȭ
        dailyQuestProceedList[(int)DailyQuestType.AttendGame] = false;

        // ����Ʈ ������� ������Ʈ
        UpdateDailyQuestState();

        // ���� �����̴� �ʱ�ȭ
        dqSlider.ResetSlider();
    }

    // ���� ����Ʈ Ŭ���� (����Ʈ ��ư���� ȣ��)
    public void ClearDailyQuest(DailyQuestType _quest)
    {
        dailyQuestList[(int)_quest].SetQuestState(QuestButtonState.Clear);

        dailyQuestClearList[(int)_quest] = true;
        dailyQuestClearCount++;

        SoundManager.Instance.PlayEffectSound(EffectSoundType.QuestClearSound);

        dqSlider.UpdateDailyQuestSlider();
    }
    // ���� ����Ʈ ���� ���� : ������ -> ������� ���� (����Ʈ�� ����Ǵ� �κп��� ȣ��)
    public void UpdateDailyQuest(DailyQuestType _quest)
    {
        if (dailyQuestList[(int)_quest].myState == QuestButtonState.Proceed)
        {
            // ���� ���׼� Ȱ��ȭ
            UserReactionManager.Instance.OnReactObject(ReactionType.Quest, true);

            dailyQuestList[(int)_quest].SetQuestState(QuestButtonState.Enable);
            // false = ����Ϸ�
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