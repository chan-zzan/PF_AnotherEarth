using UnityEngine;

[System.Serializable]
public struct Phase
{
    public int phase; // �ܰ�
    public float spawnTime; // �����ð�
    public GameObject spawnMonster; // ��������

    public float HP; // �ܰ躰 ���� ������ ü��
    public float ATK; // �ܰ躰 ���� ������ ���ݷ�
    public int monsterNum; // �ܰ躰 ���� ���� ����

}

[System.Serializable]
public struct ClearItem
{
    public int mineral; // �̳׶� ����

    [Space(30)]

    public Sprite[] spaceMaterials; // ���ּ� ��ȭ��� �̹���
    public int[] spaceMaterialsNum; // ���ּ� ��ȭ��� ����

}

[CreateAssetMenu(fileName = "StageData", menuName = "StageData", order = int.MinValue + 1)] // ��ũ���ͺ� ������Ʈ�� ���� �� �ִ� ��� ����
public class StageData_E : ScriptableObject
{
    [SerializeField]
    private Phase[] phases; // �������� �� ����� ��� �����(���� ���� �ֱ�, ���� ����)

    [SerializeField]
    private ClearItem[] items; // Ŭ����� ��� �����۵� ���


    public Phase[] Phases
    {
        get => phases;
    }

    public ClearItem[] Items
    {
        get => items;
    }
}
