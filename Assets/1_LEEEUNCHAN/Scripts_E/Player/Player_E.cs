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
        // 캐릭터의 상태를 나눠서 상태에 따라서 행동하도록 함
        None, Play, Die, Win, Stun
    }
    public STATE myState; // 상태 머신(FSM)

    [Space(20)]

    private Rigidbody2D rigid; // 물리
    private Coroutine coroutine; // 공격 코루틴 저장

    float maxHp; // 최대 체력
    public float MaxHP { get => maxHp; } // 읽기 전용

    float curHp; // 현재 체력
    public float CurHP
    {
        get => curHp;
        set
        {
            if (GameManager_E.Instance.unbeatable) return; // 무적 상태인 경우 동작 안함

            if (value / maxHp < 0.3f)
            {
                // 체력이 30프로 미만인 경우
                GameManager_E.Instance.hpWarningAnim.SetBool("hp_lack", true);
                StartCoroutine(HpWarning());
            }
            else
            {
                // 체력이 30프로 이상인 경우
                GameManager_E.Instance.hpWarningAnim.SetBool("hp_lack", false);
            }

            if (curHp > value)
            {
                // 데미지를 입은 경우
                StartCoroutine(ChangeToPlayerColor());
            }

            curHp = value;

            if (value >= maxHp)
            {
                // 체력 한도 조절
                curHp = maxHp;
            }
            else if (value <= 0)
            {
                // 체력이 0이 되어 죽은 경우
                ChangeState(STATE.Die);
                hpBar.value = 0;
                curHp = 0;
                return;
            }

            hpBar.value = value / maxHp; // hp바 UI 적용
        }
    }

    [Space(20)]

    [SerializeField] FloatingJoystick joy; // 조이스틱 프리팹
    [SerializeField] MonsterDetect_E detect; // 몬스터 탐지 콜라이더
    [SerializeField] Transform playerDir; // 플레이어 방향(캐릭터 방향)
    [SerializeField] RectTransform MoveDir; // 움직일 방향
    [SerializeField] GameObject EasyMoveDir; // 움직일 방향(1스테이지의 경로 표시용)
    [SerializeField] Transform spawnPos; // 공격 시작 위치
    [SerializeField] Slider hpBar; // hp바
    [SerializeField] Slider dpsBar; // dps바

    public GameObject easyMoveDir { get => EasyMoveDir; }

    Vector2 joyInput; // 조이스틱 입력값
    public Vector2 JoyInput { get => joyInput; } // 읽기 전용

    bool isFlip; // 플레이어의 좌우반전 여부

    [HideInInspector]
    public Transform AttackDir; // 공격 방향



    #region FSM
   void ChangeState(STATE s)
    {
        if (myState == STATE.Die || myState == STATE.Win) return; // 이기거나 졌을경우 다른 상태로 변경 불가

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
                SoundManager_E.Instance.EffectSoundPlay2(1); // 효과음 on
                GameManager_E.Instance.winPopup.SetActive(true); // 결과창 띄움
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
        GameObject curLWeapon = GameManager_E.Instance.Weapon.CurLWeapon; // 현재 장착 무기

        curLWeapon.GetComponent<Animator>().SetTrigger("stun"); // 애니메이션 동작

        // 이미지 중복 방지
        SpriteRenderer[] SRs = curLWeapon.GetComponentsInChildren<SpriteRenderer>(); 

        for (int i = 1; i < SRs.Length; i++)
        {
            SRs[i].enabled = false;
        }

        // t초간 기절한 뒤 다시 play 상태로 변경
        yield return new WaitForSeconds(t);
        ChangeState(STATE.Play);

        // 이미지 복구
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
        // 초기 HP 설정
        hpBar.value = 1;
        maxHp = StatManager.Instance.Hp_Player;
        curHp = StatManager.Instance.Hp_Player;

        if (StageManager.Instance.curStageNum == 1)
        {
            // 1스테이지일 경우 경로 안내용 점선 표시
            EasyMoveDir.SetActive(true);
        }
    }
    

    private void FixedUpdate()
    {
        if (myState == STATE.Play)
        {
            // 조이스틱에 따른 캐릭터의 이동
            joyInput = new Vector2(joy.Horizontal, joy.Vertical);
            Vector2 nextPos = rigid.position + joyInput.normalized * Time.fixedDeltaTime * StatManager.Instance.Spd_Player;

            switch (GameManager_E.Instance.Map.curMapType)
            {
                case MapType_E.Infinite:
                    {
                        // 무한 맵인 경우
                    }
                    break;
                case MapType_E.FixedVertical:
                    {
                        // 세로 고정 맵인 경우
                        nextPos.x = Mathf.Clamp(nextPos.x, -9999.9f, 9999.9f);
                        nextPos.y = Mathf.Clamp(nextPos.y, -90.0f, 85.0f);
                    }
                    break;
                case MapType_E.FixedHorizontal:
                    {
                        // 가로 고정 맵인 경우
                        
                    }
                    break;
            }

            // 보스 스폰시 이동 구역 제한
            if (GameManager_E.Instance.bossSpawn || GameManager_E.Instance.finalStage)
            {
                Vector2 limitedMapCenter = GameManager_E.Instance.monsterSpawner.center;

                nextPos.x = Mathf.Clamp(nextPos.x, limitedMapCenter.x - 80, limitedMapCenter.x + 80);
                nextPos.y = Mathf.Clamp(nextPos.y, limitedMapCenter.y - 80, limitedMapCenter.y + 85);
            }


            rigid.MovePosition(nextPos); // 이동 적용
        }
    }

    IEnumerator GameOver()
    {        
        // 죽는 애니메이션
        GameManager_E.Instance.Weapon.CurLWeapon.GetComponent<Animator>().SetTrigger("die");
        SoundManager_E.Instance.EffectSoundPlay2(0); // 효과음 on

        // 원거리 무기 해제
        Transform[] child = GameManager_E.Instance.Weapon.CurLWeapon.GetComponentsInChildren<Transform>();

        for (int i = 1; i < child.Length; i++)
        {
            child[i].gameObject.SetActive(false);
        }

        // 근접 무기 해제
        GameManager_E.Instance.Weapon.CurSWeapon.SetActive(false);

        yield return new WaitForSecondsRealtime(2.0f); // 애니메이션 대기

        GameManager_E.Instance.gameover = true;

        // 몬스터 움직임 멈춤
        Monster_E[] monsters = GameManager_E.Instance.Pool.P_Monsters[0].parent.GetComponentsInChildren<Monster_E>();

        for (int i = 0; i < monsters.Length; i++)
        {
            monsters[i].GameEnd();
        }

        // 플레이어가 죽었을 경우 실행
        if (GameManager_E.Instance.life >= 1)
        {
            Time.timeScale = 0; // 시간 멈춤
            GameManager_E.Instance.revivalPopup.SetActive(true); // 부활 팝업 띄움
            StartCoroutine(CountDown()); // 카운트 다운 시작
        }
        else
        {
            GameManager_E.Instance.defeatPopup.SetActive(true); // 패배 팝업 띄움
        }
    }

    void Attack()
    {
        // 한번만 실행
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
            // 움직이는 중일 때
            inputDir = joyInput;
        }
        else
        {
            // 멈춰있을 때
            inputDir = joy.lastPos;
        }

        // 캐릭터 좌우 반전
        isFlip = inputDir.x > 0;

        if (isFlip)
        {
            rot = new Vector3(0, 180, 0);
        }
        else
        {
            rot = new Vector3(0, 0, 0);
        }

        playerDir.rotation = Quaternion.Euler(rot); // 좌우 반전 적용

        RotData moveData;

        // 현재 이동 방향을 알려주는 화살표
        CalculateAngle(this.transform.up, this.transform.right, inputDir, out moveData);
        MoveDir.rotation = Quaternion.Euler(new Vector3(0, 0, moveData.angle * moveData.rotDir)); // 방향 적용

        if (AttackDir == null) return;

        // 현재 공격 방향 설정
        if (moveData.rotDir < 0)
        {
            AttackDir.localScale = new Vector3(-1, 1, 1); // 방향 설정
        }
        else
        {
            AttackDir.localScale = new Vector3(1, 1, 1); // 방향 설정
        }

        AttackDir.rotation = Quaternion.Euler(new Vector3(0, 0, moveData.angle * moveData.rotDir));
    }

    void CalculateAngle(Vector3 baseDir, Vector3 flipDir, Vector2 inputDir, out RotData data)
    {
        // 각도 계산
        float rad = Mathf.Acos(Vector3.Dot(baseDir, inputDir.normalized)); // 이동할 지점까지의 각도를 구함
        data.angle = 180 * (rad / Mathf.PI); // degree 각도로 바꿈
        data.rotDir = 1.0f; // 회전 방향값 => 오른쪽

        if (Vector3.Dot(flipDir, inputDir.normalized) > 0.0f)
        {
            data.rotDir = -1.0f; // 왼쪽방향
        }
    }

    IEnumerator Shotting()
    {
        while (myState == STATE.Play)
        {
            // 투사체 생성
            if (GameManager_E.Instance.Weapon.CurLWeapon.tag != "Idle") // 원거리무기를 장착한 경우에만 동작
            {
                /* 이전 코드
                //Vector3 AttackPos = spawnPos.transform.position; // 발사 위치
                //Quaternion AttackDir = new Quaternion();  // 발사 방향 -> 연속 발사시 발사하는 방향을 모두 같게 설정

                //if (detect.nearestMonster != null)
                //{
                //    // 몬스터가 감지된 경우 -> 몬스터 방향으로 발사
                //    RotData attackData;
                //    Vector3 dir = detect.nearestMonster.position - this.transform.position;

                //    CalculateAngle(this.transform, dir, out attackData); // 각도 계산

                //    AttackDir = Quaternion.Euler(new Vector3(0, 0, attackData.angle * attackData.rotDir)); 

                //}
                //else
                //{
                //    // 몬스터가 감지되지 않은 경우 -> 캐릭터가 바라보는 방향으로 발사
                //    AttackDir = MoveDir.transform.rotation; 
                //}

                // 캐릭터가 바라보는 방향으로 발사
                //AttackDir = MoveDir.transform.rotation;

                // 연사 로직
                //int shotNum = 4;

                //for (int i = 0; i < shotNum; i++)
                //{
                //    Projectile_E clone = GameManager_E.Instance.Pool._ProjectilesPool.Get(); // 투사체 생성

                //    //clone.transform.position = spawnPos.transform.position; // 투사체 위치 설정 -> 각기 다른 위치에서 발사
                //    clone.transform.position = AttackPos; // 투사체 위치 설정 -> 모두 같은 위치에서 발사
                //    clone.transform.rotation = AttackDir; // 투사체 방향 설정

                //    // pts 100 -> 0.1이 적당(내 생각)
                //    yield return new WaitForSeconds(10.0f / GameManager_E.Instance.curLWeaponPTS);
                //}
                */

                /*
                //int curWeaponLevel = 0; // 레벨당 하나씩 투사체 증가 
                //int curWeaponPTC = 0; // 투사체 갯수
                //float curWeaponDPS = 0; // 1초당 1/DPS 개씩 발사


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

                // 애니메이션 속도 적용
                GameManager_E.Instance.Weapon.CurLWeaponAnim.SetFloat("dps", 1 / curLDps);

                int soundNum = 0; // 효과음 번호

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

                int curWeaponPTC = WeaponSkillManager.Instance.curLweaponPtc; // 투사체 갯수

                // 발사갯수 제한(1~36)
                curWeaponPTC = curWeaponPTC <= 1 ? 1 : curWeaponPTC >= 36 ? 36 : curWeaponPTC;

                int FirstAngle = (curWeaponPTC - 1) * 5; // 처음 투사체가 생성되는 각도(왼쪽방향이 기준)
                int angleTick = 10; // 각 투사체 사이의 각도

                for (int i = 0; i < curWeaponPTC; i++)
                {
                    Projectile_E clone = GameManager_E.Instance.Pool._ProjectilesPool.Get(); // 투사체 생성

                    Vector3 SpawnDir = (MoveDir.transform.rotation).eulerAngles - new Vector3(0, 0, FirstAngle); // 생성방향

                    clone.transform.position = spawnPos.position; // 생성 위치
                    clone.transform.rotation = Quaternion.Euler(SpawnDir); // 각도 적용

                    FirstAngle -= angleTick; // 다음 투사체의 생성 각도

                    if (GameManager_E.Instance.Weapon.CurLWeapon.CompareTag("Rifle"))
                    {
                        yield return new WaitForSeconds(curLDps);
                    }                    
                }

                SoundManager_E.Instance.EffectSoundPlay(soundNum); // 효과음 on

                yield return ShowDPS(curLDps);
            }
            else
            {
                yield return new WaitForSeconds(0.01f); // 튕김 방지 딜레이
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
            // 데미지를 입었을 때 캐릭터가 붉은색으로 변함
            playerSprites[i].color = new Color(1.0f, 0.3f, 0.3f, 1.0f);
        }

        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < playerSprites.Length; i++)
        {
            // 다시 원래 색으로 돌아옴
            playerSprites[i].color = Color.white;
        }
    }

    IEnumerator HpWarning()
    {
        yield return new WaitForSeconds(5.0f);

        if (GameManager_E.Instance.hpWarningAnim.GetBool("hp_lack"))
        {
            // 5초 동안 경고 표시가 지속되면 그 이후에는 해제
            GameManager_E.Instance.hpWarningAnim.SetBool("hp_lack", false);
        }
    }

    IEnumerator CountDown()
    {
        float timer = 10.0f;

        while (timer > 0)
        {
            timer -= Time.unscaledDeltaTime; // timeScale이 0이어도 동작
            GameManager_E.Instance.revival_timer.text = Mathf.CeilToInt(timer).ToString();

            yield return null;
        }

        if (GameManager_E.Instance.revivalPopup.activeSelf)
        {
            // 부활 창이 계속 떠있는 경우에만 동작
            GameManager_E.Instance.revivalPopup.SetActive(false);
            GameManager_E.Instance.defeatPopup.SetActive(true);
        }
    }
}

