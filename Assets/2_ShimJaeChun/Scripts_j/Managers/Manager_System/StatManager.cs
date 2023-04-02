using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
// 원거리 타입
public enum LWeaponType
{
    Idle=0,     // 장착하지 않은 상태
    Syringe,    // 주사기
    Bow,        // 활
    Gun,        // 총
    Rifle,      // 라이플
}

// 근접 타입
public enum SWeaponType
{
    Idle=0, // 장착하지 않은 상태
    Sword,  // 검
    Hammer, // 망치
    Sycthe, // 낫
    Fire    // 화염방사기
}

public struct ST_WeaponStat
{
    public int level;
    public float atk;
    public float dps;
    public float pts;
    public int ptc;
    public float knockBack;
    public ST_WeaponStat(int _level,float _atk, float _dps, float _pts, int _ptc, int _knockBack)
    {
        level = _level;
        atk = _atk;
        dps = _dps;
        pts = _pts;
        ptc = _ptc;
        knockBack = _knockBack;
    }
}

public struct ST_WeaponRequireCoin
{
    public float defaultReq;
    public float perReq;

    public ST_WeaponRequireCoin(float _defaultReq, float _perReq)
    {
        defaultReq = _defaultReq;
        perReq = _perReq;
    }
}

public class StatManager : MonoBehaviour
{
    #region 싱글톤
    private static StatManager instance;
    public static StatManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<StatManager>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<StatManager>();
                    instance = newObj;
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        var objs = FindObjectsOfType<StatManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        // 씬 변경시에도 파괴되지 않는다.
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    // 게임 스텟 관리자
    // 우주선, 캐릭터, 미네랄 등 전반적인 스텟을 관리한다.
    #region 우주선 스텟
    [Header("우주선 스텟")]
    [SerializeField]
    private int level_SpaceShip;    // 우주선 레벨
    public int Level_SpaceShip { get { return level_SpaceShip; } }
    #endregion

    #region 플레이어 스텟

    [Header("유저 이름")]
    [SerializeField]
    private string userName;
    public string UserName { get { return userName; } }

    [Header("플레이어 스텟")]
    [SerializeField]
    private int level_Player;   // 플레이어 레벨
    public int Level_Player { get { return level_Player; } }

    [Header("10이전 플레이어 경험치 리스트")]
    [SerializeField]
    private int[] exp_PlayerList;   // 플레이어 경험치 리스트

    [SerializeField]
    private float exp_Player;     // 플레이어 경험치
    public float Exp_Player { get { return exp_Player; } }

    [SerializeField]
    private float spd_Player;     // 플레이어 이동속도
    public float Spd_Player { get { return spd_Player; } }

    [SerializeField]
    private float hp_Player;      // 플레이어 체력
    public float Hp_Player
    { get { return hp_Player; } }

    public bool usedCoupon1 = false;
    public bool usedCoupon2 = false;
    public bool usedCoupon3 = false;
    public bool reviewCheck = false;
    
    public int adsDiaCount = 3;
    public int adsEnergyCount = 3;
    public int adsRevivalCount = 3;

    [Header("0:다이아 1:에너지")]
    public Modules.Ads.AdsPlayButton[] adsPlayButtons;
    #endregion

    // 무기 관련 스텟은 json 데이터 저장 시 무기 레벨만 저장.
    #region 무기 스텟

    // 근접 무기 : 공격력(ATK), 연사력(DPS), 범위(ATR)
    #region 근접 무기 스텟

    #region 검 스텟
    [Header("검 스탯")]
    [Space(10)]

    [Header("레벨")]
    [SerializeField]

    private int level_Sword;   // 검 레벨
    public int Level_Sword { get { return level_Sword; } }

    [Header("공격력")]
    [SerializeField]
    private float atk_Sword;    // 검 공격력
    public float Atk_Sword { get { return atk_Sword; } }

    [Header("공격속도")]
    [SerializeField]
    private float dps_Sword;    // 검 연사력   
    public float Dps_Sword { get { return dps_Sword; } }

    [Header("발사개수")]
    [SerializeField]
    private int ptc_Sword;
    public int Ptc_Sword{get{return ptc_Sword;}}

    [Header("넉백 수치")]
    [SerializeField]
    private int knockBackVal_Sword;
    public int KnockBackVal_Sword { get { return knockBackVal_Sword; } }

    [Header("레벨업시 필요 미네랄")]
    [SerializeField]
    private float required_Minelral_Sword;  // 레벨업 시 필요 미네랄 앙
    public float Required_Minelral_Sword { get { return required_Minelral_Sword; } }
    #endregion

    [Space(10)]

    #region 망치 스텟
    [Header("망치 스탯")]
    [Space(10)]

    [Header("레벨")]
    [SerializeField]
    private int level_Hammer;   // 망치 레벨
    public int Level_Hammer { get { return level_Hammer; } }

    [Header("공격력")]
    [SerializeField]
    private float atk_Hammer;    // 망치 공격력
    public float Atk_Hammer { get { return atk_Hammer; } }

    [Header("공격속도")]
    [SerializeField]
    private float dps_Hammer;    // 망치 연사력   
    public float Dps_Hammer { get { return dps_Hammer; } }

    [Header("발사개수")]
    [SerializeField]
    private int ptc_Hammer;
    public int Ptc_Hammer { get { return ptc_Hammer; } }

    [Header("레벨업시 필요 미네랄")]
    [SerializeField]
    private float required_Minelral_Hammer;  // 레벨업 시 필요 미네랄 앙
    public float Required_Minelral_Hammer { get { return required_Minelral_Hammer; } }

    [Header("넉백 수치")]
    [SerializeField]
    private int knockBackVal_Hammer;
    public int KnockBackVal_Hammer { get { return knockBackVal_Hammer; } }

    #endregion

    [Space(10)]

    #region 대낫 스텟
    [Header("대낫 스탯")]
    [Space(10)]

    [Header("레벨")]
    [SerializeField]
    private int level_Scythe;   // 대낫 레벨
    public int Level_Scythe { get { return level_Scythe; } }

    [Header("공격력")]
    [SerializeField]
    private float atk_Scythe;    // 대낫 공격력
    public float Atk_Scythe { get { return atk_Scythe; } }

    [Header("공격속도")]
    [SerializeField]
    private float dps_Scythe;    // 대낫 연사력   
    public float Dps_Scythe { get { return dps_Scythe; } }

    [Header("발사개수")]
    [SerializeField]
    private int ptc_Scythe;
    public int Ptc_Scythe { get { return ptc_Scythe; } }

    [Header("레벨업시 필요 미네랄")]
    [SerializeField]
    private float required_Minelral_Scythe;  // 레벨업 시 필요 미네랄 앙
    public float Required_Minelral_Scythe { get { return required_Minelral_Scythe; } }

    [Header("넉백 수치")]
    [SerializeField]
    private int knockBackVal_Scythe;
    public int KnockBackVal_Scythe { get { return knockBackVal_Scythe; } }

    #endregion

    [Space(10)]

    #region 화염방사기 스텟
    [Header("화염방사기 스탯")]
    [Space(10)]

    [Header("레벨")]
    [SerializeField]
    private int level_Flamethrower;   // 화염방사기 레벨
    public int Level_Flamethrower { get { return level_Flamethrower; } }
    
    [Header("공격력")]
    [SerializeField]
    private float atk_Flamethrower;    // 화염방사기 공격력
    public float Atk_Flamethrower { get { return atk_Flamethrower; } }

    [Header("공격속도")]
    [SerializeField]
    private float dps_Flamethrower;    // 화염방사기 연사력   
    public float Dps_Flamethrower { get { return dps_Flamethrower; } }

    [Header("발사개수")]
    [SerializeField]
    private int ptc_Flamethrower;
    public int Ptc_Flamethrower { get { return ptc_Flamethrower; } }

    [Header("레벨업시 필요 미네랄")]
    [SerializeField]
    private float required_Minelral_Flamethrower;  // 레벨업 시 필요 미네랄 앙
    public float Required_Minelral_Flamethrower { get { return required_Minelral_Flamethrower; } }

    [Header("넉백 수치")]
    [SerializeField]
    private int knockBackVal_Flamethrower;
    public int KnockBackVal_Flamethrower { get { return knockBackVal_Flamethrower; } }

    #endregion

    [Space(10)]


    #endregion

