using UnityEngine;
using TMPro;
public class WeaponButton : MonoBehaviour
{
    [Header("무기 번호")]
    public int weaponNumber;

    [Header("해금 시 필요 재화타입")]
    public UnLockPayType payType;

    [Header("해금 시 필요 재화량")]
    public float unLockAmount;

    [Header("근거리 무기인지? (true:체크)")]
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
            // 해금 된 무기일 경우
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
        if(isShortWeapon)   // 근접 무기
        {
            // 버튼이 해금된 경우
            if (SlotManager.Instance.info_ShortWeaponUnLock[weaponNumber])
            {
                // 버튼 사운드 출력
                SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponButtonSound);

                // 팝업 오픈
                PopUpUIManager.Instance.OpenPopUp(PopUpType.ShortWeaponInfo);

                // 팝업 스테이지 정보 로드
                weaponInfoGroup.GetComponent<WeaponInfo>().UpdateUI(isShortWeapon,weaponNumber);
            }
        }
        else               // 원거리 무기
        {
            Debug.Log("onclick");

            // 버튼이 해금된 경우
            if (SlotManager.Instance.info_LongWeaponUnLock[weaponNumber])
            {
                // 버튼 사운드 출력
                SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponButtonSound);

                // 팝업 오픈
                PopUpUIManager.Instance.OpenPopUp(PopUpType.LongWeaponInfo);

                // 팝업 스테이지 정보 로드
                weaponInfoGroup.GetComponent<WeaponInfo>().UpdateUI(isShortWeapon, weaponNumber);
            }
        }
    }
}
