using UnityEngine;
using UnityEngine.UI;

public class StageButton : MonoBehaviour
{
    [Header("�������� ��ȣ")]
    public int stageNum;

    [Header("�������� ���")]
    public BattleType myType;

    [Header("�༺ ����Ʈ")]
    public GameObject planetEffect;
    [Header("�༺ ��� �̹���")]
    public GameObject lockGroup;
    [Header("�༺ ��ư")]
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
        // �ش� �������� �ر� ������ �Ҵ�
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

    // ��ư Ŭ�� �̺�Ʈ
    public void OnClickStageButton()
    {
        // �˾� ����
        PopUpUIManager.Instance.OpenPopUp(PopUpType.StageInfo);

        // �˾� �������� ���� �ε�
        stageInfoGroup.GetComponent<StageInfo>().InitialSetting(stageNum, myType);
    }
}
