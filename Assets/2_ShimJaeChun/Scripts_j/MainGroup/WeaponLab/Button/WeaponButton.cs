using UnityEngine;
using TMPro;
public class WeaponButton : MonoBehaviour
{
    [Header("���� ��ȣ")]
    public int weaponNumber;

    [Header("�ر� �� �ʿ� ��ȭŸ��")]
    public UnLockPayType payType;

    [Header("�ر� �� �ʿ� ��ȭ��")]
    public float unLockAmount;

    [Header("�ٰŸ� ��������? (true:üũ)")]
    public bool isShortWeapon;

    [Header("Layout_WeaponInfoGroup")]
    private GameObject weaponInfoGroup;

    [Header("Layout_LockGroup")]
    public GameObject lockGroup;

    [Header("PayAmount Text")]
    public TextMeshProUGUI amountText;



    private void Start()
    {
        if (isShortWeapon)
        {
            weaponInfoGroup = PopUpUIManager.Instance.popUpGroups[(int)PopUpType.ShortWeaponInfo];
        }
        else
        {
            weaponInfoGroup = PopUpUIManager.Instance.popUpGroups[(int)PopUpType.LongWeaponInfo];
        }
    }

    private void OnEnable()
    {
        WeaponUpdate();
    }
    public void WeaponUpdate()
    {
        if (isShortWeapon)
        {
            if (SlotManager.Instance.info_ShortWeaponUnLock[weaponNumber])
            {
                lockGroup.SetActive(false);
                return;
            }
        }
        else
        {
            // �ر� �� ������ ���
            if (SlotManager.Instance.info_LongWeaponUnLock[weaponNumber])
            {
                lockGroup.SetActive(false);
                return;
            }
        }

        lockGroup.SetActive(true);

        amountText.text = ScoreManager.Instance.ScoreToString(unLockAmount);
    }

    public void UnLockThisButton()
    {
        switch(payType)
        {
            case UnLockPayType.Pay_Mineral:
                {
                    if(StatManager.Instance.Own_Mineral >= unLockAmount)
                    {
                        StatManager.Instance.SubMineral(unLockAmount);
                        break;
                    }
                    else return;
                }
            case UnLockPayType.Pay_Dia:
                {
                    if (StatManager.Instance.Own_Dia >= unLockAmount)
                    {
                        StatManager.Instance.SubDia(unLockAmount);
                        break;
                    }
                    else return;
                }
        }

        if (isShortWeapon)
        {
            SlotManager.Instance.info_ShortWeaponUnLock[weaponNumber] = true;
        }
        else
        {
            SlotManager.Instance.info_LongWeaponUnLock[weaponNumber] = true;
        }

        WeaponUpdate();
    }

    public void OnClickWeaponButton()
    {
        if(isShortWeapon)   // ���� ����
        {
            // ��ư�� �رݵ� ���
            if (SlotManager.Instance.info_ShortWeaponUnLock[weaponNumber])
            {
                // ��ư ���� ���
                SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponButtonSound);

                // �˾� ����
                PopUpUIManager.Instance.OpenPopUp(PopUpType.ShortWeaponInfo);

                // �˾� �������� ���� �ε�
                weaponInfoGroup.GetComponent<WeaponInfo>().UpdateUI(isShortWeapon,weaponNumber);
            }
        }
        else               // ���Ÿ� ����
        {
            Debug.Log("onclick");

            // ��ư�� �رݵ� ���
            if (SlotManager.Instance.info_LongWeaponUnLock[weaponNumber])
            {
                // ��ư ���� ���
                SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponButtonSound);

                // �˾� ����
                PopUpUIManager.Instance.OpenPopUp(PopUpType.LongWeaponInfo);

                // �˾� �������� ���� �ε�
                weaponInfoGroup.GetComponent<WeaponInfo>().UpdateUI(isShortWeapon, weaponNumber);
            }
        }
    }
}
