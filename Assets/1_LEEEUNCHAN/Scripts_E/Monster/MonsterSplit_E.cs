using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSplit_E : MonoBehaviour
{
    [SerializeField]
    SpecialMonster_E monster; // 현재 특수몬스터의 스크립트

    [SerializeField]
    GameObject nextSplit; // 다음으로 생성될 오브젝트

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

        // 다음 몬스터 생성
        GameObject nextMonster = Instantiate(nextSplit, this.transform.position, this.transform.rotation);
    }

    //void CoinDrop()
    //{
    //    Coin_E coin = GameManager_E.Instance.Pool._MineralsPool.Get();
    //    coin.transform.position = this.transform.position;
    //    coin.value = monster.DropCoin;
    //}
}
