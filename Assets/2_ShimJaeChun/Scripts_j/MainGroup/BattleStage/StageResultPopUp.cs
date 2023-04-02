using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StageResultPopUp : MonoBehaviour
{
    [Header("�¸� �˾�����? (üũ:true)")]
    public bool isVictoryPopup;

    [Header("���� �� �̸�")]
    public string mainSceneName;

    [Header("���� �� �̸�")]
    public string battleSceneName;

    [Header("�ε� ȭ�� �׷� ���� ������Ʈ")]
    public GameObject layout_LoadingGroup;

    private void OnEnable()
    {
        // �̳׶� ���� �߰�
        if (isVictoryPopup)
        {
            StatManager.Instance.AddMineral(GameManager_E.Instance.totalMineral + StageManager.Instance.GetCurrentStageCoin());

            // �������� ���� ����
            StageManager.Instance.ClearBattleStage();

            /// ������ ���� 
            GameManager_E.Instance.clearRewardText.text = ScoreManager.Instance.ScoreToString(StageManager.Instance.GetCurrentStageCoin()); // Ŭ���� ���� ǥ��
            GameManager_E.Instance.killRewardText.text = ScoreManager.Instance.ScoreToString(GameManager_E.Instance.totalMineral); // ���� óġ ���� ǥ��
        }
        else
        {
            // �й����� ��� ���� ȹ���� ������ 20�ۼ�Ʈ�� ������
            float returnCoin = GameManager_E.Instance.totalMineral / 10 * 2;
            StatManager.Instance.AddMineral(returnCoin);
            GameManager_E.Instance.defeatRewardText.text = ScoreManager.Instance.ScoreToString(returnCoin); // UIǥ��

            // ���� �� �������� ���̻� �������� ����
            GameManager_E.Instance.monsterSpawner.SpawnAllStop();
            GameManager_E.Instance.itemSpawner.StopItemDrop();

            // Ÿ�ӽ����� ��������
            Time.timeScale = 1.0f;
        }
    }


    // ����� ��ư Ŭ����
    public void OnClickRestartButton()
    {
        if (StatManager.Instance.Own_Energy >= 5)
        {
            StatManager.Instance.SubCurrentEnergy(5);
            SceneManager.LoadScene(battleSceneName);
        }
        else
        {
            // ������ ������ ������ ���� �˾� ��
            GameManager_E.Instance.EnergyChargePopup.SetActive(true);
        }        
    }

    // Ȩ ��ưŬ���� 
    public void OnClickHomeButton()
    {
        // ���� off
        SoundManager_E.Instance.AllSoundOff();

        // �ε� ȭ�� ���
        layout_LoadingGroup.SetActive(true);

        // ���� �� �񵿱� �ε�
        GameSceneManager.Instance.LoadGameScene(mainSceneName);

        // �˾� â �ݱ�
        this.gameObject.SetActive(false);
    }
}
