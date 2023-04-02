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
    #region 싱글톤
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
        // 씬 변경시에도 파괴되지 않는다.
        DontDestroyOnLoad(gameObject);
    }

    [Header("스테이지 데이터 - 스크립터블 오브젝트 (에디터 수정)")]
    public StageDB stageDB;

    [Header("스테이지 메인 텍스트 (에디터 수정)")]
    public string[] stageMainTextList;

    [Header("스테이지 메인 이미지 (에디터 수정)")]
    public Sprite[] stageMainImgList;

    [Header("스테이지 요약 설명 (에디터 수정)")]
    public string[] stageSummaryTextList;

    [Space(30)]

    [Header("스테이지 이지모드 해금 상태 (읽기 전용)")]
    [SerializeField]
    public bool[] EasyStageStateList;

    [Header("스테이지 하드모드 해금 상태 (읽기 전용)")]
    [SerializeField]
    public bool[] HardStageStateList;

    [Header("스테이지 이지모드 클리어 여부")]
    public bool[] EasyClearList;

    [Header("스테이지 하드모드 클리어 여부")]
    public bool[] HardClearList;

    [Header("현재 플레이중인 스테이지 (읽기 전용)")]
    [SerializeField]
    private List<StageDB_Entity> curStageData;
    public List<StageDB_Entity> CurStageData { get { return curStageData; } }

    [Header("현재 플레이중인 스테이지 넘버")]
    public int curStageNum;

    [Header("현재 플레이중인 스테이지 타입")]
    public BattleType curBattleType = BattleType.Easy;

    public List<int> easyClearCoinList;
    public List<int> hardClearCoinList;

    /// 이은찬 추가
    [Space(10)]

    [Header("볼륨 조절")] 
    public float bgmSoundVolume = 0.1f;
    public float effectSoundVolume = 0.1f;
    public Slider bgmSlider;
    public Slider effectSlider;


    [Header("잠금된 스테이지 경고")]
    public GameObject stageWarningText;

    [Header("도감 튜토리얼")]
    public GameObject encyTutorial;

    [Header("유저 설문")]
    public GameObject userSurvey;

    // Start is called before the first frame update
    void Start()
    {
        curStageNum = -1;

        // 해금되지 않은 상태로 스테이지 이지모드 리스트 초기화
        EasyStageStateList = Enumerable.Repeat(false, 15).ToArray<bool>();
        // 클리어하지 않은 상태로 이지모드 클리어 리스트 초기화
        EasyClearList = Enumerable.Repeat(false, 15).ToArray<bool>();

        // 해금되지 않은 상태로 스테이지 하드모드 리스트 초기화
        HardStageStateList = Enumerable.Repeat(false, 15).ToArray<bool>();
        // 클리어하지 않은 상태로 하드모드 클리어 리스트 초기화
        HardClearList = Enumerable.Repeat(false, 15).ToArray<bool>();

        Debug.Log("스테이지 매니저 start");
    }

    public void InitialSetting(GameData_Json data)  // 초기 스테이지 정보
    {
        // 로드한 데이터가 있을 경우
        if (data != null)
        {
            StageData_Json stageData = GameDataManager.Instance.LoadStageData();

            EasyStageStateList = stageData.bEasyStageStateList;
            HardStageStateList = stageData.bHardStageStateList;
            EasyClearList = stageData.bEasyClearList;
            HardClearList = stageData.bHardClearList;

            // 해금된 스테이지 개수만큼 카운팅
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

            // 카운팅한 개수만큼 스테이지 해금
            UnLockEasyStage(1, countEasyUnLock);
            UnLockHardStage(1, countHardUnlock);
        }
        // 없을 경우
        else
        {
            // 실제코드
            // 1스테이지 이지모드만 해금
             UnLockEasyStage(1, 1);


            //테스트용
            // 9스테이지까지 이지모드 해금
            //UnLockEasyStage(1, 9);
            //테스트용
            // 9스테이지까지 하드모드 해금
            //UnLockHardStage(1, 9);
        }

        Debug.Log("스테이지 매니저 InitialSetting");
    }

    // 스테이지 클리어 상태 리턴 (이지, 하드)
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

    // 현재 스테이지 클리어 보상 수치 리턴
    public int GetCurrentStageCoin()
    {
        if(curStageNum <0)      // 하드모드인 경우
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
        // 전투하기 퀘스트 적용
        if (QuestManager.Instance.dailyQuestList[(int)DailyQuestType.StageBattle].myState == QuestButtonState.Proceed)
        {
            QuestManager.Instance.UpdateDailyQuest(DailyQuestType.StageBattle);
        }

        switch (curBattleType)
        {
            case BattleType.Easy:
                {
                    /// 인덱스는 0부터 curStageNum-1이 진행중인 스테이지 실제 넘버 

                    // 최초 클리어 시 동작 
                    if (EasyClearList[curStageNum - 1] == false)
                    {
                        if(curStageNum == 1)
                        {
                            // 스테이지 1 최초 클리어 시 도감 튜토리얼 동작
                            encyTutorial.SetActive(true);
                        }

                        // 설문조사 팝업이 한번도 안뜬 유저의 경우
                        if(!StatManager.Instance.reviewCheck)
                        {
                            // 3, 6, 9 스테이지에서 팝업 표출
                            if(curStageNum == 3|| curStageNum == 6|| curStageNum == 9)
                            {
                                userSurvey.SetActive(true);
                                StatManager.Instance.reviewCheck = true;
                            }
                        }

                        UserReactionManager.Instance.OnReactObject(ReactionType.Ency, true);
                        UserReactionManager.Instance.OnReactObject(ReactionType.Battle, true);
                        UserReactionManager.Instance.OnReactObject(ReactionType.BattleHard, true);

                        // 클리어 상태 갱신
                        EasyClearList[curStageNum - 1] = true;

                        // 하드모드 해금
                        HardStageStateList[curStageNum - 1] = true;

                        // 다음 스테이지 해금
                        EasyStageStateList[curStageNum] = true;
                    }
                    break;
                }
            case BattleType.Hard:
                {
                    // 최초 클리어 시 동작
                    if (HardClearList[-curStageNum - 1] == false)
                    {
                        // 클리어 상태 갱신
                        HardClearList[-curStageNum - 1] = true;
                    }
                    // 하드모드 클리어 시 이벤트
                    break;
                }
        }
    }

    // 스테이지 해금 정보를 리턴 (해금 : true , 잠금 : false)
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

    // 스테이지 해금 함수 (StatManager의 AddSpaceShipLevel 함수에서 호출)
    // 매개변수 : start_stage 부터 count만큼 해금
    // ex) UnLockStageEasy(1,3) = 1, 2, 3 스테이지 이지모드 해금
    public void UnLockEasyStage(int startStage, int count)  // 이지모드 해금
    {
        for (int i = startStage - 1; i < count; i++)
        {
            EasyStageStateList[i] = true;
        }
    }
    public void UnLockHardStage(int startStage, int count)  // 하드모드 해금
    {
        for (int i = startStage - 1; i < count; i++)
        {
            HardStageStateList[i] = true;
        }
    }

    public void SetStageData(int stageDataNum)  // 스테이지 데이터 세팅 (스크립터블 오브젝트)
    {
        // 인덱스 0부터 시작 -> -1
        switch (stageDataNum)
        {
            case 1:
                {
                    curStageData = stageDB.Stage1;
                    curStageNum = 1;
                    curBattleType = BattleType.Easy;
                    break;
                }
            case -1:    // 하드
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
            case -2:    // 하드
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
            case -3:     // 하드
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
            case -5:   // 하드
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
            case -6:   // 하드
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
            case -7:   // 하드
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
            case -8:   // 하드
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
            case -9:   // 하드
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
            case -10:   // 하드
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

    public void LoadStage(string stageName) // 스테이지(씬) 로드
    {
        // 볼륨 설정
        bgmSoundVolume = SoundManager.Instance.bgmAudioSource.volume;
        effectSoundVolume= SoundManager.Instance.effectSoundSource.volume;

        // 로딩 화면 출력
        CanvasSingleton.Instance.RenderLoadingScreen(true);

        // 비동기 씬 로딩
        GameSceneManager.Instance.LoadGameScene(stageName);

    }

    public void VolumeSetting(int num)
    {
        // 볼륨설정
        if (num == 0)
        {
            // 배경음
            SoundManager.Instance.bgmAudioSource.volume = bgmSlider.value;
        }
        else
        {
            // 효과음
            SoundManager.Instance.effectSoundSource.volume = effectSlider.value;
        }
        
    }

    // 해금되지 않은 스테이지를 클릭했을 경우 
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