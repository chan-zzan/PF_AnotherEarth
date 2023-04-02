using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public struct RotData
{
    public float angle;
    public float rotDir;
}

public class Player_E : MonoBehaviour
{
    public enum STATE
    {
        // ĳ������ ���¸� ������ ���¿� ���� �ൿ�ϵ��� ��
        None, Play, Die, Win, Stun
    }
    public STATE myState; // ���� �ӽ�(FSM)

    [Space(20)]

    private Rigidbody2D rigid; // ����
    private Coroutine coroutine; // ���� �ڷ�ƾ ����

    float maxHp; // �ִ� ü��
    public float MaxHP { get => maxHp; } // �б� ����

    float curHp; // ���� ü��
    public float CurHP
    {
        get => curHp;
        set
        {
            if (GameManager_E.Instance.unbeatable) return; // ���� ������ ��� ���� ����

            if (value / maxHp < 0.3f)
            {
                // ü���� 30���� �̸��� ���
                GameManager_E.Instance.hpWarningAnim.SetBool("hp_lack", true);
                StartCoroutine(HpWarning());
            }
            else
            {
                // ü���� 30���� �̻��� ���
                GameManager_E.Instance.hpWarningAnim.SetBool("hp_lack", false);
            }

            if (curHp > value)
            {
                // �������� ���� ���
                StartCoroutine(ChangeToPlayerColor());
            }

            curHp = value;

            if (value >= maxHp)
            {
                // ü�� �ѵ� ����
                curHp = maxHp;
            }
            else if (value <= 0)
            {
                // ü���� 0�� �Ǿ� ���� ���
                ChangeState(STATE.Die);
                hpBar.value = 0;
                curHp = 0;
                return;
            }

            hpBar.value = value / maxHp; // hp�� UI ����
        }
    }

    [Space(20)]

    [SerializeField] FloatingJoystick joy; // ���̽�ƽ ������
    [SerializeField] MonsterDetect_E detect; // ���� Ž�� �ݶ��̴�
    [SerializeField] Transform playerDir; // �÷��̾� ����(ĳ���� ����)
    [SerializeField] RectTransform MoveDir; // ������ ����
    [SerializeField] GameObject EasyMoveDir; // ������ ����(1���������� ��� ǥ�ÿ�)
    [SerializeField] Transform spawnPos; // ���� ���� ��ġ
    [SerializeField] Slider hpBar; // hp��
    [SerializeField] Slider dpsBar; // dps��

    public GameObject easyMoveDir { get => EasyMoveDir; }

    Vector2 joyInput; // ���̽�ƽ �Է°�
    public Vector2 JoyInput { get => joyInput; } // �б� ����

    bool isFlip; // �÷��̾��� �¿���� ����

    [HideInInspector]
    public Transform AttackDir; // ���� ����



    #region FSM
   void ChangeState(STATE s)
    {
        if (myState == STATE.Die || myState == STATE.Win) return; // �̱�ų� ������� �ٸ� ���·� ���� �Ұ�

        if (myState == s) return;
        myState = s;

        switch (myState)
        {
            case STATE.Play:
                break;
            case STATE.Die:
                StartCoroutine(GameOver());                
                break;
            case STATE.Win:
                SoundManager_E.Instance.EffectSoundPlay2(1); // ȿ���� on
                GameManager_E.Instance.winPopup.SetActive(true); // ���â ���
                break;
            case STATE.Stun:
                StartCoroutine(stunned(1.0f));
                break;
        }
    }

    void StateProcess()
    {
        switch (myState)
        {
            case STATE.None:
                ChangeState(STATE.Play);
                break;
            case STATE.Play:
                MoveJoystick();
                Attack();
                break;
            case STATE.Die:
                break;
            case STATE.Win:
                break;
            case STATE.Stun:
                break;
        }
    }

    private void Update()
    {
        StateProcess();
    }

    public void GameWin()
    {
        ChangeState(STATE.Win);
    }

    public void GameEnd()
    {
        ChangeState(STATE.Die);
    }

    public void PlayerStun()
    {
        ChangeState(STATE.Stun);
    }

    IEnumerator stunned(float t)
    {
        GameObject curLWeapon = GameManager_E.Instance.Weapon.CurLWeapon; // ���� ���� ����

        curLWeapon.GetComponent<Animator>().SetTrigger("stun"); // �ִϸ��̼� ����

        // �̹��� �ߺ� ����
        SpriteRenderer[] SRs = curLWeapon.GetComponentsInChildren<SpriteRenderer>(); 

        for (int i = 1; i < SRs.Length; i++)
        {
            SRs[i].enabled = false;
        }

        // t�ʰ� ������ �� �ٽ� play ���·� ����
        yield return new WaitForSeconds(t);
        ChangeState(STATE.Play);

        // �̹��� ����
        for(int i = 1; i < SRs.Length; i++)
        {
            SRs[i].enabled = true;
        }
    }

