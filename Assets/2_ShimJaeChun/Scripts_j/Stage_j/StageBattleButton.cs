using UnityEngine;

public class StageBattleButton : MonoBehaviour
{
    [Header("해당 버튼이 하드모드 버튼인지?")]
    public bool isHardMode;

    [Header("에너지 부족 시 팝업")]
    public GameObject buyEnergyGroup;

    private void OnDisable()
    {
        buyEnergyGroup.SetActive(false);
    }

    public void OnClickBattleButton()
    {
        if(StatManager.Instance.Own_Energy >= 5)
        {
            StatManager.Instance.SubCurrentEnergy(5);
        }
        else
        {
            buyEnergyGroup.SetActive(true);
            return;
        }

        Debug.Log("배틀 버튼 클릭");

        // 클릭된 스테이지 정보를 받아옴
        StageInfo myStageInfo = PopUpUIManager.Instance.popUpGroups[(int)PopUpType.StageInfo].GetComponent<StageInfo>();

        // 스테이지 번호 할당
        int stageNum = myStageInfo.StageNumber;
        BattleType chapterType = myStageInfo.myType;

        Debug.Log("스테이지 번호" + stageNum);
        Debug.Log("스테이지 정보" + myStageInfo);

        if (chapterType == BattleType.Hard)  // 하드모드
        {
            // 해금 조건 확인
            if (StageManager.Instance.HardStageStateList[stageNum-1])    // 해금된 경우 스테이지 진입
            { 
                Debug.Log("하드모드 해금 -> 진입");

                // 무기를 장착하지 않은 경우
                // 주사기 장착
                if (StatManager.Instance.s_Weapontype == SWeaponType.Idle
                    && StatManager.Instance.l_Weapontype == LWeaponType.Idle)
                {
                    SlotManager.Instance.EquipWeapon(false, (int)LWeaponType.Syringe);
                }

                // 전투모드 돌입 
                StatManager.Instance.isBattleMode = true;

                PopUpUIManager.Instance.ChangeScreenType(ScreenType.MainHome);

                // 스테이지 데이터 할당(스크립터블 오브젝트)
                StageManager.Instance.SetStageData(stageNum * -1);

                // 스테이지 로드
                StageManager.Instance.LoadStage(myStageInfo.StageSceneName);

                // 스테이지 정보 팝업 닫기
                PopUpUIManager.Instance.ClosePopUp(true);
            }
            else // 해금되지 않은 경우 이벤트 
            {

            }
        }
        else            // 이지모드
        {
            // 해금 조건 확인
            if (StageManager.Instance.EasyStageStateList[stageNum-1])    // 해금된 경우 스테이지 진입
            {
                Debug.Log("이지모드 해금 -> 진입");

                // 무기를 장착하지 않은 경우
                // 주사기 장착
                if(StatManager.Instance.s_Weapontype == SWeaponType.Idle 
                    && StatManager.Instance.l_Weapontype == LWeaponType.Idle)
                {
                    SlotManager.Instance.EquipWeapon(false, (int)LWeaponType.Syringe);
                }

                // 전투모드 돌입 
                StatManager.Instance.isBattleMode = true;

                PopUpUIManager.Instance.ChangeScreenType(ScreenType.MainHome);

                // 스테이지 데이터 할당(스크립터블 오브젝트)
                StageManager.Instance.SetStageData(stageNum);

                // 스테이지 로드
                StageManager.Instance.LoadStage(myStageInfo.StageSceneName);

                // 스테이지 정보 팝업 닫기
                PopUpUIManager.Instance.ClosePopUp(true);
            }
            else // 해금되지 않은 경우 이벤트 
            {

            }
        }
    }
}
