using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantInfo : MonoBehaviour
{
    [Header("�Ĺ� ������")]
    [Space(10)]

    [SerializeField]
    private PlantData plantData;
    public PlantData PlantData { get { return plantData; } }

    [Header("�Ĺ� �̸� ǥ�� �ؽ�Ʈ")]
    [SerializeField]
    private TextMeshProUGUI nameText;

    [Header("�Ĺ� �̹��� ǥ��")]
    [SerializeField]
    private Image mainIMG;

    [Header("�Ĺ� ��� ���� �ؽ�Ʈ")]
    [SerializeField]
    private TextMeshProUGUI summaryText;

    [Header("����ð� �ؽ�Ʈ")]
    [SerializeField]
    private TextMeshProUGUI[] hTimeText;


    [Header("ȹ�� ����ġ �ؽ�Ʈ")]
    [SerializeField]
    private TextMeshProUGUI expText;


    private void Start()
    {

    }

    public void InitialSetting(int _plantNum)
    {
        plantData = SlotManager.Instance.plantDataList[_plantNum - 1];

        nameText.text = plantData.PlantName;

        mainIMG.sprite = plantData.PlantSprite;

        summaryText.text = plantData.PlantSummary;

        TimeHMS myTime = ScoreManager.Instance.TimeToString(plantData.CreateTime);

        hTimeText[0].text = ((int)myTime.hour).ToString();
        hTimeText[1].text = "h";
        hTimeText[2].text = ((int)myTime.minute).ToString();
        hTimeText[3].text = "m";
        hTimeText[4].text = ((int)myTime.second).ToString();
        hTimeText[5].text = "s";


        expText.text = ScoreManager.Instance.ScoreToString(plantData.GetExpValue);
    }

    public void OnClickCreateButton()
    {
        // ��ư ���� ���
        SoundManager.Instance.PlayEffectSound(EffectSoundType.CreatePlantSound);

        // �Ĺ� ����
        SlotManager.Instance.CreatePlant(plantData.PlantNumber);
    }
}
