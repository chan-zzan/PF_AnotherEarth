using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponInfo : MonoBehaviour
{
    [Header("무기 데이터")]
    [Space(10)]

    [SerializeField]
    private WeaponInfoData weaponData;
    public WeaponInfoData WeaponData { get { return weaponData; } }

    [Header("무기 이름 표시 텍스트")]
    [SerializeField]
    private TextMeshProUGUI nameText;

    [Header("무기 이미지 리스트")]
    public GameObject[] weaponSpriteList;

    [Header("레벨 텍스트")]
    [SerializeField]
    private TextMeshProUGUI levelText;

    [Header("레벨업 필요 미네랄 텍스트")]
    [SerializeField]
    private TextMeshProUGUI requiredMineralText;

    [Header("무기 설명 텍스트")]
    [SerializeField]
    private TextMeshProUGUI summaryText;

    [Header("현재 레벨 공격력 텍스트")]
    [SerializeField]
    private TextMeshProUGUI curAtkText;

    [Header("현재 레벨 공격속도 텍스트")]
    [SerializeField]
    private TextMeshProUGUI curDpsText;

    [Header("현재 레벨 무기 개수 텍스트")]
    [SerializeField]
    private TextMeshProUGUI curPtcText;

    [Header("다음 레벨 공격력 텍스트")]
    [SerializeField]
    private TextMeshProUGUI nextAtkText;

    [Header("다음 레벨 무기 개수 텍스트")]
    [SerializeField]
    private TextMeshProUGUI nextPtcText;

    [Header("아이템 개수 텍스트")]
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
        // 창이 닫힐 때 리셋 팝업도 닫힘.
        layout_ResetPopUp.SetActive(false);

        // 아이템 관련 팝업도 닫힘.
        itemInfoPopUp.SetActive(false);
        itemSuccessPopUp.SetActive(false);
        itemFailPopUp.SetActive(false);
        itemImpossiblePopUp.SetActive(false);
    }

    // UI 업데이트
    public void UpdateUI(bool isShort, int num)
    {
        // 기존 무기 초기화
        for(int i=0; i<weaponSpriteList.Length; i++)
        {
            weaponSpriteList[i].SetActive(false);
        }

        // 무기 이미지 활성화
        weaponSpriteList[num-1].SetActive(true);

        if (isShort)
        {
            // 아이템 개수 적용
            itemRemainText.text = "("+SlotManager.Instance.shortWeaponItem.ToString() + ")";

            // 무기 타입 할당
            SWeaponType myType = (SWeaponType)num;

            weaponData = SlotManager.Instance.shortWeaponDataList[num];

            nameText.text = weaponData.WeaponName;

            switch (myType)
            {
                case SWeaponType.Sword:
                    {
                        summaryText.text = weaponData.WeaponSummary;


                        // 만렙이 5로 적용될 경우
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
            // 아이템 개수 적용
            itemRemainText.text = "(" + SlotManager.Instance.longWeaponItem.ToString() + ")";

            // 무기 타입 할당
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
        // 버튼 사운드 출력
        SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponEquipSound);

        // 무기장착 버튼클릭
        SlotManager.Instance.EquipWeapon(weaponData.IsShortWeapon, weaponData.WeaponNumber);

        // 팝업 닫기
        this.gameObject.SetActive(false);
    }

    public void OnClickLevelUpButton()
    {
        if (weaponData.IsShortWeapon)
        {
            int sWeaponLevel = StatManager.Instance.GetWeaponStat(weaponData.IsShortWeapon, weaponData.WeaponNumber).level;

            // 만렙이 아닐 경우 
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

    // 아이템 사용 강화
    public void OnClickItemButton()
    {
        if(weaponData.IsShortWeapon)
        {
            // 근접 무기의 경우 최대 레벨이 5 이므로
            // 4레벨부터는 아이템 사용이 불가.
            if(StatManager.Instance.GetWeaponStat(true, weaponData.WeaponNumber).level>=4)
            {
                SoundManager.Instance.PlayEffectSound(EffectSoundType.PopUpButtonSound);
                itemImpossiblePopUp.SetActive(true);
                return;
            }

            // 아이템이 있을 경우
            if(SlotManager.Instance.shortWeaponItem > 0)
            {
                SlotManager.Instance.shortWeaponItem--;

                // 확률 적용
                int rand = Random.Range(0, 3);
                // 성공
                if(rand == 0)
                {
                    itemSuccessPopUp.SetActive(true);
                    StatManager.Instance.AddShortWeaponLevel((SWeaponType)weaponData.WeaponNumber, true);
                }
                // 실패
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
            // 아이템이 있을 경우
            if (SlotManager.Instance.longWeaponItem > 0)
            {
                SlotManager.Instance.longWeaponItem--;

                // 확률 적용
                int rand = Random.Range(0, 3);
                // 성공
                if (rand == 0)
                {
                    itemSuccessPopUp.SetActive(true);
                    StatManager.Instance.AddLongWeaponLevel((LWeaponType)weaponData.WeaponNumber, true);
                }
                // 실패
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

        // 해당 무기 레벨이 1보다 클 경우 동작
        if (myWeaponStat.level >1)
        {

            // 무기 스텟 할당
            layout_ResetPopUp.GetComponent<WeaponResetInfo>().level = myWeaponStat.level;
            layout_ResetPopUp.GetComponent<WeaponResetInfo>().defaultCoinReq = reqCoin.defaultReq;
            layout_ResetPopUp.GetComponent<WeaponResetInfo>().perCoinReq = reqCoin.perReq;
            layout_ResetPopUp.GetComponent<WeaponResetInfo>().isShort = weaponData.IsShortWeapon;
            layout_ResetPopUp.GetComponent<WeaponResetInfo>().weaponNum = weaponData.WeaponNumber;

            Debug.Log("enable 이전 level = " + myWeaponStat.level);

            layout_ResetPopUp.SetActive(true);
        }
    }
}
