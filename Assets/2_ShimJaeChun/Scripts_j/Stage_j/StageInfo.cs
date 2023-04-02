using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Ŭ���� ���� (��ũ)
public enum StageClearRank
{
    RankC = 1,
    RankB,
    RankA
}

public class StageInfo : MonoBehaviour
{
    // �������� ������ ����ִ� Ŭ����
    // StageManager���� ����

    // �������� ����
    [Header("���������� ������ �� �̸�")]
    [SerializeField]
    private string stageSceneName;  // �������� �� �̸�
    public string StageSceneName { get { return stageSceneName; } }

    [SerializeField]
    private int stageNumber;        // �������� ��ȣ (�������� ������ ���� �� ����)
    public int StageNumber { get { return stageNumber; } }

    public BattleType myType;

    [SerializeField]
    private StageClearRank clearRankOfEasyMode;    // ���� ��ũ Ŭ���� ����
    public StageClearRank ClearRankOfEasyMode { get { return clearRankOfEasyMode; } }

    [SerializeField]
    private StageClearRank clearRankOfHardMode;    // ���� ��ũ Ŭ���� ����
    public StageClearRank ClearRankOfHardMode { get { return clearRankOfHardMode; } }

    [SerializeField]
    private TextMeshProUGUI mainText;

    [SerializeField]
    private Image mainIMG;

    private void Start()
    {
        stageSceneName = "InGame_E";
    }

    public void InitialSetting(int _stageNum, BattleType chapterType)
    {
        stageNumber = _stageNum;
        myType = chapterType;

        // index �� 0 ���� �����ϹǷ� �������� ��ȣ -1
        mainText.text = StageManager.Instance.stageMainTextList[stageNumber - 1];
        mainIMG.sprite = StageManager.Instance.stageMainImgList[stageNumber - 1];
    }
}
