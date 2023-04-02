using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSkillManager : MonoBehaviour
{
    // ���� ���õ� �Ŵ���
    #region SigleTon
    private static WeaponSkillManager instance;
    public static WeaponSkillManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<WeaponSkillManager>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<WeaponSkillManager>();
                    instance = newObj;
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        var objs = FindObjectsOfType<WeaponSkillManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion

    [Header("���� ������ ���Ÿ� ����")]
    public LWeaponType curLweapon;

    [Header("���� ������ �ٰŸ� ����")]
    public SWeaponType curSweapon;

    [Header("���� ���õ� �� �ʿ� ų ī��Ʈ ����Ʈ")]
    public List<int> reqSyringe;
    public List<int> reqBow;
    public List<int> reqGun;
    public List<int> reqRifle;

    [Space(10f)]

    [Header("���� ������ ���Ÿ� ���� ����")]
    public float curLweaponAtk;
    public float curLweaponDps;
    public float curLweaponPts;
    public int curLweaponPtc;
    public float curLweaponKnockBackValue;

    [Header("���� ������ �ٰŸ� ���� ����")]
    public float curSweaponAtk;
    public float curSweaponDps;
    public int curSweaponPtc;
    public float curSweaponKnockBackValue;

    private void Start()
    {
        //CurrentWeaponInitialSetting();
        // �׽�Ʈ �� �ڷ�ƾ
         StartCoroutine(GameStartCorutine());
    }

    IEnumerator GameStartCorutine()
    {
        yield return new WaitForSeconds(0.1f);

        CurrentWeaponInitialSetting();
    }


    public void CurrentWeaponInitialSetting()
    {
        curSweapon = StatManager.Instance.s_Weapontype;
        curLweapon = StatManager.Instance.l_Weapontype;

        // �������� �ٰŸ� ���� ����
        if (StatManager.Instance.s_Weapontype != SWeaponType.Idle)
        {
            ST_WeaponStat s_stat = StatManager.Instance.GetWeaponStat(true, (int)StatManager.Instance.s_Weapontype);
            curSweaponAtk = s_stat.atk;
            curSweaponDps = s_stat.dps;
            curSweaponPtc = s_stat.ptc;
            curSweaponKnockBackValue = s_stat.knockBack;
        }

        // �������� ���Ÿ� ���� ����
        if (StatManager.Instance.l_Weapontype != LWeaponType.Idle)
        {
            ST_WeaponStat s_stat = StatManager.Instance.GetWeaponStat(false, (int)StatManager.Instance.l_Weapontype);
            curLweaponAtk = s_stat.atk;
            curLweaponDps = s_stat.dps;
            curLweaponPts = s_stat.pts;
            curLweaponPtc = s_stat.ptc;
            curLweaponKnockBackValue = s_stat.knockBack;
        }
    }

    public void WeaponSkillLevelUp()
    {
        switch (curLweapon)
        {
            case LWeaponType.Syringe:
                {
                    curLweaponPtc++;
                    curLweaponDps -= 0.1f;
                    curLweaponPts += 5;
                    break;
                }
            case LWeaponType.Bow:
                {
                    curLweaponPtc += 2;
                    curLweaponDps -= 0.1f;
                    curLweaponPts += 5;
                    break;
                }
            case LWeaponType.Gun:
                {
                    curLweaponDps -= 0.1f;
                    curLweaponPts += 5.0f;
                    break;
                }
            case LWeaponType.Rifle:
                {
                    curLweaponDps -= 0.01f;
                    curLweaponPts += 5;
                    break;
                }
        }
    }

    // ���� Ÿ�Ժ� ī��Ʈ ����Ʈ ��ȯ
    public List<int> GetRequireKillCount(LWeaponType _type)
    {
        switch(_type)
        {
            case LWeaponType.Syringe:
                {
                    return reqSyringe;
                }
                case LWeaponType.Bow:
                {
                    return reqBow;
                }
            case LWeaponType.Gun:
                {
                    return reqGun;
                }
            case LWeaponType.Rifle:
                {
                    return reqRifle;
                }
            default: return null;
        }
    }
}
