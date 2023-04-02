using UnityEngine;

public class Weapon_E : MonoBehaviour
{
    [SerializeField]
    private Animator[] curLWeaponAnims; // 원거리 무기 애니메이션 배열

    private Animator curLWeaponAnim; // 원거리 무기 애니메이션
    public Animator CurLWeaponAnim { get => curLWeaponAnim; } // 읽기 전용

    [Space(20)]

    [SerializeField] GameObject[] LWeapons; // 원거리무기 오브젝트
    [SerializeField] GameObject[] SWeapons; // 원거리무기 오브젝트    

    GameObject curLWeapon; // 현재 원거리무기
    GameObject curSWeapon; // 현재 근접무기

    public GameObject CurLWeapon { get => curLWeapon; } // 읽기 전용
    public GameObject CurSWeapon { get => curSWeapon; } // 읽기 전용 -> 테스트

    private void Awake()
    {
        // 모든 원거리무기 장착 해제
        for (int i = 0; i < LWeapons.Length; i++)
        {
            LWeapons[i].SetActive(false);
        }

        // 모든 근접무기 장착 해제
        for (int i = 0; i < SWeapons.Length; i++)
        {
            SWeapons[i].SetActive(false);
        }
    }

    private void Start()
    {
        // 현재 원거리무기 타입에 따라 원거리무기 종류 변경
        curLWeapon = LWeapons[(int)StatManager.Instance.l_Weapontype];
        curLWeapon.SetActive(true);

        // 현재 근접무기 타입에 따라 근접무기 종류 변경
        curSWeapon = SWeapons[(int)StatManager.Instance.s_Weapontype];
        curSWeapon.SetActive(true);

        // 총, 라이플 공격 방향 설정
        if (CurLWeapon.CompareTag("Gun") || CurLWeapon.CompareTag("Rifle"))
        {
            GameManager_E.Instance.Player.AttackDir = CurLWeapon.transform.GetChild(0).transform;
        }

        // 원거리 애니메이션 설정
        curLWeaponAnim = curLWeaponAnims[(int)StatManager.Instance.l_Weapontype];
    }

    //private void Update()
    //{
    //    // 게임오버가 된 경우
    //    //if (GameManager_E.Instance.gameover && !gameover)
    //    //{
    //    //    // 원거리 무기 해제
    //    //    Transform[] child = curLWeapon.GetComponentsInChildren<Transform>();

    //    //    for (int i = 1; i < child.Length; i++)
    //    //    {
    //    //        child[i].gameObject.SetActive(false);
    //    //    }

    //    //    // 근접 무기 해제
    //    //    curSWeapon.SetActive(false);

    //    //    gameover = true;
    //    //}
    //}

    /// 테스트용 ///
    //public void T_SwitchLongWeapon(int num)
    //{
    //    if (curLWeapon == LWeapons[num]) num = 0; // 같은 무기를 한번 더 눌렀을 경우 -> 해제

    //    // 스탯매니저 변경
    //    StatManager.Instance.l_Weapontype = (LWeaponType)num;

    //    // 현재 무기 장착 해제
    //    curLWeapon.SetActive(false);

    //    // 선택 무기 장착
    //    curLWeapon = LWeapons[num];
    //    curLWeapon.SetActive(true);

    //    // 투사체 변경
    //    GameManager_E.Instance.Pool.T_ChangeObject();
    //}

    //public void T_SwitchShortWeapon(int num)
    //{
    //    if (curSWeapon == SWeapons[num]) num = 0; // 같은 무기를 한번 더 눌렀을 경우 -> 해제

    //    // 스탯매니저 변경
    //    StatManager.Instance.s_Weapontype = (SWeaponType)num;

    //    if (curSWeapon.tag == "Fire")
    //    {
    //        // 무기가 화염방사기인 경우 해당 오브젝트를 weapon의 자식으로 만듦
    //        //curSWeapon.transform.parent = this.transform;

    //        // 파티클 실행
    //        ParticleSystem[] particles = curSWeapon.GetComponentsInChildren<ParticleSystem>();

    //        for(int i = 0; i < particles.Length; i++)
    //        {
    //            particles[i].Play();
    //        }
    //    }

    //    // 현재 무기 장착 해제
    //    curSWeapon.SetActive(false);

    //    // 선택 무기 장착
    //    curSWeapon = SWeapons[num];
    //    curSWeapon.SetActive(true);

    //    if (curSWeapon.tag == "Fire")
    //    {
    //        // 파티클 실행
    //        ParticleSystem[] particles = curSWeapon.GetComponentsInChildren<ParticleSystem>();

    //        for (int i = 0; i < particles.Length; i++)
    //        {
    //            particles[i].Play();
    //        }
    //    }
    //}
}
