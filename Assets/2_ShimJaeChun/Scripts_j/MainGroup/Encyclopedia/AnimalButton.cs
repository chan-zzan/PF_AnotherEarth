using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AnimalButton : MonoBehaviour
{
    [Header("�����⿬ �������� ��ȣ (�Ϲ� ����)")]
    public int stageNumber;

    [Header("ĳ�� ���� ��ȣ (ĳ�� ������ ����.)")]
    [Header("0:������ 1:�Ǵ� 2:�ڳ���")]
    public int cashNum;

    [Header("ĳ�� ��������?")]
    public bool isCashAnimal;

    [Header("Layout_LockGroup")]
    public GameObject lockGroup;

    [Header("Layout_ReleaseButton")]
    public GameObject releaseButton;

    [Header("Layout_ComeButton")]
    public GameObject comeButton;

    [Header("���� ��ư �̹��� ����")]
    public Button animalButton;    // ���� ��ư
    public Animator anim;          // ���� �ִϸ��̼�

    [Header("������ Ǯ���� �ִ���? (üũ : true)")]
    public bool isReleaseAnimal = false;

    [Header("ĳ�� ������ ��� �ر� �ʿ� ��ȭ")]
    public float diaAmount;

    private Image glowImage;    // ��ư �۷ο� �̹���

    private void Awake()
    {
        // �ʱ� ����
        InitialSetting();
        glowImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        // ĳ�� ������ �ƴ� ���
        if (!isCashAnimal)
        {
            // �������� ������尡 Ŭ���� �Ǿ����� ���
            if (StageManager.Instance.GetIsClearStage(stageNumber, BattleType.Easy))
            {
                UnLockThisButton();
            }
            isReleaseAnimal = SlotManager.Instance.GetIsReleaseAnimal(stageNumber, isCashAnimal);
        }
        // ĳ�� ������ ���
        else
        {
            if(UserProductManager.Instance.GetIsBuyProduct(ProductType.Animal, cashNum))
            {
                UnLockThisButton();
            }
            isReleaseAnimal = SlotManager.Instance.GetIsReleaseAnimal(cashNum, isCashAnimal);
        }

        // ������ Ǯ�����ִ� ���
        if(isReleaseAnimal)
        {
            glowImage.color = new Color(0, 255, 0, 255);
        }
        else
        {
            glowImage.color = new Color(0, 255, 201, 255);
        }
    }

    public void InitialSetting()
    {
        lockGroup.SetActive(true);
        animalButton.enabled = false;
        anim.enabled = false;
    }

    public void UnLockThisButton()
    {
        lockGroup.SetActive(false);
        animalButton.enabled = true;
        anim.enabled = true;
    }
    public void DiaUnLockButton()
    {
        if(StatManager.Instance.Own_Dia >= diaAmount)
        {
            SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponLevelUpSound);
            StatManager.Instance.SubDia(diaAmount);
            UserProductManager.Instance.BuyProduct(ProductType.Animal, cashNum);
            UnLockThisButton();
        }
    }

    public void OnClickAnimalButton()
    {
        Debug.Log("���� ��ư Ŭ��");

        SoundManager.Instance.PlayEffectSound(EffectSoundType.PopUpButtonSound);

        if (isReleaseAnimal)
        {
            comeButton.SetActive(true);
            return;
        }
        else
        {
            releaseButton.SetActive(true);
            return;
        }
    }

    public void OnClickReleaseButton()
    {
        Debug.Log("Ǯ����� ��ư Ŭ��");

        releaseButton.SetActive(false);

        SoundManager.Instance.PlayEffectSound(EffectSoundType.PopUpButtonSound);

        // ĳ�� ���� ��ư�� �ƴ� ���
        if (!isCashAnimal)
        {
            if (!SlotManager.Instance.ReleaseAnimal(stageNumber))
            {
                // ���� ��ġ�� ������ ���
                Debug.Log("���� ��ġ ����! �ִ� ��ġ����" + SlotManager.Instance.maxReleaseAnimal);
            }
            else
            {
                // ��ư ���� ����
                glowImage.color = new Color(0, 255, 0, 255);
                isReleaseAnimal = true;
            }
        }
        // ĳ�� ���� ��ư�� ���
        else
        {
            SlotManager.Instance.ReleaseCashAnimal(cashNum);
            // ��ư ���� ����
            glowImage.color = new Color(0, 255, 0, 255);
            isReleaseAnimal = true;
        }
    }

    public void OnClickComeButton()
    {
        Debug.Log("�ҷ����� ��ư Ŭ��");

        SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponUnEquipSound);

        comeButton.SetActive(false);

        // ĳ�� ���� ��ư�� �ƴ� ���
        if (!isCashAnimal)
        {
            SlotManager.Instance.ComeAnimal(stageNumber);
            // ��ư ���� ����
            glowImage.color = new Color(0, 255, 201, 255);
            isReleaseAnimal = false;
        }
        else
        {
            SlotManager.Instance.ComeCashAnimal(cashNum);
            // ��ư ���� ����
            glowImage.color = new Color(0, 255, 201, 255);
            isReleaseAnimal = false;
        }
    }
}

