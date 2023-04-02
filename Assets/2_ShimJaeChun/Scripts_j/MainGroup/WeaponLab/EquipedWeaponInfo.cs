using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EquipedWeaponInfo : MonoBehaviour
{
    [Header("캐릭터 스텟 텍스트 (0:레벨 1:체력)")]
    public TextMeshProUGUI[] playerStatText;

    [Header("근거리 무기 기본 능력치 텍스트(0:이름 1:레벨 2:공격력)")]
    public TextMeshProUGUI[] sWeaponStatText;

    [Header("원거리 무기 기본 능력치 텍스트(0:이름 1:레벨 2:공격력)")]
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
                        sWeaponStatText[0].text = "검";
                        break;
                    }
                case SWeaponType.Hammer:
                    {
                        sWeaponStatText[0].text = "망치";
                        break;
                    }
                case SWeaponType.Sycthe:
                    {
                        sWeaponStatText[0].text = "대낫";
                        break;
                    }
                case SWeaponType.Fire:
                    {
                        sWeaponStatText[0].text = "화염방사기";
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
                        lWeaponStatText[0].text = "주사기";
                        break;
                    }
                case LWeaponType.Bow:
                    {
                        lWeaponStatText[0].text = "활";
                        break;
                    }
                case LWeaponType.Gun:
                    {
                        lWeaponStatText[0].text = "총";
                        break;
                    }
                case LWeaponType.Rifle:
                    {
                        lWeaponStatText[0].text = "라이플";
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
