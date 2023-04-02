using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
// ���Ÿ� Ÿ��
public enum LWeaponType
{
    Idle=0,     // �������� ���� ����
    Syringe,    // �ֻ��
    Bow,        // Ȱ
    Gun,        // ��
    Rifle,      // ������
}

// ���� Ÿ��
public enum SWeaponType
{
    Idle=0, // �������� ���� ����
    Sword,  // ��
    Hammer, // ��ġ
    Sycthe, // ��
    Fire    // ȭ������
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
    #region �̱���
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
        // �� ����ÿ��� �ı����� �ʴ´�.
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    // ���� ���� ������
    // ���ּ�, ĳ����, �̳׶� �� �������� ������ �����Ѵ�.
    #region ���ּ� ����
    [Header("���ּ� ����")]
    [SerializeField]
    private int level_SpaceShip;    // ���ּ� ����
    public int Level_SpaceShip { get { return level_SpaceShip; } }
    #endregion

    #region �÷��̾� ����

    [Header("���� �̸�")]
    [SerializeField]
    private string userName;
    public string UserName { get { return userName; } }

    [Header("�÷��̾� ����")]
    [SerializeField]
    private int level_Player;   // �÷��̾� ����
    public int Level_Player { get { return level_Player; } }

    [Header("10���� �÷��̾� ����ġ ����Ʈ")]
    [SerializeField]
    private int[] exp_PlayerList;   // �÷��̾� ����ġ ����Ʈ

    [SerializeField]
    private float exp_Player;     // �÷��̾� ����ġ
    public float Exp_Player { get { return exp_Player; } }

    [SerializeField]
    private float spd_Player;     // �÷��̾� �̵��ӵ�
    public float Spd_Player { get { return spd_Player; } }

    [SerializeField]
    private float hp_Player;      // �÷��̾� ü��
    public float Hp_Player
    { get { return hp_Player; } }

    public bool usedCoupon1 = false;
    public bool usedCoupon2 = false;
    public bool usedCoupon3 = false;
    public bool reviewCheck = false;
    
    public int adsDiaCount = 3;
    public int adsEnergyCount = 3;
    public int adsRevivalCount = 3;

    [Header("0:���̾� 1:������")]
    public Modules.Ads.AdsPlayButton[] adsPlayButtons;
    #endregion

    // ���� ���� ������ json ������ ���� �� ���� ������ ����.
    #region ���� ����

    // ���� ���� : ���ݷ�(ATK), �����(DPS), ����(ATR)
    #region ���� ���� ����

    #region �� ����
    [Header("�� ����")]
    [Space(10)]

    [Header("����")]
    [SerializeField]

    private int level_Sword;   // �� ����
    public int Level_Sword { get { return level_Sword; } }

    [Header("���ݷ�")]
    [SerializeField]
    private float atk_Sword;    // �� ���ݷ�
    public float Atk_Sword { get { return atk_Sword; } }

    [Header("���ݼӵ�")]
    [SerializeField]
    private float dps_Sword;    // �� �����   
    public float Dps_Sword { get { return dps_Sword; } }

    [Header("�߻簳��")]
    [SerializeField]
    private int ptc_Sword;
    public int Ptc_Sword{get{return ptc_Sword;}}

    [Header("�˹� ��ġ")]
    [SerializeField]
    private int knockBackVal_Sword;
    public int KnockBackVal_Sword { get { return knockBackVal_Sword; } }

    [Header("�������� �ʿ� �̳׶�")]
    [SerializeField]
    private float required_Minelral_Sword;  // ������ �� �ʿ� �̳׶� ��
    public float Required_Minelral_Sword { get { return required_Minelral_Sword; } }
    #endregion

    [Space(10)]

    #region ��ġ ����
    [Header("��ġ ����")]
    [Space(10)]

    [Header("����")]
    [SerializeField]
    private int level_Hammer;   // ��ġ ����
    public int Level_Hammer { get { return level_Hammer; } }

    [Header("���ݷ�")]
    [SerializeField]
    private float atk_Hammer;    // ��ġ ���ݷ�
    public float Atk_Hammer { get { return atk_Hammer; } }

    [Header("���ݼӵ�")]
    [SerializeField]
    private float dps_Hammer;    // ��ġ �����   
    public float Dps_Hammer { get { return dps_Hammer; } }

    [Header("�߻簳��")]
    [SerializeField]
    private int ptc_Hammer;
    public int Ptc_Hammer { get { return ptc_Hammer; } }

    [Header("�������� �ʿ� �̳׶�")]
    [SerializeField]
    private float required_Minelral_Hammer;  // ������ �� �ʿ� �̳׶� ��
    public float Required_Minelral_Hammer { get { return required_Minelral_Hammer; } }

    [Header("�˹� ��ġ")]
    [SerializeField]
    private int knockBackVal_Hammer;
    public int KnockBackVal_Hammer { get { return knockBackVal_Hammer; } }

    #endregion

    [Space(10)]

    #region �보 ����
    [Header("�보 ����")]
    [Space(10)]

    [Header("����")]
    [SerializeField]
    private int level_Scythe;   // �보 ����
    public int Level_Scythe { get { return level_Scythe; } }

    [Header("���ݷ�")]
    [SerializeField]
    private float atk_Scythe;    // �보 ���ݷ�
    public float Atk_Scythe { get { return atk_Scythe; } }

    [Header("���ݼӵ�")]
    [SerializeField]
    private float dps_Scythe;    // �보 �����   
    public float Dps_Scythe { get { return dps_Scythe; } }

    [Header("�߻簳��")]
    [SerializeField]
    private int ptc_Scythe;
    public int Ptc_Scythe { get { return ptc_Scythe; } }

    [Header("�������� �ʿ� �̳׶�")]
    [SerializeField]
    private float required_Minelral_Scythe;  // ������ �� �ʿ� �̳׶� ��
    public float Required_Minelral_Scythe { get { return required_Minelral_Scythe; } }

    [Header("�˹� ��ġ")]
    [SerializeField]
    private int knockBackVal_Scythe;
    public int KnockBackVal_Scythe { get { return knockBackVal_Scythe; } }

    #endregion

    [Space(10)]

