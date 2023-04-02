using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpecialMonster_E : Monster_E
{
    public enum SpawnType // 스폰 방식
    {
        OneRandom, // 랜덤 위치에 하나씩 소환
        ManyRamdom, //  랜덤 위치에서 한번에 소환
        ManySpecific, // 특정 위치에서 한번에 소환
    }
    public SpawnType mySpawnType;

    public enum Dir // 스폰 방향
    {
        Idle, Up, Down, Right, Left, Axis, Random
    }
    public Dir myDir = Dir.Idle;

    public float[] mySpawnTime; // 몬스터 별 생성 시간

    int myStageNum; // 몬스터 별 스테이지 번호
    public int MyStageNum { get => myStageNum; }

    [SerializeField]
    protected float myHP; // 몬스터 체력

    [SerializeField]
    protected int myDamage; // 몬스터 데미지

    [SerializeField]
    protected int mySpeed; // 몬스터 속도

    [SerializeField]
    protected int dropCoin; // 몬스터 처치 코인
    public int DropCoin { get => dropCoin; }

    private void OnEnable()
    {
        // 상태 변경
        myState = STATE.Move;

        myRenderer = GetComponentsInChildren<SpriteRenderer>();
        myAnim = GetComponentInChildren<Animator>();
        mySkill = GetComponent<MonsterSkill_E>();

        // 색들을 저장
        for (int i = 0; i < myRenderer.Length; i++)
        {
            myBaseColors.Add(myRenderer[i].color); // 몬스터의 기본 색상
        }

        // 자신의 스테이지 번호 저장
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
            // 특별몬스터 강아지 -> 시작시 부메랑 생성
            //Instantiate(mySkill.throwObject, this.transform.position, this.transform.rotation);
        }

    }

    public void ApplyStatus(float Hp, int coin, int damage)
    {
        // 능력치 대입
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
                    dir = GameManager_E.Instance.Player.transform.position - this.transform.position; // 플레이어 방향으로 이동
                    break;
                case Dir.Up:
                    dir = Vector2.up; // 위쪽방향으로 이동
                    break;
                case Dir.Down:
                    dir = Vector2.down; // 아래쪽방향으로 이동
                    break;
                case Dir.Right:
                    dir = Vector2.right; // 오른쪽방향으로 이동
                    break;
                case Dir.Left:
                    dir = Vector2.left; // 왼쪽방향으로 이동
                    break;
                case Dir.Axis:
                    dir = GameManager_E.Instance.monsterSpawner.specialWaves_H[1].transform.up; // 축의 방향대로 이동
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
