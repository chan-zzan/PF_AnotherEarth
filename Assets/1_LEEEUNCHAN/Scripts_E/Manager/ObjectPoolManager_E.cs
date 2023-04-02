using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Pool;

public class ObjectPoolManager_E : MonoBehaviour
{
    // 오브젝트 풀링

    // 오브젝트 생성 위치
    public Transform[] P_Monsters;
    public Transform P_projectiles; 
    public Transform P_coins;

    // 몬스터 풀
    public IObjectPool<Monster_E> _BabyMonsterPools;
    public IObjectPool<Monster_E> _NormalMonsterPools;
    public IObjectPool<Monster_E> _CrossMonsterPools;
    public IObjectPool<Monster_E> _CircleMonsterPools;
    public IObjectPool<Monster_E> _BossMonsterPools;
    public IObjectPool<Monster_E> _SpecialMonsterPools;
    public IObjectPool<Monster_E> _TileMonsterPools;

    // 투사체 풀
    public IObjectPool<Projectile_E> _ProjectilesPool;

    GameObject selectedProj; // 선택한 투사체
    bool isFirst = true; // 처음 투사체 결정인지

    // 투사체 별 업그레이드 오브젝트
    public GameObject[] syringeUpgradeProjectiles;
    public GameObject[] bowUpgradeProjectiles;
    public GameObject[] gunUpgradeProjectiles;
    public GameObject[] rifleUpgradeProjectiles;

    // 코인 풀 리스트
    public List<IObjectPool<Coin_E>> _CoinPools = new List<IObjectPool<Coin_E>>();

    // 생성할 코인 오브젝트 리스트
    public List<Coin_E> CoinObjectList;

    [HideInInspector]
    public Coin_E curCoinObject; // 생성할 코인 오브젝트


    private void Awake()
    {
        // 몬스터 풀
        _BabyMonsterPools = new ObjectPool<Monster_E>(CreateMonster, OnGetMonster, OnReleaseMonster, OnDestroyMonster, maxSize: 20);
        _NormalMonsterPools = new ObjectPool<Monster_E>(CreateMonster, OnGetMonster, OnReleaseMonster, OnDestroyMonster, maxSize: 20);
        _CrossMonsterPools = new ObjectPool<Monster_E>(CreateMonster, OnGetMonster, OnReleaseMonster, OnDestroyMonster, maxSize: 30);
        _CircleMonsterPools = new ObjectPool<Monster_E>(CreateMonster, OnGetMonster, OnReleaseMonster, OnDestroyMonster, maxSize: 40);
        _BossMonsterPools = new ObjectPool<Monster_E>(CreateMonster, OnGetMonster, OnReleaseMonster, OnDestroyMonster, maxSize: 1);
        _SpecialMonsterPools = new ObjectPool<Monster_E>(CreateMonster, OnGetMonster, OnReleaseMonster, OnDestroyMonster, maxSize: 50);
        _TileMonsterPools = new ObjectPool<Monster_E>(CreateMonster, OnGetMonster, OnReleaseMonster, OnDestroyMonster, maxSize: 100);

        // 투사체 풀
        _ProjectilesPool = new ObjectPool<Projectile_E>(CreateProjectile, OnGetProjectile, OnReleaseProjectile, OnDestroyProjectile, defaultCapacity: 20, maxSize: 100);

        // 코인 풀
        _CoinPools.Add(new ObjectPool<Coin_E>(CreateCoin, OnGetCoin, OnReleaseCoin, OnDestroyCoin, maxSize: 20));
    }

    #region 몬스터 풀

    [HideInInspector]
    public GameObject curSpawnMonster; // 생성할 몬스터

    Monster_E CreateMonster()
    {
        IObjectPool<Monster_E> _MonsterPools;
        Transform P_monsters;

        // 풀에서 몬스터 생성
        Monster_E mon = Instantiate(curSpawnMonster).GetComponent<Monster_E>();

        // 몬스터 생성 위치 및 생성 풀 설정
        switch (mon.myType)
        {
            case Monster_E.MonsterType.Baby:
                P_monsters = P_Monsters[0];
                _MonsterPools = _BabyMonsterPools;
                //print("babySpawn");
                break;
            case Monster_E.MonsterType.Normal:
                P_monsters = P_Monsters[1];
                _MonsterPools = _NormalMonsterPools;
                break;
            case Monster_E.MonsterType.Cross:
                P_monsters = P_Monsters[2];
                _MonsterPools = _CrossMonsterPools;
                break;
            case Monster_E.MonsterType.Circle:
                P_monsters = P_Monsters[3];
                _MonsterPools = _CircleMonsterPools;
                break;
            case Monster_E.MonsterType.Boss:
                P_monsters = P_Monsters[4];
                _MonsterPools = _BossMonsterPools;
                break;
            case Monster_E.MonsterType.Special:
                P_monsters = P_Monsters[5];
                _MonsterPools = _SpecialMonsterPools;
                break;
            case Monster_E.MonsterType.Tile:
                P_monsters = P_Monsters[6];
                _MonsterPools = _TileMonsterPools;
                break;
            default:
                P_monsters = null;
                _MonsterPools = null;
                break;
        }

        // 몬스터 생성 위치 및 생성 풀 적용
        mon.transform.parent = P_monsters;
        mon.SetManagedPool(_MonsterPools);
        return mon;
    }

