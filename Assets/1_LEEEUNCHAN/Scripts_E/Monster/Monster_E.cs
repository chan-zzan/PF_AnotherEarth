using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Mathematics;
using Unity.VisualScripting;
//using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Pool;

// 몬스터 스탯 -> scriptableObject 사용
// -> 우리 게임에서는 같은 몬스터가 많이 나오는데 그러면 (스텟 변수 x 몬스터 수)만큼 데이터를 사용하게 됨 -> 메모리 샤용 증가
// -> 하지만 ScriptableObject를 사용하면 데이터의 원본만 저장하여 참조하는 방식으로 사용되어 메모리 소모량을 줄임


public class Monster_E : MonoBehaviour
{
    protected Animator myAnim;
    MonsterSpawner_E spawner;

    public enum STATE
    {
        // 상태를 나눠서 상태에 따라서 행동하도록 함
        Move, UseSkill, Die, GameEnd, Tile
    }
    public STATE myState;

    public enum MonsterType
    {
        Baby, Normal, Cross, Circle, Boss, Special, Tile
    }
    public MonsterType myType;

    protected Rigidbody2D rigid;

    // 몬스터 방향관련
    protected Vector3 originScale;
    protected Vector3 flipScale;

    float curHp; // 현재체력
    public float CurHp
    {
        get => curHp;
        set
        {
            curHp = value;

            if (value <= 0)
            {
                // 몬스터 사망
                ChangeState(STATE.Die);
            }
        }
    }
        

    protected MonsterSkill_E mySkill; // 스킬 스크립트
    float moveTime;

    int monsterNum; // 몬스터 번호
    private int MonsterNum { get => monsterNum; } // 읽기전용

    protected int curCoin; // 몬스터별 코인
    protected float curDamage; // 몬스터별 데미지
    public float CurDamage { get => curDamage; } // 읽기전

    protected SpriteRenderer[] myRenderer;
    protected List<Color> myBaseColors = new List<Color>();

    [SerializeField]
    protected bool fixDirection = false; // 몬스터 방향 고정 여부

    [SerializeField]
    protected bool isRoll = false; // 구르고 있는지 여부(돼지)

    #region FSM
    void ChangeState(STATE s)
    {
        if (myState == STATE.GameEnd) return; // 게임이 끝난경우 다른 상태로 변환 불가

        if (myState == s) return;
        myState = s;

        switch (myState)
        {
            case STATE.Move:
                break;
            case STATE.UseSkill:
                if (mySkill.UseSkills.Count <= 0)
                {
                    ChangeState(STATE.Move); // 스킬이 없는 경우
                }
                else
                {
                    mySkill.ActiveSkill((int)mySkill.UseSkills[UnityEngine.Random.Range(0, mySkill.UseSkills.Count)]); // 자신의 스킬 중 하나를 랜덤하게 발동
                }
                break;
            case STATE.Die:
                GameManager_E.Instance.killCountText.text = ScoreManager.Instance.ScoreToString(++GameManager_E.Instance.killCount); // 몬스터 킬 카운트 증가
                GameManager_E.Instance.weaponSkillSlider.UpdateSliderValue();

                if (myType == MonsterType.Boss && GameManager_E.Instance.curBoss.CurHp <= 0)
                {
                    // 보스가 죽은 경우
                    BossMonsterDead();
                }
                else
                {
                    // 일반 몬스터가 죽은 경우
                    MonsterDead();
                }
                break;
            case STATE.GameEnd:
                break;
        }
    }