    // 원거리 무기 : 공격력(ATK), 연사력(DPS), 투사체속도(PTS), 범위(ATR)
    #region 원거리 무기 스텟

    #region 주사기 스텟
    [Header("주사기 스탯")]
    [Space(10)]

    [Header("레벨")]
    [SerializeField]
    private int level_Syringe;   // 주사기 레벨
    public int Level_Syringe { get { return level_Syringe; } }

    [Header("공격력")]
    [SerializeField]
    private float atk_Syringe;    // 주사기 공격력
    public float Atk_Syringe { get { return atk_Syringe; } }

    [Header("공격속도")]
    [SerializeField]
    private float dps_Syringe;    // 주사기 연사력   
    public float Dps_Syringe { get { return dps_Syringe; } }

    [Header("투사체속도")]
    [SerializeField]
    private float pts_Syringe;    // 주사기 투사체 속도 
    public float Pts_Syringe { get { return pts_Syringe; } }

    [Header("발사개수")]
    [SerializeField]
    private int ptc_Syringe;
    public int Ptc_Syringe { get { return ptc_Syringe; } }

    [Header("레벨업시 필요 미네랄")]
    [SerializeField]
    private float required_Minelral_Syringe;  // 레벨업 시 필요 미네랄 앙
    public float Required_Minelral_Syringe { get { return required_Minelral_Syringe; } }

    [Header("넉백 수치")]
    [SerializeField]
    private int knockBackVal_Syringe;
    public int KnockBackVal_Syringe { get { return knockBackVal_Syringe; } }

    #endregion

    [Space(10)]

    #region 활 스텟
    [Header("활 스탯")]
    [Space(10)]

    [Header("레벨")]
    [SerializeField]
    private int level_Bow;   // 활 레벨
    public int Level_Bow { get { return level_Bow; } }

    [Header("공격력")]
    [SerializeField]
    private float atk_Bow;    // 활 공격력
    public float Atk_Bow { get { return atk_Bow; } }

    [Header("공격속도")]
    [SerializeField]
    private float dps_Bow;    // 활 연사력   
    public float Dps_Bow { get { return dps_Bow; } }

    [Header("투사체속도")]
    [SerializeField]
    private float pts_Bow;    // 활 투사체 속도 
    public float Pts_Bow { get { return pts_Bow; } }

    [Header("발사개수")]
    [SerializeField]
    private int ptc_Bow;
    public int Ptc_Bow { get { return ptc_Bow; } }

    [Header("레벨업시 필요 미네랄")]
    [SerializeField]
    private float required_Minelral_Bow;  // 레벨업 시 필요 미네랄 앙
    public float Required_Minelral_Bow { get { return required_Minelral_Bow; } }

    [Header("넉백 수치")]
    [SerializeField]
    private int knockBackVal_Bow;
    public int KnockBackVal_Bow { get { return knockBackVal_Bow; } }

    #endregion

    [Space(10)]

    #region 총 스텟
    [Header("총 스탯")]
    [Space(10)]

    [Header("레벨")]
    [SerializeField]
    private int level_Gun;   // 총 레벨
    public int Level_Gun { get { return level_Gun; } }
    
    [Header("공격력")]
    [SerializeField]
    private float atk_Gun;    // 총 공격력
    public float Atk_Gun { get { return atk_Gun; } }

    [Header("공격속도")]
    [SerializeField]
    private float dps_Gun;    // 총 연사력   
    public float Dps_Gun { get { return dps_Gun; } }

    [Header("투사체속도")]
    [SerializeField]
    private float pts_Gun;    // 총 투사체 속도 
    public float Pts_Gun { get { return pts_Gun; } }

    [Header("발사개수")]
    [SerializeField]
    private int ptc_Gun;
    public int Ptc_Gun { get { return ptc_Gun; } }

    [Header("레벨업시 필요 미네랄")]
    [SerializeField]
    private float required_Minelral_Gun;  // 레벨업 시 필요 미네랄 앙
    public float Required_Minelral_Gun { get { return required_Minelral_Gun; } }

    [Header("넉백 수치")]
    [SerializeField]
    private int knockBackVal_Gun;
    public int KnockBackVal_Gun { get { return knockBackVal_Gun; } }

    #endregion

    [Space(10)]

    #region 라이플건 스텟
    [Header("라이플건 스탯")]
    [Space(10)]

    [Header("레벨")]
    [SerializeField]
    private int level_RifleGun;   // 라이플건 레벨
    public int Level_RifleGun { get { return level_RifleGun; } }

    [Header("공격력")]
    [SerializeField]
    private float atk_RifleGun;    // 라이플건 공격력
    public float Atk_RifleGun { get { return atk_RifleGun; } }

    [Header("공격속도")]
    [SerializeField]
    private float dps_RifleGun;    // 라이플건 연사력   
    public float Dps_RifleGun { get { return dps_RifleGun; } }

    [Header("투사체속도")]
    [SerializeField]
    private float pts_RifleGun;
    public float Pts_RifleGun { get { return pts_RifleGun; } }

    [Header("발사개수")]
    [SerializeField]
    private int ptc_RifleGun;
    public int Ptc_RifleGun { get { return ptc_RifleGun; } }

    [Header("레벨업시 필요 미네랄")]
    [SerializeField]
    private float required_Minelral_RifleGun;  // 레벨업 시 필요 미네랄 앙
    public float Required_Minelral_RifleGun { get { return required_Minelral_RifleGun; } }

    [Header("넉백 수치")]
    [SerializeField]
    private int knockBackVal_RifleGun;
    public int KnockBackVal_RifleGun { get { return knockBackVal_RifleGun; } }

    #endregion

    [Space(10)]

    #endregion

    #region 무기 레벨당 필요 재화량
    [Header("무기 첫 레벨업 시 필요 재화량")]
    [Space(10f)]

    public float defaultReq_Sword = 600000f;
    public float defaultReq_Hammer = 640000f;
    public float defaultReq_Scythe = 680000f;
    public float defaultReq_Flamethrower = 720000f;

    public float defaultReq_Syringe = 10000f;
    public float defaultReq_Bow = 15000f;
    public float defaultReq_Gun = 20000f;
    public float defaultReq_RifleGun = 25000f;

    [Header("무기 레벨 당 시 필요 재화 증가량")]
    [Space(10f)]

    public float perReq_Sword = 600000f;
    public float perReq_Hammer = 640000f;
    public float perReq_Scythe = 680000f;
    public float perReq_Flamethrower = 720000;

    public float perReq_Syringe = 10000f;
    public float perReq_Bow = 15000f;
    public float perReq_Gun = 10000f;
    public float perReq_RifleGun = 25000f;

    #endregion

    #endregion

    #region 포인트 스텟
    [Header("재화, 수치 관련 스텟")]
    [SerializeField]
    private float own_Mineral;  // 소지한 미네랄 단위 
    public float Own_Mineral { get { return own_Mineral; } }


    [SerializeField]
    private float own_Dia;      // 소지한 다이아
    public float Own_Dia { get { return own_Dia; } }

    [SerializeField]
    private int max_Energy;   // 에너지 최대치
    public int Max_Energy { get { return max_Energy; } }

    [SerializeField]
    private int own_Energy;  // 소지한 에너지
    public int Own_Energy { get { return own_Energy; } }
    #endregion

    /// <배틀모드인지?>
    /// 유저가 전투 상태에 진입하였을 경우 전투가 종료되어 메인 홈으로 돌아온 시점에 실시해야 함.
    /// 초기화는 false, 전투 모드에 진입할 때 true로 변경, 전투 모드가 종료되어 메인화면으로 돌아올 때 false로 변경
    /// </배틀모드인지?>
    public bool isBattleMode;  // 현재 유저가 전투 모드인지? (전투 스테이지에 진입한 상태인지?)

    #region 읽기 전용 능력치

    [Header("스텟에 따른 능력치(읽기 전용)")]
    [SerializeField]
    private float mine_MineralPerSeconds;   // 초당 획득 미네랄
    public float Mine_MineralPerSeconds { get { return mine_MineralPerSeconds; } }

    [SerializeField]
    private float requiredExp;      // 레벨업 시 필요 경험치
    public float RequiredExp { get { return requiredExp; } }

    // 레벨-2 필요 경험치
    private float fprevExpReq;
    // 레벨-1 필요 경험치
    private float lprevExpReq;

