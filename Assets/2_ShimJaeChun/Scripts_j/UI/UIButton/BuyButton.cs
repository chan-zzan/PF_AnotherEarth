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
    [Header("�Ҹ��ϴ� ��ȭ Ÿ��")]
    public BuyType buyType;

    [Header("�����ϴ� ��")]
    public int buyAmount;       // ���̾ƿ� ������ ��� ����ϴ� ���
                                // buyAmount�� ���̾�

    [Header("ù��° �����ϴ� �� �ؽ�Ʈ")]
    public TextMeshProUGUI textFirstAmount;

    [Header("�ι�°�� �����ϴ� �� (��ȭ Ÿ���� �ΰ��� ���)")]
    public int secondBuyAmount; // ����

    [Header("�ι�° �����ϴ� �� �ؽ�Ʈ")]
    public TextMeshProUGUI textSecondAmount;

    [Header("���� Ÿ��")]
    public GetRewardType getType;

    [Header("���� ��")]
    public int getAmount;

    [Header("���� �˾� â (���ٸ� null)")]
    public GameObject disablePopUp;

    [Header("������Ʈ �˾�")]
    public GameObject requestPopUp;

    [Header("������ ���� ��")]
    public GameObject lockGroup;

    [Header("���� ��ġ ������ ���̾Ƹ���Ʈ")]
    public int[] encyBuyDia;

    [Header("���� ��ġ ������ ���θ���Ʈ")]
    public int[] encyBuyCoin;


    private void OnEnable()
    {
        // ���� �������� ��� UI ������Ʈ�� �ʿ�
        if(getType == GetRewardType.EncyItem)
        {
            EncyBuyAmountUpdate();
        }
    }

    // ���� ��ġ ������ ��ȭ UI ������Ʈ
    public void EncyBuyAmountUpdate()
    {
        // �������� �ִ�� ������ ���
        // ���� �Ұ��� ����
        if (SlotManager.Instance.maxReleaseAnimal == 12)
        {
            if (!lockGroup.activeSelf)
            {
                lockGroup.SetActive(true);
            }
        }
        else
        {
            // ������ ���� Ƚ�� ���
            // 5 : �⺻��
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
                    // ������ ��ȭ�� ����� ���
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
                                    Debug.Log("���� ���� ���� : ���� ��ȭ�� �������� �ʾҽ��ϴ�.");
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
                    // ������ ��ȭ�� ����� ���
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
                                    Debug.Log("���� ���� ���� : ���� ��ȭ�� �������� �ʾҽ��ϴ�.");
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
                    // ������ ��ȭ�� ����� ���
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
                                    Debug.Log("���� ���� ���� : ���� ��ȭ�� �������� �ʾҽ��ϴ�.");
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