    void StateProcess()
    {
        switch (myState)
        {
            case STATE.Move:
                {
                    if (myType == MonsterType.Cross) return;

                    if (!isRoll)
                    {
                        myAnim.Play("walk");
                    }
                    else
                    {
                        myAnim.Play("roll");
                        return;
                    }

                    // 플레이어 기준으로 몬스터의 좌우 방향 변경
                    if (!fixDirection)
                    {
                        // 플레이어와 몬스터 사이의 x축 거리에 따라 방향 전환
                        float dist_x = GameManager_E.Instance.Player.transform.position.x - this.transform.position.x;

                        if (dist_x < 0)
                        {
                            this.transform.localScale = originScale;
                        }
                        else
                        {
                            this.transform.localScale = flipScale;
                        }
                    }

                    if (myType == MonsterType.Boss)
                    {
                        moveTime += Time.deltaTime;

                        if (moveTime > mySkill.skillCycle)
                        {
                            // 일정 시간이 지날 때마다 특이 이동 발생
                            moveTime = 0;
                            ChangeState(STATE.UseSkill);
                        }
                    }                  
                    break;
                }                
            case STATE.UseSkill:
                break;
            case STATE.Die:                                
                break;
            case STATE.GameEnd:
                //myAnim.SetTrigger("idle");
                myAnim.Play("idle");
                rigid.MovePosition(this.transform.position); // 몬스터가 물리영향 받지않고 가만히 있도록 함
                break;

        }
    }

    public void ChangeToMove()
    {
        ChangeState(STATE.Move);
    }

    public void GameEnd()
    {
        ChangeState(STATE.GameEnd);
    }

#endregion

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spawner = GameManager_E.Instance.monsterSpawner;

        // 기본 방향
        originScale = this.transform.localScale; 

        // 플립된 방향
        flipScale = originScale; 
        flipScale.x *= -1;

