using UnityEngine;

// 팝업 종류
public enum ScreenType
{
    PlantLab=0,       // 식물 연구소 
    WeaponLab,      // 무기 연구소 
    MainHome,       // 메인 화면
    SelectStage,    // 스테이지 선택 창
    Shop            // 상점
}
[Tooltip("0:닫기전용(None) 1:우주선 2:스테이지 3:옵션 4:퀘스트 5:도감 6:업적 7:식물 8:무기(원) 9:무기(근) 10:챕터 선택")]

public enum PopUpType
{
    Quit = 0,         // 팝업 닫기 전용 (ButtonQuit) 
    SpaceShip,        // 우주선
    StageInfo,        // 스테이지 창
    Option,           // 옵션
    Quest,            // 퀘스트
    Encyclopedia,     // 도감
    Achievements,     // 업적
    PlantInfo,        // 식물
    LongWeaponInfo,   // 원거리 무기
    ShortWeaponInfo,  // 근거리 무기
    SelectChapter,    // 챕터 선택 팝업
    License,          // 라이센스 팝업
    LevelUp           // 레벨업 팝업
}

public class PopUpUIManager : MonoBehaviour
{
    // 팝업 창 관리자
    // 현재 열려있는 팝업 UI를 관리
    #region SigleTon
    private static PopUpUIManager instance;
    public static PopUpUIManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<PopUpUIManager>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<PopUpUIManager>();
                    instance = newObj;
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        var objs = FindObjectsOfType<PopUpUIManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    [Space(10)]

    [Header("각 화면 별 UI 그룹")]
    [Space(10)]

    [Header("상, 하단 패널 그룹")]
    [Space(10)]

    [SerializeField]
    public GameObject topPanelGroup;
    public GameObject bottomPanelGroup;
    [Space(10)]

    [Header("스크린 그룹")]
    [Space(10)]

    [Tooltip("0:재화획득 1:식물연구소 2:메인화면 3:무기연구소 4:상점 5:스테이지선택")]
    public GameObject[] screenGroups;

    [Header("현재 오픈된 스크린")]
    [SerializeField]
    private GameObject currentScreen;
    public GameObject CurrentScreen { get { return currentScreen; } }

    private ScreenType currentScreenType;

    [Header("스크린 연동 버튼")]
    [SerializeField]
    public ScreenButton[] screenButtonList;

    [Space(10)]

    [Header("팝업 그룹")]
    [Space(10)]

    [Tooltip("0:닫기전용(None) 1:우주선 2:스테이지 3:옵션 4:퀘스트 5:도감 6:업적 7:식물 8:무기(원) 9:무기(근) 10:챕터 선택 11:라이센스 12:레벨업")]
    public GameObject[] popUpGroups;

    [Header("현재 오픈된 팝업")]
    [Space(10)]

    private GameObject currentPopUp;
    public GameObject CurrentPopUp { get { return currentPopUp; } }

    [Header("Layout_ReleaseAnimalGroup")]
    public GameObject layout_ReleaseAnimalGroup;

    [Header("스크린 변경 이벤트 오브젝트")]
    public GameObject screenChangeEvent;

    [Header("튜토리얼 매니저")]
    public TutorialManager tutorialManager;

    [Header("스테이지 선택 스크린 이벤트 오브젝트")]
    public GameObject stageScreenChangeEvent;

    [Header("스테이지-메인 홈 선택 스크린 이벤트 오브젝트")]
    public GameObject mainScreenChangeEvent;

    [Header("초기시작 유저 액션")]
    public GameObject firstStartGroup;

    private void Start()
    {
        // 처음 시작하는 유저일경우 컷신, 튜토리얼 진행
        if(!GameSceneManager.Instance.isLoadGame && !firstStartGroup.activeSelf)
        {
            firstStartGroup.SetActive(true);
            SoundManager.Instance.PlayBackGroundSound(MainSoundType.CutSceneSound);
        }
        else
        {
            if(firstStartGroup.activeSelf)
            {
                firstStartGroup.SetActive(false);
            }
            SoundManager.Instance.PlayBackGroundSound(MainSoundType.MainStageSound);
        }


        // 초기 세팅
        currentScreen = screenGroups[(int)ScreenType.MainHome];
        currentScreenType = ScreenType.MainHome;
        
        screenButtonList[(int)ScreenType.MainHome].ClickedScreenButton(true);
    }

