using UnityEngine;

[CreateAssetMenu(fileName = "Plant Data", menuName = "ScriptableObject_j/Plant Data", order = int.MaxValue)]
public class PlantData : ScriptableObject
{
    [Header("식물 넘버")]
    [SerializeField]
    private int plantNumber;
    public int PlantNumber { get { return plantNumber; } }

    [Header("식물 스프라이트")]
    [SerializeField]
    private Sprite plantSprite;
    public Sprite PlantSprite { get { return plantSprite; } }

    [Header("생산시간")]
    [SerializeField]
    private int createTime;
    public int CreateTime { get { return createTime; } }

    [Header("획득 경험치")]
    [SerializeField]
    private int getExpValue;
    public int GetExpValue { get { return getExpValue; } }

    [Header("식물 이름")]
    [SerializeField]
    private string plantName;
    public string PlantName { get { return plantName; } }

    [Header("식물 요약 설명")]
    [SerializeField]
    [Multiline(3)]
    private string plantSummary;
    public string PlantSummary { get { return plantSummary; } }
}
