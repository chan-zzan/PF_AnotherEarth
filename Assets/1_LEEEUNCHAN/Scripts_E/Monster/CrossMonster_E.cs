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
    float myHP; // ���� ü��

    [SerializeField]
    int Damage; // ���� ������

    [SerializeField]
    int mySpeed; // ���� �ӵ�

    [SerializeField]
    int dropCoin; // ���� óġ ����
    public int DropCoin { get => dropCoin; }

    private void OnEnable()
    {
        myState = STATE.Move;

        axis = GameManager_E.Instance.monsterSpawner.crossAxis;
        myRenderer = this.GetComponents<SpriteRenderer>();
        myAnim = GetComponentInChildren<Animator>();
        mySkill = GetComponent<MonsterSkill_E>();

        // ������ ����
        for (int i = 0; i < myRenderer.Length; i++)
        {
            myBaseColors.Add(myRenderer[i].color); // ������ �⺻ ����
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
        // �ɷ�ġ ����
        CurHp = Hp;
        curCoin = coin;
        curDamage = Damage;
    }

    IEnumerator DestroyCrossMonster()
    {
        float destroyTime = 10.0f;

        while (destroyTime > 0)
        {
            this.transform.Translate(-axis.up * Time.deltaTime * mySpeed); // ���� ������ �̵�
            destroyTime-= Time.deltaTime;
            yield return null;
        }

        // 10�ʵڿ��� ����ִٸ� ����
        this.DestroyMonster(1);
    }
}
