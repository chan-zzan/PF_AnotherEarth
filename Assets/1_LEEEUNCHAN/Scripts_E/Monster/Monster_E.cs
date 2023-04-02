using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Mathematics;
using Unity.VisualScripting;
//using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Pool;

// ���� ���� -> scriptableObject ���
// -> �츮 ���ӿ����� ���� ���Ͱ� ���� �����µ� �׷��� (���� ���� x ���� ��)��ŭ �����͸� ����ϰ� �� -> �޸� ���� ����
// -> ������ ScriptableObject�� ����ϸ� �������� ������ �����Ͽ� �����ϴ� ������� ���Ǿ� �޸� �Ҹ��� ����


public class Monster_E : MonoBehaviour
{
    protected Animator myAnim;
    MonsterSpawner_E spawner;

    public enum STATE
    {
        // ���¸� ������ ���¿� ���� �ൿ�ϵ��� ��
        Move, UseSkill, Die, GameEnd, Tile
    }
    public STATE myState;

    public enum MonsterType
    {
        Baby, Normal, Cross, Circle, Boss, Special, Tile
    }
    public MonsterType myType;

    protected Rigidbody2D rigid;

    // ���� �������
    protected Vector3 originScale;
    protected Vector3 flipScale;

    float curHp; // ����ü��
    public float CurHp
    {
        get => curHp;
        set
        {
            curHp = value;

            if (value <= 0)
            {
                // ���� ���
                ChangeState(STATE.Die);
            }
        }
    }
        

    protected MonsterSkill_E mySkill; // ��ų ��ũ��Ʈ
    float moveTime;

    int monsterNum; // ���� ��ȣ
    private int MonsterNum { get => monsterNum; } // �б�����

    protected int curCoin; // ���ͺ� ����
    protected float curDamage; // ���ͺ� ������
    public float CurDamage { get => curDamage; } // �б���

    protected SpriteRenderer[] myRenderer;
    protected List<Color> myBaseColors = new List<Color>();

    [SerializeField]
    protected bool fixDirection = false; // ���� ���� ���� ����

    [SerializeField]
    protected bool isRoll = false; // ������ �ִ��� ����(����)

