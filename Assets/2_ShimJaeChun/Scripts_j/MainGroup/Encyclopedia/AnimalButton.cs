using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AnimalButton : MonoBehaviour
{
    [Header("동물출연 스테이지 번호 (일반 동물)")]
    public int stageNumber;

    [Header("캐시 동물 번호 (캐시 동물에 한함.)")]
    [Header("0:아프간 1:판다 2:코끼리")]
    public int cashNum;

    [Header("캐시 동물인지?")]
    public bool isCashAnimal;

    [Header("Layout_LockGroup")]
    public GameObject lockGroup;

    [Header("Layout_ReleaseButton")]
    public GameObject releaseButton;

    [Header("Layout_ComeButton")]
    public GameObject comeButton;

    [Header("동물 버튼 이미지 관련")]
    public Button animalButton;    // 동물 버튼
    public Animator anim;          // 동물 애니메이션

    [Header("동물이 풀려져 있는지? (체크 : true)")]
    public bool isReleaseAnimal = false;

    [Header("캐시 동물일 경우 해금 필요 재화")]
    public float diaAmount;

    private Image glowImage;    // 버튼 글로우 이미지

    private void Awake()
    {
        // 초기 세팅
        InitialSetting();
        glowImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        // 캐시 동물이 아닐 경우
        if (!isCashAnimal)
        {
            // 스테이지 이지모드가 클리어 되어있을 경우
            if (StageManager.Instance.GetIsClearStage(stageNumber, BattleType.Easy))
            {
                UnLockThisButton();
            }
            isReleaseAnimal = SlotManager.Instance.GetIsReleaseAnimal(stageNumber, isCashAnimal);
        }
        // 캐시 동물일 경우
        else
        {
            if(UserProductManager.Instance.GetIsBuyProduct(ProductType.Animal, cashNum))
            {
                UnLockThisButton();
            }
            isReleaseAnimal = SlotManager.Instance.GetIsReleaseAnimal(cashNum, isCashAnimal);
        }

        // 동물이 풀려져있는 경우
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
        Debug.Log("동물 버튼 클릭");

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
        Debug.Log("풀어놓기 버튼 클릭");

        releaseButton.SetActive(false);

        SoundManager.Instance.PlayEffectSound(EffectSoundType.PopUpButtonSound);

        // 캐시 동물 버튼이 아닐 경우
        if (!isCashAnimal)
        {
            if (!SlotManager.Instance.ReleaseAnimal(stageNumber))
            {
                // 동물 배치에 실패한 경우
                Debug.Log("동물 배치 실패! 최대 배치개수" + SlotManager.Instance.maxReleaseAnimal);
            }
            else
            {
                // 버튼 색상 변경
                glowImage.color = new Color(0, 255, 0, 255);
                isReleaseAnimal = true;
            }
        }
        // 캐시 동물 버튼일 경우
        else
        {
            SlotManager.Instance.ReleaseCashAnimal(cashNum);
            // 버튼 색상 변경
            glowImage.color = new Color(0, 255, 0, 255);
            isReleaseAnimal = true;
        }
    }

    public void OnClickComeButton()
    {
        Debug.Log("불러오기 버튼 클릭");

        SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponUnEquipSound);

        comeButton.SetActive(false);

        // 캐시 동물 버튼이 아닐 경우
        if (!isCashAnimal)
        {
            SlotManager.Instance.ComeAnimal(stageNumber);
            // 버튼 색상 변경
            glowImage.color = new Color(0, 255, 201, 255);
            isReleaseAnimal = false;
        }
        else
        {
            SlotManager.Instance.ComeCashAnimal(cashNum);
            // 버튼 색상 변경
            glowImage.color = new Color(0, 255, 201, 255);
            isReleaseAnimal = false;
        }
    }
}

