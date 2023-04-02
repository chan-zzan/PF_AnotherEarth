using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Animations;
using UnityEngine;

public class CrossMonster_E : Monster_E
{
    Transform axis;
    Coroutine myCoroutine;

    [SerializeField]
    float myHP; // 몬스터 체력

    [SerializeField]
    int Damage; // 몬스터 데미지

    [SerializeField]
    int mySpeed; // 몬스터 속도

    [SerializeField]
    int dropCoin; // 몬스터 처치 코인
    public int DropCoin { get => dropCoin; }

    private void OnEnable()
    {
        myState = STATE.Move;

        axis = GameManager_E.Instance.monsterSpawner.crossAxis;
        myRenderer = this.GetComponents<SpriteRenderer>();
        myAnim = GetComponentInChildren<Animator>();
        mySkill = GetComponent<MonsterSkill_E>();

        // 색들을 저장
        for (int i = 0; i < myRenderer.Length; i++)
        {
            myBaseColors.Add(myRenderer[i].color); // 몬스터의 기본 색상
        }

        if (axis.rotation.eulerAngles.z < 180)
        {
            myRenderer[0].flipX = true;
        }
        else
        {
            myRenderer[0].flipX = false;
        }

        myHP = GameManager_E.Instance.monsterSpawner.curPhase.hp;
        dropCoin = GameManager_E.Instance.monsterSpawner.curPhase.coin;

        ApplyStatus(myHP, dropCoin);

        myCoroutine = StartCoroutine(DestroyCrossMonster());
    }

    private void OnDisable()
    {
        StopCoroutine(myCoroutine);
    }

    public void ApplyStatus(float Hp, int coin)
    {
        // 능력치 대입
        CurHp = Hp;
        curCoin = coin;
        curDamage = Damage;
    }

    IEnumerator DestroyCrossMonster()
    {
        float destroyTime = 10.0f;

        while (destroyTime > 0)
        {
            this.transform.Translate(-axis.up * Time.deltaTime * mySpeed); // 축의 방향대로 이동
            destroyTime-= Time.deltaTime;
            yield return null;
        }

        // 10초뒤에도 살아있다면 삭제
        this.DestroyMonster(1);
    }
}