    #region FSM
    void ChangeState(STATE s)
    {
        if (myState == STATE.GameEnd) return; // ������ ������� �ٸ� ���·� ��ȯ �Ұ�

        if (myState == s) return;
        myState = s;

        switch (myState)
        {
            case STATE.Move:
                break;
            case STATE.UseSkill:
                if (mySkill.UseSkills.Count <= 0)
                {
                    ChangeState(STATE.Move); // ��ų�� ���� ���
                }
                else
                {
                    mySkill.ActiveSkill((int)mySkill.UseSkills[UnityEngine.Random.Range(0, mySkill.UseSkills.Count)]); // �ڽ��� ��ų �� �ϳ��� �����ϰ� �ߵ�
                }
                break;
            case STATE.Die:
                GameManager_E.Instance.killCountText.text = ScoreManager.Instance.ScoreToString(++GameManager_E.Instance.killCount); // ���� ų ī��Ʈ ����
                GameManager_E.Instance.weaponSkillSlider.UpdateSliderValue();

                if (myType == MonsterType.Boss && GameManager_E.Instance.curBoss.CurHp <= 0)
                {
                    // ������ ���� ���
                    BossMonsterDead();
                }
                else
                {
                    // �Ϲ� ���Ͱ� ���� ���
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

                    // �÷��̾� �������� ������ �¿� ���� ����
                    if (!fixDirection)
                    {
                        // �÷��̾�� ���� ������ x�� �Ÿ��� ���� ���� ��ȯ
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
                            // ���� �ð��� ���� ������ Ư�� �̵� �߻�
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
                rigid.MovePosition(this.transform.position); // ���Ͱ� �������� �����ʰ� ������ �ֵ��� ��
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

        // �⺻ ����
        originScale = this.transform.localScale; 

        // �ø��� ����
        flipScale = originScale; 
        flipScale.x *= -1;

        if (myType == MonsterType.Boss)
        {
            GameManager_E.Instance.curBoss = this;

            if (!GameManager_E.Instance.finalStage || (GameManager_E.Instance.finalStage && spawner.finalPhaseNum == 9))
            {
                // ���̳� ���������� �ƴϰų�, ���̳� ���������� ������ �������� ���
                GameManager_E.Instance.bossSpawn = true;
            }
        }
    }

    public void OnDamage(float damage, int weaponType = 0) // 0 �̸� ���Ÿ�, 1�̸� �ٰŸ��� ���� ������
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
        //            // ���̳� ���� ���� Hp��
        //            GameManager_E.Instance.bossHP.value -= damage / spawner._Phases[spawner.finalPhaseNum].hp;
        //            return;
        //        }                
        //    }

        //    // ������ ������ ���
        //    GameManager_E.Instance.bossHP.value -= damage / spawner.curPhase.hp;
        //    return;
        //}

        if (myType == MonsterType.Boss)
        {
            if (GameManager_E.Instance.finalStage)
            {
                // ���̳� ���� ���� Hp��
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

        // ������ ����
        for (int i = 0; i < myRenderer.Length; i++)
        {
            myBaseColors.Add(myRenderer[i].color); // ������ �⺻ ����
        }

        ChangeState(STATE.Move);


        if (GameManager_E.Instance.finalStage && myType == MonsterType.Boss)
        {
            // ���̳� ���������� �������� ���
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
            // ĳ���͸� ����ٴϵ��� ��
            Vector2 dir = GameManager_E.Instance.Player.transform.position - this.transform.position;
            dir.Normalize();


            if (myType == MonsterType.Circle)
            {
                // ���� ���ʹ� �ӵ��� ������ ����
                rigid.MovePosition(rigid.position + dir * 7 * Time.fixedDeltaTime);
            }
            else
            {
                rigid.MovePosition(rigid.position + dir * spawner.curPhase.speed * Time.fixedDeltaTime);
            }
        }
    }

    #region ������Ʈ Ǯ
    IObjectPool<Monster_E> myPool;

    public void SetManagedPool(IObjectPool<Monster_E> pool)
    {
        // Ǯ�� PoolManager�� ����
        myPool = pool;
    }

    public void DestroyMonster(int num)
    {
        if (num == 0)
        {
            // ���ݿ� ���� ���� ���
            StartCoroutine(DestroyEffect());
        }
        else
        {
            // ���� ������ ���� ���� ���
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
                    alphaValue -= Time.deltaTime * 3; // ũ�ν� ���ʹ� ���� ���������
                }
                else
                {
                    alphaValue -= Time.deltaTime * 0.5f;
                }                

                myRenderer[i].color = new Color(myBaseColors[i].r, myBaseColors[i].g, myBaseColors[i].b, alphaValue);
            }

            yield return null;
        }

        // ���Ͱ� �׾��� �� �ٽ� ���� ������ �ٲ�
        for (int i = 0; i < myRenderer.Length; i++)
        {
            myRenderer[i].color = myBaseColors[i];
        }

        if (GameManager_E.Instance.finalStage)
        {
            // ���̳� �������� -> �׳� ����(�ٽ� �� ���� ���� ���� ����)
            Destroy(this.gameObject);
        }
        else
        {
            // ���͸� �ٽ� Ǯ�� ��������
            myPool.Release(this);
        }
    }

    #endregion

    IEnumerator OnKnockback(int weaponType = 0) // 0�̸� ���Ÿ�, 1�̸� �ٰŸ� ���⿡ ���� �˹�
    {
        float knockbackDist;

        if (weaponType == 0)
        {
            // ���Ÿ� ����� �������� ���� ���
            knockbackDist = WeaponSkillManager.Instance.curLweaponKnockBackValue;
        }
        else
        {
            // �ٰŸ� ����� �������� ���� ���
            knockbackDist = WeaponSkillManager.Instance.curSweaponKnockBackValue;
        }

        Vector2 dir = -1 * (GameManager_E.Instance.Player.transform.position - this.transform.position).normalized; // �˹� ����        

        while (knockbackDist > 0)
        {
            if (CurHp <= 0) break;

            float delta = Time.fixedDeltaTime * 70;
            delta = delta > knockbackDist? knockbackDist : delta;

            this.transform.Translate(dir * delta);

            knockbackDist -= delta;

            yield return new WaitForFixedUpdate();
        }

        // �ǰݽ� ���� �ٲ�
        for (int j = 0; j < myRenderer.Length; j++)
        {
            myRenderer[j].color = new Color(0.9f, 0.3f, 0.3f, 1f);
        }

        yield return new WaitForSeconds(0.1f);

        // �ٽ� ���� ������ ����
        for (int k = 0; k < myRenderer.Length; k++)
        {
            myRenderer[k].color = myBaseColors[k];
        }

        yield return new WaitForSeconds(0.1f);
    }

    void BossMonsterDead()
    {
        // ���� ���Ͱ� �׾��� ���
        DestroyMonster(0);

        // ���̳� ���������� ���
        if (GameManager_E.Instance.finalStage && spawner.finalPhaseNum != 9)
        {
            GameManager_E.Instance.finalStageCount++; // ���� ������ ���� ����
            GameManager_E.Instance.bossHP.gameObject.SetActive(false); // ���� ü�� UI ǥ�� ����
        }
        else
        {
            GameManager_E.Instance.Player.GameWin(); // �÷��̾� ���� ����
        }
    }

    void MonsterDead()
    {
        CoinDrop(); // ���� ���
        DestroyMonster(0);
    }    

    public void CoinDrop()
    {
        Coin_E coin;
        ObjectPoolManager_E poolManager = GameManager_E.Instance.Pool;

        // ������ ���� ����
        int unit = curCoin < 1000 ? 0 : curCoin < 2000 ? 1 : curCoin < 3000 ? 2 : 3;

        // Ǯ�� ���� �������� ���� ���
        while (poolManager._CoinPools.Count < unit + 1)
        {
            poolManager.CreatePool();
        }

        poolManager.curCoinObject = poolManager.CoinObjectList[unit]; // ������ ������Ʈ ����
        coin = poolManager._CoinPools[unit].Get(); // ������Ʈ�� Ǯ���� ������
        coin.transform.position = this.transform.position; // ���� ��ġ ����
        coin.value = curCoin; // �� ����

        //    // ������ ���� ����Ǵ� ������ �̹��� ����
        //    if (curCoin < 1000)
        //    {
        //        // �⺻
        //        poolManager.curCoinObject = poolManager.CoinObjectList[0];
        //        coin = poolManager._CoinPools[0].Get();
        //    }
        //    else if (curCoin < 1000000)
        //    {
        //        // K����
        //        if (poolManager._CoinPools.Count < 2)
        //        {
        //            // Ǯ�� ���� �������� ���� ���
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
        //        // M���� �̻�
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
        //coin.value = curCoin; // �� ����
    }


    float damageTime = 0.8f; // 0.8�ʿ� �ѹ��� �������� ����

    void DamageToPlayer()
    {
        damageTime = 0.8f; // ������ �ð� �ʱ�ȭ

        if (myType == MonsterType.Cross)
        {
            // cross ������ ���
            GameManager_E.Instance.Player.CurHP -= curDamage;
        }
        else
        {
            // ĳ���Ϳ� �΋H�� ���
            GameManager_E.Instance.Player.CurHP -= curDamage;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (myState == STATE.Die) return;

        if (collision.CompareTag("Player"))
        {
            DamageToPlayer(); // �������� ����
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
                DamageToPlayer(); // �������� ����
            }

        }
    }
}
