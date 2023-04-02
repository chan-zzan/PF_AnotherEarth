using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponInfo : MonoBehaviour
{
    [Header("���� ������")]
    [Space(10)]

    [SerializeField]
    private WeaponInfoData weaponData;
    public WeaponInfoData WeaponData { get { return weaponData; } }

    [Header("���� �̸� ǥ�� �ؽ�Ʈ")]
    [SerializeField]
    private TextMeshProUGUI nameText;

    [Header("���� �̹��� ����Ʈ")]
    public GameObject[] weaponSpriteList;

    [Header("���� �ؽ�Ʈ")]
    [SerializeField]
    private TextMeshProUGUI levelText;

    [Header("������ �ʿ� �̳׶� �ؽ�Ʈ")]
    [SerializeField]
    private TextMeshProUGUI requiredMineralText;

    [Header("���� ���� �ؽ�Ʈ")]
    [SerializeField]
    private TextMeshProUGUI summaryText;

    [Header("���� ���� ���ݷ� �ؽ�Ʈ")]
    [SerializeField]
    private TextMeshProUGUI curAtkText;

    [Header("���� ���� ���ݼӵ� �ؽ�Ʈ")]
    [SerializeField]
    private TextMeshProUGUI curDpsText;

    [Header("���� ���� ���� ���� �ؽ�Ʈ")]
    [SerializeField]
    private TextMeshProUGUI curPtcText;

    [Header("���� ���� ���ݷ� �ؽ�Ʈ")]
    [SerializeField]
    private TextMeshProUGUI nextAtkText;

    [Header("���� ���� ���� ���� �ؽ�Ʈ")]
    [SerializeField]
    private TextMeshProUGUI nextPtcText;

    [Header("������ ���� �ؽ�Ʈ")]
    public TextMeshProUGUI itemRemainText;

    [Header("Layout_ResetPopUp")]
    public GameObject layout_ResetPopUp;

    [Header("ItemInfoPopUp")]
    public GameObject itemInfoPopUp;

    [Header("ItemSuccessPopUp")]
    public GameObject itemSuccessPopUp;

    [Header("ItemFailPopUp")]
    public GameObject itemFailPopUp;

    [Header("ItemImpossiblePopUp")]
    public GameObject itemImpossiblePopUp;


    private void OnDisable()
    {
        // â�� ���� �� ���� �˾��� ����.
        layout_ResetPopUp.SetActive(false);

        // ������ ���� �˾��� ����.
        itemInfoPopUp.SetActive(false);
        itemSuccessPopUp.SetActive(false);
        itemFailPopUp.SetActive(false);
        itemImpossiblePopUp.SetActive(false);
    }

    // UI ������Ʈ
    public void UpdateUI(bool isShort, int num)
    {
        // ���� ���� �ʱ�ȭ
        for(int i=0; i<weaponSpriteList.Length; i++)
        {
            weaponSpriteList[i].SetActive(false);
        }

        // ���� �̹��� Ȱ��ȭ
        weaponSpriteList[num-1].SetActive(true);

        if (isShort)
        {
            // ������ ���� ����
            itemRemainText.text = "("+SlotManager.Instance.shortWeaponItem.ToString() + ")";

            // ���� Ÿ�� �Ҵ�
            SWeaponType myType = (SWeaponType)num;

            weaponData = SlotManager.Instance.shortWeaponDataList[num];

            nameText.text = weaponData.WeaponName;

            switch (myType)
            {
                case SWeaponType.Sword:
                    {
                        summaryText.text = weaponData.WeaponSummary;


                        // ������ 5�� ����� ���
                        if (StatManager.Instance.Level_Sword == 5)
                        {
                            levelText.text = "Max";
                            requiredMineralText.text = "";
                        }
                        else
                        {
                            levelText.text = "+" + StatManager.Instance.Level_Sword.ToString();
                            requiredMineralText.text = ScoreManager.Instance.ScoreToString(StatManager.Instance.Required_Minelral_Sword);

                        }
                        
                        curAtkText.text = StatManager.Instance.Atk_Sword.ToString();

                        curDpsText.text = ((int)StatManager.Instance.Dps_Sword).ToString();

                        curPtcText.text = "1+" + (StatManager.Instance.Ptc_Sword - 1).ToString();

                        if (StatManager.Instance.Level_Sword == 5)
                        {
                            nextAtkText.text = "";
                            nextPtcText.text = "";
                        }
                        else
                        {
                            nextAtkText.text = (StatManager.Instance.Atk_Sword + weaponData.AtkPerLevel).ToString();

                            if (((StatManager.Instance.Level_Sword + 1) % weaponData.PtcPerLevel) == 0
                                && StatManager.Instance.Ptc_Sword < weaponData.MaxPtc)
                            {
                                nextPtcText.text = "1+" + (StatManager.Instance.Ptc_Sword - 1).ToString();
                            }
                            else
                            {
                                nextPtcText.text = curPtcText.text;
                            }
                        }

                        break;
                    }
                case SWeaponType.Hammer:
                    {
                        summaryText.text = weaponData.WeaponSummary;

                        if (StatManager.Instance.Level_Hammer == 5)
                        {
                            levelText.text = "Max";
                            requiredMineralText.text = "";

                        }
                        else
                        {
                            levelText.text = "+" + StatManager.Instance.Level_Hammer.ToString();
                            requiredMineralText.text = ScoreManager.Instance.ScoreToString(StatManager.Instance.Required_Minelral_Hammer);
                        }

                        curAtkText.text = StatManager.Instance.Atk_Hammer.ToString();

                        curDpsText.text = ((int)StatManager.Instance.Dps_Hammer).ToString();

                        curPtcText.text = "1+" + (StatManager.Instance.Ptc_Hammer - 1).ToString();

                        if(StatManager.Instance.Level_Hammer == 5)
                        {
                            nextAtkText.text = "";
                            nextPtcText.text = "";
                        }
                        else
                        {
                            nextAtkText.text = (StatManager.Instance.Atk_Hammer + weaponData.AtkPerLevel).ToString();

                            if (((StatManager.Instance.Level_Hammer + 1) % weaponData.PtcPerLevel) == 0
                                && StatManager.Instance.Ptc_Hammer < weaponData.MaxPtc)
                            {
                                nextPtcText.text = "1+" + (StatManager.Instance.Ptc_Hammer - 1).ToString();
                            }
                            else
                            {
                                nextPtcText.text = curPtcText.text;
                            }
                        }

                        break;
                    }
                case SWeaponType.Sycthe:
                    {
                        summaryText.text = weaponData.WeaponSummary;

                        if(StatManager.Instance.Level_Scythe == 5)
                        {
                            levelText.text = "Max";
                            requiredMineralText.text ="";
                        }
                        else
                        {
                            levelText.text = "+" + StatManager.Instance.Level_Scythe.ToString();
                            requiredMineralText.text = ScoreManager.Instance.ScoreToString(StatManager.Instance.Required_Minelral_Scythe);
                        }

                        curAtkText.text = StatManager.Instance.Atk_Scythe.ToString();

                        curDpsText.text = ((int)StatManager.Instance.Dps_Scythe).ToString();

                        curPtcText.text = "1+" + (StatManager.Instance.Ptc_Scythe - 1).ToString();

                        if (StatManager.Instance.Level_Scythe == 5)
                        {
                            nextAtkText.text = "";
                            nextPtcText.text = "";
                        }
                        else
                        {
                            nextAtkText.text = (StatManager.Instance.Atk_Scythe + weaponData.AtkPerLevel).ToString();

                            if (((StatManager.Instance.Level_Scythe + 1) % weaponData.PtcPerLevel) == 0
                                && StatManager.Instance.Ptc_Scythe < weaponData.MaxPtc)
                            {
                                nextPtcText.text = "1+" + (StatManager.Instance.Ptc_Scythe - 1).ToString();
                            }
                            else
                            {
                                nextPtcText.text = curPtcText.text;
                            }
                        }

                        break;
                    }
                case SWeaponType.Fire:
                    {
                        summaryText.text = weaponData.WeaponSummary;

                        if(StatManager.Instance.Level_Flamethrower == 5)
                        {
                            levelText.text = "Max";
                            requiredMineralText.text = "";
                        }
                        else
                        {
                            levelText.text = "+" + StatManager.Instance.Level_Flamethrower.ToString();
                            requiredMineralText.text = ScoreManager.Instance.ScoreToString(StatManager.Instance.Required_Minelral_Flamethrower);
                        }


                        curAtkText.text = StatManager.Instance.Atk_Flamethrower.ToString();

                        curDpsText.text = ((int)StatManager.Instance.Dps_Flamethrower).ToString();

                        curPtcText.text = "1+" + (StatManager.Instance.Ptc_Flamethrower - 1).ToString();

                        if (StatManager.Instance.Level_Flamethrower == 5)
                        {
                            nextAtkText.text = "";
                            nextPtcText.text = "";
                        }
                        else
                        {
                            nextAtkText.text = (StatManager.Instance.Atk_Flamethrower + weaponData.AtkPerLevel).ToString();

                            if (((StatManager.Instance.Level_Flamethrower + 1) % weaponData.PtcPerLevel) == 0
                                && StatManager.Instance.Ptc_Flamethrower < weaponData.MaxPtc)
                            {
                                nextPtcText.text = "1+" + (StatManager.Instance.Ptc_Flamethrower - 1).ToString();
                            }
                            else
                            {
                                nextPtcText.text = curPtcText.text;
                            }
                        }
                        break;
                    }
            }
        }
        else
        {
            // ������ ���� ����
            itemRemainText.text = "(" + SlotManager.Instance.longWeaponItem.ToString() + ")";

            // ���� Ÿ�� �Ҵ�
            LWeaponType myType = (LWeaponType)num;

            weaponData = SlotManager.Instance.longWeaponDataList[num];

            nameText.text = weaponData.WeaponName;

            switch (myType)
            {
                case LWeaponType.Syringe:
                    {
                        levelText.text = "+" + StatManager.Instance.Level_Syringe.ToString();

                        summaryText.text = weaponData.WeaponSummary;

                        requiredMineralText.text = ScoreManager.Instance.ScoreToString(StatManager.Instance.Required_Minelral_Syringe);

                        curAtkText.text = StatManager.Instance.Atk_Syringe.ToString();

                        curDpsText.text = StatManager.Instance.Dps_Syringe.ToString();

                        curPtcText.text = "1+"+(StatManager.Instance.Ptc_Syringe-1).ToString();

                        nextAtkText.text = (StatManager.Instance.Atk_Syringe + weaponData.AtkPerLevel).ToString();

                        break;
                    }
                case LWeaponType.Bow:
                    {

                        levelText.text = "+" + StatManager.Instance.Level_Bow.ToString();

                        summaryText.text = weaponData.WeaponSummary;

                        requiredMineralText.text = ScoreManager.Instance.ScoreToString(StatManager.Instance.Required_Minelral_Bow);

                        curAtkText.text = StatManager.Instance.Atk_Bow.ToString();

                        curDpsText.text = StatManager.Instance.Dps_Bow.ToString();

                        curPtcText.text = "1+" + (StatManager.Instance.Ptc_Bow - 1).ToString();

                        nextAtkText.text = (StatManager.Instance.Atk_Bow + weaponData.AtkPerLevel).ToString();

                        break;
                    }
                case LWeaponType.Gun:   
                    {
                        levelText.text = "+" + StatManager.Instance.Level_Gun.ToString();

                        summaryText.text = weaponData.WeaponSummary;

                        requiredMineralText.text = ScoreManager.Instance.ScoreToString(StatManager.Instance.Required_Minelral_Gun);

                        curAtkText.text = StatManager.Instance.Atk_Gun.ToString();

                        curDpsText.text = StatManager.Instance.Dps_Gun.ToString();

                        curPtcText.text = "1";

                        nextAtkText.text = (StatManager.Instance.Atk_Gun + weaponData.AtkPerLevel).ToString();

                        break;
                    }
                case LWeaponType.Rifle:
                    {
                        levelText.text = "+" + StatManager.Instance.Level_RifleGun.ToString();

                        summaryText.text = weaponData.WeaponSummary;

                        requiredMineralText.text = ScoreManager.Instance.ScoreToString(StatManager.Instance.Required_Minelral_RifleGun);

                        curAtkText.text = StatManager.Instance.Atk_RifleGun.ToString();

                        curDpsText.text = StatManager.Instance.Dps_RifleGun.ToString();

                        curPtcText.text = "1";

                        nextAtkText.text = (StatManager.Instance.Atk_RifleGun + weaponData.AtkPerLevel).ToString();

                        break;
                    }
            }
        }
    }

    public void OnClickEquipButton()
    {
        // ��ư ���� ���
        SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponEquipSound);

        // �������� ��ưŬ��
        SlotManager.Instance.EquipWeapon(weaponData.IsShortWeapon, weaponData.WeaponNumber);

        // �˾� �ݱ�
        this.gameObject.SetActive(false);
    }

    public void OnClickLevelUpButton()
    {
        if (weaponData.IsShortWeapon)
        {
            int sWeaponLevel = StatManager.Instance.GetWeaponStat(weaponData.IsShortWeapon, weaponData.WeaponNumber).level;

            // ������ �ƴ� ��� 
            if (sWeaponLevel < 5)
            {
                StatManager.Instance.AddShortWeaponLevel((SWeaponType)weaponData.WeaponNumber,false);
            }
            else
            {
                itemImpossiblePopUp.SetActive(true);
            }
        }
        else
        {
            StatManager.Instance.AddLongWeaponLevel((LWeaponType)weaponData.WeaponNumber,false);
        }
    }

    // ������ ��� ��ȭ
    public void OnClickItemButton()
    {
        if(weaponData.IsShortWeapon)
        {
            // ���� ������ ��� �ִ� ������ 5 �̹Ƿ�
            // 4�������ʹ� ������ ����� �Ұ�.
            if(StatManager.Instance.GetWeaponStat(true, weaponData.WeaponNumber).level>=4)
            {
                SoundManager.Instance.PlayEffectSound(EffectSoundType.PopUpButtonSound);
                itemImpossiblePopUp.SetActive(true);
                return;
            }

            // �������� ���� ���
            if(SlotManager.Instance.shortWeaponItem > 0)
            {
                SlotManager.Instance.shortWeaponItem--;

                // Ȯ�� ����
                int rand = Random.Range(0, 3);
                // ����
                if(rand == 0)
                {
                    itemSuccessPopUp.SetActive(true);
                    StatManager.Instance.AddShortWeaponLevel((SWeaponType)weaponData.WeaponNumber, true);
                }
                // ����
                else
                {
                    itemFailPopUp.SetActive(true);
                    StatManager.Instance.AddShortWeaponLevel((SWeaponType)weaponData.WeaponNumber, false);
                }

                UpdateUI(true, weaponData.WeaponNumber);
            }
            else
            {
                SoundManager.Instance.PlayEffectSound(EffectSoundType.PopUpButtonSound);
                itemInfoPopUp.SetActive(true);
            }
        }
        else
        {
            // �������� ���� ���
            if (SlotManager.Instance.longWeaponItem > 0)
            {
                SlotManager.Instance.longWeaponItem--;

                // Ȯ�� ����
                int rand = Random.Range(0, 3);
                // ����
                if (rand == 0)
                {
                    itemSuccessPopUp.SetActive(true);
                    StatManager.Instance.AddLongWeaponLevel((LWeaponType)weaponData.WeaponNumber, true);
                }
                // ����
                else
                {
                    itemFailPopUp.SetActive(true);
                    StatManager.Instance.AddLongWeaponLevel((LWeaponType)weaponData.WeaponNumber, false);
                }

                UpdateUI(false, weaponData.WeaponNumber);
            }
            else
            {
                SoundManager.Instance.PlayEffectSound(EffectSoundType.PopUpButtonSound);
                itemInfoPopUp.SetActive(true);
            }
        }
    }

    public void OnClickResetLevelButton()
    {
        ST_WeaponStat myWeaponStat = StatManager.Instance.GetWeaponStat(weaponData.IsShortWeapon, weaponData.WeaponNumber);
        ST_WeaponRequireCoin reqCoin = StatManager.Instance.GetRequireCoin(weaponData.IsShortWeapon, weaponData.WeaponNumber);

        // �ش� ���� ������ 1���� Ŭ ��� ����
        if (myWeaponStat.level >1)
        {

            // ���� ���� �Ҵ�
            layout_ResetPopUp.GetComponent<WeaponResetInfo>().level = myWeaponStat.level;
            layout_ResetPopUp.GetComponent<WeaponResetInfo>().defaultCoinReq = reqCoin.defaultReq;
            layout_ResetPopUp.GetComponent<WeaponResetInfo>().perCoinReq = reqCoin.perReq;
            layout_ResetPopUp.GetComponent<WeaponResetInfo>().isShort = weaponData.IsShortWeapon;
            layout_ResetPopUp.GetComponent<WeaponResetInfo>().weaponNum = weaponData.WeaponNumber;

            Debug.Log("enable ���� level = " + myWeaponStat.level);

            layout_ResetPopUp.SetActive(true);
        }
    }
}
