using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlantSlotInfo : MonoBehaviour
{
    [Header("���� �ѹ�")]
    public int slotNumber;

    [Header("�ر� �� �ʿ� ��ȭŸ��")]
    public UnLockPayType payType;

    [Header("�ر� �� �ʿ� ��ȭ��")]
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
        // �� ������ �رݵǾ� ���� ���
        if (SlotManager.Instance.info_PlantSlot[slotNumber])
        {
            isUnLock = true;
            lockGroup.SetActive(false);
            unLockGroup.SetActive(true);
        }
        // �� ������ ����ִ� ���
        else
        {
            isUnLock = false;
            lockGroup.SetActive(true);
            unLockGroup.SetActive(false);

            // ���� ������ �رݵǾ��� ���
            // �� ���Կ� �ر� ��ư ���
            if (SlotManager.Instance.info_PlantSlot[slotNumber - 1])
            {
                unLockButton.SetActive(true);
                amountText.text = ScoreManager.Instance.ScoreToString(unLockAmount);
            }
        }
    }

    // �ر� ��ư Ŭ�� ��
    public void OnClickUnLockButton()
    {
        // �ر� �Ϸ� ��
        if(SlotManager.Instance.UnLockPlantSlot(payType, unLockAmount))
        {
            SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponLevelUpSound);

            // �̺�Ʈ ���
            unLockGroup.SetActive(true);
            lockGroup.SetActive(false);

            if (slotNumber < 7)
            {
                SlotManager.Instance.plantSlotList[slotNumber + 1].SlotUpdate();
            }
            
        }
    }
}
