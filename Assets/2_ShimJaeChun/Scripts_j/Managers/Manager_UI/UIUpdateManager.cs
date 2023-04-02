using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUpdateManager : MonoBehaviour
{
    // 플레이 중 변경되는 UI 업데이트

    #region 싱글톤
    private static UIUpdateManager instance;
    public static UIUpdateManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<UIUpdateManager>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<UIUpdateManager>();
                    instance = newObj;
                }
            }
            return instance;
        }
    }
    #endregion
    private void Awake()
    {
        var objs = FindObjectsOfType<UIUpdateManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        // 테스트 시 빼놓기
        // 씬 변경시에도 파괴되지 않는다.
        DontDestroyOnLoad(gameObject);
    }

    #region 플레이어 데이터

    // 플레이어 이름, 레벨, 경험치
    [Header("플레이어 데이터")]
    [Space(10)]

    [SerializeField]
    private TextMeshProUGUI userNameText;        // 유저 이름 텍스트
    [SerializeField]
    private TextMeshProUGUI userLevelText;       // 유저 레벨 텍스트
    [SerializeField]
    private TextMeshProUGUI userExpPercentText;  // 유저 경험치 퍼센트 텍스트
    [SerializeField]
    private Slider userExpSlider;                // 유저 경험치 바 

    #endregion

    [Space(10)]

    #region 재화, 점수 관련 데이터

    // 미네랄, 다이아, 에너지, 환경점수
    [Header("재화, 점수 관련 데이터")]
    [Space(10)]

    [SerializeField]
    private TextMeshProUGUI mineralText;    // 미네랄 텍스트
    [SerializeField]
    private TextMeshProUGUI diaText;        // 다이아 텍스트
    [SerializeField]
    private TextMeshProUGUI energyText;     // 에너지 텍스트
    [SerializeField]
    private TextMeshProUGUI eVNText;        // 환경점수 텍스트

    #endregion

    // 데이터 관련 초기 세팅
    // StatManager에서 정보 불러와 로드
    public void InitialSetting()
    {

    }

    /// <CallbyStatUp>
    /// 다음의 함수들은 StatManager에서 각 스텟이 올라갈 경우 호출됨.
    /// </CallbyStatUp>

    public void UpdateAll()
    {
        UpdatePlayerData();
        UpdateMineral();
        UpdateDia();
        UpdateEnergy();
        UpdateUserName();
    }

    public void UpdateUserName()
    {
        userNameText.text = "헁"+StatManager.Instance.UserName;
    }

    public void UpdatePlayerData()  // 플레이어 데이터 관련 UI업데이트
    {
            userLevelText.text = "Lv." + StatManager.Instance.Level_Player.ToString();

            // 경험치 바
            float sliderValue = (float)(StatManager.Instance.Exp_Player / StatManager.Instance.RequiredExp);
            if (sliderValue >= 0.1f)    // 1 퍼센트 이상일 경우 동작
            {
                userExpSlider.value = sliderValue;
                // 경험치 퍼센트 텍스트
                int expPercent = (int)(userExpSlider.value * 100);
                userExpPercentText.text = expPercent.ToString() + "%";
            }
            else
            {
            userExpSlider.value = 0.1f;
            userExpPercentText.text = "1%";
            }

    }
    public void UpdateMineral()     // 미네랄 관련 UI 업데이트
    {
        mineralText.text = ScoreManager.Instance.ScoreToString(StatManager.Instance.Own_Mineral);
    }
    public void UpdateDia()         // 다이아 관련 UI 업데이트
    {
        diaText.text = ScoreManager.Instance.ScoreToString(StatManager.Instance.Own_Dia);
    }
    public void UpdateEnergy()      // 에너지 관련 UI 업데이트
    {
        energyText.text = StatManager.Instance.Own_Energy.ToString() + "/" + StatManager.Instance.Max_Energy.ToString();
    }
    
}
