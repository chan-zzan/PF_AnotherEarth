using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Weapon Info Data", menuName = "ScriptableObject_j/Weapon Info Data", order = int.MaxValue)]
public class WeaponInfoData : ScriptableObject
{
    [Header("무기 타입 : 근거리인지? (근거리/원거리)")]
    [SerializeField]
    private bool isShortWeapon;
    public bool IsShortWeapon { get { return isShortWeapon; } }

    [Header("무기 번호")]
    [Header("원거리 1:주사기 2:활 3:총 4:라이플")]
    [Header("근거리 1:검 2:망치 3:대낫 4:화염방사기")]
    [SerializeField]
    private int weaponNumber;
    public int WeaponNumber { get { return weaponNumber; } }

    [Header("무기 이름")]
    [SerializeField]
    private string weaponName;  
    public string WeaponName { get { return weaponName; } }

    [Header("무기 스프라이트")]
    [SerializeField]
    private Sprite weaponSprite;
    public Sprite WeaponSprite { get { return weaponSprite; } }

    [Header("무기 설명")]
    [SerializeField]
    [Multiline(3)]
    private string weaponSummary;
    public string WeaponSummary { get { return weaponSummary; } }

    [Header("처음 공격력")]
    [SerializeField]
    private float defaultAtk;
    public float DefaultAtk { get { return defaultAtk; } }

    [Header("레벨 당 공격력 증가량")]
    [SerializeField]
    private float atkPerLevel;
    public float AtkPerLevel { get { return atkPerLevel; } }


    [Header("처음 공격속도")]
    [SerializeField]
    private float defaultDps;
    public float DefaultDps { get { return defaultDps; } }

    [Header("최대 공격속도")]
    [SerializeField]
    private float maxDps;
    public float MaxDps { get { return maxDps; } }

    [Header("무기 레벨에 따른 공격속도 증가 ( ex) 10 : 10 레벨 당 씩 증가 )")]
    [SerializeField]
    private float dpsPerLevel;
    public float DpsPerLevel { get { return dpsPerLevel; } }

    [Header("처음 탄속")]
    [SerializeField]
    private float defaultPts;
    public float DefaultPts { get { return defaultPts; } }

    [Header("최대 탄속")]
    [SerializeField]
    private float maxPts;
    public float MaxPts { get { return maxPts; } }

    [Header("무기 레벨에 따른 탄속 증가 ( ex) 10 : 10 레벨 당 1씩 증가 )")]
    [SerializeField]
    private int ptsPerLevel;
    public int PtsPerLevel { get { return ptsPerLevel; } }

    [Header("최대 무기 개수")]
    [SerializeField]
    private int maxPtc;
    public int MaxPtc { get { return maxPtc; } }

    [Header("무기 레벨에 따른 무기 개수 증가 ( ex) 10 : 10 레벨 당 1개 증가 )")]
    [SerializeField]
    private int ptcPerLevel;
    public int PtcPerLevel { get { return ptcPerLevel; } }

    [Header("넉백 수치")]
    [SerializeField]
    private int knockBackValue;
    public int KnockBackValue { get { return knockBackValue; } }

}