    #endregion

    public LWeaponType l_Weapontype;    // 현재 장착한 원거리 무기
    public SWeaponType s_Weapontype;    // 현재 장착한 근거리 무기

    [Header("EquipedWeaponInfo")]
    public EquipedWeaponInfo equipedWeaponInfo;

    void Start()
    {
        LoadStat();
    }

    // 테스트 이후 업데이트 부분 삭제해야 함.
    private void Update()
    {
        /// 테스트용 - 이은찬
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EscapePopUp.SetActive(true);
        }
    }

    /// <테스트용 - 이은찬>

    [Space(30)]
    [Header("종료팝업(임시)")]
    public GameObject EscapePopUp;

    public void EscapeGame()
    {
        GameDataManager.Instance.AutoSave();
        Application.Quit(); // 게임종료

        print("escape");
    }

    /// <테스트용 - 이은찬>

    // 초기 세팅
    public void FirstInitialSetting()
    {
        Debug.Log("첫 세팅");

        // 처음 시작한 경우

        #region 우주선 세팅
        level_SpaceShip = 1;
        #endregion

        #region 플레이어 세팅
        userName = "";
        usedCoupon1 = false;
        usedCoupon2 = false;
        usedCoupon3 = false;
        reviewCheck = false;

        adsDiaCount = 3;
        adsEnergyCount = 3;
        adsRevivalCount = 3;
        level_Player = 1;
        exp_Player = 0f;
        spd_Player = 40;
        hp_Player = 100;

        fprevExpReq = 0f;
        lprevExpReq = 0f;
        requiredExp = 10;    // 레벨업 시 필요 경험치
    
        #endregion

        #region 무기 세팅

        #region 근접 무기

        // 검
        level_Sword = 1;
        atk_Sword = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sword].DefaultAtk;
        dps_Sword = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sword].DefaultDps;
        ptc_Sword = 1;
        knockBackVal_Sword = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sword].KnockBackValue;
        required_Minelral_Sword = defaultReq_Sword;

        // 망치
        level_Hammer = 1;
        atk_Hammer = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Hammer].DefaultAtk;
        dps_Hammer = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Hammer].DefaultDps;
        ptc_Hammer = 1;
        knockBackVal_Hammer = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Hammer].KnockBackValue;
        required_Minelral_Hammer = defaultReq_Hammer;

        // 대낫
        level_Scythe = 1;
        atk_Scythe = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sycthe].DefaultAtk;
        dps_Scythe = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sycthe].DefaultDps;
        ptc_Scythe = 1;
        knockBackVal_Scythe = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sycthe].KnockBackValue;
        required_Minelral_Scythe = defaultReq_Scythe;

        // 화염방사기
        level_Flamethrower = 1;
        atk_Flamethrower = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Fire].DefaultAtk;
        dps_Flamethrower = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Fire].DefaultDps;
        ptc_Flamethrower = 1;
        knockBackVal_Flamethrower = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Fire].KnockBackValue;
        required_Minelral_Flamethrower = defaultReq_Flamethrower;

        #endregion

        #region 원거리 무기

        // 주사기
        level_Syringe = 1;
        atk_Syringe = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Syringe].DefaultAtk;
        dps_Syringe = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Syringe].DefaultDps;
        pts_Syringe = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Syringe].DefaultPts;
        ptc_Syringe = 1;
        required_Minelral_Syringe = defaultReq_Syringe;

        // 활
        level_Bow = 1;
        atk_Bow = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Bow].DefaultAtk;
        dps_Bow = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Bow].DefaultDps;
        pts_Bow = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Bow].DefaultPts;
        ptc_Bow = 2;
        required_Minelral_Bow = defaultReq_Bow;

        // 총
        level_Gun = 1;
        atk_Gun = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Gun].DefaultAtk;
        dps_Gun = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Gun].DefaultDps;
        pts_Gun = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Gun].DefaultPts;
        ptc_Gun = 1;
        required_Minelral_Gun = defaultReq_Gun;


        // 라이플건
        level_RifleGun = 1;
        atk_RifleGun = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Rifle].DefaultAtk;
        dps_RifleGun = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Rifle].DefaultDps;
        pts_RifleGun = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Rifle].DefaultPts;
        ptc_RifleGun = 4;
        required_Minelral_RifleGun = defaultReq_RifleGun;

        #endregion

        #endregion

        #region 재화, 점수 관련 초기세팅
        own_Mineral = 0;
        own_Dia = 0;
        max_Energy = 35;
        own_Energy = 35;
        #endregion


        #region 장착 중인 무기 세팅
        s_Weapontype = SWeaponType.Idle;
        l_Weapontype = LWeaponType.Syringe;

        #endregion

        GameTimeManager.Instance.InitialSetting(null);
        UserProductManager.Instance.UserProductInitialSetting(null);
        StageManager.Instance.InitialSetting(null);
        SlotManager.Instance.PlantSlotInitialSetting(true, null);
        SlotManager.Instance.WeaponSlotInitialSetting(true, null);
    }

    public void LoadedInitialSetting(GameData_Json data)
    {
        Debug.Log("로드 세팅");

        #region 우주선 세팅
        level_SpaceShip = data.iSpaceShipLevel;
        #endregion

        #region 플레이어 세팅
        userName = data.sUserName;
        usedCoupon1 = data.bUsedCoupon1;
        usedCoupon2 = data.bUsedCoupon2;
        usedCoupon3 = data.bUsedCoupon3;
        reviewCheck = data.bReviewCheck;

        adsDiaCount = data.iAdsDiaCount;
        adsEnergyCount = data.iAdsEnergyCount;
        adsRevivalCount = data.iAdsRevivalCount;
        level_Player = data.iPlayerLevel;
        exp_Player = data.fPlayerEXP;
        spd_Player = 40;
        if(level_Player < 10)
        {
            if(level_Player == 1)
            {
                hp_Player = 100;
            }
            else
            {
                hp_Player = 100 + (level_Player - 1) * 20;
            }

            requiredExp = exp_PlayerList[level_Player-1];

            Debug.Log("레벨: " + Level_Player + " 의 필요 경험치는 " + requiredExp);
        }
        else
        {
            // 레벨 업 시 플레이어 코어 스텟 증가
            // 20~40, 40~60, 60~80, 80~ 구간별 hp 증가폭이 변경
            // 각 구간마다 이전 구간 hp 총합을 더해줌.
            if (1 <= level_Player && level_Player < 21)
            {
                hp_Player = ((level_Player - 1) * 20) + 100;
            }
            else if (21 <= level_Player && level_Player < 41)
            {
                int prevHp = 20 * 19 + 100;
                int temp = level_Player - 20;
                hp_Player = (temp * 30) + prevHp;
            }
            else if (41 <= level_Player && level_Player < 61)
            {
                int prevHp = (20 * 19)+ (30 * 20) + 100;
                int temp = level_Player - 40;
                hp_Player = (temp * 40) + prevHp;
            }
            else if (61 <= level_Player && level_Player < 81)
            {
                int prevHp = (20 * 19) + (70 * 20) + 100;
                int temp = level_Player - 60;
                hp_Player = (temp * 50) + prevHp;
            }
            else
            {
                int prevHp = (20 * 19) + (120 * 20) +100;
                int temp = level_Player - 80;
                hp_Player = (temp * 60) + prevHp;
            }

            fprevExpReq = 600f;
            lprevExpReq = 1000f;

            for (int i = 10; i <= Level_Player; i++)
            {
                requiredExp = lprevExpReq + (lprevExpReq - fprevExpReq) * 1.1f;

                fprevExpReq = lprevExpReq;
                lprevExpReq = requiredExp;
            }

            Debug.Log("레벨: " + Level_Player+" 의 필요 경험치는 " + requiredExp);
        }

        //requiredExp = (float)(level_Player * 100f);    // 레벨업 시 필요 경험치
        #endregion

        #region 무기 세팅

        #region 근접 무기

        // 검
        level_Sword = data.iSwordLevel;
        atk_Sword = Level_Sword * SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sword].AtkPerLevel;
        dps_Sword = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sword].DefaultDps;        
        ptc_Sword = level_Sword;
        required_Minelral_Sword = defaultReq_Sword + (level_Sword-1)*perReq_Sword;
        knockBackVal_Sword = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sword].KnockBackValue;

        // 망치
        level_Hammer = data.iHammerLevel;
        atk_Hammer = level_Hammer * SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Hammer].AtkPerLevel;
        dps_Hammer = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Hammer].DefaultDps;
        ptc_Hammer = level_Hammer;
        required_Minelral_Hammer = defaultReq_Hammer + (level_Hammer - 1) * perReq_Hammer;
        knockBackVal_Hammer = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Hammer].KnockBackValue;

        // 대낫
        level_Scythe = data.iScytheLevel;
        atk_Scythe = level_Scythe * SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sycthe].AtkPerLevel;
        dps_Scythe = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sycthe].DefaultDps;
        ptc_Scythe = level_Scythe;
        required_Minelral_Scythe = defaultReq_Scythe + (level_Scythe - 1) * perReq_Scythe;
        knockBackVal_Scythe = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sycthe].KnockBackValue;

        // 화염방사기
        level_Flamethrower = data.iFlamethrowerLevel;
        atk_Flamethrower = level_Flamethrower*SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Fire].DefaultAtk;
        dps_Flamethrower = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Fire].DefaultDps;
        ptc_Flamethrower = level_Flamethrower;        
        required_Minelral_Flamethrower = defaultReq_Flamethrower + (level_Flamethrower - 1) * perReq_Flamethrower;
        knockBackVal_Flamethrower = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Fire].KnockBackValue;

        #endregion

        #region 원거리 무기

        // 주사기
        level_Syringe = data.iSyringeLevel;
        atk_Syringe = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Syringe].DefaultAtk + (level_Syringe - 1) * SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Syringe].AtkPerLevel;

        dps_Syringe = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Syringe].DefaultDps;
        pts_Syringe = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Syringe].DefaultPts;
        ptc_Syringe = 1;

        required_Minelral_Syringe = defaultReq_Syringe + (level_Syringe - 1) * perReq_Syringe;

        // 활
        level_Bow = data.iBowLevel;
        atk_Bow = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Bow].DefaultAtk + (level_Bow - 1) * SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Bow].AtkPerLevel;

        dps_Bow = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Bow].DefaultDps;
        pts_Bow = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Bow].DefaultPts;
        ptc_Bow = 2;


        required_Minelral_Bow = defaultReq_Bow + (level_Bow - 1) * perReq_Bow;

        // 총
        level_Gun = data.iGunLevel;
        atk_Gun = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Gun].DefaultAtk + (level_Gun - 1) * SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Gun].AtkPerLevel;

        dps_Gun = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Gun].DefaultDps;
        pts_Gun = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Gun].DefaultPts;
        ptc_Gun = 1;


        required_Minelral_Gun = defaultReq_Gun + (level_Gun - 1) * perReq_Gun;

        // 라이플건
        level_RifleGun = data.iRifleGunLevel;
        atk_RifleGun = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Rifle].DefaultAtk + (level_RifleGun - 1) * SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Rifle].AtkPerLevel;

        dps_RifleGun = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Rifle].DefaultDps;
        pts_RifleGun = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Rifle].DefaultPts;
        ptc_RifleGun = 4;

        required_Minelral_RifleGun = defaultReq_RifleGun + (level_RifleGun - 1) * perReq_RifleGun;

        #endregion

        #endregion

        #region 재화, 점수 관련 로드세팅
        own_Mineral = data.fMineral;
        mine_MineralPerSeconds = 1f;  // 초당 미네랄 획득량
        own_Dia = data.fDia;
        max_Energy = data.iMaxEnergy;
        own_Energy = data.iOwnEnergy;
        #endregion

        #region 장착 중인 무기 세팅
        s_Weapontype = (SWeaponType)data.iIsEquipedShortWeaponNum;
        l_Weapontype = (LWeaponType)data.iIsEquipedLongWeaponNum;

        #endregion

        GameTimeManager.Instance.InitialSetting(data);
        UserProductManager.Instance.UserProductInitialSetting(data);
        StageManager.Instance.InitialSetting(data);
        SlotManager.Instance.WeaponSlotInitialSetting(false, data);
        SlotManager.Instance.PlantSlotInitialSetting(false, data);
        SlotManager.Instance.AnimalSlotInitialSetting(data);

        // 튜토리얼 진행 중 비정상 종료한 유저의 경우 다시 튜토리얼 진행
        // 튜토리얼이 정상 종료되었을 경우 유저의 레벨은 2
        if (Level_Player <= 1 && !PopUpUIManager.Instance.firstStartGroup.activeSelf)
        {
            PopUpUIManager.Instance.firstStartGroup.SetActive(true);
            SoundManager.Instance.PlayBackGroundSound(MainSoundType.CutSceneSound);
        }
    }

    // 데이터 로드
    public void LoadStat()
    {
        // 전투모드 초기화
        isBattleMode = false;

        GameData_Json loadedData = GameDataManager.Instance.LoadData();

        // 데이터 매니저를 통해서 저장된 유저 데이터 불러옴
        if (loadedData != null)
        {
            LoadedInitialSetting(loadedData);

            UIUpdateManager.Instance.UpdateAll();
        }
        else
        {
            // 저장된 유저 데이터가 없을 경우
            FirstInitialSetting();

            UIUpdateManager.Instance.UpdateAll();
        }
    }

    public void SetUserName(string name)
    {
        userName = name;
        UIUpdateManager.Instance.UpdateUserName();
    }

    public void AdsCounting(RewardAdsType type)
    {
        switch(type)
        {
            case RewardAdsType.Ads_Dia:
                {
                    --adsDiaCount;
                    adsPlayButtons[0].UpdateUI();
                    break;
                }
            case RewardAdsType.Ads_Energy:
                {
                    --adsEnergyCount;
                    adsPlayButtons[1].UpdateUI();
                    adsPlayButtons[2].UpdateUI();
                    break;
                }
            case RewardAdsType.Ads_Revival:
                {
                    --adsRevivalCount;
                    break;
                }
            default: break;
        }
    }

    // 플레이어 경험치 관련
    public void AddPlayerLevel(int amount)      // 플레이어 레벨 업
    {
        // 레벨 증가
        level_Player += amount;

        if (level_Player < 10)
        {
            requiredExp = exp_PlayerList[level_Player-1];
        }
        else
        {
            if (level_Player == 10)
            {
                fprevExpReq = 600f;
                lprevExpReq = 1000f;
            }
            // 경험치 갱신
            requiredExp = lprevExpReq + (lprevExpReq - fprevExpReq) * 1.1f;   // 필요 경험치 갱신
            fprevExpReq = lprevExpReq;
            lprevExpReq = requiredExp;
        }

        exp_Player = 0;     // 경험치 초기화

        // 레벨 업 시 플레이어 코어 스텟 증가
        if(1 <= level_Player && level_Player < 21)
        {
            hp_Player += 20f;
        }
        else if(21 <= level_Player && level_Player < 41)
        {
            hp_Player += 30f;
        }
        else if(41 <= level_Player && level_Player < 61)
        {
            hp_Player += 40f;
        }
        else if (61 <= level_Player && level_Player < 81)
        {
            hp_Player += 50f;
        }
        else
        {
            hp_Player += 60f;
        }

        // 전투 모드인지 확인
        if (!isBattleMode)
        { // UI 업데이트
            UIUpdateManager.Instance.UpdatePlayerData();

            // 레벨업 팝업
            PopUpUIManager.Instance.OpenPopUp(PopUpType.LevelUp);

            Debug.Log("레벨: " + Level_Player + " 의 필요 경험치는 " + requiredExp);
        }
    }
    public void AddPlayerEXP(float amount)      // 플레이어 경험치 업
    {
        if (amount > 0)
        {
            if (exp_Player + amount < requiredExp)   // 필요 경험치 보다 작은 경우
            {
                // 경험치 추가
                exp_Player += amount;
            }
            else                                    // 경험치가 초과한 경우
            {
                AddPlayerLevel(1);  // 1 레벨업
                AddPlayerEXP(exp_Player + amount - requiredExp);    // 여분의 경험치 다시 추가
            }
        }

        // 전투 모드인지 확인
        if (!isBattleMode)
        { // UI 업데이트
            UIUpdateManager.Instance.UpdatePlayerData();
        }
    }

    // 재화, 점수 관련
    public void AddMineral(float amount)        // 미네랄 추가
    {
        own_Mineral += amount;

        // 전투 모드인지 확인
        if (!isBattleMode)
        { // UI 업데이트
            UIUpdateManager.Instance.UpdateMineral();
        }
    }
    public void SubMineral(float amount)        // 미네랄 소모
    {
        own_Mineral -= amount;

        // 전투 모드인지 확인
        if (!isBattleMode)
        { // UI 업데이트
            UIUpdateManager.Instance.UpdateMineral();
        }
    }

    public void AddDia(float amount)            // 다이아 추가
    {
        own_Dia += amount;

        // 전투 모드인지 확인
        if (!isBattleMode)
        { // UI 업데이트
            UIUpdateManager.Instance.UpdateDia();
        }
    }
    public void SubDia(float amount)            // 다이아 소모
    {
        own_Dia -= amount;

        // 전투 모드인지 확인
        if (!isBattleMode)
        { // UI 업데이트
            UIUpdateManager.Instance.UpdateDia();
        }
    }
    /// <에너지추가>
    /// 시간 당 에너지 추가 = 최대 에너지 이상으로 추가 불가
    /// 보상, 구매로 에너지 추가 = 최대 에너지 이상으로 추가 가능
    /// </에너지추가>
    public void AddMaxEnergy(int amount)      // 최대 에너지 추가
    {
        max_Energy += amount;
        if (!isBattleMode)
        { // UI 업데이트
            UIUpdateManager.Instance.UpdateEnergy();
        }
    }
    public void AddCurrentEnergy(int amount)  // 현재 에너지 추가
    {
        if (own_Energy >= max_Energy)
        {
            return;
        }
        own_Energy += amount;

        if(own_Energy > max_Energy)
        {
            own_Energy = max_Energy;
        }

        // 전투 모드인지 확인
        if (!isBattleMode)
        { // UI 업데이트
            UIUpdateManager.Instance.UpdateEnergy();
        }
    }
    public void SubCurrentEnergy(int amount)  // 현재 에너지 소모
    {
        own_Energy -= amount;

        if (own_Energy < max_Energy)
        { 
            // 에너지 타이머 실행
            GameTimeManager.Instance.EnergyTimer();
        }

        // 전투 모드인지 확인
        if (!isBattleMode)
        { // UI 업데이트
            UIUpdateManager.Instance.UpdateEnergy();
        }
    }
    public void AddExtraEnergy(int amount)
    {
        own_Energy += amount;
        
        // 전투 모드인지 확인
        if (!isBattleMode)
        { // UI 업데이트
            UIUpdateManager.Instance.UpdateEnergy();
        }
    }
    // 무기 관련
    public void AddShortWeaponLevel(SWeaponType tr_Weapon, bool isDoubleUp)
    {
        GameObject weaponInfoGroup = PopUpUIManager.Instance.popUpGroups[(int)PopUpType.ShortWeaponInfo];

        switch (tr_Weapon)
        {
            case SWeaponType.Sword:
                {
                    // 실제 코드 (미네랄 조건)
                    // 필요 미네랄 조건 충족 여부
                    if (own_Mineral >= required_Minelral_Sword)
                    {
                        SubMineral(required_Minelral_Sword);

                        // 버튼 사운드 출력
                        SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponLevelUpSound);

                        // 1+1 강화
                        if (isDoubleUp)
                        { 
                            // 강화 로직 두번 반복
                            for (int i=0; i<2; i++)
                            {
                                // 레벨 갱신
                                level_Sword += 1;   // 레벨 추가

                                atk_Sword += SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sword].AtkPerLevel;

                                ptc_Sword += 1;

                                // 필요 미네랄 갱신
                                required_Minelral_Sword += perReq_Sword;
                            }
                        }
                        else
                        { 
                            // 레벨 갱신
                            level_Sword += 1;   // 레벨 추가

                            atk_Sword += SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sword].AtkPerLevel;

                            ptc_Sword += 1;

                            // 필요 미네랄 갱신
                            required_Minelral_Sword += perReq_Sword;
                        }
                        // 무기 레벨업 퀘스트 적용
                        if (QuestManager.Instance.dailyQuestList[(int)DailyQuestType.WeaponLevelUp].myState == QuestButtonState.Proceed)
                        {
                            QuestManager.Instance.UpdateDailyQuest(DailyQuestType.WeaponLevelUp);
                        }

                        if (s_Weapontype == SWeaponType.Sword)
                        {
                            // 현재 장착한 무기일 경우 스텟 UI 업데이트
                            equipedWeaponInfo.UpdateStat();
                        }
                    }
                    else
                    {
                        // 미네랄 양 부족 이벤트
                        Debug.Log(" 미네랄 부족 ");
                    }
                    break;
                }
            case SWeaponType.Hammer:
                {
                    // 필요 미네랄 조건 충족 여부
                    if (own_Mineral >= required_Minelral_Hammer)
                    {
                        SubMineral(required_Minelral_Hammer);

                        // 버튼 사운드 출력
                        SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponLevelUpSound);

                        // 1+1 강화
                        if (isDoubleUp)
                        {
                            // 강화 로직 두번 반복
                            for (int i = 0; i < 2; i++)
                            {
                                // 레벨 갱신
                                level_Hammer += 1;   // 레벨 추가

                                atk_Hammer += SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Hammer].AtkPerLevel;

                                ptc_Hammer += 1;

                                // 필요 미네랄 갱신
                                required_Minelral_Hammer = defaultReq_Hammer + (level_Hammer - 1) * perReq_Hammer;
                            }
                        }
                        else
                        {
                            // 레벨 갱신
                            level_Hammer += 1;   // 레벨 추가

                            atk_Hammer += SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Hammer].AtkPerLevel;

                            ptc_Hammer += 1;

                            // 필요 미네랄 갱신
                            required_Minelral_Hammer = defaultReq_Hammer + (level_Hammer - 1) * perReq_Hammer;
                        }
                        // 무기 레벨업 퀘스트 적용
                        if (QuestManager.Instance.dailyQuestList[(int)DailyQuestType.WeaponLevelUp].myState == QuestButtonState.Proceed)
                        {
                            QuestManager.Instance.UpdateDailyQuest(DailyQuestType.WeaponLevelUp);
                        }

                        if (s_Weapontype == SWeaponType.Hammer)
                        {
                            // 현재 장착한 무기일 경우 스텟 UI 업데이트
                            equipedWeaponInfo.UpdateStat();
                        }
                    }
                    else
                    {
                        // 미네랄 양 부족 이벤트
                        Debug.Log(" 미네랄 부족 ");
                    }
                    break;
                }
            case SWeaponType.Sycthe:
                {
                    // 실제 코드 (미네랄 조건)
                    // 필요 미네랄 조건 충족 여부
                    if (own_Mineral >= required_Minelral_Scythe)
                    {
                        SubMineral(required_Minelral_Scythe);

                        // 버튼 사운드 출력
                        SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponLevelUpSound);

                        // 1+1 강화
                        if (isDoubleUp)
                        {
                            // 강화 로직 두번 반복
                            for (int i = 0; i < 2; i++)
                            {
                                level_Scythe += 1;   // 레벨 추가

                                atk_Scythe += SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sycthe].AtkPerLevel;

                                ptc_Scythe += 1;

                                // 필요 미네랄 갱신
                                required_Minelral_Scythe = defaultReq_Scythe + (level_Scythe - 1) * perReq_Scythe;
                            }
                        }
                        else
                        {
                            level_Scythe += 1;   // 레벨 추가

                            atk_Scythe += SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sycthe].AtkPerLevel;

                            ptc_Scythe += 1;


                            // 필요 미네랄 갱신
                            required_Minelral_Scythe = defaultReq_Scythe + (level_Scythe - 1) * perReq_Scythe;
                        }
                        // 무기 레벨업 퀘스트 적용
                        if (QuestManager.Instance.dailyQuestList[(int)DailyQuestType.WeaponLevelUp].myState == QuestButtonState.Proceed)
                        {
                            QuestManager.Instance.UpdateDailyQuest(DailyQuestType.WeaponLevelUp);
                        }

                        if (s_Weapontype == SWeaponType.Sycthe)
                        {
                            // 현재 장착한 무기일 경우 스텟 UI 업데이트
                            equipedWeaponInfo.UpdateStat();
                        }

                    }
                    else
                    {
                        // 미네랄 양 부족 이벤트
                        Debug.Log(" 미네랄 부족 ");
                    }
                    break;
                }
            case SWeaponType.Fire:
                {
                    // 실제 코드 (미네랄 조건)
                    // 필요 미네랄 조건 충족 여부
                    if (own_Mineral >= required_Minelral_Flamethrower)
                    {
                        SubMineral(required_Minelral_Flamethrower);

                        // 버튼 사운드 출력
                        SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponLevelUpSound);

                        // 1+1 강화
                        if (isDoubleUp)
                        {
                            // 강화 로직 두번 반복
                            for (int i = 0; i < 2; i++)
                            {
                                // 레벨 갱신
                                level_Flamethrower += 1;   // 레벨 추가

                                atk_Flamethrower += SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Fire].AtkPerLevel;

                                ptc_Flamethrower += 1;

                                // 필요 미네랄 갱신
                                required_Minelral_Flamethrower = defaultReq_Flamethrower + (level_Flamethrower - 1) * perReq_Flamethrower;
                            }
                        }
                        else
                        {
                            // 레벨 갱신
                            level_Flamethrower += 1;   // 레벨 추가

                            atk_Flamethrower += SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Fire].AtkPerLevel;

                            ptc_Flamethrower += 1;

                            // 필요 미네랄 갱신
                            required_Minelral_Flamethrower = defaultReq_Flamethrower + (level_Flamethrower - 1) * perReq_Flamethrower;
                        }
                        // 무기 레벨업 퀘스트 적용
                        if (QuestManager.Instance.dailyQuestList[(int)DailyQuestType.WeaponLevelUp].myState == QuestButtonState.Proceed)
                        {
                            QuestManager.Instance.UpdateDailyQuest(DailyQuestType.WeaponLevelUp);
                        }

                        if (s_Weapontype == SWeaponType.Fire)
                        {
                            // 현재 장착한 무기일 경우 스텟 UI 업데이트
                            equipedWeaponInfo.UpdateStat();
                        }
                    }
                    else
                    {
                        // 미네랄 양 부족 이벤트
                        Debug.Log(" 미네랄 부족 ");
                    }
                    break;
                }
            default:
                {
                    Debug.Log("-- Weapon Type Error --");
                    break;
                }
        }

        // UI 업데이트
        weaponInfoGroup.GetComponent<WeaponInfo>().UpdateUI(true, (int)tr_Weapon);
    }
    public void AddLongWeaponLevel(LWeaponType tr_Weapon, bool isDoubleUp)
    {
        GameObject weaponInfoGroup = PopUpUIManager.Instance.popUpGroups[(int)PopUpType.LongWeaponInfo];

        switch (tr_Weapon)
        {
            case LWeaponType.Syringe:
                {
                    // 필요 미네랄 조건 충족 여부
                    if (own_Mineral >= required_Minelral_Syringe)
                    {
                        // 미네랄 소모
                        SubMineral(required_Minelral_Syringe);

                        // 버튼 사운드 출력
                        SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponLevelUpSound);

                        // 1+1 강화
                        if (isDoubleUp)
                        {
                            // 강화 로직 두번 반복
                            for (int i = 0; i < 2; i++)
                            {
                                // 레벨 갱신
                                level_Syringe += 1;   // 레벨 추가

                                atk_Syringe += SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Syringe].AtkPerLevel;

                                // 필요 미네랄 갱신
                                required_Minelral_Syringe = defaultReq_Syringe + (level_Syringe - 1) * perReq_Syringe;
                            }
                        }
                        else
                        {
                            // 레벨 갱신
                            level_Syringe += 1;   // 레벨 추가

                            atk_Syringe += SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Syringe].AtkPerLevel;

                            // 필요 미네랄 갱신
                            required_Minelral_Syringe = defaultReq_Syringe + (level_Syringe - 1) * perReq_Syringe;
                        }
                        // 무기 레벨업 퀘스트 적용
                        if (QuestManager.Instance.dailyQuestList[(int)DailyQuestType.WeaponLevelUp].myState == QuestButtonState.Proceed)
                        {
                            QuestManager.Instance.UpdateDailyQuest(DailyQuestType.WeaponLevelUp);
                        }

                        if (l_Weapontype == LWeaponType.Syringe)
                        {
                            // 현재 장착한 무기일 경우 스텟 UI 업데이트
                            equipedWeaponInfo.UpdateStat();
                        }

                        Debug.Log("레벨업");
                    }
                    else
                    {
                        // 미네랄 양 부족 이벤트
                        Debug.Log(" 미네랄 부족 ");
                    }
                    break;
                }
            case LWeaponType.Bow:
                {
                    // 필요 미네랄 조건 충족 여부
                    if (own_Mineral >= required_Minelral_Bow)
                    {
                        // 미네랄 소모
                        SubMineral(required_Minelral_Bow);

                        // 버튼 사운드 출력
                        SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponLevelUpSound);

                        // 1+1 강화
                        if (isDoubleUp)
                        {
                            // 강화 로직 두번 반복
                            for (int i = 0; i < 2; i++)
                            {
                                // 레벨 갱신
                                level_Bow += 1;   // 레벨 추가

                                atk_Bow += SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Bow].AtkPerLevel;

                                // 필요 미네랄 갱신
                                required_Minelral_Bow = defaultReq_Bow + (level_Bow - 1) * perReq_Bow;
                            }
                        }
                        else
                        {
                            // 레벨 갱신
                            level_Bow += 1;   // 레벨 추가

                            atk_Bow += SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Bow].AtkPerLevel;

                            // 필요 미네랄 갱신
                            required_Minelral_Bow = defaultReq_Bow + (level_Bow - 1) * perReq_Bow;
                        }
                        // 무기 레벨업 퀘스트 적용
                        if (QuestManager.Instance.dailyQuestList[(int)DailyQuestType.WeaponLevelUp].myState == QuestButtonState.Proceed)
                        {
                            QuestManager.Instance.UpdateDailyQuest(DailyQuestType.WeaponLevelUp);
                        }

                        if (l_Weapontype == LWeaponType.Bow)
                        {
                            // 현재 장착한 무기일 경우 스텟 UI 업데이트
                            equipedWeaponInfo.UpdateStat();
                        }
                    }
                    else
                    {
                        // 미네랄 양 부족 이벤트
                        Debug.Log(" 미네랄 부족 ");
                    }
                    break;
                }
            case LWeaponType.Gun:
                {                   
                    // 필요 미네랄 조건 충족 여부
                    if (own_Mineral >= required_Minelral_Gun)
                    {
                        SubMineral(required_Minelral_Gun);

                        // 버튼 사운드 출력
                        SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponLevelUpSound);

                        // 1+1 강화
                        if (isDoubleUp)
                        {
                            // 강화 로직 두번 반복
                            for (int i = 0; i < 2; i++)
                            {
                                level_Gun += 1;   // 레벨 추가

                                atk_Gun += SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Gun].AtkPerLevel;

                                // 필요 미네랄 갱신
                                required_Minelral_Gun = defaultReq_Gun + (level_Gun - 1) * perReq_Gun;
                            }
                        }
                        else
                        {
                            level_Gun += 1;   // 레벨 추가

                            atk_Gun += SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Gun].AtkPerLevel;

                            // 필요 미네랄 갱신
                            required_Minelral_Gun = defaultReq_Gun + (level_Gun - 1) * perReq_Gun;
                        }
                        // 무기 레벨업 퀘스트 적용
                        if (QuestManager.Instance.dailyQuestList[(int)DailyQuestType.WeaponLevelUp].myState == QuestButtonState.Proceed)
                        {
                            QuestManager.Instance.UpdateDailyQuest(DailyQuestType.WeaponLevelUp);
                        }

                        if (l_Weapontype == LWeaponType.Gun)
                        {
                            // 현재 장착한 무기일 경우 스텟 UI 업데이트
                            equipedWeaponInfo.UpdateStat();
                        }
                    }
                    else
                    {
                        // 미네랄 양 부족 이벤트
                        Debug.Log(" 미네랄 부족 ");
                    }

                    break;
                }
            case LWeaponType.Rifle:
                {
                    // 필요 미네랄 조건 충족 여부
                    if (own_Mineral >= required_Minelral_RifleGun)
                    {
                        SubMineral(required_Minelral_RifleGun);

                        // 버튼 사운드 출력
                        SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponLevelUpSound);

                        // 1+1 강화
                        if (isDoubleUp)
                        {
                            // 강화 로직 두번 반복
                            for (int i = 0; i < 2; i++)
                            {
                                // 레벨 갱신
                                level_RifleGun += 1;   // 레벨 추가

                                atk_RifleGun += SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Rifle].AtkPerLevel;

                                // 필요 미네랄 갱신
                                required_Minelral_RifleGun = defaultReq_RifleGun + (level_RifleGun - 1) * perReq_RifleGun;
                            }
                        }
                        else
                        {
                            // 레벨 갱신
                            level_RifleGun += 1;   // 레벨 추가

                            atk_RifleGun += SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Rifle].AtkPerLevel;

                            // 필요 미네랄 갱신
                            required_Minelral_RifleGun = defaultReq_RifleGun + (level_RifleGun - 1) * perReq_RifleGun;
                        }

                        // 무기 레벨업 퀘스트 적용
                        if (QuestManager.Instance.dailyQuestList[(int)DailyQuestType.WeaponLevelUp].myState == QuestButtonState.Proceed)
                        {
                            QuestManager.Instance.UpdateDailyQuest(DailyQuestType.WeaponLevelUp);
                        }

                        if (l_Weapontype == LWeaponType.Rifle)
                        {
                            // 현재 장착한 무기일 경우 스텟 UI 업데이트
                            equipedWeaponInfo.UpdateStat();
                        }
                    }
                    else
                    {
                        // 미네랄 양 부족 이벤트
                        Debug.Log(" 미네랄 부족 ");
                    }
                    break;
                }
            default:
                break;
        }


        Debug.Log("ui 업데이트");
        // UI 업데이트
        weaponInfoGroup.GetComponent<WeaponInfo>().UpdateUI(false, (int)tr_Weapon);
    }

    // 무기 공격력, 공격속도, 탄 개수 반환
    public ST_WeaponStat GetWeaponStat(bool isShort, int weaponNum)
    {
        // !!!!!!ptc : projectile cound 기획 나오면 수정!!!!!!

        ST_WeaponStat stat = new ST_WeaponStat(0, 0, 0, 0, 0,0);

        if(isShort)
        {
            // 근거리 무기
            switch(weaponNum)
            {
                case 1:
                    {
                        stat.level = level_Sword;
                        stat.atk = atk_Sword;
                        stat.dps = dps_Sword;
                        stat.ptc = ptc_Sword;
                        stat.knockBack = knockBackVal_Sword;
                        break;
                    }
                case 2:
                    {
                        stat.level = level_Hammer;
                        stat.atk = atk_Hammer;
                        stat.dps = dps_Hammer;
                        stat.ptc = ptc_Hammer;
                        stat.knockBack = knockBackVal_Hammer;
                        break;
                    }
                case 3:
                    {
                        stat.level = level_Scythe;
                        stat.atk = atk_Scythe;
                        stat.dps = dps_Scythe;
                        stat.ptc = ptc_Scythe;
                        stat.knockBack = knockBackVal_Scythe;
                        break;
                    }
                case 4:
                    {
                        stat.level = level_Flamethrower;
                        stat.atk = atk_Flamethrower;
                        stat.dps = dps_Flamethrower;
                        stat.ptc = ptc_Flamethrower;
                        stat.knockBack = knockBackVal_Flamethrower;
                        break;
                    }
            }
            return stat;
        }
        else
        {
            switch (weaponNum)
            {
                case 1:
                    {
                        stat.level = level_Syringe;
                        stat.atk = atk_Syringe;
                        stat.dps = dps_Syringe;
                        stat.pts = pts_Syringe;
                        stat.ptc = ptc_Syringe;
                        stat.knockBack = knockBackVal_Syringe;
                        break;
                    }
                case 2:
                    {
                        stat.level = level_Bow;
                        stat.atk = atk_Bow;
                        stat.dps = dps_Bow;
                        stat.pts = pts_Bow;
                        stat.ptc = ptc_Bow;
                        stat.knockBack = knockBackVal_Bow;
                        break;
                    }
                case 3:
                    {
                        stat.level = level_Gun;
                        stat.atk = atk_Gun;
                        stat.dps = dps_Gun;
                        stat.pts = pts_Gun;
                        stat.ptc = ptc_Gun;
                        stat.knockBack = knockBackVal_Gun;
                        break;
                    }
                case 4:
                    {
                        stat.level = level_RifleGun;
                        stat.atk = atk_RifleGun;
                        stat.dps = dps_RifleGun;
                        stat.pts = pts_RifleGun;
                        stat.ptc= ptc_RifleGun;
                        stat.knockBack = knockBackVal_RifleGun;
                        break;
                    }
            }
            return stat;

        }
    }

    public ST_WeaponRequireCoin GetRequireCoin(bool isShort, int weaponNum)
    {
        ST_WeaponRequireCoin req = new ST_WeaponRequireCoin(0, 0);

        if (isShort)
        {
            // 근거리 무기
            switch (weaponNum)
            {
                case 1:
                    {
                        req.defaultReq = defaultReq_Sword;
                        req.perReq = perReq_Sword;
                        break;
                    }
                case 2:
                    {
                        req.defaultReq = defaultReq_Hammer;
                        req.perReq = perReq_Hammer;
                        break;
                    }
                case 3:
                    {
                        req.defaultReq = defaultReq_Scythe;
                        req.perReq = perReq_Scythe;
                        break;
                    }
                case 4:
                    {
                        req.defaultReq = defaultReq_Flamethrower;
                        req.perReq = perReq_Flamethrower;
                        break;
                    }
            }
            return req;
        }
        else
        {
            switch (weaponNum)
            {
                case 1:
                    {
                        req.defaultReq = defaultReq_Syringe;
                        req.perReq = perReq_Syringe;
                        break;
                    }
                case 2:
                    {
                        req.defaultReq = defaultReq_Bow;
                        req.perReq = perReq_Bow;
                        break;
                    }
                case 3:
                    {
                        req.defaultReq = defaultReq_Gun;
                        req.perReq = perReq_Gun;
                        break;
                    }
                case 4:
                    {
                        req.defaultReq = defaultReq_RifleGun;
                        req.perReq = perReq_RifleGun;
                        break;
                    }
            }
            return req;

        }

    }
    
    public void ResetWeapon(bool isShort, int weaponNum)
    {
        SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponLevelUpSound);

        if (isShort)
        {
            // 근거리 무기
            switch (weaponNum)
            {
                case 1:
                    {
                        level_Sword = 1;
                        atk_Sword = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sword].DefaultAtk;
                        dps_Sword = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sword].DefaultDps;
                        ptc_Sword = 1;
                        knockBackVal_Sword = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sword].KnockBackValue;
                        required_Minelral_Sword = defaultReq_Sword;

                        if (s_Weapontype == (SWeaponType)weaponNum)
                        {
                            // 현재 장착한 무기일 경우 스텟 UI 업데이트
                            equipedWeaponInfo.UpdateStat();
                        }
                        break;
                    }
                case 2:
                    {
                        level_Hammer = 1;
                        atk_Hammer = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Hammer].DefaultAtk;
                        dps_Hammer = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Hammer].DefaultDps;
                        ptc_Hammer = 1;
                        knockBackVal_Hammer = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Hammer].KnockBackValue;
                        required_Minelral_Hammer = defaultReq_Hammer;

                        if (s_Weapontype == (SWeaponType)weaponNum)
                        {
                            // 현재 장착한 무기일 경우 스텟 UI 업데이트
                            equipedWeaponInfo.UpdateStat();
                        }
                        break;
                    }
                case 3:
                    {
                        level_Scythe = 1;
                        atk_Scythe = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sycthe].DefaultAtk;
                        dps_Scythe = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sycthe].DefaultDps;
                        ptc_Scythe = 1;
                        knockBackVal_Scythe = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sycthe].KnockBackValue;
                        required_Minelral_Scythe = defaultReq_Scythe;

                        if (s_Weapontype == (SWeaponType)weaponNum)
                        {
                            // 현재 장착한 무기일 경우 스텟 UI 업데이트
                            equipedWeaponInfo.UpdateStat();
                        }
                        break;
                    }
                case 4:
                    {
                        level_Flamethrower = 1;
                        atk_Flamethrower = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Fire].DefaultAtk;
                        dps_Flamethrower = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Fire].DefaultDps;
                        ptc_Flamethrower = 1;
                        knockBackVal_Flamethrower = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Fire].KnockBackValue;
                        required_Minelral_Flamethrower = defaultReq_Flamethrower;

                        if (s_Weapontype == (SWeaponType)weaponNum)
                        {
                            // 현재 장착한 무기일 경우 스텟 UI 업데이트
                            equipedWeaponInfo.UpdateStat();
                        }
                        break;
                    }
                default: return;
            }
            return;
        }
        else
        {
            switch (weaponNum)
            {
                case 1:
                    {
                        level_Syringe = 1;
                        atk_Syringe = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Syringe].DefaultAtk;
                        dps_Syringe = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Syringe].DefaultDps;
                        pts_Syringe = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Syringe].DefaultPts;
                        ptc_Syringe = 1;
                        knockBackVal_Syringe = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Syringe].KnockBackValue;
                        required_Minelral_Syringe = defaultReq_Syringe;

                        if (l_Weapontype == (LWeaponType)weaponNum)
                        {
                            // 현재 장착한 무기일 경우 스텟 UI 업데이트
                            equipedWeaponInfo.UpdateStat();
                        }
                        break;
                    }
                case 2:
                    {
                        level_Bow = 1;
                        atk_Bow = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Bow].DefaultAtk;
                        dps_Bow = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Bow].DefaultDps;
                        pts_Bow = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Bow].DefaultPts;
                        ptc_Bow = 2;
                        knockBackVal_Bow = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Bow].KnockBackValue;
                        required_Minelral_Bow = defaultReq_Bow;

                        if (l_Weapontype == (LWeaponType)weaponNum)
                        {
                            // 현재 장착한 무기일 경우 스텟 UI 업데이트
                            equipedWeaponInfo.UpdateStat();
                        }

                        break;
                    }
                case 3:
                    {
                        level_Gun = 1;
                        atk_Gun = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Gun].DefaultAtk;
                        dps_Gun = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Gun].DefaultDps;
                        pts_Gun = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Gun].DefaultPts;
                        ptc_Gun = 1;
                        knockBackVal_Gun = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Gun].KnockBackValue;
                        required_Minelral_Gun = defaultReq_Gun;

                        if (l_Weapontype == (LWeaponType)weaponNum)
                        {
                            // 현재 장착한 무기일 경우 스텟 UI 업데이트
                            equipedWeaponInfo.UpdateStat();
                        }
                        break;
                    }
                case 4:
                    {
                        level_RifleGun = 1;
                        atk_RifleGun = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Rifle].DefaultAtk;
                        dps_RifleGun = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Rifle].DefaultDps;
                        pts_RifleGun = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Rifle].DefaultPts;
                        ptc_RifleGun = 1;
                        knockBackVal_RifleGun = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Rifle].KnockBackValue;
                        required_Minelral_RifleGun = defaultReq_RifleGun;

                        if (l_Weapontype == (LWeaponType)weaponNum)
                        {
                            // 현재 장착한 무기일 경우 스텟 UI 업데이트
                            equipedWeaponInfo.UpdateStat();
                        }
                        break;
                    }
                default: return;
            }
            return;
        }

    }
}

