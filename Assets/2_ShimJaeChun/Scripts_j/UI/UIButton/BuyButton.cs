using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public enum BuyType
{
    Coin = 0,
    Dia,
    BattleDia,
}
public enum GetRewardType
{
    Coin,
    Dia,
    Energy,
    Revival,
    Battle,
    PlantItem,
    ShortWeaponItem,
    LongWeaponItem,
    EncyItem
}

public class BuyButton : MonoBehaviour
{
    [Header("소모하는 재화 타입")]
    public BuyType buyType;

    [Header("지불하는 양")]
    public int buyAmount;       // 다이아와 코인을 모두 사용하는 경우
                                // buyAmount는 다이아

    [Header("첫번째 지불하는 양 텍스트")]
    public TextMeshProUGUI textFirstAmount;

    [Header("두번째로 지불하는 양 (재화 타입이 두개인 경우)")]
    public int secondBuyAmount; // 코인

    [Header("두번째 지불하는 양 텍스트")]
    public TextMeshProUGUI textSecondAmount;

    [Header("보상 타입")]
    public GetRewardType getType;

    [Header("보상 양")]
    public int getAmount;

    [Header("닫을 팝업 창 (없다면 null)")]
    public GameObject disablePopUp;

    [Header("리퀘스트 팝업")]
    public GameObject requestPopUp;

    [Header("아이템 구매 락")]
    public GameObject lockGroup;

    [Header("동물 배치 아이템 다이아리스트")]
    public int[] encyBuyDia;

    [Header("동물 배치 아이템 코인리스트")]
    public int[] encyBuyCoin;


    private void OnEnable()
    {
        // 동물 아이템의 경우 UI 업데이트가 필요
        if(getType == GetRewardType.EncyItem)
        {
            EncyBuyAmountUpdate();
        }
    }

    // 동물 배치 아이템 재화 UI 업데이트
    public void EncyBuyAmountUpdate()
    {
        // 아이템을 최대로 구매한 경우
        // 구매 불가로 설정
        if (SlotManager.Instance.maxReleaseAnimal == 12)
        {
            if (!lockGroup.activeSelf)
            {
                lockGroup.SetActive(true);
            }
        }
        else
        {
            // 아이템 구매 횟수 계산
            // 5 : 기본값
            int buyCount = SlotManager.Instance.maxReleaseAnimal - 5;

            buyAmount = encyBuyDia[buyCount];
            secondBuyAmount = encyBuyCoin[buyCount];

            textFirstAmount.text = buyAmount.ToString();
            textSecondAmount.text = ScoreManager.Instance.ScoreToString(secondBuyAmount);
        }
    }

