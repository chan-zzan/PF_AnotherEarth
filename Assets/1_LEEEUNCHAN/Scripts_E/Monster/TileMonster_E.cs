using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Monster_E;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class TileMonster_E : SpecialMonster_E
{
    private void OnEnable()
    {
        myState = STATE.Tile;

        myRenderer = GetComponentsInChildren<SpriteRenderer>();
        myAnim = GetComponentInChildren<Animator>();
        mySkill = GetComponent<MonsterSkill_E>();

        // 색들을 저장
        for (int i = 0; i < myRenderer.Length; i++)
        {
            myBaseColors.Add(myRenderer[i].color); // 몬스터의 기본 색상
        }

        // 스탯 적용
        ApplyStatus(myHP, dropCoin, myDamage);

        StartCoroutine(Destory());
    }

    IEnumerator Destory()
    {
        // 20초후에 삭제
        yield return new WaitForSeconds(20.0f);
        base.DestroyMonster(1);
    }
}
