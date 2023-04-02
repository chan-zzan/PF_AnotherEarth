using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSplit_E : MonoBehaviour
{
    [SerializeField]
    SpecialMonster_E monster; // ���� Ư�������� ��ũ��Ʈ

    [SerializeField]
    GameObject nextSplit; // �������� ������ ������Ʈ

    private void Update()
    {
        if (monster.myState == Monster_E.STATE.Die)
        {
            NextMonsterSpawn();
            monster.CoinDrop();
            Destroy(this.gameObject);
        }
    }


    void NextMonsterSpawn()
    {
        if (nextSplit == null) return;

        // ���� ���� ����
        GameObject nextMonster = Instantiate(nextSplit, this.transform.position, this.transform.rotation);
    }

    //void CoinDrop()
    //{
    //    Coin_E coin = GameManager_E.Instance.Pool._MineralsPool.Get();
    //    coin.transform.position = this.transform.position;
    //    coin.value = monster.DropCoin;
    //}
}
