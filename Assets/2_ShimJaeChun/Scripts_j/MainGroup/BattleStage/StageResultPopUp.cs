using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StageResultPopUp : MonoBehaviour
{
    [Header("승리 팝업인지? (체크:true)")]
    public bool isVictoryPopup;

    [Header("메인 씬 이름")]
    public string mainSceneName;

    [Header("전투 씬 이름")]
    public string battleSceneName;

    [Header("로딩 화면 그룹 게임 오브젝트")]
    public GameObject layout_LoadingGroup;

    private void OnEnable()
    {
        // 미네랄 보상 추가
        if (isVictoryPopup)
        {
            StatManager.Instance.AddMineral(GameManager_E.Instance.totalMineral + StageManager.Instance.GetCurrentStageCoin());

            // 스테이지 정보 갱신
            StageManager.Instance.ClearBattleStage();

            /// 이은찬 수정 
            GameManager_E.Instance.clearRewardText.text = ScoreManager.Instance.ScoreToString(StageManager.Instance.GetCurrentStageCoin()); // 클리어 보상 표시
            GameManager_E.Instance.killRewardText.text = ScoreManager.Instance.ScoreToString(GameManager_E.Instance.totalMineral); // 몬스터 처치 보상 표시
        }
        else
        {
            // 패배했을 경우 현재 획득한 코인의 20퍼센트만 돌려줌
            float returnCoin = GameManager_E.Instance.totalMineral / 10 * 2;
            StatManager.Instance.AddMineral(returnCoin);
            GameManager_E.Instance.defeatRewardText.text = ScoreManager.Instance.ScoreToString(returnCoin); // UI표시

            // 몬스터 및 아이템을 더이상 스폰하지 않음
            GameManager_E.Instance.monsterSpawner.SpawnAllStop();
            GameManager_E.Instance.itemSpawner.StopItemDrop();

            // 타임스케일 돌려놓음
            Time.timeScale = 1.0f;
        }
    }


    // 재시작 버튼 클릭시
    public void OnClickRestartButton()
    {
        if (StatManager.Instance.Own_Energy >= 5)
        {
            StatManager.Instance.SubCurrentEnergy(5);
            SceneManager.LoadScene(battleSceneName);
        }
        else
        {
            // 에너지 부족시 에너지 충전 팝업 뜸
            GameManager_E.Instance.EnergyChargePopup.SetActive(true);
        }        
    }

    // 홈 버튼클릭시 
    public void OnClickHomeButton()
    {
        // 사운드 off
        SoundManager_E.Instance.AllSoundOff();

        // 로딩 화면 출력
        layout_LoadingGroup.SetActive(true);

        // 메인 씬 비동기 로드
        GameSceneManager.Instance.LoadGameScene(mainSceneName);

        // 팝업 창 닫기
        this.gameObject.SetActive(false);
    }
}