    // 스크린 전환 관련
    public void ChangeScreenType(ScreenType type)
    {
        //// 스테이지 선택 -> 스테이지 선택
        //// (챕터만 변경한 경우)
        //if(currentScreen == screenGroups[(int)type] && type == ScreenType.SelectStage)
        //{

        //}
        // 현재 스크린과 클릭한 스크린이 다른 경우 실행
        if (currentScreen != screenGroups[(int)type])
        {
            // 스테이지 선택 부분은 다른 애니메이션으로 변경될 예정
            // 스테이지 선택 스크린으로 변경하는 것이 아닐 경우 
            if (type != ScreenType.SelectStage && !StatManager.Instance.isBattleMode)
            {
                // 스크린 전환 이벤트 
                screenChangeEvent.SetActive(true);
                // 스크린 전환 사운드 출력
                SoundManager.Instance.PlayEffectSound(EffectSoundType.ScreenChangeSound);
            }

            // 기존 스크린 버튼 비활성화 상태로 변경
            screenButtonList[(int)currentScreenType].ClickedScreenButton(false);
            currentScreen.SetActive(false);

            currentScreen = screenGroups[(int)type];
            currentScreenType = type;

            // 변경된 스크린 버튼 활성화 상태로 변경
            currentScreen.SetActive(true);
            screenButtonList[(int)type].ClickedScreenButton(true);
        }
    }

    // 팝업 관련
    public void OpenPopUp(PopUpType type)
    {

        if (type != PopUpType.LevelUp)
        {
            SoundManager.Instance.PlayEffectSound(EffectSoundType.PopUpButtonSound);
        }
        else
        {
            SoundManager.Instance.PlayEffectSound(EffectSoundType.PlayerLevelUpSound);
        }

        if (!currentPopUp)
        {
            currentPopUp = popUpGroups[(int)type];
            currentPopUp.SetActive(true);
        }
        else
        {
            currentPopUp.SetActive(false);
            currentPopUp = popUpGroups[(int)type];
            currentPopUp.SetActive(true);
        }

        // 메인화면관련 팝업일 경우
        if (type != PopUpType.PlantInfo
        && type != PopUpType.ShortWeaponInfo
        && type != PopUpType.LongWeaponInfo
        && type != PopUpType.StageInfo)
        {
            layout_ReleaseAnimalGroup.SetActive(false);
        }
    }
    public void ClosePopUp(bool isScreenChange)
    {
        if (currentPopUp)
        {
            if (currentPopUp.activeSelf)
            {
                if (!isScreenChange)
                {
                    SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponUnEquipSound);
                }

                currentPopUp.SetActive(false);
                // 메인화면관련 팝업 일 경우
                if (currentPopUp != popUpGroups[(int)PopUpType.PlantInfo]
                    && currentPopUp != popUpGroups[(int)PopUpType.ShortWeaponInfo]
                    && currentPopUp != popUpGroups[(int)PopUpType.LongWeaponInfo]
                    && currentPopUp != popUpGroups[(int)PopUpType.StageInfo])
                    layout_ReleaseAnimalGroup.SetActive(true);
            }
        }
    }

    // 모든 팝업 닫기
    public void CloseAllPopUp()
    {
        // UI 계층구조 소팅이 필요할 경우 호출하는 함수
        // SortSiblingUiGroup(false);
    }

    // UI 출력 순서 소팅
    // 팝업 그룹 게임 오브젝트보다 하위에 위치한 객체들은 팝업 창 위에 출력되는 경우가 있음.
    // 팝업 창을 열 때 해당 오브젝트들을 먼저 출력하고, 팝업을 닫을 때 인덱스 순서대로 출력하기 위함.
    private void SortSiblingUiGroup(bool isPopUpOpen)
    {
        if (isPopUpOpen)
        {
            topPanelGroup.GetComponent<RectTransform>().SetAsFirstSibling();
            bottomPanelGroup.GetComponent<RectTransform>().SetAsFirstSibling();
        }
        else
        {
            topPanelGroup.GetComponent<RectTransform>().SetAsLastSibling();
            bottomPanelGroup.GetComponent<RectTransform>().SetAsLastSibling();
        }
    }

}
