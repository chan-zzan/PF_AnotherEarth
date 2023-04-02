using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager_E : MonoBehaviour
{
    #region 싱글톤
    private static GameManager_E instance = null;

    public static GameManager_E Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager_E>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "GameManager_E";
                    DontDestroyOnLoad(obj);
                    instance = obj.AddComponent<GameManager_E>();
                }
            }
            return instance;
        }
    }
    #endregion

    // 스크립트
    [Header("Script")]
    public Player_E Player;
    public ObjectPoolManager_E Pool;
    public TimeManager_E Time;
    public MonsterSpawner_E monsterSpawner; // 몬스터 생성자
    public ItemSpawner_E itemSpawner;
    public Weapon_E Weapon; // 무기
    public WeaponSkillSlider weaponSkillSlider; // 무기 숙련도
    public MapManager_E Map; // 맵

    [Space(10)]

    // 캐릭터 관련
    [Header("Character")]
    public int life = 1;
    public int curSWeaponPTC; // 현재 근거리 무기 갯수    
    public bool gameover = false; // 게임 종료 여부
    public bool unbeatable = false; // 플레이어의 무적 여부
    public bool finalStage = false; // 마지막 스테이지 여부
    public int finalStageCount = 0; // 파이널 스테이지 잡은 보스의 갯수

    [Space(10)]

    // 몬스터 관련
    [Header("Monster")]
    public Monster_E curBoss = null; // 현재 보스
    public bool bossSpawn = false; // 보스 등장 여부    
    public bool isHardMode = false; // 하드모드인지 여부
    public int killCount = 0;
    public DashInfo dashInfo; // 대쉬 정보

    [Space(10)]

    // UI 관련
    [Header("UI")]
    public GameObject winPopup; // 승리 팝업
    public GameObject defeatPopup; // 패배 팝업
    public GameObject revivalPopup; // 부활 팝업
    public GameObject EnergyChargePopup; // 에너지 충전 팝업
    public Slider bossHP; // 보스 HP
    public TMPro.TMP_Text bossText; // 보스 등장 표시
    public TMPro.TMP_Text coinText; // 전투 중 코인 획득량
    public TMPro.TMP_Text killCountText; // 몬스터 처치 수
    public TMPro.TMP_Text clearRewardText; // 클리어 보상
    public TMPro.TMP_Text killRewardText; // 몬스터 처치 보상    
    public TMPro.TMP_Text defeatRewardText; // 패배 시 코인 획득량
    public TMPro.TMP_Text revival_timer; // 부활 타이머
    

    //[Space(10)]

    // 기타
    [Space(30)]
    [Header("종료팝업(임시)")]
    public GameObject EscapePopUp;

    // 재화 프리팹
    public GameObject coin; // 코인

    // 획득 재화 갯수
    public int totalMineral; // 얻은 미네랄의 갯수

    // 경고
    public GameObject warningPopup; // 경고팝업
    public GameObject BossWarningPopup; // 보스경고팝업
    public GameObject FinalWarningPopup; // 파이널경고팝업
    public GameObject FinalWarningPopup2; // 파이널경고팝업2
    public Animator warningAnim; // 경고 애니메이션
    public Animator hpWarningAnim; // hp 경고 애니메이션

    private void Awake()
    {
        // 라이프 초기화
        life = 1;

        // 미네랄 양 초기화
        coinText.text = "0";

        // 몬스터 킬 카운트 초기화
        killCountText.text = "0";

        //if(StageManager.Instance.curStageNum == 10 || )
    }


    private void Update()
    {
        // 회전 무기 개수가 바뀐 경우에만 동작
        if (curSWeaponPTC != WeaponSkillManager.Instance.curSweaponPtc)
        {
            curSWeaponPTC = WeaponSkillManager.Instance.curSweaponPtc; ;
            RotateWeaponChangePtc(curSWeaponPTC);
        }

        /// 디버그용
        if (Input.GetKeyDown(KeyCode.P))
        {
            Player.CurHP = 10;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            StatManager.Instance.AddDia(1000);
        }

        /// 테스트용 - 이은찬
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EscapePopUp.SetActive(true);
        }
    }

    void RotateWeaponChangePtc(int ptc)
    {
        if (ptc < 1) ptc = 1; // 최소 무기 갯수 = 1
        if (ptc > 5) ptc = 5; // 최대 무기 갯수 = 5

        Transform curWeaponTr = Weapon.CurSWeapon.transform;
        List<RWeapon_E> rWeapons = new List<RWeapon_E>();// R_Axis의 RWeapon_E 스크립트

        for (int i = 0; i < curWeaponTr.childCount; i++)
        {
            curWeaponTr.GetChild(i).gameObject.SetActive(true); // 모든 오브젝트 활성화 -> 스크립트에 접근하기 위함
            rWeapons.Add(curWeaponTr.GetChild(i).GetChild(0).GetComponent<RWeapon_E>()); // 스크립트 저장
            rWeapons[i].rotAxis.rotation = Quaternion.Euler(0, 0, 0); // 모든 오브젝트 원위치
        }

        for (int i = 0; i < ptc; i++)
        {
            int curRot = 360 / ptc; // 개수에 따라 회전각도를 설정

            // 번호별로 각도를 다르게 적용
            switch (rWeapons[i].weaponNumbers)
            {
                case RWeaponNumber.First:
                    curRot *= 0;
                    break;
                case RWeaponNumber.Second:
                    curRot *= 1;
                    break;
                case RWeaponNumber.Third:
                    curRot *= 2;
                    break;
                case RWeaponNumber.Fourth:
                    curRot *= 3;
                    break;
                case RWeaponNumber.Fifth:
                    curRot *= 4;
                    break;
            }

            rWeapons[i].rotAxis.rotation = Quaternion.Euler(0, 0, curRot); // 번호별 회전 적용
        }

        for(int i = ptc; i < rWeapons.Count; i++)
        {
            // 안쓰는 번호는 비활성화
            rWeapons[i].rotAxis.gameObject.SetActive(false);
        }
    }

    // 부활
    public void Revival(bool useDia)
    {
        if (useDia)
        {
            float revivalDia = 30; // 부활 시 필요 다이아

            if (StatManager.Instance.Own_Dia >= revivalDia)
            {
                StatManager.Instance.SubDia(revivalDia);
                Player.CurHP = Player.MaxHP; // 최대체력으로 회복
                revivalPopup.SetActive(false); // 부활 팝업 닫음
            }
            else
            {
                // 다이아가 부족할 경우
                SoundManager_E.Instance.EffectSoundPlay(6);
                return;
            }
        }
        else
        {
            Player.CurHP = Player.MaxHP / 2; // 50% 체력으로 회복
            revivalPopup.SetActive(false); // 부활 팝업 닫음
        }

        life = 0; // 목숨 감소
        Player.myState = Player_E.STATE.Play; // 다시 플레이 상태로 변경
        UnityEngine.Time.timeScale = 1; // 시간 멈춤 해제

        Weapon.CurLWeapon.GetComponent<Animator>().SetTrigger("revival"); // 애니메이션 동작
        gameover = false;

        StartCoroutine(UnBeatable(2.0f)); // 2초간 무적 상태로 변경

        // 몬스터들도 다시 움직이도록 변경
        Monster_E[] monsters = Pool.P_Monsters[0].parent.GetComponentsInChildren<Monster_E>();

        for (int i = 0; i < monsters.Length; i++)
        {
            monsters[i].myState = Monster_E.STATE.Move; // 직접 변경
        }

        // 원거리 무기 장착
        Transform[] child = Weapon.CurLWeapon.GetComponentsInChildren<Transform>(true);

        for (int i = 1; i < child.Length; i++)
        {
            child[i].gameObject.SetActive(true);
        }

        // 근접 무기 장착
        Weapon.CurSWeapon.SetActive(true);

        Weapon.CurLWeapon.GetComponent<Animator>().Play("idle");
    }

    IEnumerator UnBeatable(float t)
    {
        unbeatable = true;
        yield return UnbeatableAnimation(t);
        unbeatable = false;
    }

    IEnumerator UnbeatableAnimation(float t)
    {
        // 원거리 무기의 자식들에 접근
        SpriteRenderer[] child = Weapon.CurLWeapon.GetComponentsInChildren<SpriteRenderer>(true);

        List<Color> MinAlphaVals = new List<Color>();
        List<Color> MaxAlphaVals = new List<Color>();

        for (int i = 0; i < child.Length; i++)
        {
            // 색깔 값 저장
            MinAlphaVals.Add(new Color(child[i].color.r, child[i].color.g, child[i].color.b, 0.2f));
            MaxAlphaVals.Add(new Color(child[i].color.r, child[i].color.g, child[i].color.b, 0.5f));
        }

        int count = 5; // 깜빡일 횟수
        float time = t / count / 2; // 깜빡일 시간

        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < child.Length; j++)
            {
                child[j].color = MinAlphaVals[j];
            }

            yield return new WaitForSeconds(time);

            for (int j = 0; j < child.Length; j++)
            {
                child[j].color = MaxAlphaVals[j];
            }

            yield return new WaitForSeconds(time);
        }

        for (int i = 1; i < child.Length; i++)
        {
            // 원래 색깔 값으로 변경
            child[i].color = Color.white;
        }
    }

    // 씬로드
    public void LoadScene(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }

    //public void CoolTimeStart(float time)
    //{
    //    // 쿨타임 세팅
    //    CoolTime.gameObject.SetActive(true);
    //    CoolTime_text.text = time.ToString();

    //    StartCoroutine(CoolTimer(time));
    //}


    //IEnumerator CoolTimer(float time)
    //{
    //    float curTime = time;

    //    while (curTime > 0)
    //    {
    //        curTime -= UnityEngine.Time.deltaTime;
    //        CoolTime.fillAmount = curTime / time;
    //        CoolTime_text.text = "" + Mathf.CeilToInt(curTime);
    //        yield return null;
    //    }

    //    CoolTime.gameObject.SetActive(false);
    //}
}
