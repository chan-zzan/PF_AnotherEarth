using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleType
{
    Easy = 0,
    Hard
}

public class StageManager : MonoBehaviour
{
    #region �̱���
    private static StageManager instance;
    public static StageManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<StageManager>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<StageManager>();
                    instance = newObj;
                }
            }
            return instance;
        }
    }
    #endregion
    private void Awake()
    {
        var objs = FindObjectsOfType<StageManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        // �� ����ÿ��� �ı����� �ʴ´�.
        DontDestroyOnLoad(gameObject);
    }

    [Header("�������� ������ - ��ũ���ͺ� ������Ʈ (������ ����)")]
    public StageDB stageDB;

    [Header("�������� ���� �ؽ�Ʈ (������ ����)")]
    public string[] stageMainTextList;

    [Header("�������� ���� �̹��� (������ ����)")]
    public Sprite[] stageMainImgList;

    [Header("�������� ��� ���� (������ ����)")]
    public string[] stageSummaryTextList;

    [Space(30)]

    [Header("�������� ������� �ر� ���� (�б� ����)")]
    [SerializeField]
    public bool[] EasyStageStateList;

    [Header("�������� �ϵ��� �ر� ���� (�б� ����)")]
    [SerializeField]
    public bool[] HardStageStateList;

    [Header("�������� ������� Ŭ���� ����")]
    public bool[] EasyClearList;

    [Header("�������� �ϵ��� Ŭ���� ����")]
    public bool[] HardClearList;

    [Header("���� �÷������� �������� (�б� ����)")]
    [SerializeField]
    private List<StageDB_Entity> curStageData;
    public List<StageDB_Entity> CurStageData { get { return curStageData; } }

    [Header("���� �÷������� �������� �ѹ�")]
    public int curStageNum;

    [Header("���� �÷������� �������� Ÿ��")]
    public BattleType curBattleType = BattleType.Easy;

    public List<int> easyClearCoinList;
    public List<int> hardClearCoinList;

    /// ������ �߰�
    [Space(10)]

    [Header("���� ����")] 
    public float bgmSoundVolume = 0.1f;
    public float effectSoundVolume = 0.1f;
    public Slider bgmSlider;
    public Slider effectSlider;


    [Header("��ݵ� �������� ���")]
    public GameObject stageWarningText;

    [Header("���� Ʃ�丮��")]
    public GameObject encyTutorial;

    [Header("���� ����")]
    public GameObject userSurvey;

    // Start is called before the first frame update
    void Start()
    {
        curStageNum = -1;

        // �رݵ��� ���� ���·� �������� ������� ����Ʈ �ʱ�ȭ
        EasyStageStateList = Enumerable.Repeat(false, 15).ToArray<bool>();
        // Ŭ�������� ���� ���·� ������� Ŭ���� ����Ʈ �ʱ�ȭ
        EasyClearList = Enumerable.Repeat(false, 15).ToArray<bool>();

        // �رݵ��� ���� ���·� �������� �ϵ��� ����Ʈ �ʱ�ȭ
        HardStageStateList = Enumerable.Repeat(false, 15).ToArray<bool>();
        // Ŭ�������� ���� ���·� �ϵ��� Ŭ���� ����Ʈ �ʱ�ȭ
        HardClearList = Enumerable.Repeat(false, 15).ToArray<bool>();

        Debug.Log("�������� �Ŵ��� start");
    }

    public void InitialSetting(GameData_Json data)  // �ʱ� �������� ����
    {
        // �ε��� �����Ͱ� ���� ���
        if (data != null)
        {
            StageData_Json stageData = GameDataManager.Instance.LoadStageData();

            EasyStageStateList = stageData.bEasyStageStateList;
            HardStageStateList = stageData.bHardStageStateList;
            EasyClearList = stageData.bEasyClearList;
            HardClearList = stageData.bHardClearList;

            // �رݵ� �������� ������ŭ ī����
            int countEasyUnLock = 0;
            int countHardUnlock = 0;    

            for(int i=0; i<EasyStageStateList.Length; i++)
            {
                if(EasyStageStateList[i] == true)
                {
                    countEasyUnLock++;
                }
                if(HardStageStateList[i] == true)
                {
                    countHardUnlock++;
                }
            }

            // ī������ ������ŭ �������� �ر�
            UnLockEasyStage(1, countEasyUnLock);
            UnLockHardStage(1, countHardUnlock);
        }
        // ���� ���
        else
        {
            // �����ڵ�
            // 1�������� ������常 �ر�
             UnLockEasyStage(1, 1);


            //�׽�Ʈ��
            // 9������������ ������� �ر�
            //UnLockEasyStage(1, 9);
            //�׽�Ʈ��
            // 9������������ �ϵ��� �ر�
            //UnLockHardStage(1, 9);
        }

        Debug.Log("�������� �Ŵ��� InitialSetting");
    }

    // �������� Ŭ���� ���� ���� (����, �ϵ�)
    public bool GetIsClearStage(int StageNum, BattleType type)
    {
        switch(type)
        {
            case BattleType.Easy:
                {
                    return EasyClearList[StageNum-1];
                }
            case BattleType.Hard:
                {
                    return HardClearList[StageNum - 1];
                }
            default: return false;
        }
    }

    // ���� �������� Ŭ���� ���� ��ġ ����
    public int GetCurrentStageCoin()
    {
        if(curStageNum <0)      // �ϵ����� ���
        {
            return hardClearCoinList[(-1*curStageNum)-1];
        }
        else
        {
            return easyClearCoinList[curStageNum - 1];
        }
    }

    public void ClearBattleStage()
    {
        // �����ϱ� ����Ʈ ����
        if (QuestManager.Instance.dailyQuestList[(int)DailyQuestType.StageBattle].myState == QuestButtonState.Proceed)
        {
            QuestManager.Instance.UpdateDailyQuest(DailyQuestType.StageBattle);
        }

        switch (curBattleType)
        {
            case BattleType.Easy:
                {
                    /// �ε����� 0���� curStageNum-1�� �������� �������� ���� �ѹ� 

                    // ���� Ŭ���� �� ���� 
                    if (EasyClearList[curStageNum - 1] == false)
                    {
                        if(curStageNum == 1)
                        {
                            // �������� 1 ���� Ŭ���� �� ���� Ʃ�丮�� ����
                            encyTutorial.SetActive(true);
                        }

                        // �������� �˾��� �ѹ��� �ȶ� ������ ���
                        if(!StatManager.Instance.reviewCheck)
                        {
                            // 3, 6, 9 ������������ �˾� ǥ��
                            if(curStageNum == 3|| curStageNum == 6|| curStageNum == 9)
                            {
                                userSurvey.SetActive(true);
                                StatManager.Instance.reviewCheck = true;
                            }
                        }

                        UserReactionManager.Instance.OnReactObject(ReactionType.Ency, true);
                        UserReactionManager.Instance.OnReactObject(ReactionType.Battle, true);
                        UserReactionManager.Instance.OnReactObject(ReactionType.BattleHard, true);

                        // Ŭ���� ���� ����
                        EasyClearList[curStageNum - 1] = true;

                        // �ϵ��� �ر�
                        HardStageStateList[curStageNum - 1] = true;

                        // ���� �������� �ر�
                        EasyStageStateList[curStageNum] = true;
                    }
                    break;
                }
            case BattleType.Hard:
                {
                    // ���� Ŭ���� �� ����
                    if (HardClearList[-curStageNum - 1] == false)
                    {
                        // Ŭ���� ���� ����
                        HardClearList[-curStageNum - 1] = true;
                    }
                    // �ϵ��� Ŭ���� �� �̺�Ʈ
                    break;
                }
        }
    }

    // �������� �ر� ������ ���� (�ر� : true , ��� : false)
    public bool GetIsUnLockStage(int StageNum, BattleType type)
    {
        switch (type)
        {
            case BattleType.Easy:
                {
                    return EasyStageStateList[StageNum - 1];
                }
            case BattleType.Hard:
                {
                    return HardStageStateList[StageNum - 1];
                }
            default: return false;
        }
    }

    // �������� �ر� �Լ� (StatManager�� AddSpaceShipLevel �Լ����� ȣ��)
    // �Ű����� : start_stage ���� count��ŭ �ر�
    // ex) UnLockStageEasy(1,3) = 1, 2, 3 �������� ������� �ر�
    public void UnLockEasyStage(int startStage, int count)  // ������� �ر�
    {
        for (int i = startStage - 1; i < count; i++)
        {
            EasyStageStateList[i] = true;
        }
    }
    public void UnLockHardStage(int startStage, int count)  // �ϵ��� �ر�
    {
        for (int i = startStage - 1; i < count; i++)
        {
            HardStageStateList[i] = true;
        }
    }

    public void SetStageData(int stageDataNum)  // �������� ������ ���� (��ũ���ͺ� ������Ʈ)
    {
        // �ε��� 0���� ���� -> -1
        switch (stageDataNum)
        {
            case 1:
                {
                    curStageData = stageDB.Stage1;
                    curStageNum = 1;
                    curBattleType = BattleType.Easy;
                    break;
                }
            case -1:    // �ϵ�
                {
                    curStageData = stageDB.Stage1_H;
                    curStageNum = -1;
                    curBattleType = BattleType.Hard;
                    break;
                }
            case 2:
                {
                    curStageData = stageDB.Stage2;
                    curStageNum = 2;
                    curBattleType = BattleType.Easy;
                    break;
                }
            case -2:    // �ϵ�
                {
                    curStageData = stageDB.Stage2_H;
                    curStageNum = -2;
                    curBattleType = BattleType.Hard;
                    break;
                }
            case 3:
                {
                    curStageData = stageDB.Stage3;
                    curStageNum = 3;
                    curBattleType = BattleType.Easy;
                    break;
                }
            case -3:     // �ϵ�
                {
                    curStageData = stageDB.Stage3_H;
                    curStageNum = -3;
                    curBattleType = BattleType.Hard;
                    break;
                }
            case 4:
                {
                    curStageData = stageDB.Stage4;
                    curStageNum = 4;
                    curBattleType = BattleType.Easy;
                    break;
                }
            case -4:
                {
                    curStageData = stageDB.Stage4_H;
                    curStageNum = -4;
                    curBattleType = BattleType.Hard;
                    break;
                }
            case 5:
                {
                    curStageData = stageDB.Stage5;
                    curStageNum = 5;
                    curBattleType = BattleType.Easy;
                    break;
                }
            case -5:   // �ϵ�
                {
                    curStageData = stageDB.Stage5_H;
                    curStageNum = -5;
                    curBattleType = BattleType.Hard;
                    break;
                }
            case 6:
                {
                    curStageData = stageDB.Stage6;
                    curStageNum = 6;
                    curBattleType = BattleType.Easy;
                    break;
                }
            case -6:   // �ϵ�
                {
                    curStageData = stageDB.Stage6_H;
                    curStageNum = -6;
                    curBattleType = BattleType.Hard;
                    break;
                }
            case 7:
                {
                    curStageData = stageDB.Stage7;
                    curStageNum = 7;
                    curBattleType = BattleType.Easy;
                    break;
                }
            case -7:   // �ϵ�
                {
                    curStageData = stageDB.Stage7_H;
                    curStageNum = -7;
                    curBattleType = BattleType.Hard;
                    break;
                }
            case 8:
                {
                    curStageData = stageDB.Stage8;
                    curStageNum = 8;
                    curBattleType = BattleType.Easy;
                    break;
                }
            case -8:   // �ϵ�
                {
                    curStageData = stageDB.Stage8_H;
                    curStageNum = -8;
                    curBattleType = BattleType.Hard;
                    break;
                }
            case 9:
                {
                    curStageData = stageDB.Stage9;
                    curStageNum = 9;
                    curBattleType = BattleType.Easy;
                    break;
                }
            case -9:   // �ϵ�
                {
                    curStageData = stageDB.Stage9_H;
                    curStageNum = -9;
                    curBattleType = BattleType.Hard;
                    break;
                }
            case 10:
                {
                    curStageData = stageDB.Stage10;
                    curStageNum = 10;
                    curBattleType = BattleType.Easy;
                    break;
                }
            case -10:   // �ϵ�
                {
                    curStageData = stageDB.Stage10_H;
                    curStageNum = -10;
                    curBattleType = BattleType.Hard;
                    break;
                }
            default:
                break;
        }
    }

    public void LoadStage(string stageName) // ��������(��) �ε�
    {
        // ���� ����
        bgmSoundVolume = SoundManager.Instance.bgmAudioSource.volume;
        effectSoundVolume= SoundManager.Instance.effectSoundSource.volume;

        // �ε� ȭ�� ���
        CanvasSingleton.Instance.RenderLoadingScreen(true);

        // �񵿱� �� �ε�
        GameSceneManager.Instance.LoadGameScene(stageName);

    }

    public void VolumeSetting(int num)
    {
        // ��������
        if (num == 0)
        {
            // �����
            SoundManager.Instance.bgmAudioSource.volume = bgmSlider.value;
        }
        else
        {
            // ȿ����
            SoundManager.Instance.effectSoundSource.volume = effectSlider.value;
        }
        
    }

    // �رݵ��� ���� ���������� Ŭ������ ��� 
    public void OnClickLockStage()
    {
        stageWarningText.SetActive(true);
        StartCoroutine(WarningTimer());
    }

    IEnumerator WarningTimer()
    {
        yield return new WaitForSeconds(2.0f);

        stageWarningText.SetActive(false);
    }
}

public class StageData_Json
{
    public bool[] bEasyStageStateList;
    public bool[] bHardStageStateList;
    public bool[] bEasyClearList;
    public bool[] bHardClearList;
    
    public StageData_Json()
    {
        bEasyStageStateList = StageManager.Instance.EasyStageStateList;
        bHardStageStateList = StageManager.Instance.HardStageStateList;
        bEasyClearList = StageManager.Instance.EasyClearList;
        bHardClearList = StageManager.Instance.HardClearList;
    }
}