    #region ȭ������ ����
    [Header("ȭ������ ����")]
    [Space(10)]

    [Header("����")]
    [SerializeField]
    private int level_Flamethrower;   // ȭ������ ����
    public int Level_Flamethrower { get { return level_Flamethrower; } }
    
    [Header("���ݷ�")]
    [SerializeField]
    private float atk_Flamethrower;    // ȭ������ ���ݷ�
    public float Atk_Flamethrower { get { return atk_Flamethrower; } }

    [Header("���ݼӵ�")]
    [SerializeField]
    private float dps_Flamethrower;    // ȭ������ �����   
    public float Dps_Flamethrower { get { return dps_Flamethrower; } }

    [Header("�߻簳��")]
    [SerializeField]
    private int ptc_Flamethrower;
    public int Ptc_Flamethrower { get { return ptc_Flamethrower; } }

    [Header("�������� �ʿ� �̳׶�")]
    [SerializeField]
    private float required_Minelral_Flamethrower;  // ������ �� �ʿ� �̳׶� ��
    public float Required_Minelral_Flamethrower { get { return required_Minelral_Flamethrower; } }

    [Header("�˹� ��ġ")]
    [SerializeField]
    private int knockBackVal_Flamethrower;
    public int KnockBackVal_Flamethrower { get { return knockBackVal_Flamethrower; } }

    #endregion

    [Space(10)]


    #endregion

    // ���Ÿ� ���� : ���ݷ�(ATK), �����(DPS), ����ü�ӵ�(PTS), ����(ATR)
    #region ���Ÿ� ���� ����

    #region �ֻ�� ����
    [Header("�ֻ�� ����")]
    [Space(10)]

    [Header("����")]
    [SerializeField]
    private int level_Syringe;   // �ֻ�� ����
    public int Level_Syringe { get { return level_Syringe; } }

    [Header("���ݷ�")]
    [SerializeField]
    private float atk_Syringe;    // �ֻ�� ���ݷ�
    public float Atk_Syringe { get { return atk_Syringe; } }

    [Header("���ݼӵ�")]
    [SerializeField]
    private float dps_Syringe;    // �ֻ�� �����   
    public float Dps_Syringe { get { return dps_Syringe; } }

    [Header("����ü�ӵ�")]
    [SerializeField]
    private float pts_Syringe;    // �ֻ�� ����ü �ӵ� 
    public float Pts_Syringe { get { return pts_Syringe; } }

    [Header("�߻簳��")]
    [SerializeField]
    private int ptc_Syringe;
    public int Ptc_Syringe { get { return ptc_Syringe; } }

    [Header("�������� �ʿ� �̳׶�")]
    [SerializeField]
    private float required_Minelral_Syringe;  // ������ �� �ʿ� �̳׶� ��
    public float Required_Minelral_Syringe { get { return required_Minelral_Syringe; } }

    [Header("�˹� ��ġ")]
    [SerializeField]
    private int knockBackVal_Syringe;
    public int KnockBackVal_Syringe { get { return knockBackVal_Syringe; } }

    #endregion

    [Space(10)]

    #region Ȱ ����
    [Header("Ȱ ����")]
    [Space(10)]

    [Header("����")]
    [SerializeField]
    private int level_Bow;   // Ȱ ����
    public int Level_Bow { get { return level_Bow; } }

    [Header("���ݷ�")]
    [SerializeField]
    private float atk_Bow;    // Ȱ ���ݷ�
    public float Atk_Bow { get { return atk_Bow; } }

    [Header("���ݼӵ�")]
    [SerializeField]
    private float dps_Bow;    // Ȱ �����   
    public float Dps_Bow { get { return dps_Bow; } }

    [Header("����ü�ӵ�")]
    [SerializeField]
    private float pts_Bow;    // Ȱ ����ü �ӵ� 
    public float Pts_Bow { get { return pts_Bow; } }

    [Header("�߻簳��")]
    [SerializeField]
    private int ptc_Bow;
    public int Ptc_Bow { get { return ptc_Bow; } }

    [Header("�������� �ʿ� �̳׶�")]
    [SerializeField]
    private float required_Minelral_Bow;  // ������ �� �ʿ� �̳׶� ��
    public float Required_Minelral_Bow { get { return required_Minelral_Bow; } }

    [Header("�˹� ��ġ")]
    [SerializeField]
    private int knockBackVal_Bow;
    public int KnockBackVal_Bow { get { return knockBackVal_Bow; } }

    #endregion

    [Space(10)]

    #region �� ����
    [Header("�� ����")]
    [Space(10)]

    [Header("����")]
    [SerializeField]
    private int level_Gun;   // �� ����
    public int Level_Gun { get { return level_Gun; } }
    
    [Header("���ݷ�")]
    [SerializeField]
    private float atk_Gun;    // �� ���ݷ�
    public float Atk_Gun { get { return atk_Gun; } }

    [Header("���ݼӵ�")]
    [SerializeField]
    private float dps_Gun;    // �� �����   
    public float Dps_Gun { get { return dps_Gun; } }

    [Header("����ü�ӵ�")]
    [SerializeField]
    private float pts_Gun;    // �� ����ü �ӵ� 
    public float Pts_Gun { get { return pts_Gun; } }

    [Header("�߻簳��")]
    [SerializeField]
    private int ptc_Gun;
    public int Ptc_Gun { get { return ptc_Gun; } }

    [Header("�������� �ʿ� �̳׶�")]
    [SerializeField]
    private float required_Minelral_Gun;  // ������ �� �ʿ� �̳׶� ��
    public float Required_Minelral_Gun { get { return required_Minelral_Gun; } }

    [Header("�˹� ��ġ")]
    [SerializeField]
    private int knockBackVal_Gun;
    public int KnockBackVal_Gun { get { return knockBackVal_Gun; } }

    #endregion

    [Space(10)]

    #region �����ð� ����
    [Header("�����ð� ����")]
    [Space(10)]

    [Header("����")]
    [SerializeField]
    private int level_RifleGun;   // �����ð� ����
    public int Level_RifleGun { get { return level_RifleGun; } }

