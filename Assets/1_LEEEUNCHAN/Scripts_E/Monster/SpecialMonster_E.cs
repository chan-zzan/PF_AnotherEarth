using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpecialMonster_E : Monster_E
{
    public enum SpawnType // ���� ���
    {
        OneRandom, // ���� ��ġ�� �ϳ��� ��ȯ
        ManyRamdom, //  ���� ��ġ���� �ѹ��� ��ȯ
        ManySpecific, // Ư�� ��ġ���� �ѹ��� ��ȯ
    }
    public SpawnType mySpawnType;

    public enum Dir // ���� ����
    {
        Idle, Up, Down, Right, Left, Axis, Random
    }
    public Dir myDir = Dir.Idle;

    public float[] mySpawnTime; // ���� �� ���� �ð�

    int myStageNum; // ���� �� �������� ��ȣ
    public int MyStageNum { get => myStageNum; }

    [SerializeField]
    protected float myHP; // ���� ü��

    [SerializeField]
    protected int myDamage; // ���� ������

    [SerializeField]
    protected int mySpeed; // ���� �ӵ�

    [SerializeField]
    protected int dropCoin; // ���� óġ ����
    public int DropCoin { get => dropCoin; }

    private void OnEnable()
    {
        // ���� ����
        myState = STATE.Move;

        myRenderer = GetComponentsInChildren<SpriteRenderer>();
        myAnim = GetComponentInChildren<Animator>();
        mySkill = GetComponent<MonsterSkill_E>();

        // ������ ����
        for (int i = 0; i < myRenderer.Length; i++)
        {
            myBaseColors.Add(myRenderer[i].color); // ������ �⺻ ����
        }

        // �ڽ��� �������� ��ȣ ����
        myStageNum = StageManager.Instance.curStageNum;

        ApplyStatus(myHP, dropCoin, myDamage);

        if (fixDirection)
        {
            StartCoroutine(DestroyFixedDirectionMonster());
        }

        if (isRoll)
        {
            StartCoroutine(Rolling());
        }

        if ((myType == MonsterType.Special && mySkill.monsterName == MonsterName.Dog))
        {
            // Ư������ ������ -> ���۽� �θ޶� ����
            //Instantiate(mySkill.throwObject, this.transform.position, this.transform.rotation);
        }

    }

    public void ApplyStatus(float Hp, int coin, int damage)
    {
        // �ɷ�ġ ����
        CurHp = Hp;
        curCoin = coin;
        curDamage = damage;
    }

    void FixedUpdate()
    {
        if (myState == STATE.Move)
        {
            Vector2 dir = new Vector2();

            switch(myDir)
            {
                case Dir.Idle:
                    dir = GameManager_E.Instance.Player.transform.position - this.transform.position; // �÷��̾� �������� �̵�
                    break;
                case Dir.Up:
                    dir = Vector2.up; // ���ʹ������� �̵�
                    break;
                case Dir.Down:
                    dir = Vector2.down; // �Ʒ��ʹ������� �̵�
                    break;
                case Dir.Right:
                    dir = Vector2.right; // �����ʹ������� �̵�
                    break;
                case Dir.Left:
                    dir = Vector2.left; // ���ʹ������� �̵�
                    break;
                case Dir.Axis:
                    dir = GameManager_E.Instance.monsterSpawner.specialWaves_H[1].transform.up; // ���� ������ �̵�
                    break;
                case Dir.Random:
                    print(this.GetComponentsInChildren<Animator>()[1].name);
                    this.GetComponentsInChildren<Animator>()[0].Play("walk");
                    return;
            }

            rigid.MovePosition(rigid.position + dir.normalized * mySpeed * Time.fixedDeltaTime);
        }
    }

    IEnumerator DestroyFixedDirectionMonster()
    {
        yield return new WaitForSeconds(10.0f);
        Destroy(this.gameObject);
    }

    IEnumerator Rolling()
    {
        print("roll start");

        float rollTimer = 10.0f;

        while (rollTimer > 0)
        {
            rollTimer -= Time.deltaTime;
            yield return null;
        }

        mySpeed = 14;

        print("roll end");
        isRoll = false;

        yield return null;
    }
}
