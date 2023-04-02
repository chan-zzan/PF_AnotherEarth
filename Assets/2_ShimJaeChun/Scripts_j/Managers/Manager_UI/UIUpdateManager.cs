using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUpdateManager : MonoBehaviour
{
    // �÷��� �� ����Ǵ� UI ������Ʈ

    #region �̱���
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
        // �׽�Ʈ �� ������
        // �� ����ÿ��� �ı����� �ʴ´�.
        DontDestroyOnLoad(gameObject);
    }

    #region �÷��̾� ������

    // �÷��̾� �̸�, ����, ����ġ
    [Header("�÷��̾� ������")]
    [Space(10)]

    [SerializeField]
    private TextMeshProUGUI userNameText;        // ���� �̸� �ؽ�Ʈ
    [SerializeField]
    private TextMeshProUGUI userLevelText;       // ���� ���� �ؽ�Ʈ
    [SerializeField]
    private TextMeshProUGUI userExpPercentText;  // ���� ����ġ �ۼ�Ʈ �ؽ�Ʈ
    [SerializeField]
    private Slider userExpSlider;                // ���� ����ġ �� 

    #endregion

    [Space(10)]

    #region ��ȭ, ���� ���� ������

    // �̳׶�, ���̾�, ������, ȯ������
    [Header("��ȭ, ���� ���� ������")]
    [Space(10)]

    [SerializeField]
    private TextMeshProUGUI mineralText;    // �̳׶� �ؽ�Ʈ
    [SerializeField]
    private TextMeshProUGUI diaText;        // ���̾� �ؽ�Ʈ
    [SerializeField]
    private TextMeshProUGUI energyText;     // ������ �ؽ�Ʈ
    [SerializeField]
    private TextMeshProUGUI eVNText;        // ȯ������ �ؽ�Ʈ

    #endregion

    // ������ ���� �ʱ� ����
    // StatManager���� ���� �ҷ��� �ε�
    public void InitialSetting()
    {

    }

    /// <CallbyStatUp>
    /// ������ �Լ����� StatManager���� �� ������ �ö� ��� ȣ���.
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
        userNameText.text = "��"+StatManager.Instance.UserName;
    }

    public void UpdatePlayerData()  // �÷��̾� ������ ���� UI������Ʈ
    {
            userLevelText.text = "Lv." + StatManager.Instance.Level_Player.ToString();

            // ����ġ ��
            float sliderValue = (float)(StatManager.Instance.Exp_Player / StatManager.Instance.RequiredExp);
            if (sliderValue >= 0.1f)    // 1 �ۼ�Ʈ �̻��� ��� ����
            {
                userExpSlider.value = sliderValue;
                // ����ġ �ۼ�Ʈ �ؽ�Ʈ
                int expPercent = (int)(userExpSlider.value * 100);
                userExpPercentText.text = expPercent.ToString() + "%";
            }
            else
            {
            userExpSlider.value = 0.1f;
            userExpPercentText.text = "1%";
            }

    }
    public void UpdateMineral()     // �̳׶� ���� UI ������Ʈ
    {
        mineralText.text = ScoreManager.Instance.ScoreToString(StatManager.Instance.Own_Mineral);
    }
    public void UpdateDia()         // ���̾� ���� UI ������Ʈ
    {
        diaText.text = ScoreManager.Instance.ScoreToString(StatManager.Instance.Own_Dia);
    }
    public void UpdateEnergy()      // ������ ���� UI ������Ʈ
    {
        energyText.text = StatManager.Instance.Own_Energy.ToString() + "/" + StatManager.Instance.Max_Energy.ToString();
    }
    
}
