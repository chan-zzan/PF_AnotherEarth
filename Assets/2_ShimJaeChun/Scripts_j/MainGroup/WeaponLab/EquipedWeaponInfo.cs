using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EquipedWeaponInfo : MonoBehaviour
{
    [Header("ĳ���� ���� �ؽ�Ʈ (0:���� 1:ü��)")]
    public TextMeshProUGUI[] playerStatText;

    [Header("�ٰŸ� ���� �⺻ �ɷ�ġ �ؽ�Ʈ(0:�̸� 1:���� 2:���ݷ�)")]
    public TextMeshProUGUI[] sWeaponStatText;

    [Header("���Ÿ� ���� �⺻ �ɷ�ġ �ؽ�Ʈ(0:�̸� 1:���� 2:���ݷ�)")]
    public TextMeshProUGUI[] lWeaponStatText;

    public GameObject sWeaponStat;
    public GameObject lWeaponStat;

    private void OnEnable()
    {
        UpdateStat();
    }

    public void UpdateStat()
    {
        playerStatText[0].text = StatManager.Instance.Level_Player.ToString();
        playerStatText[1].text = ((int)StatManager.Instance.Hp_Player).ToString();

        if(StatManager.Instance.s_Weapontype != SWeaponType.Idle)
        {
            ST_WeaponStat weaponStat = StatManager.Instance.GetWeaponStat(true,(int)StatManager.Instance.s_Weapontype);

            sWeaponStat.SetActive(true);

            switch (StatManager.Instance.s_Weapontype)
            {
                case SWeaponType.Sword:
                    {
                        sWeaponStatText[0].text = "��";
                        break;
                    }
                case SWeaponType.Hammer:
                    {
                        sWeaponStatText[0].text = "��ġ";
                        break;
                    }
                case SWeaponType.Sycthe:
                    {
                        sWeaponStatText[0].text = "�보";
                        break;
                    }
                case SWeaponType.Fire:
                    {
                        sWeaponStatText[0].text = "ȭ������";
                        break;
                    }
                default: break;
            }
            sWeaponStatText[1].text = (weaponStat.level).ToString();
            sWeaponStatText[2].text = (weaponStat.atk).ToString();
        }
        else
        {
            sWeaponStat.SetActive(false);
            Debug.Log("not equip short");
            sWeaponStatText[0].text = "";
            sWeaponStatText[1].text = "";
            sWeaponStatText[2].text = "";
        }

        if (StatManager.Instance.l_Weapontype != LWeaponType.Idle)
        {
            ST_WeaponStat weaponStat = StatManager.Instance.GetWeaponStat(false, (int)StatManager.Instance.l_Weapontype);
            lWeaponStat.SetActive(true);

            switch(StatManager.Instance.l_Weapontype)
            {
                case LWeaponType.Syringe:
                    {
                        lWeaponStatText[0].text = "�ֻ��";
                        break;
                    }
                case LWeaponType.Bow:
                    {
                        lWeaponStatText[0].text = "Ȱ";
                        break;
                    }
                case LWeaponType.Gun:
                    {
                        lWeaponStatText[0].text = "��";
                        break;
                    }
                case LWeaponType.Rifle:
                    {
                        lWeaponStatText[0].text = "������";
                        break;
                    }
                default: break;
            }
            lWeaponStatText[1].text = (weaponStat.level).ToString();
            lWeaponStatText[2].text = (weaponStat.atk).ToString();
        }
        else
        {
            lWeaponStat.SetActive(false);
            Debug.Log("not equip long");
            lWeaponStatText[0].text = "";
            lWeaponStatText[1].text = "";
            lWeaponStatText[2].text = "";
        }

    }
}
