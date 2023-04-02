using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PausePopUpInfo : MonoBehaviour
{
    [Header("���� �� �̸�")]
    public string stageSceneName;

    [Header("���� Ȩ �� �̸�")]
    public string mainSceneName;

    [Header("���� ȹ���� ���� �ؽ�Ʈ")]
    public TextMeshProUGUI currentCoinText;

    [Header("���� �簳 �� ī��Ʈ�ٿ� �ؽ�Ʈ")]
    public TextMeshProUGUI countDownText;

    [Header("���� �簳 �� ī��Ʈ�ٿ� �ð�(��)")]
    public int resumeCountDownTime;

    [Header("Layout_PausePopUpInfo")]
    public GameObject layout_PausePopUpInfo;

    [Header("Layout_ResumeCount")]
    public GameObject layout_ResumeCount;

    [Header("�ε� ȭ�� �׷� ���� ������Ʈ")]
    public GameObject layout_LoadingGroup;

    private void OnEnable()
    {
        Time.timeScale = 0;

        layout_PausePopUpInfo.SetActive(true);

        layout_ResumeCount.SetActive(false);

        countDownText.text = "";

        currentCoinText.text = ScoreManager.Instance.ScoreToString(GameManager_E.Instance.totalMineral);
    }

    // �ٽ��ϱ� ��ư
    public void OnClickRepeatButton()
    {
        if (StatManager.Instance.Own_Energy >= 5)
        {
            StatManager.Instance.SubCurrentEnergy(5);

            this.gameObject.SetActive(false);

            SceneManager.LoadScene(stageSceneName);

            // �߰� - ������
            Time.timeScale = 1; // �ٽ��ϱ� ������ timeScale�� �ٽ� ���ƿ����� ����
        }
        else
        {
            // ������ ������ ������ ���� �˾� ��
            GameManager_E.Instance.EnergyChargePopup.SetActive(true);
        }        
    }

    // �簳 ��ư
    public void OnClickResumeButton()
    {
        layout_PausePopUpInfo.SetActive(false);

        layout_ResumeCount.SetActive(true);

        countDownText.text = resumeCountDownTime.ToString();

        StartCoroutine(ResumeCount());
    }

    IEnumerator ResumeCount()
    {
        yield return new WaitForSecondsRealtime(1f);
        
        if(resumeCountDownTime <= 1)
        {
            layout_ResumeCount.SetActive(false);

            this.gameObject.SetActive(false);

            resumeCountDownTime = 3;

            Time.timeScale = 1;
        }
        else
        {
            --resumeCountDownTime;

            countDownText.text = resumeCountDownTime.ToString();

            StartCoroutine(ResumeCount());
        }
    }

    // ���� Ȩ ��ư
    public void OnClickMainHome()
    {
        // ���� off
        SoundManager_E.Instance.AllSoundOff();

        Time.timeScale = 1;

        // �ε� ȭ�� ���
        layout_LoadingGroup.SetActive(true);

        // �� �񵿱� �ε�
        GameSceneManager.Instance.LoadGameScene(mainSceneName);

        this.gameObject.SetActive(false);
    }
}
