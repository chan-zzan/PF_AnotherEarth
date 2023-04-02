using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 클리어 상태 (랭크)
public enum StageClearRank
{
    RankC = 1,
    RankB,
    RankA
}

public class StageInfo : MonoBehaviour
{
    // 스테이지 정보를 담고있는 클래스
    // StageManager에서 관리

    // 스테이지 정보
    [Header("스테이지에 연동된 씬 이름")]
    [SerializeField]
    private string stageSceneName;  // 스테이지 씬 이름
    public string StageSceneName { get { return stageSceneName; } }

    [SerializeField]
    private int stageNumber;        // 스테이지 번호 (스테이지 레벨로 사용될 수 있음)
    public int StageNumber { get { return stageNumber; } }

    public BattleType myType;

    [SerializeField]
    private StageClearRank clearRankOfEasyMode;    // 이지 랭크 클리어 상태
    public StageClearRank ClearRankOfEasyMode { get { return clearRankOfEasyMode; } }

    [SerializeField]
    private StageClearRank clearRankOfHardMode;    // 이지 랭크 클리어 상태
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

        // index 가 0 부터 시작하므로 스테이지 번호 -1
        mainText.text = StageManager.Instance.stageMainTextList[stageNumber - 1];
        mainIMG.sprite = StageManager.Instance.stageMainImgList[stageNumber - 1];
    }
}
