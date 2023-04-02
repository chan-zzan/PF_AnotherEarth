using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantInfo : MonoBehaviour
{
    [Header("식물 데이터")]
    [Space(10)]

    [SerializeField]
    private PlantData plantData;
    public PlantData PlantData { get { return plantData; } }

    [Header("식물 이름 표시 텍스트")]
    [SerializeField]
    private TextMeshProUGUI nameText;

    [Header("식물 이미지 표시")]
    [SerializeField]
    private Image mainIMG;

    [Header("식물 요약 설명 텍스트")]
    [SerializeField]
    private TextMeshProUGUI summaryText;

    [Header("생산시간 텍스트")]
    [SerializeField]
    private TextMeshProUGUI[] hTimeText;


    [Header("획득 경험치 텍스트")]
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
        // 버튼 사운드 출력
        SoundManager.Instance.PlayEffectSound(EffectSoundType.CreatePlantSound);

        // 식물 생산
        SlotManager.Instance.CreatePlant(plantData.PlantNumber);
    }
}