// 제이슨 직렬화를 위한 데이터 클래스
// Monobehaviour 상속 없이 데이터만을 가지는 클래스
public class GameData_Json
{
    #region 우주선 스텟

    public int iSpaceShipLevel;

    #endregion

    #region 유저 스텟

    public string sUserName;
    public bool bUsedCoupon1;
    public bool bUsedCoupon2;
    public bool bUsedCoupon3;
    public bool bReviewCheck;
    public int iAdsDiaCount;
    public int iAdsEnergyCount;
    public int iAdsRevivalCount;

    public int iPlayerLevel;
    public float fPlayerEXP;
    public float fMineral;
    public float fDia;
    public int iOwnEnergy;
    public int iMaxEnergy;

    #endregion

    #region 무기 스텟

    public int iSwordLevel;
    public int iHammerLevel;
    public int iScytheLevel;
    public int iFlamethrowerLevel;

    public int iSyringeLevel;
    public int iBowLevel;
    public int iGunLevel;
    public int iRifleGunLevel;

    public bool[] bArrLongWeaponUnLockInfo;
    public bool[] bArrShortWeaponUnLockInfo;

    public int iIsEquipedShortWeaponNum;
    public int iIsEquipedLongWeaponNum;

    #endregion

    #region 식물 스텟

    public bool[] bArrPlantSlotUnLockInfo;