    #endregion

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // �ʱ� HP ����
        hpBar.value = 1;
        maxHp = StatManager.Instance.Hp_Player;
        curHp = StatManager.Instance.Hp_Player;

        if (StageManager.Instance.curStageNum == 1)
        {
            // 1���������� ��� ��� �ȳ��� ���� ǥ��
            EasyMoveDir.SetActive(true);
        }
    }
    

    private void FixedUpdate()
    {
        if (myState == STATE.Play)
        {
            // ���̽�ƽ�� ���� ĳ������ �̵�
            joyInput = new Vector2(joy.Horizontal, joy.Vertical);
            Vector2 nextPos = rigid.position + joyInput.normalized * Time.fixedDeltaTime * StatManager.Instance.Spd_Player;

            switch (GameManager_E.Instance.Map.curMapType)
            {
                case MapType_E.Infinite:
                    {
                        // ���� ���� ���
                    }
                    break;
                case MapType_E.FixedVertical:
                    {
                        // ���� ���� ���� ���
                        nextPos.x = Mathf.Clamp(nextPos.x, -9999.9f, 9999.9f);
                        nextPos.y = Mathf.Clamp(nextPos.y, -90.0f, 85.0f);
                    }
                    break;
                case MapType_E.FixedHorizontal:
                    {
                        // ���� ���� ���� ���
                        
                    }
                    break;
            }

            // ���� ������ �̵� ���� ����
            if (GameManager_E.Instance.bossSpawn || GameManager_E.Instance.finalStage)
            {
                Vector2 limitedMapCenter = GameManager_E.Instance.monsterSpawner.center;

                nextPos.x = Mathf.Clamp(nextPos.x, limitedMapCenter.x - 80, limitedMapCenter.x + 80);
                nextPos.y = Mathf.Clamp(nextPos.y, limitedMapCenter.y - 80, limitedMapCenter.y + 85);
            }


            rigid.MovePosition(nextPos); // �̵� ����
        }
    }

    IEnumerator GameOver()
    {        
        // �״� �ִϸ��̼�
        GameManager_E.Instance.Weapon.CurLWeapon.GetComponent<Animator>().SetTrigger("die");
        SoundManager_E.Instance.EffectSoundPlay2(0); // ȿ���� on

        // ���Ÿ� ���� ����
        Transform[] child = GameManager_E.Instance.Weapon.CurLWeapon.GetComponentsInChildren<Transform>();

        for (int i = 1; i < child.Length; i++)
        {
            child[i].gameObject.SetActive(false);
        }

        // ���� ���� ����
        GameManager_E.Instance.Weapon.CurSWeapon.SetActive(false);

        yield return new WaitForSecondsRealtime(2.0f); // �ִϸ��̼� ���

        GameManager_E.Instance.gameover = true;

        // ���� ������ ����
        Monster_E[] monsters = GameManager_E.Instance.Pool.P_Monsters[0].parent.GetComponentsInChildren<Monster_E>();

        for (int i = 0; i < monsters.Length; i++)
        {
            monsters[i].GameEnd();
        }

        // �÷��̾ �׾��� ��� ����
        if (GameManager_E.Instance.life >= 1)
        {
            Time.timeScale = 0; // �ð� ����
            GameManager_E.Instance.revivalPopup.SetActive(true); // ��Ȱ �˾� ���
            StartCoroutine(CountDown()); // ī��Ʈ �ٿ� ����
        }
        else
        {
            GameManager_E.Instance.defeatPopup.SetActive(true); // �й� �˾� ���
        }
    }

    void Attack()
    {
        // �ѹ��� ����
        if (coroutine == null)
        {
            coroutine = StartCoroutine(StartAttack());
        }
    }

    IEnumerator StartAttack()
    {
        yield return new WaitForSeconds(0.1f);

        StartCoroutine(Shotting());
    }

    void MoveJoystick()
    {
        Vector3 rot;
        Vector3 inputDir;

        if (joy.isMove)
        {
            // �����̴� ���� ��
            inputDir = joyInput;
        }
        else
        {
            // �������� ��
            inputDir = joy.lastPos;
        }

        // ĳ���� �¿� ����
        isFlip = inputDir.x > 0;

        if (isFlip)
        {
            rot = new Vector3(0, 180, 0);
        }
        else
        {
            rot = new Vector3(0, 0, 0);
        }

        playerDir.rotation = Quaternion.Euler(rot); // �¿� ���� ����

        RotData moveData;

        // ���� �̵� ������ �˷��ִ� ȭ��ǥ
        CalculateAngle(this.transform.up, this.transform.right, inputDir, out moveData);
        MoveDir.rotation = Quaternion.Euler(new Vector3(0, 0, moveData.angle * moveData.rotDir)); // ���� ����

        if (AttackDir == null) return;

        // ���� ���� ���� ����
        if (moveData.rotDir < 0)
        {
            AttackDir.localScale = new Vector3(-1, 1, 1); // ���� ����
        }
        else
        {
            AttackDir.localScale = new Vector3(1, 1, 1); // ���� ����
        }

        AttackDir.rotation = Quaternion.Euler(new Vector3(0, 0, moveData.angle * moveData.rotDir));
    }

    void CalculateAngle(Vector3 baseDir, Vector3 flipDir, Vector2 inputDir, out RotData data)
    {
        // ���� ���
        float rad = Mathf.Acos(Vector3.Dot(baseDir, inputDir.normalized)); // �̵��� ���������� ������ ����
        data.angle = 180 * (rad / Mathf.PI); // degree ������ �ٲ�
        data.rotDir = 1.0f; // ȸ�� ���Ⱚ => ������

        if (Vector3.Dot(flipDir, inputDir.normalized) > 0.0f)
        {
            data.rotDir = -1.0f; // ���ʹ���
        }
    }

    IEnumerator Shotting()
    {
        while (myState == STATE.Play)
        {
            // ����ü ����
            if (GameManager_E.Instance.Weapon.CurLWeapon.tag != "Idle") // ���Ÿ����⸦ ������ ��쿡�� ����
            {
                /* ���� �ڵ�
                //Vector3 AttackPos = spawnPos.transform.position; // �߻� ��ġ
                //Quaternion AttackDir = new Quaternion();  // �߻� ���� -> ���� �߻�� �߻��ϴ� ������ ��� ���� ����

                //if (detect.nearestMonster != null)
                //{
                //    // ���Ͱ� ������ ��� -> ���� �������� �߻�
                //    RotData attackData;
                //    Vector3 dir = detect.nearestMonster.position - this.transform.position;

                //    CalculateAngle(this.transform, dir, out attackData); // ���� ���

                //    AttackDir = Quaternion.Euler(new Vector3(0, 0, attackData.angle * attackData.rotDir)); 

                //}
                //else
                //{
                //    // ���Ͱ� �������� ���� ��� -> ĳ���Ͱ� �ٶ󺸴� �������� �߻�
                //    AttackDir = MoveDir.transform.rotation; 
                //}

                // ĳ���Ͱ� �ٶ󺸴� �������� �߻�
                //AttackDir = MoveDir.transform.rotation;

                // ���� ����
                //int shotNum = 4;

                //for (int i = 0; i < shotNum; i++)
                //{
                //    Projectile_E clone = GameManager_E.Instance.Pool._ProjectilesPool.Get(); // ����ü ����

                //    //clone.transform.position = spawnPos.transform.position; // ����ü ��ġ ���� -> ���� �ٸ� ��ġ���� �߻�
                //    clone.transform.position = AttackPos; // ����ü ��ġ ���� -> ��� ���� ��ġ���� �߻�
                //    clone.transform.rotation = AttackDir; // ����ü ���� ����

                //    // pts 100 -> 0.1�� ����(�� ����)
                //    yield return new WaitForSeconds(10.0f / GameManager_E.Instance.curLWeaponPTS);
                //}
                */

                /*
                //int curWeaponLevel = 0; // ������ �ϳ��� ����ü ���� 
                //int curWeaponPTC = 0; // ����ü ����
                //float curWeaponDPS = 0; // 1�ʴ� 1/DPS ���� �߻�


                //switch (GameManager_E.Instance.Weapon.CurLWeapon.tag)
                //{
                //    case "Idle":
                //        break;
                //    case "Syringe":
                //        soundNum = 0;
                //        //curWeaponLevel = StatManager.Instance.Level_Syringe;
                //        curWeaponPTC = StatManager.Instance.Ptc_Syringe;
                //        curWeaponDPS = StatManager.Instance.Dps_Syringe;
                //        break;
                //    case "Bow":
                //        soundNum = 1;
                //        //curWeaponLevel = StatManager.Instance.Level_Bow;
                //        curWeaponPTC = StatManager.Instance.Ptc_Bow;
                //        curWeaponDPS = StatManager.Instance.Dps_Bow;
                //        break;
                //    case "Gun":
                //        soundNum = 2;
                //        //curWeaponLevel = StatManager.Instance.Level_Gun;
                //        curWeaponPTC = StatManager.Instance.Ptc_Gun;
                //        curWeaponDPS = StatManager.Instance.Dps_Gun;
                //        break;
                //    case "Rifle":
                //        soundNum = 3;
                //        //curWeaponLevel = StatManager.Instance.Level_RifleGun;
                //        curWeaponPTC = StatManager.Instance.Ptc_RifleGun;
                //        curWeaponDPS = StatManager.Instance.Dps_RifleGun;
                //        break;
                //}
                */

                float curLDps = WeaponSkillManager.Instance.curLweaponDps != 0 ? WeaponSkillManager.Instance.curLweaponDps : 1;

                // �ִϸ��̼� �ӵ� ����
                GameManager_E.Instance.Weapon.CurLWeaponAnim.SetFloat("dps", 1 / curLDps);

                int soundNum = 0; // ȿ���� ��ȣ

                switch (GameManager_E.Instance.Weapon.CurLWeapon.tag)
                {
                    case "Idle":
                        break;
                    case "Syringe":
                        soundNum = 0;
                        break;
                    case "Bow":
                        soundNum = 1;
                        break;
                    case "Gun":
                        soundNum = 2;
                        break;
                    case "Rifle":
                        soundNum = 3;
                        break;
                }

                int curWeaponPTC = WeaponSkillManager.Instance.curLweaponPtc; // ����ü ����

                // �߻簹�� ����(1~36)
                curWeaponPTC = curWeaponPTC <= 1 ? 1 : curWeaponPTC >= 36 ? 36 : curWeaponPTC;

                int FirstAngle = (curWeaponPTC - 1) * 5; // ó�� ����ü�� �����Ǵ� ����(���ʹ����� ����)
                int angleTick = 10; // �� ����ü ������ ����

                for (int i = 0; i < curWeaponPTC; i++)
                {
                    Projectile_E clone = GameManager_E.Instance.Pool._ProjectilesPool.Get(); // ����ü ����

                    Vector3 SpawnDir = (MoveDir.transform.rotation).eulerAngles - new Vector3(0, 0, FirstAngle); // ��������

                    clone.transform.position = spawnPos.position; // ���� ��ġ
                    clone.transform.rotation = Quaternion.Euler(SpawnDir); // ���� ����

                    FirstAngle -= angleTick; // ���� ����ü�� ���� ����

                    if (GameManager_E.Instance.Weapon.CurLWeapon.CompareTag("Rifle"))
                    {
                        yield return new WaitForSeconds(curLDps);
                    }                    
                }

                SoundManager_E.Instance.EffectSoundPlay(soundNum); // ȿ���� on

                yield return ShowDPS(curLDps);
            }
            else
            {
                yield return new WaitForSeconds(0.01f); // ƨ�� ���� ������
            }
        }

        coroutine = null;
    }

    IEnumerator ShowDPS(float dps)
    {
        float curDps = 0;
        dpsBar.value = 0;

        while (curDps < dps)
        {
            curDps += Time.deltaTime;
            dpsBar.value = curDps / dps;
            yield return null;
        }

        dpsBar.value = 1;
    }


    IEnumerator ChangeToPlayerColor()
    {
        SpriteRenderer[] playerSprites = GameManager_E.Instance.Weapon.CurLWeapon.GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < playerSprites.Length; i++)
        {
            // �������� �Ծ��� �� ĳ���Ͱ� ���������� ����
            playerSprites[i].color = new Color(1.0f, 0.3f, 0.3f, 1.0f);
        }

        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < playerSprites.Length; i++)
        {
            // �ٽ� ���� ������ ���ƿ�
            playerSprites[i].color = Color.white;
        }
    }

    IEnumerator HpWarning()
    {
        yield return new WaitForSeconds(5.0f);

        if (GameManager_E.Instance.hpWarningAnim.GetBool("hp_lack"))
        {
            // 5�� ���� ��� ǥ�ð� ���ӵǸ� �� ���Ŀ��� ����
            GameManager_E.Instance.hpWarningAnim.SetBool("hp_lack", false);
        }
    }

    IEnumerator CountDown()
    {
        float timer = 10.0f;

        while (timer > 0)
        {
            timer -= Time.unscaledDeltaTime; // timeScale�� 0�̾ ����
            GameManager_E.Instance.revival_timer.text = Mathf.CeilToInt(timer).ToString();

            yield return null;
        }

        if (GameManager_E.Instance.revivalPopup.activeSelf)
        {
            // ��Ȱ â�� ��� ���ִ� ��쿡�� ����
            GameManager_E.Instance.revivalPopup.SetActive(false);
            GameManager_E.Instance.defeatPopup.SetActive(true);
        }
    }
}

