using UnityEngine;

// �˾� ����
public enum ScreenType
{
    PlantLab=0,       // �Ĺ� ������ 
    WeaponLab,      // ���� ������ 
    MainHome,       // ���� ȭ��
    SelectStage,    // �������� ���� â
    Shop            // ����
}
[Tooltip("0:�ݱ�����(None) 1:���ּ� 2:�������� 3:�ɼ� 4:����Ʈ 5:���� 6:���� 7:�Ĺ� 8:����(��) 9:����(��) 10:é�� ����")]

public enum PopUpType
{
    Quit = 0,         // �˾� �ݱ� ���� (ButtonQuit) 
    SpaceShip,        // ���ּ�
    StageInfo,        // �������� â
    Option,           // �ɼ�
    Quest,            // ����Ʈ
    Encyclopedia,     // ����
    Achievements,     // ����
    PlantInfo,        // �Ĺ�
    LongWeaponInfo,   // ���Ÿ� ����
    ShortWeaponInfo,  // �ٰŸ� ����
    SelectChapter,    // é�� ���� �˾�
    License,          // ���̼��� �˾�
    LevelUp           // ������ �˾�
}

public class PopUpUIManager : MonoBehaviour
{
    // �˾� â ������
    // ���� �����ִ� �˾� UI�� ����
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

    [Header("�� ȭ�� �� UI �׷�")]
    [Space(10)]

    [Header("��, �ϴ� �г� �׷�")]
    [Space(10)]

    [SerializeField]
    public GameObject topPanelGroup;
    public GameObject bottomPanelGroup;
    [Space(10)]

    [Header("��ũ�� �׷�")]
    [Space(10)]

    [Tooltip("0:��ȭȹ�� 1:�Ĺ������� 2:����ȭ�� 3:���⿬���� 4:���� 5:������������")]
    public GameObject[] screenGroups;

    [Header("���� ���µ� ��ũ��")]
    [SerializeField]
    private GameObject currentScreen;
    public GameObject CurrentScreen { get { return currentScreen; } }

    private ScreenType currentScreenType;

    [Header("��ũ�� ���� ��ư")]
    [SerializeField]
    public ScreenButton[] screenButtonList;

    [Space(10)]

    [Header("�˾� �׷�")]
    [Space(10)]

    [Tooltip("0:�ݱ�����(None) 1:���ּ� 2:�������� 3:�ɼ� 4:����Ʈ 5:���� 6:���� 7:�Ĺ� 8:����(��) 9:����(��) 10:é�� ���� 11:���̼��� 12:������")]
    public GameObject[] popUpGroups;

    [Header("���� ���µ� �˾�")]
    [Space(10)]

    private GameObject currentPopUp;
    public GameObject CurrentPopUp { get { return currentPopUp; } }

    [Header("Layout_ReleaseAnimalGroup")]
    public GameObject layout_ReleaseAnimalGroup;

    [Header("��ũ�� ���� �̺�Ʈ ������Ʈ")]
    public GameObject screenChangeEvent;

    [Header("Ʃ�丮�� �Ŵ���")]
    public TutorialManager tutorialManager;

    [Header("�������� ���� ��ũ�� �̺�Ʈ ������Ʈ")]
    public GameObject stageScreenChangeEvent;

    [Header("��������-���� Ȩ ���� ��ũ�� �̺�Ʈ ������Ʈ")]
    public GameObject mainScreenChangeEvent;

    [Header("�ʱ���� ���� �׼�")]
    public GameObject firstStartGroup;

    private void Start()
    {
        // ó�� �����ϴ� �����ϰ�� �ƽ�, Ʃ�丮�� ����
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


        // �ʱ� ����
        currentScreen = screenGroups[(int)ScreenType.MainHome];
        currentScreenType = ScreenType.MainHome;
        
        screenButtonList[(int)ScreenType.MainHome].ClickedScreenButton(true);
    }

    // ��ũ�� ��ȯ ����
    public void ChangeScreenType(ScreenType type)
    {
        //// �������� ���� -> �������� ����
        //// (é�͸� ������ ���)
        //if(currentScreen == screenGroups[(int)type] && type == ScreenType.SelectStage)
        //{

        //}
        // ���� ��ũ���� Ŭ���� ��ũ���� �ٸ� ��� ����
        if (currentScreen != screenGroups[(int)type])
        {
            // �������� ���� �κ��� �ٸ� �ִϸ��̼����� ����� ����
            // �������� ���� ��ũ������ �����ϴ� ���� �ƴ� ��� 
            if (type != ScreenType.SelectStage && !StatManager.Instance.isBattleMode)
            {
                // ��ũ�� ��ȯ �̺�Ʈ 
                screenChangeEvent.SetActive(true);
                // ��ũ�� ��ȯ ���� ���
                SoundManager.Instance.PlayEffectSound(EffectSoundType.ScreenChangeSound);
            }

            // ���� ��ũ�� ��ư ��Ȱ��ȭ ���·� ����
            screenButtonList[(int)currentScreenType].ClickedScreenButton(false);
            currentScreen.SetActive(false);

            currentScreen = screenGroups[(int)type];
            currentScreenType = type;

            // ����� ��ũ�� ��ư Ȱ��ȭ ���·� ����
            currentScreen.SetActive(true);
            screenButtonList[(int)type].ClickedScreenButton(true);
        }
    }

    // �˾� ����
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

        // ����ȭ����� �˾��� ���
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
                // ����ȭ����� �˾� �� ���
                if (currentPopUp != popUpGroups[(int)PopUpType.PlantInfo]
                    && currentPopUp != popUpGroups[(int)PopUpType.ShortWeaponInfo]
                    && currentPopUp != popUpGroups[(int)PopUpType.LongWeaponInfo]
                    && currentPopUp != popUpGroups[(int)PopUpType.StageInfo])
                    layout_ReleaseAnimalGroup.SetActive(true);
            }
        }
    }

    // ��� �˾� �ݱ�
    public void CloseAllPopUp()
    {
        // UI �������� ������ �ʿ��� ��� ȣ���ϴ� �Լ�
        // SortSiblingUiGroup(false);
    }

    // UI ��� ���� ����
    // �˾� �׷� ���� ������Ʈ���� ������ ��ġ�� ��ü���� �˾� â ���� ��µǴ� ��찡 ����.
    // �˾� â�� �� �� �ش� ������Ʈ���� ���� ����ϰ�, �˾��� ���� �� �ε��� ������� ����ϱ� ����.
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