    [Header("���ݷ�")]
    [SerializeField]
    private float atk_RifleGun;    // �����ð� ���ݷ�
    public float Atk_RifleGun { get { return atk_RifleGun; } }

    [Header("���ݼӵ�")]
    [SerializeField]
    private float dps_RifleGun;    // �����ð� �����   
    public float Dps_RifleGun { get { return dps_RifleGun; } }

    [Header("����ü�ӵ�")]
    [SerializeField]
    private float pts_RifleGun;
    public float Pts_RifleGun { get { return pts_RifleGun; } }

    [Header("�߻簳��")]
    [SerializeField]
    private int ptc_RifleGun;
    public int Ptc_RifleGun { get { return ptc_RifleGun; } }

    [Header("�������� �ʿ� �̳׶�")]
    [SerializeField]
    private float required_Minelral_RifleGun;  // ������ �� �ʿ� �̳׶� ��
    public float Required_Minelral_RifleGun { get { return required_Minelral_RifleGun; } }

    [Header("�˹� ��ġ")]
    [SerializeField]
    private int knockBackVal_RifleGun;
    public int KnockBackVal_RifleGun { get { return knockBackVal_RifleGun; } }

    #endregion

    [Space(10)]

    #endregion

    #region ���� ������ �ʿ� ��ȭ��
    [Header("���� ù ������ �� �ʿ� ��ȭ��")]
    [Space(10f)]

    public float defaultReq_Sword = 600000f;
    public float defaultReq_Hammer = 640000f;
    public float defaultReq_Scythe = 680000f;
    public float defaultReq_Flamethrower = 720000f;

    public float defaultReq_Syringe = 10000f;
    public float defaultReq_Bow = 15000f;
    public float defaultReq_Gun = 20000f;
    public float defaultReq_RifleGun = 25000f;

    [Header("���� ���� �� �� �ʿ� ��ȭ ������")]
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

    #region ����Ʈ ����
    [Header("��ȭ, ��ġ ���� ����")]
    [SerializeField]
    private float own_Mineral;  // ������ �̳׶� ���� 
    public float Own_Mineral { get { return own_Mineral; } }


    [SerializeField]
    private float own_Dia;      // ������ ���̾�
    public float Own_Dia { get { return own_Dia; } }

    [SerializeField]
    private int max_Energy;   // ������ �ִ�ġ
    public int Max_Energy { get { return max_Energy; } }

    [SerializeField]
    private int own_Energy;  // ������ ������
    public int Own_Energy { get { return own_Energy; } }
    #endregion

    /// <��Ʋ�������?>
    /// ������ ���� ���¿� �����Ͽ��� ��� ������ ����Ǿ� ���� Ȩ���� ���ƿ� ������ �ǽ��ؾ� ��.
    /// �ʱ�ȭ�� false, ���� ��忡 ������ �� true�� ����, ���� ��尡 ����Ǿ� ����ȭ������ ���ƿ� �� false�� ����
    /// </��Ʋ�������?>
    public bool isBattleMode;  // ���� ������ ���� �������? (���� ���������� ������ ��������?)

    #region �б� ���� �ɷ�ġ

    [Header("���ݿ� ���� �ɷ�ġ(�б� ����)")]
    [SerializeField]
    private float mine_MineralPerSeconds;   // �ʴ� ȹ�� �̳׶�
    public float Mine_MineralPerSeconds { get { return mine_MineralPerSeconds; } }

    [SerializeField]
    private float requiredExp;      // ������ �� �ʿ� ����ġ
    public float RequiredExp { get { return requiredExp; } }

    // ����-2 �ʿ� ����ġ
    private float fprevExpReq;
    // ����-1 �ʿ� ����ġ
    private float lprevExpReq;

    #endregion

    public LWeaponType l_Weapontype;    // ���� ������ ���Ÿ� ����
    public SWeaponType s_Weapontype;    // ���� ������ �ٰŸ� ����

    [Header("EquipedWeaponInfo")]
    public EquipedWeaponInfo equipedWeaponInfo;

    void Start()
    {
        LoadStat();
    }

