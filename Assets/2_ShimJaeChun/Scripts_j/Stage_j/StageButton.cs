using UnityEngine;
using UnityEngine.UI;

public class StageButton : MonoBehaviour
{
    [Header("스테이지 번호")]
    public int stageNum;

    [Header("스테이지 모드")]
    public BattleType myType;

    [Header("행성 이펙트")]
    public GameObject planetEffect;
    [Header("행성 잠금 이미지")]
    public GameObject lockGroup;
    [Header("행성 버튼")]
    public Button stageButton;


    [Header("Layout_StageInfo")]
    [SerializeField]
    private GameObject stageInfoGroup;

    public bool isUnLock;

    public void Awake()
    {
        InitialSetting();
    }

    private void Start()
    {
    }

    private void OnEnable()
    {
        // 해당 스테이지 해금 정보를 할당
        if (StageManager.Instance.GetIsUnLockStage(stageNum, myType) && !isUnLock)
        {
            UnLockStageButton();
        }
    }

    public void UnLockStageButton()
    {
        isUnLock = true;
        planetEffect.SetActive(true);
        lockGroup.SetActive(false);
        stageButton.enabled = true;
    }

    public void InitialSetting()
    {
        stageInfoGroup = PopUpUIManager.Instance.popUpGroups[(int)PopUpType.StageInfo];

        planetEffect.SetActive(false);
        lockGroup.SetActive(true);
        stageButton.enabled = false;
        isUnLock = false;
    }

    // 버튼 클릭 이벤트
    public void OnClickStageButton()
    {
        // 팝업 오픈
        PopUpUIManager.Instance.OpenPopUp(PopUpType.StageInfo);

        // 팝업 스테이지 정보 로드
        stageInfoGroup.GetComponent<StageInfo>().InitialSetting(stageNum, myType);
    }
}