        if (myType == MonsterType.Boss)
        {
            GameManager_E.Instance.curBoss = this;

            if (!GameManager_E.Instance.finalStage || (GameManager_E.Instance.finalStage && spawner.finalPhaseNum == 9))
            {
                // 파이널 스테이지가 아니거나, 파이널 스테이지의 마지막 페이즈일 경우
                GameManager_E.Instance.bossSpawn = true;
            }
        }
    }

    public void OnDamage(float damage, int weaponType = 0) // 0 이면 원거리, 1이면 근거리에 의한 데미지
    {
        CurHp -= damage;
        SoundManager_E.Instance.EffectSoundPlay(5);

        MonsterSpawner_E spawner = GameManager_E.Instance.monsterSpawner;

        //if (GameManager_E.Instance.bossSpawn)
        //{
        //    if (GameManager_E.Instance.finalStage)
        //    {
        //        if (myType == MonsterType.Boss)
        //        {
        //            // 파이널 라운드 보스 Hp바
        //            GameManager_E.Instance.bossHP.value -= damage / spawner._Phases[spawner.finalPhaseNum].hp;
        //            return;
        //        }                
        //    }

        //    // 보스가 등장한 경우
        //    GameManager_E.Instance.bossHP.value -= damage / spawner.curPhase.hp;
        //    return;
        //}

        if (myType == MonsterType.Boss)
        {
            if (GameManager_E.Instance.finalStage)
            {
                // 파이널 라운드 보스 Hp바
                GameManager_E.Instance.bossHP.value -= damage / spawner._Phases[spawner.finalPhaseNum + 10].hp;
                return;
            }
            else
            {
                GameManager_E.Instance.bossHP.value -= damage / spawner.curPhase.hp;
                return;
            }
        }

        if (myState == STATE.Die) return;

        StartCoroutine(OnKnockback(weaponType));
        
    }

    private void OnEnable()
    {
        myRenderer = GetComponentsInChildren<SpriteRenderer>();
        myAnim = GetComponentInChildren<Animator>();
        mySkill = GetComponent<MonsterSkill_E>();

        // 색들을 저장
        for (int i = 0; i < myRenderer.Length; i++)
        {
            myBaseColors.Add(myRenderer[i].color); // 몬스터의 기본 색상
        }

        ChangeState(STATE.Move);


        if (GameManager_E.Instance.finalStage && myType == MonsterType.Boss)
        {
            // 파이널 스테이지의 보스들인 경우
            CurHp = spawner._Phases[spawner.finalPhaseNum + 10].hp;
            curCoin = spawner._Phases[spawner.finalPhaseNum + 10].coin;
            curDamage = spawner._Phases[spawner.finalPhaseNum + 10].damage;
            
        }
        else
        {
            monsterNum = spawner.curPhase.monsterType;
            CurHp = spawner.curPhase.hp;
            curCoin = spawner.curPhase.coin;
            curDamage = spawner.curPhase.damage;
        }
    }

        

    private void Update()
    {
        StateProcess();
    }

    void FixedUpdate()
    {
        if (myState == STATE.Move)
        {
            // 캐릭터를 따라다니도록 함
            Vector2 dir = GameManager_E.Instance.Player.transform.position - this.transform.position;
            dir.Normalize();


            if (myType == MonsterType.Circle)
            {
                // 원형 몬스터는 속도를 느리게 설정
                rigid.MovePosition(rigid.position + dir * 7 * Time.fixedDeltaTime);
            }
            else
            {
                rigid.MovePosition(rigid.position + dir * spawner.curPhase.speed * Time.fixedDeltaTime);
            }
        }
    }

    #region 오브젝트 풀
    IObjectPool<Monster_E> myPool;

    public void SetManagedPool(IObjectPool<Monster_E> pool)
    {
        // 풀을 PoolManager에 저장
        myPool = pool;
    }

    public void DestroyMonster(int num)
    {
        if (num == 0)
        {
            // 공격에 의해 죽은 경우
            StartCoroutine(DestroyEffect());
        }
        else
        {
            // 보스 생성에 의해 죽은 경우
            myPool.Release(this);
        }
    }

    IEnumerator DestroyEffect()
    {
        float alphaValue = 1;

        while (alphaValue > 0)
        {
            for (int i = 0; i < myRenderer.Length; i++)
            {
                if (myType == MonsterType.Cross)
                {
                    alphaValue -= Time.deltaTime * 3; // 크로스 몬스터는 빨리 사라지도록
                }
                else
                {
                    alphaValue -= Time.deltaTime * 0.5f;
                }                

                myRenderer[i].color = new Color(myBaseColors[i].r, myBaseColors[i].g, myBaseColors[i].b, alphaValue);
            }

            yield return null;
        }

        // 몬스터가 죽었을 때 다시 원래 색으로 바꿈
        for (int i = 0; i < myRenderer.Length; i++)
        {
            myRenderer[i].color = myBaseColors[i];
        }

        if (GameManager_E.Instance.finalStage)
        {
            // 파이널 스테이지 -> 그냥 삭제(다시 쓸 일이 거의 없기 때문)
            Destroy(this.gameObject);
        }
        else
        {
            // 몬스터를 다시 풀에 돌려놓음
            myPool.Release(this);
        }
    }

    #endregion

    IEnumerator OnKnockback(int weaponType = 0) // 0이면 원거리, 1이면 근거리 무기에 의한 넉백
    {
        float knockbackDist;

        if (weaponType == 0)
        {
            // 원거리 무기로 데미지를 입은 경우
            knockbackDist = WeaponSkillManager.Instance.curLweaponKnockBackValue;
        }
        else
        {
            // 근거리 무기로 데미지를 입은 경우
            knockbackDist = WeaponSkillManager.Instance.curSweaponKnockBackValue;
        }

        Vector2 dir = -1 * (GameManager_E.Instance.Player.transform.position - this.transform.position).normalized; // 넉백 방향        

        while (knockbackDist > 0)
        {
            if (CurHp <= 0) break;

            float delta = Time.fixedDeltaTime * 70;
            delta = delta > knockbackDist? knockbackDist : delta;

            this.transform.Translate(dir * delta);

            knockbackDist -= delta;

            yield return new WaitForFixedUpdate();
        }

        // 피격시 색을 바꿈
        for (int j = 0; j < myRenderer.Length; j++)
        {
            myRenderer[j].color = new Color(0.9f, 0.3f, 0.3f, 1f);
        }

        yield return new WaitForSeconds(0.1f);

        // 다시 원래 색으로 변경
        for (int k = 0; k < myRenderer.Length; k++)
        {
            myRenderer[k].color = myBaseColors[k];
        }

        yield return new WaitForSeconds(0.1f);
    }

    void BossMonsterDead()
    {
        // 보스 몬스터가 죽었을 경우
        DestroyMonster(0);

        // 파이널 스테이지인 경우
        if (GameManager_E.Instance.finalStage && spawner.finalPhaseNum != 9)
        {
            GameManager_E.Instance.finalStageCount++; // 죽은 보스의 갯수 증가
            GameManager_E.Instance.bossHP.gameObject.SetActive(false); // 보스 체력 UI 표시 해제
        }
        else
        {
            GameManager_E.Instance.Player.GameWin(); // 플레이어 상태 변경
        }
    }

    void MonsterDead()
    {
        CoinDrop(); // 코인 드랍
        DestroyMonster(0);
    }    

    public void CoinDrop()
    {
        Coin_E coin;
        ObjectPoolManager_E poolManager = GameManager_E.Instance.Pool;

        // 코인의 단위 설정
        int unit = curCoin < 1000 ? 0 : curCoin < 2000 ? 1 : curCoin < 3000 ? 2 : 3;

        // 풀이 아직 생성되지 않은 경우
        while (poolManager._CoinPools.Count < unit + 1)
        {
            poolManager.CreatePool();
        }

        poolManager.curCoinObject = poolManager.CoinObjectList[unit]; // 생성할 오브젝트 설정
        coin = poolManager._CoinPools[unit].Get(); // 오브젝트를 풀에서 가져옴
        coin.transform.position = this.transform.position; // 생성 위치 설정
        coin.value = curCoin; // 값 대입

        //    // 단위에 따라 드랍되는 코인의 이미지 변경
        //    if (curCoin < 1000)
        //    {
        //        // 기본
        //        poolManager.curCoinObject = poolManager.CoinObjectList[0];
        //        coin = poolManager._CoinPools[0].Get();
        //    }
        //    else if (curCoin < 1000000)
        //    {
        //        // K단위
        //        if (poolManager._CoinPools.Count < 2)
        //        {
        //            // 풀이 아직 생성되지 않은 경우
        //            while (poolManager._CoinPools.Count >= 2)
        //            {
        //                poolManager.CreatePool();
        //            }              
        //        }

        //        poolManager.curCoinObject = poolManager.CoinObjectList[1];
        //        coin = poolManager._CoinPools[1].Get();
        //    }
        //    else
        //    {
        //        // M단위 이상
        //        if (poolManager._CoinPools.Count < 3)
        //        {
        //            while (poolManager._CoinPools.Count >= 3)
        //            {
        //                poolManager.CreatePool();
        //            }
        //        }

        //        poolManager.curCoinObject = poolManager.CoinObjectList[2];
        //        coin = poolManager._CoinPools[2].Get();
        //    }

        //coin.transform.position = this.transform.position;
        //coin.value = curCoin; // 값 대입
    }


    float damageTime = 0.8f; // 0.8초에 한번씩 데미지를 입힘

    void DamageToPlayer()
    {
        damageTime = 0.8f; // 데미지 시간 초기화

        if (myType == MonsterType.Cross)
        {
            // cross 몬스터인 경우
            GameManager_E.Instance.Player.CurHP -= curDamage;
        }
        else
        {
            // 캐릭터와 부딫힐 경우
            GameManager_E.Instance.Player.CurHP -= curDamage;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (myState == STATE.Die) return;

        if (collision.CompareTag("Player"))
        {
            DamageToPlayer(); // 데미지를 입힘
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (myState == STATE.Die) return;

        if (collision.CompareTag("Player"))
        {
            damageTime -= Time.deltaTime;

            if (damageTime < 0f)
            {
                DamageToPlayer(); // 데미지를 입힘
            }

        }
    }
}
