using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PausePopUpInfo : MonoBehaviour
{
    [Header("현재 씬 이름")]
    public string stageSceneName;

    [Header("메인 홈 씬 이름")]
    public string mainSceneName;

    [Header("현재 획득한 코인 텍스트")]
    public TextMeshProUGUI currentCoinText;

    [Header("게임 재개 시 카운트다운 텍스트")]
    public TextMeshProUGUI countDownText;

    [Header("게임 재개 시 카운트다운 시간(초)")]
    public int resumeCountDownTime;

    [Header("Layout_PausePopUpInfo")]
    public GameObject layout_PausePopUpInfo;

    [Header("Layout_ResumeCount")]
    public GameObject layout_ResumeCount;

    [Header("로딩 화면 그룹 게임 오브젝트")]
    public GameObject layout_LoadingGroup;

    private void OnEnable()
    {
        Time.timeScale = 0;

        layout_PausePopUpInfo.SetActive(true);

        layout_ResumeCount.SetActive(false);

        countDownText.text = "";

        currentCoinText.text = ScoreManager.Instance.ScoreToString(GameManager_E.Instance.totalMineral);
    }

    // 다시하기 버튼
    public void OnClickRepeatButton()
    {
        if (StatManager.Instance.Own_Energy >= 5)
        {
            StatManager.Instance.SubCurrentEnergy(5);

            this.gameObject.SetActive(false);

            SceneManager.LoadScene(stageSceneName);

            // 추가 - 이은찬
            Time.timeScale = 1; // 다시하기 했을때 timeScale이 다시 돌아오도록 수정
        }
        else
        {
            // 에너지 부족시 에너지 충전 팝업 뜸
            GameManager_E.Instance.EnergyChargePopup.SetActive(true);
        }        
    }

    // 재개 버튼
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

    // 메인 홈 버튼
    public void OnClickMainHome()
    {
        // 사운드 off
        SoundManager_E.Instance.AllSoundOff();

        Time.timeScale = 1;

        // 로딩 화면 출력
        layout_LoadingGroup.SetActive(true);

        // 씬 비동기 로드
        GameSceneManager.Instance.LoadGameScene(mainSceneName);

        this.gameObject.SetActive(false);
    }
}