    public void OnClickBuyButton()
    {
        if(requestPopUp && SlotManager.Instance.isRequestOn)
        {
            requestPopUp.SetActive(true);
            requestPopUp.GetComponent<ShopReQuestionPopUp>().UpdateBuyInfo(this);
            return;
        }

        switch(buyType)
        {
            case BuyType.Coin:
            {
                    // 소지한 재화가 충분한 경우
                    if (StatManager.Instance.Own_Mineral >= buyAmount)
                    {
                        SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponLevelUpSound);

                        switch (getType)
                        {
                            case GetRewardType.Dia:
                                {
                                    StatManager.Instance.SubMineral(buyAmount);
                                    StatManager.Instance.AddDia(getAmount);
                                    if (disablePopUp)
                                    {
                                        disablePopUp.SetActive(false);
                                    }
                                    break;
                                }
                            case GetRewardType.Energy:
                                {
                                    StatManager.Instance.SubMineral(buyAmount);
                                    StatManager.Instance.AddExtraEnergy(getAmount);
                                    if (disablePopUp)
                                    {
                                        disablePopUp.SetActive(false);
                                    }
                                    break;
                                }
                            default:
                                {
                                    Debug.Log("보상 지급 실패 : 보상 재화가 설정되지 않았습니다.");
                                    break;
                                }
                        }
                    }
                    else
                    {
                        SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponUnEquipSound);
                    }
                    break;
            }
            case BuyType.Dia:
            {
                    // 소지한 재화가 충분한 경우
                    if (StatManager.Instance.Own_Dia >= buyAmount)
                    {
                        SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponLevelUpSound);

                        switch (getType)
                        {
                            case GetRewardType.Coin:
                                {
                                    StatManager.Instance.SubDia(buyAmount);
                                    StatManager.Instance.AddMineral(getAmount);
                                    if (disablePopUp)
                                    {
                                        disablePopUp.SetActive(false);
                                    }
                                    break;
                                }
                            case GetRewardType.Energy:
                                {
                                    StatManager.Instance.SubDia(buyAmount);
                                    StatManager.Instance.AddExtraEnergy(getAmount);
                                    if (disablePopUp)
                                    {
                                        disablePopUp.SetActive(false);
                                    }
                                    break;
                                }
                            case GetRewardType.Battle:
                                {
                                    StatManager.Instance.SubDia(buyAmount);
                                    StatManager.Instance.AddExtraEnergy(10);
                                    SceneManager.LoadScene("InGame_E");
                                    break;
                                }
                            case GetRewardType.EncyItem:
                                {
                                    if(StatManager.Instance.Own_Mineral >= secondBuyAmount)
                                    {
                                        StatManager.Instance.SubDia(buyAmount);
                                        StatManager.Instance.SubMineral(secondBuyAmount);
                                        SlotManager.Instance.maxReleaseAnimal++;
                                        EncyBuyAmountUpdate();
                                    }
                                    break;
                                }
                            case GetRewardType.ShortWeaponItem:
                                {
                                    StatManager.Instance.SubDia(buyAmount);
                                    SlotManager.Instance.shortWeaponItem++;
                                    break;
                                }
                            case GetRewardType.LongWeaponItem:
                                {
                                    StatManager.Instance.SubDia(buyAmount);
                                    SlotManager.Instance.longWeaponItem++;
                                    break;
                                }
                            case GetRewardType.PlantItem:
                                {
                                    StatManager.Instance.SubDia(buyAmount);
                                    SlotManager.Instance.plantItem++;
                                    break;
                                }
                            default:
                                {
                                    Debug.Log("보상 지급 실패 : 보상 재화가 설정되지 않았습니다.");
                                    break;
                                }
                        }
                    }
                    else
                    {
                        if (getType != GetRewardType.Battle)
                        {
                            SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponUnEquipSound);
                        }
                    }
                    break;
            }
            case BuyType.BattleDia:
                {
                    if(StatManager.Instance.Own_Dia>=buyAmount)
                    {
                        StatManager.Instance.SubDia(buyAmount);
                        StatManager.Instance.AddExtraEnergy(10);
                        SceneManager.LoadScene("InGame_E");
                        Time.timeScale = 1f;
                    }
                    break;
                }
        default: break;
        }
    }

    public void BuyProduct()
    {
        switch(buyType)
        {
            case BuyType.Coin:
                {
                    break;
                }
            case BuyType.Dia:
                {
                    // 소지한 재화가 충분한 경우
                    if (StatManager.Instance.Own_Dia >= buyAmount)
                    {
                        SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponLevelUpSound);

                        switch (getType)
                        {
                            case GetRewardType.Coin:
                                {
                                    StatManager.Instance.SubDia(buyAmount);
                                    StatManager.Instance.AddMineral(getAmount);
                                    if (disablePopUp)
                                    {
                                        disablePopUp.SetActive(false);
                                    }
                                    break;
                                }
                            case GetRewardType.Energy:
                                {
                                    StatManager.Instance.SubDia(buyAmount);
                                    StatManager.Instance.AddExtraEnergy(getAmount);
                                    if (disablePopUp)
                                    {
                                        disablePopUp.SetActive(false);
                                    }
                                    break;
                                }
                            case GetRewardType.Battle:
                                {
                                    StatManager.Instance.SubDia(buyAmount);
                                    StatManager.Instance.AddExtraEnergy(10);
                                    SceneManager.LoadScene("InGame_E");
                                    break;
                                }
                            case GetRewardType.EncyItem:
                                {
                                    if (StatManager.Instance.Own_Mineral >= secondBuyAmount)
                                    {
                                        StatManager.Instance.SubDia(buyAmount);
                                        StatManager.Instance.SubMineral(secondBuyAmount);
                                        SlotManager.Instance.maxReleaseAnimal++;
                                        EncyBuyAmountUpdate();
                                    }
                                    break;
                                }
                            case GetRewardType.ShortWeaponItem:
                                {
                                    StatManager.Instance.SubDia(buyAmount);
                                    SlotManager.Instance.shortWeaponItem++;
                                    break;
                                }
                            case GetRewardType.LongWeaponItem:
                                {
                                    StatManager.Instance.SubDia(buyAmount);
                                    SlotManager.Instance.longWeaponItem++;
                                    break;
                                }
                            case GetRewardType.PlantItem:
                                {
                                    StatManager.Instance.SubDia(buyAmount);
                                    SlotManager.Instance.plantItem++;
                                    break;
                                }
                            default:
                                {
                                    Debug.Log("보상 지급 실패 : 보상 재화가 설정되지 않았습니다.");
                                    break;
                                }
                        }
                    }
                    else
                    {
                        if (getType != GetRewardType.Battle)
                        {
                            SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponUnEquipSound);
                        }
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}
