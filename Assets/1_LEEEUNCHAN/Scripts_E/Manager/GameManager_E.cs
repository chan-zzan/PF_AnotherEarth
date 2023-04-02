using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager_E : MonoBehaviour
{
    #region �̱���
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

    // ��ũ��Ʈ
    [Header("Script")]
    public Player_E Player;
    public ObjectPoolManager_E Pool;
    public TimeManager_E Time;
    public MonsterSpawner_E monsterSpawner; // ���� ������
    public ItemSpawner_E itemSpawner;
    public Weapon_E Weapon; // ����
    public WeaponSkillSlider weaponSkillSlider; // ���� ���õ�
    public MapManager_E Map; // ��

    [Space(10)]

    // ĳ���� ����
    [Header("Character")]
    public int life = 1;
    public int curSWeaponPTC; // ���� �ٰŸ� ���� ����    
    public bool gameover = false; // ���� ���� ����
    public bool unbeatable = false; // �÷��̾��� ���� ����
    public bool finalStage = false; // ������ �������� ����
    public int finalStageCount = 0; // ���̳� �������� ���� ������ ����

    [Space(10)]

    // ���� ����
    [Header("Monster")]
    public Monster_E curBoss = null; // ���� ����
    public bool bossSpawn = false; // ���� ���� ����    
    public bool isHardMode = false; // �ϵ������� ����
    public int killCount = 0;
    public DashInfo dashInfo; // �뽬 ����

    [Space(10)]

    // UI ����
    [Header("UI")]
    public GameObject winPopup; // �¸� �˾�
    public GameObject defeatPopup; // �й� �˾�
    public GameObject revivalPopup; // ��Ȱ �˾�
    public GameObject EnergyChargePopup; // ������ ���� �˾�
    public Slider bossHP; // ���� HP
    public TMPro.TMP_Text bossText; // ���� ���� ǥ��
    public TMPro.TMP_Text coinText; // ���� �� ���� ȹ�淮
    public TMPro.TMP_Text killCountText; // ���� óġ ��
    public TMPro.TMP_Text clearRewardText; // Ŭ���� ����
    public TMPro.TMP_Text killRewardText; // ���� óġ ����    
    public TMPro.TMP_Text defeatRewardText; // �й� �� ���� ȹ�淮
    public TMPro.TMP_Text revival_timer; // ��Ȱ Ÿ�̸�
    

    //[Space(10)]

    // ��Ÿ
    [Space(30)]
    [Header("�����˾�(�ӽ�)")]
    public GameObject EscapePopUp;

    // ��ȭ ������
    public GameObject coin; // ����

    // ȹ�� ��ȭ ����
    public int totalMineral; // ���� �̳׶��� ����

    // ���
    public GameObject warningPopup; // ����˾�
    public GameObject BossWarningPopup; // ��������˾�
    public GameObject FinalWarningPopup; // ���̳ΰ���˾�
    public GameObject FinalWarningPopup2; // ���̳ΰ���˾�2
    public Animator warningAnim; // ��� �ִϸ��̼�
    public Animator hpWarningAnim; // hp ��� �ִϸ��̼�

    private void Awake()
    {
        // ������ �ʱ�ȭ
        life = 1;

        // �̳׶� �� �ʱ�ȭ
        coinText.text = "0";

        // ���� ų ī��Ʈ �ʱ�ȭ
        killCountText.text = "0";

        //if(StageManager.Instance.curStageNum == 10 || )
    }


    private void Update()
    {
        // ȸ�� ���� ������ �ٲ� ��쿡�� ����
        if (curSWeaponPTC != WeaponSkillManager.Instance.curSweaponPtc)
        {
            curSWeaponPTC = WeaponSkillManager.Instance.curSweaponPtc; ;
            RotateWeaponChangePtc(curSWeaponPTC);
        }

        /// ����׿�
        if (Input.GetKeyDown(KeyCode.P))
        {
            Player.CurHP = 10;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            StatManager.Instance.AddDia(1000);
        }

        /// �׽�Ʈ�� - ������
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EscapePopUp.SetActive(true);
        }
    }

    void RotateWeaponChangePtc(int ptc)
    {
        if (ptc < 1) ptc = 1; // �ּ� ���� ���� = 1
        if (ptc > 5) ptc = 5; // �ִ� ���� ���� = 5

        Transform curWeaponTr = Weapon.CurSWeapon.transform;
        List<RWeapon_E> rWeapons = new List<RWeapon_E>();// R_Axis�� RWeapon_E ��ũ��Ʈ

        for (int i = 0; i < curWeaponTr.childCount; i++)
        {
            curWeaponTr.GetChild(i).gameObject.SetActive(true); // ��� ������Ʈ Ȱ��ȭ -> ��ũ��Ʈ�� �����ϱ� ����
            rWeapons.Add(curWeaponTr.GetChild(i).GetChild(0).GetComponent<RWeapon_E>()); // ��ũ��Ʈ ����
            rWeapons[i].rotAxis.rotation = Quaternion.Euler(0, 0, 0); // ��� ������Ʈ ����ġ
        }

        for (int i = 0; i < ptc; i++)
        {
            int curRot = 360 / ptc; // ������ ���� ȸ�������� ����

            // ��ȣ���� ������ �ٸ��� ����
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

            rWeapons[i].rotAxis.rotation = Quaternion.Euler(0, 0, curRot); // ��ȣ�� ȸ�� ����
        }

        for(int i = ptc; i < rWeapons.Count; i++)
        {
            // �Ⱦ��� ��ȣ�� ��Ȱ��ȭ
            rWeapons[i].rotAxis.gameObject.SetActive(false);
        }
    }

    // ��Ȱ
    public void Revival(bool useDia)
    {
        if (useDia)
        {
            float revivalDia = 30; // ��Ȱ �� �ʿ� ���̾�

            if (StatManager.Instance.Own_Dia >= revivalDia)
            {
                StatManager.Instance.SubDia(revivalDia);
                Player.CurHP = Player.MaxHP; // �ִ�ü������ ȸ��
                revivalPopup.SetActive(false); // ��Ȱ �˾� ����
            }
            else
            {
                // ���̾ư� ������ ���
                SoundManager_E.Instance.EffectSoundPlay(6);
                return;
            }
        }
        else
        {
            Player.CurHP = Player.MaxHP / 2; // 50% ü������ ȸ��
            revivalPopup.SetActive(false); // ��Ȱ �˾� ����
        }

        life = 0; // ��� ����
        Player.myState = Player_E.STATE.Play; // �ٽ� �÷��� ���·� ����
        UnityEngine.Time.timeScale = 1; // �ð� ���� ����

        Weapon.CurLWeapon.GetComponent<Animator>().SetTrigger("revival"); // �ִϸ��̼� ����
        gameover = false;

        StartCoroutine(UnBeatable(2.0f)); // 2�ʰ� ���� ���·� ����

        // ���͵鵵 �ٽ� �����̵��� ����
        Monster_E[] monsters = Pool.P_Monsters[0].parent.GetComponentsInChildren<Monster_E>();

        for (int i = 0; i < monsters.Length; i++)
        {
            monsters[i].myState = Monster_E.STATE.Move; // ���� ����
        }

        // ���Ÿ� ���� ����
        Transform[] child = Weapon.CurLWeapon.GetComponentsInChildren<Transform>(true);

        for (int i = 1; i < child.Length; i++)
        {
            child[i].gameObject.SetActive(true);
        }

        // ���� ���� ����
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
        // ���Ÿ� ������ �ڽĵ鿡 ����
        SpriteRenderer[] child = Weapon.CurLWeapon.GetComponentsInChildren<SpriteRenderer>(true);

        List<Color> MinAlphaVals = new List<Color>();
        List<Color> MaxAlphaVals = new List<Color>();

        for (int i = 0; i < child.Length; i++)
        {
            // ���� �� ����
            MinAlphaVals.Add(new Color(child[i].color.r, child[i].color.g, child[i].color.b, 0.2f));
            MaxAlphaVals.Add(new Color(child[i].color.r, child[i].color.g, child[i].color.b, 0.5f));
        }

        int count = 5; // ������ Ƚ��
        float time = t / count / 2; // ������ �ð�

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
            // ���� ���� ������ ����
            child[i].color = Color.white;
        }
    }

    // ���ε�
    public void LoadScene(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }

    //public void CoolTimeStart(float time)
    //{
    //    // ��Ÿ�� ����
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
