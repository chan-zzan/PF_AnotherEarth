using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlantSlotInfo : MonoBehaviour
{
    [Header("슬롯 넘버")]
    public int slotNumber;

    [Header("해금 시 필요 재화타입")]
    public UnLockPayType payType;

    [Header("해금 시 필요 재화량")]
    public float unLockAmount;

    [Header("Layout_LockGroup")]
    public GameObject lockGroup;

    [Header("Layout_UnLockGroup")]
    public GameObject unLockGroup;

    [Header("Button_UnLock")]
    public GameObject unLockButton;

    [Header("PayAmount Text")]
    public TextMeshProUGUI amountText;

    [Header("Slot is UnLocok?")]
    public bool isUnLock;

    private void Start()
    {
        isUnLock = false;
    }

    private void OnEnable()
    {
        SlotUpdate();
    }

    public void SlotUpdate()
    {
        // 이 슬롯이 해금되어 있을 경우
        if (SlotManager.Instance.info_PlantSlot[slotNumber])
        {
            isUnLock = true;
            lockGroup.SetActive(false);
            unLockGroup.SetActive(true);
        }
        // 이 슬롯이 잠겨있는 경우
        else
        {
            isUnLock = false;
            lockGroup.SetActive(true);
            unLockGroup.SetActive(false);

            // 이전 슬롯이 해금되었을 경우
            // 이 슬롯에 해금 버튼 출력
            if (SlotManager.Instance.info_PlantSlot[slotNumber - 1])
            {
                unLockButton.SetActive(true);
                amountText.text = ScoreManager.Instance.ScoreToString(unLockAmount);
            }
        }
    }

    // 해금 버튼 클릭 시
    public void OnClickUnLockButton()
    {
        // 해금 완료 시
        if(SlotManager.Instance.UnLockPlantSlot(payType, unLockAmount))
        {
            SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponLevelUpSound);

            // 이벤트 출력
            unLockGroup.SetActive(true);
            lockGroup.SetActive(false);

            if (slotNumber < 7)
            {
                SlotManager.Instance.plantSlotList[slotNumber + 1].SlotUpdate();
            }
            
        }
    }
}