    void OnGetMonster(Monster_E mon)
    {
        // 오브젝트 활성화
        mon.gameObject.SetActive(true);
    }

    void OnReleaseMonster(Monster_E mon)
    {
        // 오브젝트 비활성화
        mon.gameObject.SetActive(false);
    }

    void OnDestroyMonster(Monster_E mon)
    {
        // 오브젝트 파괴
        Destroy(mon.gameObject);
    }
    #endregion

    #region 투사체 풀
    Projectile_E CreateProjectile()
    {
        if (isFirst)
        {
            // 처음 생성하는 경우
            ChangeProjectile(1);
            isFirst = false;
        }

        // 풀에서 투사체 생성
        Projectile_E proj = Instantiate(selectedProj, P_projectiles).GetComponent<Projectile_E>();
        proj.SetManagedPool(_ProjectilesPool);
        return proj;
    }

    void OnGetProjectile(Projectile_E proj)
    {
        // 오브젝트 활성화
        proj.gameObject.SetActive(true);
    }

    void OnReleaseProjectile(Projectile_E proj)
    {
        // 오브젝트 비활성화
        proj.gameObject.SetActive(false);
    }

    void OnDestroyProjectile(Projectile_E proj)
    {
        // 오브젝트 파괴
        Destroy(proj.gameObject);
    }

    public void ChangeProjectile(int level)
    {
        GameObject prevProj = selectedProj;

        // 생성할 오브젝트 변경
        switch (StatManager.Instance.l_Weapontype)
        {
            case LWeaponType.Syringe:
                selectedProj = syringeUpgradeProjectiles[level - 1];
                break;
            case LWeaponType.Bow:
                selectedProj = bowUpgradeProjectiles[level - 1];
                break;
            case LWeaponType.Gun:
                selectedProj = gunUpgradeProjectiles[level - 1];
                break;
            case LWeaponType.Rifle:
                selectedProj = rifleUpgradeProjectiles[level - 1];
                break;
        }

        if (level == 1) return;

        if (selectedProj == prevProj) return; // 바뀐 투사체가 이전 투사체와 같을 경우 기존 오브젝트 풀 사용

        // 현재 비활성화로 남아있는 오브젝트들 삭제
        for (int i = 0; i < P_projectiles.childCount; i++)
        {
            Destroy(P_projectiles.GetChild(i).gameObject);
        }

        // 투사체 풀 새로 생성
        _ProjectilesPool = new ObjectPool<Projectile_E>(CreateProjectile, OnGetProjectile, OnReleaseProjectile, OnDestroyProjectile, maxSize: 20);
    }

    #endregion

    #region 코인 풀
    Coin_E CreateCoin()
    {
        // 풀에서 코인 생성
        Coin_E coin = Instantiate(curCoinObject).GetComponent<Coin_E>();

        // 코인 타입에 따라 풀 설정
        coin.SetManagedPool(_CoinPools[(int)coin.myType]);
        coin.transform.parent = P_coins.GetChild((int)coin.myType).transform; // 부모 설정

        return coin;
    }

    public void CreatePool()
    {
        // 새로운 풀 추가
        _CoinPools.Add(new ObjectPool<Coin_E>(CreateCoin, OnGetCoin, OnReleaseCoin, OnDestroyCoin, maxSize: 20));
    }

    void OnGetCoin(Coin_E coin)
    {
        // 오브젝트 활성화
        coin.gameObject.SetActive(true);
    }

    void OnReleaseCoin(Coin_E coin)
    {
        // 오브젝트 비활성화
        coin.gameObject.SetActive(false);
    }

    void OnDestroyCoin(Coin_E coin)
    {
        // 오브젝트 파괴
        Destroy(coin.gameObject);
    }
    #endregion
}