    // �׽�Ʈ ���� ������Ʈ �κ� �����ؾ� ��.
    private void Update()
    {
        /// �׽�Ʈ�� - ������
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EscapePopUp.SetActive(true);
        }
    }

    /// <�׽�Ʈ�� - ������>

    [Space(30)]
    [Header("�����˾�(�ӽ�)")]
    public GameObject EscapePopUp;

    public void EscapeGame()
    {
        GameDataManager.Instance.AutoSave();
        Application.Quit(); // ��������

        print("escape");
    }

    /// <�׽�Ʈ�� - ������>

    // �ʱ� ����
    public void FirstInitialSetting()
    {
        Debug.Log("ù ����");

        // ó�� ������ ���

        #region ���ּ� ����
        level_SpaceShip = 1;
        #endregion

        #region �÷��̾� ����
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
        requiredExp = 10;    // ������ �� �ʿ� ����ġ
    
        #endregion

        #region ���� ����

        #region ���� ����

        // ��
        level_Sword = 1;
        atk_Sword = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sword].DefaultAtk;
        dps_Sword = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sword].DefaultDps;
        ptc_Sword = 1;
        knockBackVal_Sword = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sword].KnockBackValue;
        required_Minelral_Sword = defaultReq_Sword;

        // ��ġ
        level_Hammer = 1;
        atk_Hammer = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Hammer].DefaultAtk;
        dps_Hammer = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Hammer].DefaultDps;
        ptc_Hammer = 1;
        knockBackVal_Hammer = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Hammer].KnockBackValue;
        required_Minelral_Hammer = defaultReq_Hammer;

        // �보
        level_Scythe = 1;
        atk_Scythe = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sycthe].DefaultAtk;
        dps_Scythe = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sycthe].DefaultDps;
        ptc_Scythe = 1;
        knockBackVal_Scythe = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sycthe].KnockBackValue;
        required_Minelral_Scythe = defaultReq_Scythe;

        // ȭ������
        level_Flamethrower = 1;
        atk_Flamethrower = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Fire].DefaultAtk;
        dps_Flamethrower = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Fire].DefaultDps;
        ptc_Flamethrower = 1;
        knockBackVal_Flamethrower = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Fire].KnockBackValue;
        required_Minelral_Flamethrower = defaultReq_Flamethrower;

        #endregion

        #region ���Ÿ� ����

        // �ֻ��
        level_Syringe = 1;
        atk_Syringe = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Syringe].DefaultAtk;
        dps_Syringe = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Syringe].DefaultDps;
        pts_Syringe = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Syringe].DefaultPts;
        ptc_Syringe = 1;
        required_Minelral_Syringe = defaultReq_Syringe;

        // Ȱ
        level_Bow = 1;
        atk_Bow = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Bow].DefaultAtk;
        dps_Bow = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Bow].DefaultDps;
        pts_Bow = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Bow].DefaultPts;
        ptc_Bow = 2;
        required_Minelral_Bow = defaultReq_Bow;

        // ��
        level_Gun = 1;
        atk_Gun = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Gun].DefaultAtk;
        dps_Gun = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Gun].DefaultDps;
        pts_Gun = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Gun].DefaultPts;
        ptc_Gun = 1;
        required_Minelral_Gun = defaultReq_Gun;


        // �����ð�
        level_RifleGun = 1;
        atk_RifleGun = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Rifle].DefaultAtk;
        dps_RifleGun = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Rifle].DefaultDps;
        pts_RifleGun = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Rifle].DefaultPts;
        ptc_RifleGun = 4;
        required_Minelral_RifleGun = defaultReq_RifleGun;

        #endregion

        #endregion

        #region ��ȭ, ���� ���� �ʱ⼼��
        own_Mineral = 0;
        own_Dia = 0;
        max_Energy = 35;
        own_Energy = 35;
        #endregion


        #region ���� ���� ���� ����
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
        Debug.Log("�ε� ����");

        #region ���ּ� ����
        level_SpaceShip = data.iSpaceShipLevel;
        #endregion

        #region �÷��̾� ����
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

            Debug.Log("����: " + Level_Player + " �� �ʿ� ����ġ�� " + requiredExp);
        }
        else
        {
            // ���� �� �� �÷��̾� �ھ� ���� ����
            // 20~40, 40~60, 60~80, 80~ ������ hp �������� ����
            // �� �������� ���� ���� hp ������ ������.
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

            Debug.Log("����: " + Level_Player+" �� �ʿ� ����ġ�� " + requiredExp);
        }

        //requiredExp = (float)(level_Player * 100f);    // ������ �� �ʿ� ����ġ
        #endregion

        #region ���� ����

        #region ���� ����

        // ��
        level_Sword = data.iSwordLevel;
        atk_Sword = Level_Sword * SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sword].AtkPerLevel;
        dps_Sword = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sword].DefaultDps;        
        ptc_Sword = level_Sword;
        required_Minelral_Sword = defaultReq_Sword + (level_Sword-1)*perReq_Sword;
        knockBackVal_Sword = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sword].KnockBackValue;

        // ��ġ
        level_Hammer = data.iHammerLevel;
        atk_Hammer = level_Hammer * SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Hammer].AtkPerLevel;
        dps_Hammer = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Hammer].DefaultDps;
        ptc_Hammer = level_Hammer;
        required_Minelral_Hammer = defaultReq_Hammer + (level_Hammer - 1) * perReq_Hammer;
        knockBackVal_Hammer = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Hammer].KnockBackValue;

        // �보
        level_Scythe = data.iScytheLevel;
        atk_Scythe = level_Scythe * SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sycthe].AtkPerLevel;
        dps_Scythe = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sycthe].DefaultDps;
        ptc_Scythe = level_Scythe;
        required_Minelral_Scythe = defaultReq_Scythe + (level_Scythe - 1) * perReq_Scythe;
        knockBackVal_Scythe = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sycthe].KnockBackValue;

        // ȭ������
        level_Flamethrower = data.iFlamethrowerLevel;
        atk_Flamethrower = level_Flamethrower*SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Fire].DefaultAtk;
        dps_Flamethrower = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Fire].DefaultDps;
        ptc_Flamethrower = level_Flamethrower;        
        required_Minelral_Flamethrower = defaultReq_Flamethrower + (level_Flamethrower - 1) * perReq_Flamethrower;
        knockBackVal_Flamethrower = SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Fire].KnockBackValue;

        #endregion

        #region ���Ÿ� ����

        // �ֻ��
        level_Syringe = data.iSyringeLevel;
        atk_Syringe = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Syringe].DefaultAtk + (level_Syringe - 1) * SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Syringe].AtkPerLevel;

        dps_Syringe = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Syringe].DefaultDps;
        pts_Syringe = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Syringe].DefaultPts;
        ptc_Syringe = 1;

        required_Minelral_Syringe = defaultReq_Syringe + (level_Syringe - 1) * perReq_Syringe;

        // Ȱ
        level_Bow = data.iBowLevel;
        atk_Bow = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Bow].DefaultAtk + (level_Bow - 1) * SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Bow].AtkPerLevel;

        dps_Bow = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Bow].DefaultDps;
        pts_Bow = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Bow].DefaultPts;
        ptc_Bow = 2;


        required_Minelral_Bow = defaultReq_Bow + (level_Bow - 1) * perReq_Bow;

        // ��
        level_Gun = data.iGunLevel;
        atk_Gun = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Gun].DefaultAtk + (level_Gun - 1) * SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Gun].AtkPerLevel;

        dps_Gun = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Gun].DefaultDps;
        pts_Gun = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Gun].DefaultPts;
        ptc_Gun = 1;


        required_Minelral_Gun = defaultReq_Gun + (level_Gun - 1) * perReq_Gun;

        // �����ð�
        level_RifleGun = data.iRifleGunLevel;
        atk_RifleGun = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Rifle].DefaultAtk + (level_RifleGun - 1) * SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Rifle].AtkPerLevel;

        dps_RifleGun = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Rifle].DefaultDps;
        pts_RifleGun = SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Rifle].DefaultPts;
        ptc_RifleGun = 4;

        required_Minelral_RifleGun = defaultReq_RifleGun + (level_RifleGun - 1) * perReq_RifleGun;

        #endregion

        #endregion

        #region ��ȭ, ���� ���� �ε弼��
        own_Mineral = data.fMineral;
        mine_MineralPerSeconds = 1f;  // �ʴ� �̳׶� ȹ�淮
        own_Dia = data.fDia;
        max_Energy = data.iMaxEnergy;
        own_Energy = data.iOwnEnergy;
        #endregion

        #region ���� ���� ���� ����
        s_Weapontype = (SWeaponType)data.iIsEquipedShortWeaponNum;
        l_Weapontype = (LWeaponType)data.iIsEquipedLongWeaponNum;

        #endregion

        GameTimeManager.Instance.InitialSetting(data);
        UserProductManager.Instance.UserProductInitialSetting(data);
        StageManager.Instance.InitialSetting(data);
        SlotManager.Instance.WeaponSlotInitialSetting(false, data);
        SlotManager.Instance.PlantSlotInitialSetting(false, data);
        SlotManager.Instance.AnimalSlotInitialSetting(data);

        // Ʃ�丮�� ���� �� ������ ������ ������ ��� �ٽ� Ʃ�丮�� ����
        // Ʃ�丮���� ���� ����Ǿ��� ��� ������ ������ 2
        if (Level_Player <= 1 && !PopUpUIManager.Instance.firstStartGroup.activeSelf)
        {
            PopUpUIManager.Instance.firstStartGroup.SetActive(true);
            SoundManager.Instance.PlayBackGroundSound(MainSoundType.CutSceneSound);
        }
    }

    // ������ �ε�
    public void LoadStat()
    {
        // ������� �ʱ�ȭ
        isBattleMode = false;

        GameData_Json loadedData = GameDataManager.Instance.LoadData();

        // ������ �Ŵ����� ���ؼ� ����� ���� ������ �ҷ���
        if (loadedData != null)
        {
            LoadedInitialSetting(loadedData);

            UIUpdateManager.Instance.UpdateAll();
        }
        else
        {
            // ����� ���� �����Ͱ� ���� ���
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

    // �÷��̾� ����ġ ����
    public void AddPlayerLevel(int amount)      // �÷��̾� ���� ��
    {
        // ���� ����
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
            // ����ġ ����
            requiredExp = lprevExpReq + (lprevExpReq - fprevExpReq) * 1.1f;   // �ʿ� ����ġ ����
            fprevExpReq = lprevExpReq;
            lprevExpReq = requiredExp;
        }

        exp_Player = 0;     // ����ġ �ʱ�ȭ

        // ���� �� �� �÷��̾� �ھ� ���� ����
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

        // ���� ������� Ȯ��
        if (!isBattleMode)
        { // UI ������Ʈ
            UIUpdateManager.Instance.UpdatePlayerData();

            // ������ �˾�
            PopUpUIManager.Instance.OpenPopUp(PopUpType.LevelUp);

            Debug.Log("����: " + Level_Player + " �� �ʿ� ����ġ�� " + requiredExp);
        }
    }
    public void AddPlayerEXP(float amount)      // �÷��̾� ����ġ ��
    {
        if (amount > 0)
        {
            if (exp_Player + amount < requiredExp)   // �ʿ� ����ġ ���� ���� ���
            {
                // ����ġ �߰�
                exp_Player += amount;
            }
            else                                    // ����ġ�� �ʰ��� ���
            {
                AddPlayerLevel(1);  // 1 ������
                AddPlayerEXP(exp_Player + amount - requiredExp);    // ������ ����ġ �ٽ� �߰�
            }
        }

        // ���� ������� Ȯ��
        if (!isBattleMode)
        { // UI ������Ʈ
            UIUpdateManager.Instance.UpdatePlayerData();
        }
    }

    // ��ȭ, ���� ����
    public void AddMineral(float amount)        // �̳׶� �߰�
    {
        own_Mineral += amount;

        // ���� ������� Ȯ��
        if (!isBattleMode)
        { // UI ������Ʈ
            UIUpdateManager.Instance.UpdateMineral();
        }
    }
    public void SubMineral(float amount)        // �̳׶� �Ҹ�
    {
        own_Mineral -= amount;

        // ���� ������� Ȯ��
        if (!isBattleMode)
        { // UI ������Ʈ
            UIUpdateManager.Instance.UpdateMineral();
        }
    }

    public void AddDia(float amount)            // ���̾� �߰�
    {
        own_Dia += amount;

        // ���� ������� Ȯ��
        if (!isBattleMode)
        { // UI ������Ʈ
            UIUpdateManager.Instance.UpdateDia();
        }
    }
    public void SubDia(float amount)            // ���̾� �Ҹ�
    {
        own_Dia -= amount;

        // ���� ������� Ȯ��
        if (!isBattleMode)
        { // UI ������Ʈ
            UIUpdateManager.Instance.UpdateDia();
        }
    }
    /// <�������߰�>
    /// �ð� �� ������ �߰� = �ִ� ������ �̻����� �߰� �Ұ�
    /// ����, ���ŷ� ������ �߰� = �ִ� ������ �̻����� �߰� ����
    /// </�������߰�>
    public void AddMaxEnergy(int amount)      // �ִ� ������ �߰�
    {
        max_Energy += amount;
        if (!isBattleMode)
        { // UI ������Ʈ
            UIUpdateManager.Instance.UpdateEnergy();
        }
    }
    public void AddCurrentEnergy(int amount)  // ���� ������ �߰�
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

        // ���� ������� Ȯ��
        if (!isBattleMode)
        { // UI ������Ʈ
            UIUpdateManager.Instance.UpdateEnergy();
        }
    }
    public void SubCurrentEnergy(int amount)  // ���� ������ �Ҹ�
    {
        own_Energy -= amount;

        if (own_Energy < max_Energy)
        { 
            // ������ Ÿ�̸� ����
            GameTimeManager.Instance.EnergyTimer();
        }

        // ���� ������� Ȯ��
        if (!isBattleMode)
        { // UI ������Ʈ
            UIUpdateManager.Instance.UpdateEnergy();
        }
    }
    public void AddExtraEnergy(int amount)
    {
        own_Energy += amount;
        
        // ���� ������� Ȯ��
        if (!isBattleMode)
        { // UI ������Ʈ
            UIUpdateManager.Instance.UpdateEnergy();
        }
    }
    // ���� ����
    public void AddShortWeaponLevel(SWeaponType tr_Weapon, bool isDoubleUp)
    {
        GameObject weaponInfoGroup = PopUpUIManager.Instance.popUpGroups[(int)PopUpType.ShortWeaponInfo];

        switch (tr_Weapon)
        {
            case SWeaponType.Sword:
                {
                    // ���� �ڵ� (�̳׶� ����)
                    // �ʿ� �̳׶� ���� ���� ����
                    if (own_Mineral >= required_Minelral_Sword)
                    {
                        SubMineral(required_Minelral_Sword);

                        // ��ư ���� ���
                        SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponLevelUpSound);

                        // 1+1 ��ȭ
                        if (isDoubleUp)
                        { 
                            // ��ȭ ���� �ι� �ݺ�
                            for (int i=0; i<2; i++)
                            {
                                // ���� ����
                                level_Sword += 1;   // ���� �߰�

                                atk_Sword += SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sword].AtkPerLevel;

                                ptc_Sword += 1;

                                // �ʿ� �̳׶� ����
                                required_Minelral_Sword += perReq_Sword;
                            }
                        }
                        else
                        { 
                            // ���� ����
                            level_Sword += 1;   // ���� �߰�

                            atk_Sword += SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sword].AtkPerLevel;

                            ptc_Sword += 1;

                            // �ʿ� �̳׶� ����
                            required_Minelral_Sword += perReq_Sword;
                        }
                        // ���� ������ ����Ʈ ����
                        if (QuestManager.Instance.dailyQuestList[(int)DailyQuestType.WeaponLevelUp].myState == QuestButtonState.Proceed)
                        {
                            QuestManager.Instance.UpdateDailyQuest(DailyQuestType.WeaponLevelUp);
                        }

                        if (s_Weapontype == SWeaponType.Sword)
                        {
                            // ���� ������ ������ ��� ���� UI ������Ʈ
                            equipedWeaponInfo.UpdateStat();
                        }
                    }
                    else
                    {
                        // �̳׶� �� ���� �̺�Ʈ
                        Debug.Log(" �̳׶� ���� ");
                    }
                    break;
                }
            case SWeaponType.Hammer:
                {
                    // �ʿ� �̳׶� ���� ���� ����
                    if (own_Mineral >= required_Minelral_Hammer)
                    {
                        SubMineral(required_Minelral_Hammer);

                        // ��ư ���� ���
                        SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponLevelUpSound);

                        // 1+1 ��ȭ
                        if (isDoubleUp)
                        {
                            // ��ȭ ���� �ι� �ݺ�
                            for (int i = 0; i < 2; i++)
                            {
                                // ���� ����
                                level_Hammer += 1;   // ���� �߰�

                                atk_Hammer += SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Hammer].AtkPerLevel;

                                ptc_Hammer += 1;

                                // �ʿ� �̳׶� ����
                                required_Minelral_Hammer = defaultReq_Hammer + (level_Hammer - 1) * perReq_Hammer;
                            }
                        }
                        else
                        {
                            // ���� ����
                            level_Hammer += 1;   // ���� �߰�

                            atk_Hammer += SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Hammer].AtkPerLevel;

                            ptc_Hammer += 1;

                            // �ʿ� �̳׶� ����
                            required_Minelral_Hammer = defaultReq_Hammer + (level_Hammer - 1) * perReq_Hammer;
                        }
                        // ���� ������ ����Ʈ ����
                        if (QuestManager.Instance.dailyQuestList[(int)DailyQuestType.WeaponLevelUp].myState == QuestButtonState.Proceed)
                        {
                            QuestManager.Instance.UpdateDailyQuest(DailyQuestType.WeaponLevelUp);
                        }

                        if (s_Weapontype == SWeaponType.Hammer)
                        {
                            // ���� ������ ������ ��� ���� UI ������Ʈ
                            equipedWeaponInfo.UpdateStat();
                        }
                    }
                    else
                    {
                        // �̳׶� �� ���� �̺�Ʈ
                        Debug.Log(" �̳׶� ���� ");
                    }
                    break;
                }
            case SWeaponType.Sycthe:
                {
                    // ���� �ڵ� (�̳׶� ����)
                    // �ʿ� �̳׶� ���� ���� ����
                    if (own_Mineral >= required_Minelral_Scythe)
                    {
                        SubMineral(required_Minelral_Scythe);

                        // ��ư ���� ���
                        SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponLevelUpSound);

                        // 1+1 ��ȭ
                        if (isDoubleUp)
                        {
                            // ��ȭ ���� �ι� �ݺ�
                            for (int i = 0; i < 2; i++)
                            {
                                level_Scythe += 1;   // ���� �߰�

                                atk_Scythe += SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sycthe].AtkPerLevel;

                                ptc_Scythe += 1;

                                // �ʿ� �̳׶� ����
                                required_Minelral_Scythe = defaultReq_Scythe + (level_Scythe - 1) * perReq_Scythe;
                            }
                        }
                        else
                        {
                            level_Scythe += 1;   // ���� �߰�

                            atk_Scythe += SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Sycthe].AtkPerLevel;

                            ptc_Scythe += 1;


                            // �ʿ� �̳׶� ����
                            required_Minelral_Scythe = defaultReq_Scythe + (level_Scythe - 1) * perReq_Scythe;
                        }
                        // ���� ������ ����Ʈ ����
                        if (QuestManager.Instance.dailyQuestList[(int)DailyQuestType.WeaponLevelUp].myState == QuestButtonState.Proceed)
                        {
                            QuestManager.Instance.UpdateDailyQuest(DailyQuestType.WeaponLevelUp);
                        }

                        if (s_Weapontype == SWeaponType.Sycthe)
                        {
                            // ���� ������ ������ ��� ���� UI ������Ʈ
                            equipedWeaponInfo.UpdateStat();
                        }

                    }
                    else
                    {
                        // �̳׶� �� ���� �̺�Ʈ
                        Debug.Log(" �̳׶� ���� ");
                    }
                    break;
                }
            case SWeaponType.Fire:
                {
                    // ���� �ڵ� (�̳׶� ����)
                    // �ʿ� �̳׶� ���� ���� ����
                    if (own_Mineral >= required_Minelral_Flamethrower)
                    {
                        SubMineral(required_Minelral_Flamethrower);

                        // ��ư ���� ���
                        SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponLevelUpSound);

                        // 1+1 ��ȭ
                        if (isDoubleUp)
                        {
                            // ��ȭ ���� �ι� �ݺ�
                            for (int i = 0; i < 2; i++)
                            {
                                // ���� ����
                                level_Flamethrower += 1;   // ���� �߰�

                                atk_Flamethrower += SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Fire].AtkPerLevel;

                                ptc_Flamethrower += 1;

                                // �ʿ� �̳׶� ����
                                required_Minelral_Flamethrower = defaultReq_Flamethrower + (level_Flamethrower - 1) * perReq_Flamethrower;
                            }
                        }
                        else
                        {
                            // ���� ����
                            level_Flamethrower += 1;   // ���� �߰�

                            atk_Flamethrower += SlotManager.Instance.shortWeaponDataList[(int)SWeaponType.Fire].AtkPerLevel;

                            ptc_Flamethrower += 1;

                            // �ʿ� �̳׶� ����
                            required_Minelral_Flamethrower = defaultReq_Flamethrower + (level_Flamethrower - 1) * perReq_Flamethrower;
                        }
                        // ���� ������ ����Ʈ ����
                        if (QuestManager.Instance.dailyQuestList[(int)DailyQuestType.WeaponLevelUp].myState == QuestButtonState.Proceed)
                        {
                            QuestManager.Instance.UpdateDailyQuest(DailyQuestType.WeaponLevelUp);
                        }

                        if (s_Weapontype == SWeaponType.Fire)
                        {
                            // ���� ������ ������ ��� ���� UI ������Ʈ
                            equipedWeaponInfo.UpdateStat();
                        }
                    }
                    else
                    {
                        // �̳׶� �� ���� �̺�Ʈ
                        Debug.Log(" �̳׶� ���� ");
                    }
                    break;
                }
            default:
                {
                    Debug.Log("-- Weapon Type Error --");
                    break;
                }
        }

        // UI ������Ʈ
        weaponInfoGroup.GetComponent<WeaponInfo>().UpdateUI(true, (int)tr_Weapon);
    }
    public void AddLongWeaponLevel(LWeaponType tr_Weapon, bool isDoubleUp)
    {
        GameObject weaponInfoGroup = PopUpUIManager.Instance.popUpGroups[(int)PopUpType.LongWeaponInfo];

        switch (tr_Weapon)
        {
            case LWeaponType.Syringe:
                {
                    // �ʿ� �̳׶� ���� ���� ����
                    if (own_Mineral >= required_Minelral_Syringe)
                    {
                        // �̳׶� �Ҹ�
                        SubMineral(required_Minelral_Syringe);

                        // ��ư ���� ���
                        SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponLevelUpSound);

                        // 1+1 ��ȭ
                        if (isDoubleUp)
                        {
                            // ��ȭ ���� �ι� �ݺ�
                            for (int i = 0; i < 2; i++)
                            {
                                // ���� ����
                                level_Syringe += 1;   // ���� �߰�

                                atk_Syringe += SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Syringe].AtkPerLevel;

                                // �ʿ� �̳׶� ����
                                required_Minelral_Syringe = defaultReq_Syringe + (level_Syringe - 1) * perReq_Syringe;
                            }
                        }
                        else
                        {
                            // ���� ����
                            level_Syringe += 1;   // ���� �߰�

                            atk_Syringe += SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Syringe].AtkPerLevel;

                            // �ʿ� �̳׶� ����
                            required_Minelral_Syringe = defaultReq_Syringe + (level_Syringe - 1) * perReq_Syringe;
                        }
                        // ���� ������ ����Ʈ ����
                        if (QuestManager.Instance.dailyQuestList[(int)DailyQuestType.WeaponLevelUp].myState == QuestButtonState.Proceed)
                        {
                            QuestManager.Instance.UpdateDailyQuest(DailyQuestType.WeaponLevelUp);
                        }

                        if (l_Weapontype == LWeaponType.Syringe)
                        {
                            // ���� ������ ������ ��� ���� UI ������Ʈ
                            equipedWeaponInfo.UpdateStat();
                        }

                        Debug.Log("������");
                    }
                    else
                    {
                        // �̳׶� �� ���� �̺�Ʈ
                        Debug.Log(" �̳׶� ���� ");
                    }
                    break;
                }
            case LWeaponType.Bow:
                {
                    // �ʿ� �̳׶� ���� ���� ����
                    if (own_Mineral >= required_Minelral_Bow)
                    {
                        // �̳׶� �Ҹ�
                        SubMineral(required_Minelral_Bow);

                        // ��ư ���� ���
                        SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponLevelUpSound);

                        // 1+1 ��ȭ
                        if (isDoubleUp)
                        {
                            // ��ȭ ���� �ι� �ݺ�
                            for (int i = 0; i < 2; i++)
                            {
                                // ���� ����
                                level_Bow += 1;   // ���� �߰�

                                atk_Bow += SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Bow].AtkPerLevel;

                                // �ʿ� �̳׶� ����
                                required_Minelral_Bow = defaultReq_Bow + (level_Bow - 1) * perReq_Bow;
                            }
                        }
                        else
                        {
                            // ���� ����
                            level_Bow += 1;   // ���� �߰�

                            atk_Bow += SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Bow].AtkPerLevel;

                            // �ʿ� �̳׶� ����
                            required_Minelral_Bow = defaultReq_Bow + (level_Bow - 1) * perReq_Bow;
                        }
                        // ���� ������ ����Ʈ ����
                        if (QuestManager.Instance.dailyQuestList[(int)DailyQuestType.WeaponLevelUp].myState == QuestButtonState.Proceed)
                        {
                            QuestManager.Instance.UpdateDailyQuest(DailyQuestType.WeaponLevelUp);
                        }

                        if (l_Weapontype == LWeaponType.Bow)
                        {
                            // ���� ������ ������ ��� ���� UI ������Ʈ
                            equipedWeaponInfo.UpdateStat();
                        }
                    }
                    else
                    {
                        // �̳׶� �� ���� �̺�Ʈ
                        Debug.Log(" �̳׶� ���� ");
                    }
                    break;
                }
            case LWeaponType.Gun:
                {                   
                    // �ʿ� �̳׶� ���� ���� ����
                    if (own_Mineral >= required_Minelral_Gun)
                    {
                        SubMineral(required_Minelral_Gun);

                        // ��ư ���� ���
                        SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponLevelUpSound);

                        // 1+1 ��ȭ
                        if (isDoubleUp)
                        {
                            // ��ȭ ���� �ι� �ݺ�
                            for (int i = 0; i < 2; i++)
                            {
                                level_Gun += 1;   // ���� �߰�

                                atk_Gun += SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Gun].AtkPerLevel;

                                // �ʿ� �̳׶� ����
                                required_Minelral_Gun = defaultReq_Gun + (level_Gun - 1) * perReq_Gun;
                            }
                        }
                        else
                        {
                            level_Gun += 1;   // ���� �߰�

                            atk_Gun += SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Gun].AtkPerLevel;

                            // �ʿ� �̳׶� ����
                            required_Minelral_Gun = defaultReq_Gun + (level_Gun - 1) * perReq_Gun;
                        }
                        // ���� ������ ����Ʈ ����
                        if (QuestManager.Instance.dailyQuestList[(int)DailyQuestType.WeaponLevelUp].myState == QuestButtonState.Proceed)
                        {
                            QuestManager.Instance.UpdateDailyQuest(DailyQuestType.WeaponLevelUp);
                        }

                        if (l_Weapontype == LWeaponType.Gun)
                        {
                            // ���� ������ ������ ��� ���� UI ������Ʈ
                            equipedWeaponInfo.UpdateStat();
                        }
                    }
                    else
                    {
                        // �̳׶� �� ���� �̺�Ʈ
                        Debug.Log(" �̳׶� ���� ");
                    }

                    break;
                }
            case LWeaponType.Rifle:
                {
                    // �ʿ� �̳׶� ���� ���� ����
                    if (own_Mineral >= required_Minelral_RifleGun)
                    {
                        SubMineral(required_Minelral_RifleGun);

                        // ��ư ���� ���
                        SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponLevelUpSound);

                        // 1+1 ��ȭ
                        if (isDoubleUp)
                        {
                            // ��ȭ ���� �ι� �ݺ�
                            for (int i = 0; i < 2; i++)
                            {
                                // ���� ����
                                level_RifleGun += 1;   // ���� �߰�

                                atk_RifleGun += SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Rifle].AtkPerLevel;

                                // �ʿ� �̳׶� ����
                                required_Minelral_RifleGun = defaultReq_RifleGun + (level_RifleGun - 1) * perReq_RifleGun;
                            }
                        }
                        else
                        {
                            // ���� ����
                            level_RifleGun += 1;   // ���� �߰�

                            atk_RifleGun += SlotManager.Instance.longWeaponDataList[(int)LWeaponType.Rifle].AtkPerLevel;

                            // �ʿ� �̳׶� ����
                            required_Minelral_RifleGun = defaultReq_RifleGun + (level_RifleGun - 1) * perReq_RifleGun;
                        }

                        // ���� ������ ����Ʈ ����
                        if (QuestManager.Instance.dailyQuestList[(int)DailyQuestType.WeaponLevelUp].myState == QuestButtonState.Proceed)
                        {
                            QuestManager.Instance.UpdateDailyQuest(DailyQuestType.WeaponLevelUp);
                        }

                        if (l_Weapontype == LWeaponType.Rifle)
                        {
                            // ���� ������ ������ ��� ���� UI ������Ʈ
                            equipedWeaponInfo.UpdateStat();
                        }
                    }
                    else
                    {
                        // �̳׶� �� ���� �̺�Ʈ
                        Debug.Log(" �̳׶� ���� ");
                    }
                    break;
                }
            default:
                break;
        }


        Debug.Log("ui ������Ʈ");
        // UI ������Ʈ
        weaponInfoGroup.GetComponent<WeaponInfo>().UpdateUI(false, (int)tr_Weapon);
    }

    // ���� ���ݷ�, ���ݼӵ�, ź ���� ��ȯ
    public ST_WeaponStat GetWeaponStat(bool isShort, int weaponNum)
    {
        // !!!!!!ptc : projectile cound ��ȹ ������ ����!!!!!!

        ST_WeaponStat stat = new ST_WeaponStat(0, 0, 0, 0, 0,0);

        if(isShort)
        {
            // �ٰŸ� ����
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
            // �ٰŸ� ����
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
            // �ٰŸ� ����
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
                            // ���� ������ ������ ��� ���� UI ������Ʈ
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
                            // ���� ������ ������ ��� ���� UI ������Ʈ
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
                            // ���� ������ ������ ��� ���� UI ������Ʈ
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
                            // ���� ������ ������ ��� ���� UI ������Ʈ
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
                            // ���� ������ ������ ��� ���� UI ������Ʈ
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
                            // ���� ������ ������ ��� ���� UI ������Ʈ
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
                            // ���� ������ ������ ��� ���� UI ������Ʈ
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
                            // ���� ������ ������ ��� ���� UI ������Ʈ
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

// ���̽� ����ȭ�� ���� ������ Ŭ����
// Monobehaviour ��� ���� �����͸��� ������ Ŭ����
public class GameData_Json
{
    #region ���ּ� ����

    public int iSpaceShipLevel;

    #endregion

    #region ���� ����

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

    #region ���� ����

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

    #region �Ĺ� ����

    public bool[] bArrPlantSlotUnLockInfo;

    public int[] iArrHarvestCount;

    #endregion

    #region ��ġ�� ����
    public List<int> iReleaseStageAnimalList = new List<int>();
    public List<int> iReleaseCashAnimalList = new List<int>();
    #endregion

    #region �ð� ����

    public int iFirstLogInTime;
    public int iGameEndTime;
    public int iNextDayTime;
    public int iDayCount;
    public bool bIsEnergyTimer;
    public int iEnergyTimerStartTime;
    #endregion

    #region ������
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
