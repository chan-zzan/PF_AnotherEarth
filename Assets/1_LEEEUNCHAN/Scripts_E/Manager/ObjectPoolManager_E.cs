using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Pool;

public class ObjectPoolManager_E : MonoBehaviour
{
    // ������Ʈ Ǯ��

    // ������Ʈ ���� ��ġ
    public Transform[] P_Monsters;
    public Transform P_projectiles; 
    public Transform P_coins;

    // ���� Ǯ
    public IObjectPool<Monster_E> _BabyMonsterPools;
    public IObjectPool<Monster_E> _NormalMonsterPools;
    public IObjectPool<Monster_E> _CrossMonsterPools;
    public IObjectPool<Monster_E> _CircleMonsterPools;
    public IObjectPool<Monster_E> _BossMonsterPools;
    public IObjectPool<Monster_E> _SpecialMonsterPools;
    public IObjectPool<Monster_E> _TileMonsterPools;

    // ����ü Ǯ
    public IObjectPool<Projectile_E> _ProjectilesPool;

    GameObject selectedProj; // ������ ����ü
    bool isFirst = true; // ó�� ����ü ��������

    // ����ü �� ���׷��̵� ������Ʈ
    public GameObject[] syringeUpgradeProjectiles;
    public GameObject[] bowUpgradeProjectiles;
    public GameObject[] gunUpgradeProjectiles;
    public GameObject[] rifleUpgradeProjectiles;

    // ���� Ǯ ����Ʈ
    public List<IObjectPool<Coin_E>> _CoinPools = new List<IObjectPool<Coin_E>>();

    // ������ ���� ������Ʈ ����Ʈ
    public List<Coin_E> CoinObjectList;

    [HideInInspector]
    public Coin_E curCoinObject; // ������ ���� ������Ʈ


    private void Awake()
    {
        // ���� Ǯ
        _BabyMonsterPools = new ObjectPool<Monster_E>(CreateMonster, OnGetMonster, OnReleaseMonster, OnDestroyMonster, maxSize: 20);
        _NormalMonsterPools = new ObjectPool<Monster_E>(CreateMonster, OnGetMonster, OnReleaseMonster, OnDestroyMonster, maxSize: 20);
        _CrossMonsterPools = new ObjectPool<Monster_E>(CreateMonster, OnGetMonster, OnReleaseMonster, OnDestroyMonster, maxSize: 30);
        _CircleMonsterPools = new ObjectPool<Monster_E>(CreateMonster, OnGetMonster, OnReleaseMonster, OnDestroyMonster, maxSize: 40);
        _BossMonsterPools = new ObjectPool<Monster_E>(CreateMonster, OnGetMonster, OnReleaseMonster, OnDestroyMonster, maxSize: 1);
        _SpecialMonsterPools = new ObjectPool<Monster_E>(CreateMonster, OnGetMonster, OnReleaseMonster, OnDestroyMonster, maxSize: 50);
        _TileMonsterPools = new ObjectPool<Monster_E>(CreateMonster, OnGetMonster, OnReleaseMonster, OnDestroyMonster, maxSize: 100);

        // ����ü Ǯ
        _ProjectilesPool = new ObjectPool<Projectile_E>(CreateProjectile, OnGetProjectile, OnReleaseProjectile, OnDestroyProjectile, defaultCapacity: 20, maxSize: 100);

        // ���� Ǯ
        _CoinPools.Add(new ObjectPool<Coin_E>(CreateCoin, OnGetCoin, OnReleaseCoin, OnDestroyCoin, maxSize: 20));
    }

    #region ���� Ǯ

    [HideInInspector]
    public GameObject curSpawnMonster; // ������ ����

    Monster_E CreateMonster()
    {
        IObjectPool<Monster_E> _MonsterPools;
        Transform P_monsters;

        // Ǯ���� ���� ����
        Monster_E mon = Instantiate(curSpawnMonster).GetComponent<Monster_E>();

        // ���� ���� ��ġ �� ���� Ǯ ����
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

        // ���� ���� ��ġ �� ���� Ǯ ����
        mon.transform.parent = P_monsters;
        mon.SetManagedPool(_MonsterPools);
        return mon;
    }

    void OnGetMonster(Monster_E mon)
    {
        // ������Ʈ Ȱ��ȭ
        mon.gameObject.SetActive(true);
    }

    void OnReleaseMonster(Monster_E mon)
    {
        // ������Ʈ ��Ȱ��ȭ
        mon.gameObject.SetActive(false);
    }

    void OnDestroyMonster(Monster_E mon)
    {
        // ������Ʈ �ı�
        Destroy(mon.gameObject);
    }
    #endregion

    #region ����ü Ǯ
    Projectile_E CreateProjectile()
    {
        if (isFirst)
        {
            // ó�� �����ϴ� ���
            ChangeProjectile(1);
            isFirst = false;
        }

        // Ǯ���� ����ü ����
        Projectile_E proj = Instantiate(selectedProj, P_projectiles).GetComponent<Projectile_E>();
        proj.SetManagedPool(_ProjectilesPool);
        return proj;
    }

    void OnGetProjectile(Projectile_E proj)
    {
        // ������Ʈ Ȱ��ȭ
        proj.gameObject.SetActive(true);
    }

    void OnReleaseProjectile(Projectile_E proj)
    {
        // ������Ʈ ��Ȱ��ȭ
        proj.gameObject.SetActive(false);
    }

    void OnDestroyProjectile(Projectile_E proj)
    {
        // ������Ʈ �ı�
        Destroy(proj.gameObject);
    }

    public void ChangeProjectile(int level)
    {
        GameObject prevProj = selectedProj;

        // ������ ������Ʈ ����
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

        if (selectedProj == prevProj) return; // �ٲ� ����ü�� ���� ����ü�� ���� ��� ���� ������Ʈ Ǯ ���

        // ���� ��Ȱ��ȭ�� �����ִ� ������Ʈ�� ����
        for (int i = 0; i < P_projectiles.childCount; i++)
        {
            Destroy(P_projectiles.GetChild(i).gameObject);
        }

        // ����ü Ǯ ���� ����
        _ProjectilesPool = new ObjectPool<Projectile_E>(CreateProjectile, OnGetProjectile, OnReleaseProjectile, OnDestroyProjectile, maxSize: 20);
    }

    #endregion

    #region ���� Ǯ
    Coin_E CreateCoin()
    {
        // Ǯ���� ���� ����
        Coin_E coin = Instantiate(curCoinObject).GetComponent<Coin_E>();

        // ���� Ÿ�Կ� ���� Ǯ ����
        coin.SetManagedPool(_CoinPools[(int)coin.myType]);
        coin.transform.parent = P_coins.GetChild((int)coin.myType).transform; // �θ� ����

        return coin;
    }

    public void CreatePool()
    {
        // ���ο� Ǯ �߰�
        _CoinPools.Add(new ObjectPool<Coin_E>(CreateCoin, OnGetCoin, OnReleaseCoin, OnDestroyCoin, maxSize: 20));
    }

    void OnGetCoin(Coin_E coin)
    {
        // ������Ʈ Ȱ��ȭ
        coin.gameObject.SetActive(true);
    }

    void OnReleaseCoin(Coin_E coin)
    {
        // ������Ʈ ��Ȱ��ȭ
        coin.gameObject.SetActive(false);
    }

    void OnDestroyCoin(Coin_E coin)
    {
        // ������Ʈ �ı�
        Destroy(coin.gameObject);
    }
    #endregion
}
