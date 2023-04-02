using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Weapon Info Data", menuName = "ScriptableObject_j/Weapon Info Data", order = int.MaxValue)]
public class WeaponInfoData : ScriptableObject
{
    [Header("���� Ÿ�� : �ٰŸ�����? (�ٰŸ�/���Ÿ�)")]
    [SerializeField]
    private bool isShortWeapon;
    public bool IsShortWeapon { get { return isShortWeapon; } }

    [Header("���� ��ȣ")]
    [Header("���Ÿ� 1:�ֻ�� 2:Ȱ 3:�� 4:������")]
    [Header("�ٰŸ� 1:�� 2:��ġ 3:�보 4:ȭ������")]
    [SerializeField]
    private int weaponNumber;
    public int WeaponNumber { get { return weaponNumber; } }

    [Header("���� �̸�")]
    [SerializeField]
    private string weaponName;  
    public string WeaponName { get { return weaponName; } }

    [Header("���� ��������Ʈ")]
    [SerializeField]
    private Sprite weaponSprite;
    public Sprite WeaponSprite { get { return weaponSprite; } }

    [Header("���� ����")]
    [SerializeField]
    [Multiline(3)]
    private string weaponSummary;
    public string WeaponSummary { get { return weaponSummary; } }

    [Header("ó�� ���ݷ�")]
    [SerializeField]
    private float defaultAtk;
    public float DefaultAtk { get { return defaultAtk; } }

    [Header("���� �� ���ݷ� ������")]
    [SerializeField]
    private float atkPerLevel;
    public float AtkPerLevel { get { return atkPerLevel; } }


    [Header("ó�� ���ݼӵ�")]
    [SerializeField]
    private float defaultDps;
    public float DefaultDps { get { return defaultDps; } }

    [Header("�ִ� ���ݼӵ�")]
    [SerializeField]
    private float maxDps;
    public float MaxDps { get { return maxDps; } }

    [Header("���� ������ ���� ���ݼӵ� ���� ( ex) 10 : 10 ���� �� �� ���� )")]
    [SerializeField]
    private float dpsPerLevel;
    public float DpsPerLevel { get { return dpsPerLevel; } }

    [Header("ó�� ź��")]
    [SerializeField]
    private float defaultPts;
    public float DefaultPts { get { return defaultPts; } }

    [Header("�ִ� ź��")]
    [SerializeField]
    private float maxPts;
    public float MaxPts { get { return maxPts; } }

    [Header("���� ������ ���� ź�� ���� ( ex) 10 : 10 ���� �� 1�� ���� )")]
    [SerializeField]
    private int ptsPerLevel;
    public int PtsPerLevel { get { return ptsPerLevel; } }

    [Header("�ִ� ���� ����")]
    [SerializeField]
    private int maxPtc;
    public int MaxPtc { get { return maxPtc; } }

    [Header("���� ������ ���� ���� ���� ���� ( ex) 10 : 10 ���� �� 1�� ���� )")]
    [SerializeField]
    private int ptcPerLevel;
    public int PtcPerLevel { get { return ptcPerLevel; } }

    [Header("�˹� ��ġ")]
    [SerializeField]
    private int knockBackValue;
    public int KnockBackValue { get { return knockBackValue; } }

}
