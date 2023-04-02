using UnityEngine;

public class StageBattleButton : MonoBehaviour
{
    [Header("�ش� ��ư�� �ϵ��� ��ư����?")]
    public bool isHardMode;

    [Header("������ ���� �� �˾�")]
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

        Debug.Log("��Ʋ ��ư Ŭ��");

        // Ŭ���� �������� ������ �޾ƿ�
        StageInfo myStageInfo = PopUpUIManager.Instance.popUpGroups[(int)PopUpType.StageInfo].GetComponent<StageInfo>();

        // �������� ��ȣ �Ҵ�
        int stageNum = myStageInfo.StageNumber;
        BattleType chapterType = myStageInfo.myType;

        Debug.Log("�������� ��ȣ" + stageNum);
        Debug.Log("�������� ����" + myStageInfo);

        if (chapterType == BattleType.Hard)  // �ϵ���
        {
            // �ر� ���� Ȯ��
            if (StageManager.Instance.HardStageStateList[stageNum-1])    // �رݵ� ��� �������� ����
            { 
                Debug.Log("�ϵ��� �ر� -> ����");

                // ���⸦ �������� ���� ���
                // �ֻ�� ����
                if (StatManager.Instance.s_Weapontype == SWeaponType.Idle
                    && StatManager.Instance.l_Weapontype == LWeaponType.Idle)
                {
                    SlotManager.Instance.EquipWeapon(false, (int)LWeaponType.Syringe);
                }

                // ������� ���� 
                StatManager.Instance.isBattleMode = true;

                PopUpUIManager.Instance.ChangeScreenType(ScreenType.MainHome);

                // �������� ������ �Ҵ�(��ũ���ͺ� ������Ʈ)
                StageManager.Instance.SetStageData(stageNum * -1);

                // �������� �ε�
                StageManager.Instance.LoadStage(myStageInfo.StageSceneName);

                // �������� ���� �˾� �ݱ�
                PopUpUIManager.Instance.ClosePopUp(true);
            }
            else // �رݵ��� ���� ��� �̺�Ʈ 
            {

            }
        }
        else            // �������
        {
            // �ر� ���� Ȯ��
            if (StageManager.Instance.EasyStageStateList[stageNum-1])    // �رݵ� ��� �������� ����
            {
                Debug.Log("������� �ر� -> ����");

                // ���⸦ �������� ���� ���
                // �ֻ�� ����
                if(StatManager.Instance.s_Weapontype == SWeaponType.Idle 
                    && StatManager.Instance.l_Weapontype == LWeaponType.Idle)
                {
                    SlotManager.Instance.EquipWeapon(false, (int)LWeaponType.Syringe);
                }

                // ������� ���� 
                StatManager.Instance.isBattleMode = true;

                PopUpUIManager.Instance.ChangeScreenType(ScreenType.MainHome);

                // �������� ������ �Ҵ�(��ũ���ͺ� ������Ʈ)
                StageManager.Instance.SetStageData(stageNum);

                // �������� �ε�
                StageManager.Instance.LoadStage(myStageInfo.StageSceneName);

                // �������� ���� �˾� �ݱ�
                PopUpUIManager.Instance.ClosePopUp(true);
            }
            else // �رݵ��� ���� ��� �̺�Ʈ 
            {

            }
        }
    }
}
