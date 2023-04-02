using UnityEngine;

public class Weapon_E : MonoBehaviour
{
    [SerializeField]
    private Animator[] curLWeaponAnims; // ���Ÿ� ���� �ִϸ��̼� �迭

    private Animator curLWeaponAnim; // ���Ÿ� ���� �ִϸ��̼�
    public Animator CurLWeaponAnim { get => curLWeaponAnim; } // �б� ����

    [Space(20)]

    [SerializeField] GameObject[] LWeapons; // ���Ÿ����� ������Ʈ
    [SerializeField] GameObject[] SWeapons; // ���Ÿ����� ������Ʈ    

    GameObject curLWeapon; // ���� ���Ÿ�����
    GameObject curSWeapon; // ���� ��������

    public GameObject CurLWeapon { get => curLWeapon; } // �б� ����
    public GameObject CurSWeapon { get => curSWeapon; } // �б� ���� -> �׽�Ʈ

    private void Awake()
    {
        // ��� ���Ÿ����� ���� ����
        for (int i = 0; i < LWeapons.Length; i++)
        {
            LWeapons[i].SetActive(false);
        }

        // ��� �������� ���� ����
        for (int i = 0; i < SWeapons.Length; i++)
        {
            SWeapons[i].SetActive(false);
        }
    }

    private void Start()
    {
        // ���� ���Ÿ����� Ÿ�Կ� ���� ���Ÿ����� ���� ����
        curLWeapon = LWeapons[(int)StatManager.Instance.l_Weapontype];
        curLWeapon.SetActive(true);

        // ���� �������� Ÿ�Կ� ���� �������� ���� ����
        curSWeapon = SWeapons[(int)StatManager.Instance.s_Weapontype];
        curSWeapon.SetActive(true);

        // ��, ������ ���� ���� ����
        if (CurLWeapon.CompareTag("Gun") || CurLWeapon.CompareTag("Rifle"))
        {
            GameManager_E.Instance.Player.AttackDir = CurLWeapon.transform.GetChild(0).transform;
        }

        // ���Ÿ� �ִϸ��̼� ����
        curLWeaponAnim = curLWeaponAnims[(int)StatManager.Instance.l_Weapontype];
    }

    //private void Update()
    //{
    //    // ���ӿ����� �� ���
    //    //if (GameManager_E.Instance.gameover && !gameover)
    //    //{
    //    //    // ���Ÿ� ���� ����
    //    //    Transform[] child = curLWeapon.GetComponentsInChildren<Transform>();

    //    //    for (int i = 1; i < child.Length; i++)
    //    //    {
    //    //        child[i].gameObject.SetActive(false);
    //    //    }

    //    //    // ���� ���� ����
    //    //    curSWeapon.SetActive(false);

    //    //    gameover = true;
    //    //}
    //}

    /// �׽�Ʈ�� ///
    //public void T_SwitchLongWeapon(int num)
    //{
    //    if (curLWeapon == LWeapons[num]) num = 0; // ���� ���⸦ �ѹ� �� ������ ��� -> ����

    //    // ���ȸŴ��� ����
    //    StatManager.Instance.l_Weapontype = (LWeaponType)num;

    //    // ���� ���� ���� ����
    //    curLWeapon.SetActive(false);

    //    // ���� ���� ����
    //    curLWeapon = LWeapons[num];
    //    curLWeapon.SetActive(true);

    //    // ����ü ����
    //    GameManager_E.Instance.Pool.T_ChangeObject();
    //}

    //public void T_SwitchShortWeapon(int num)
    //{
    //    if (curSWeapon == SWeapons[num]) num = 0; // ���� ���⸦ �ѹ� �� ������ ��� -> ����

    //    // ���ȸŴ��� ����
    //    StatManager.Instance.s_Weapontype = (SWeaponType)num;

    //    if (curSWeapon.tag == "Fire")
    //    {
    //        // ���Ⱑ ȭ�������� ��� �ش� ������Ʈ�� weapon�� �ڽ����� ����
    //        //curSWeapon.transform.parent = this.transform;

    //        // ��ƼŬ ����
    //        ParticleSystem[] particles = curSWeapon.GetComponentsInChildren<ParticleSystem>();

    //        for(int i = 0; i < particles.Length; i++)
    //        {
    //            particles[i].Play();
    //        }
    //    }

    //    // ���� ���� ���� ����
    //    curSWeapon.SetActive(false);

    //    // ���� ���� ����
    //    curSWeapon = SWeapons[num];
    //    curSWeapon.SetActive(true);

    //    if (curSWeapon.tag == "Fire")
    //    {
    //        // ��ƼŬ ����
    //        ParticleSystem[] particles = curSWeapon.GetComponentsInChildren<ParticleSystem>();

    //        for (int i = 0; i < particles.Length; i++)
    //        {
    //            particles[i].Play();
    //        }
    //    }
    //}
}
