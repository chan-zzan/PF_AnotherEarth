using UnityEngine;

[CreateAssetMenu(fileName = "Plant Data", menuName = "ScriptableObject_j/Plant Data", order = int.MaxValue)]
public class PlantData : ScriptableObject
{
    [Header("�Ĺ� �ѹ�")]
    [SerializeField]
    private int plantNumber;
    public int PlantNumber { get { return plantNumber; } }

    [Header("�Ĺ� ��������Ʈ")]
    [SerializeField]
    private Sprite plantSprite;
    public Sprite PlantSprite { get { return plantSprite; } }

    [Header("����ð�")]
    [SerializeField]
    private int createTime;
    public int CreateTime { get { return createTime; } }

    [Header("ȹ�� ����ġ")]
    [SerializeField]
    private int getExpValue;
    public int GetExpValue { get { return getExpValue; } }

    [Header("�Ĺ� �̸�")]
    [SerializeField]
    private string plantName;
    public string PlantName { get { return plantName; } }

    [Header("�Ĺ� ��� ����")]
    [SerializeField]
    [Multiline(3)]
    private string plantSummary;
    public string PlantSummary { get { return plantSummary; } }
}