    public int[] iArrHarvestCount;

    #endregion

    #region 배치한 동물
    public List<int> iReleaseStageAnimalList = new List<int>();
    public List<int> iReleaseCashAnimalList = new List<int>();
    #endregion

    #region 시간 관련

    public int iFirstLogInTime;
    public int iGameEndTime;
    public int iNextDayTime;
    public int iDayCount;
    public bool bIsEnergyTimer;
    public int iEnergyTimerStartTime;
    #endregion

    #region 아이템
    public int iMaxReleaseAnimal;
    public int iPlantItem;
    public int iShortWeaponItem;
    public int iLongWeaponItem;
    #endregion

    public GameData_Json()
    {
        iSpaceShipLevel = StatManager.Instance.Level_SpaceShip;

        sUserName = StatManager.Instance.UserName;
        bUsedCoupon1 = StatManager.Instance.usedCoupon1;
        bUsedCoupon2 = StatManager.Instance.usedCoupon2;
        bUsedCoupon3 = StatManager.Instance.usedCoupon3;
        bReviewCheck = StatManager.Instance.reviewCheck;

        iAdsDiaCount = StatManager.Instance.adsDiaCount;
        iAdsEnergyCount = StatManager.Instance.adsEnergyCount;
        iAdsRevivalCount = StatManager.Instance.adsRevivalCount;
    
        iPlayerLevel = StatManager.Instance.Level_Player;
        fPlayerEXP = StatManager.Instance.Exp_Player;
        fMineral = StatManager.Instance.Own_Mineral;
        fDia = StatManager.Instance.Own_Dia;
        iOwnEnergy = StatManager.Instance.Own_Energy;
        iMaxEnergy = StatManager.Instance.Max_Energy;

        iSwordLevel = StatManager.Instance.Level_Sword;
        iHammerLevel = StatManager.Instance.Level_Hammer;
        iScytheLevel = StatManager.Instance.Level_Scythe;
        iFlamethrowerLevel = StatManager.Instance.Level_Flamethrower;

        iSyringeLevel = StatManager.Instance.Level_Syringe;
        iBowLevel = StatManager.Instance.Level_Bow;
        iGunLevel = StatManager.Instance.Level_Gun;
        iRifleGunLevel = StatManager.Instance.Level_RifleGun;

        bArrLongWeaponUnLockInfo = SlotManager.Instance.info_LongWeaponUnLock;
        bArrShortWeaponUnLockInfo = SlotManager.Instance.info_ShortWeaponUnLock;
        iIsEquipedShortWeaponNum = (int)StatManager.Instance.s_Weapontype;
        iIsEquipedLongWeaponNum = (int)StatManager.Instance.l_Weapontype;

        bArrPlantSlotUnLockInfo = SlotManager.Instance.info_PlantSlot;
        iArrHarvestCount = SlotManager.Instance.harvestCount;

        iFirstLogInTime = GameTimeManager.Instance.firstLogInTime;
        iGameEndTime = GameTimeManager.Instance.startWebTime + GameTimeManager.Instance.gamePlayTime;
        iNextDayTime = GameTimeManager.Instance.nextDayTime;
        iDayCount = GameTimeManager.Instance.dayCounting;
        bIsEnergyTimer = GameTimeManager.Instance.isEnergyTimer;
        iEnergyTimerStartTime = GameTimeManager.Instance.energyTimerStartTime;

        iReleaseStageAnimalList = SlotManager.Instance.releaseStageAnimalList;
        iReleaseCashAnimalList = SlotManager.Instance.releaseCashAnimalList;

        iMaxReleaseAnimal = SlotManager.Instance.maxReleaseAnimal;
        iPlantItem = SlotManager.Instance.plantItem;
        iShortWeaponItem = SlotManager.Instance.shortWeaponItem;
        iLongWeaponItem = SlotManager.Instance.longWeaponItem;  
    }
}
