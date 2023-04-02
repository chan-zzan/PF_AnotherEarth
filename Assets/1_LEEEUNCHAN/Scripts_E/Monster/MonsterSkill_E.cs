using System.Collections;
using System.Collections.Generic;
//using UnityEditor.UIElements;
using UnityEngine;

[System.Serializable]
public struct DashInfo
{
    public float dashDist;
    public float dashSpeed;
}

public enum SkillName
{
    None,
    Dash,            //1    // All
    Throw,           //2    // 1,2,3,4,8
    BigThrow,        //3    // 1,6,7
    Punch,           //4    // 2,4
    Sound,           //5    // 3,7
    Push,            //6    // 5
    Pull,            //7    // 5
    Laser,           //8    // 6
    Cycle,           //9    // 8
    Roll,            //10   // 9
    Scatter,         //11   // 9
}

public enum MonsterName
{
    None,
    Rabbit,         //1 
    Duck,           //2
    Chicken,        //3
    Cat,            //4
    Guineapig,      //5
    Alpaca,         //6
    Sheep,          //7
    Dog,            //8
    Pig             //9
}

public class MonsterSkill_E : MonoBehaviour // 몬스터의 특수 이동 관리
{
    Monster_E curMonster;
    Animator anim;

    public MonsterName monsterName;

    [Space(10)]

    public SkillName curUseSkill; // 현재 사용중인 스킬
    public List<SkillName> UseSkills; // 사용할 스킬들

    [Space(10)]

    public float skillCycle; // 스킬 주기
    public Transform createAxis; // 생성 축
    public Transform[] createPoints; // 생성 위치

    [Space(10)]

    // 생성 물체 
    [Header("CreateObjects")]
    public GameObject attackSignal; // 공격 전조 신호
    public GameObject throwObject;
    public GameObject bigThrowObject;
    public GameObject punchObject;
    public GameObject soundObject;
    public GameObject gravityObject;
    public GameObject laserObject;    
    public GameObject scatterObject;

    private void OnEnable()
    {
        anim = GetComponentInChildren<Animator>();
        curMonster = GetComponent<Monster_E>();
    }

    public void ActiveSkill(int num)
    {
        StartCoroutine(StartSkill(num));
    }

    IEnumerator StartSkill(int num)
    {
        // idle 애니메이션으로 변경
        anim.SetTrigger("idle");

        // 발동할 스킬
        curUseSkill = (SkillName)num;

        // 전조 증상이 끝난 뒤 스킬 시작
        yield return PreviewSignal();

        switch (num)
        {
            case 0:
                // 없음
                curMonster.ChangeToMove(); // 다시 바로 그냥 이동 상태로 돌아감
                break;
            case 1:
                // 대쉬
                Dash();
                break;
            case 2:
                // 던지기
                Throw();
                break;
            case 3:
                // 큰 물체 던지기
                BigThrow();
                break;
            case 4:
                // 펀치
                Punch();
                break;
            case 5:
                // 소리
                Sound();
                break;
            case 6:
                // 밀기
                Push();
                break;
            case 7:
                // 당기기
                Pull();
                break;
            case 8:
                // 레이저
                Laser();
                break;
            case 9:
                // 돌기
                Cycle();
                break;
            case 10:
                // 구르기
                Roll();
                break;
            case 11:
                // 뿌리기
                Scatter();
                break;
        }
    }

    IEnumerator PreviewSignal()
    {
        for (int i = 0; i < 2; i++)
        {
            attackSignal.SetActive(true);
            yield return new WaitForSeconds(0.25f);

            attackSignal.SetActive(false);
            yield return new WaitForSeconds(0.25f);
        }
    }

    void CalculateAngle(Vector2 baseDir, Vector2 flipDir, Vector2 inputDir, out RotData data) // 각도 계산
    {
        float rad = Mathf.Acos(Vector2.Dot(baseDir, inputDir.normalized)); // 이동할 지점까지의 각도를 구함
        data.angle = 180 * (rad / Mathf.PI); // degree 각도로 바꿈
        data.rotDir = 1.0f; // 회전 방향값 => 오른쪽

        if (Vector2.Dot(flipDir, inputDir.normalized) > 0.0f)
        {
            data.rotDir = -1.0f; // 왼쪽방향
        }
    }

    IEnumerator CreateObject(GameObject createObject, int num, float waitingTime) // 오브젝트 생성 코루틴(생성물체, 생성갯수, 생성후 대기시간)
    {
        // 오브젝트 생성 방향 -> 고정(플레이어 방향)
        Vector2 dir = GameManager_E.Instance.Player.transform.position - createAxis.position; 

        // 각도 계산
        RotData createData;
        CalculateAngle(this.transform.up, this.transform.right, dir, out createData);

        // 생성 방향 적용
        createAxis.rotation = Quaternion.Euler(new Vector3(0, 0, createData.angle * createData.rotDir));

        // 생성 갯수 적용
        for (int i = 0; i < num; i++)
        {
            GameObject obj = Instantiate(createObject);
            obj.transform.position = createPoints[i].transform.position;
            obj.transform.rotation = createPoints[i].transform.rotation;
        }

        yield return new WaitForSeconds(waitingTime);

        curMonster.ChangeToMove();
    }

    #region 1.Dash
    void Dash()
    {
        print("dash");
        StartCoroutine(Dashing());
    }

    IEnumerator Dashing()
    {
        // 몇초 정도 대기하면서 위험함을 알림(위험 경로 표시)
        anim.SetTrigger("idle");

        Vector2 dir = GameManager_E.Instance.Player.transform.position - this.transform.position; // 대쉬 방향
        dir.Normalize();

        // 위험 경로 표시
        Transform dangerRotate = this.transform.GetChild(0); // 위험 경로 회전 담당
        dangerRotate.gameObject.SetActive(true); // 경로 표시

        // 각도 계산
        RotData moveData;
        CalculateAngle(-this.transform.up, -this.transform.right, dir, out moveData);

        // 위험 경로 회전
        dangerRotate.transform.rotation = Quaternion.Euler(new Vector3(0, 0, moveData.angle * moveData.rotDir));

        Transform dangerLine = dangerRotate.transform.GetChild(0); // 위험 경로 스케일 담당
        dangerLine.localScale = new Vector3(1, 0, 1); // 스케일 기본값 세팅

        float curScale = 0.0f;
        float maxScale = 2.5f;

        // 위험 경로 안내 애니메이션
        while (curScale < maxScale)
        {
            float delta = Time.deltaTime * maxScale;
            delta = delta > (maxScale - curScale) ? (maxScale - curScale) : delta;

            dangerLine.localScale += new Vector3(0, delta, 0);

            curScale += delta;

            yield return null;
        }

        // 위험 경로 표시 끔
        dangerRotate.gameObject.SetActive(false);

        // 대쉬
        float dist = GameManager_E.Instance.dashInfo.dashDist; // 남은 이동 거리

        anim.SetTrigger("walk");

        while (dist > 0)
        {
            float delta = Time.fixedDeltaTime * GameManager_E.Instance.dashInfo.dashSpeed;  // 움직일 거리
            delta = delta > dist ? dist : delta; // 움직일 거리가 남은 거리보다 큰 경우 남은 거리만큼만 이동

            Rigidbody2D rigid = this.GetComponent<Rigidbody2D>();
            rigid.MovePosition(rigid.position + (Vector2)(dir * delta)); // 이동

            dist -= delta; // 남은 거리 갱신

            yield return new WaitForFixedUpdate();
        }

        // 다시 원래대로 돌아감
        yield return new WaitForSeconds(0.5f);
        curMonster.ChangeToMove();
    }

    #endregion

    #region 2.Throw

    void Throw()
    {        
        print("throw");
        StartCoroutine(CreateObject(throwObject, createPoints.Length, 1.0f));
    }

    //void ThrowCarrot()
    //{
    //    print("throw carrot");
    //    StartCoroutine(CreateObject(createObject[1], 1, 1.0f));
    //}

    //void SuperThrow()
    //{
    //    print("super throw");
    //    StartCoroutine(CreateObject(createObject[1], 1, 1.0f));
    //}

    //void ThrowWool()
    //{
    //    print("throw wool");
    //    this.projectile.GetComponent<MonsterProjectile_E>().collisionDecrease = true;
    //    StartCoroutine(CreateObject(createObject[0], createPoints.Length, 1.0f));
    //}

    //IEnumerator Throwing(int num)
    //{
    //    anim.SetTrigger("idle");

    //    Vector2 dir = GameManager_E.Instance.Player.transform.position - createAxis.position; // 투사체 생성 방향

    //    RotData moveData;

    //    // 각도 계산
    //    CalculateAngle(this.transform.up, this.transform.right, dir, out moveData);

    //    // 축 회전 -> 축을 기준으로 투사체를 날림
    //    createAxis.rotation = Quaternion.Euler(new Vector3(0, 0, moveData.angle * moveData.rotDir));

    //    if (num == 0)
    //    {
    //        for (int i = 0; i < createPoints.Length; i++)
    //        {
    //            GameObject obj = Instantiate(projectile);
    //            obj.transform.position = createPoints[i].transform.position;
    //            obj.transform.rotation = createPoints[i].transform.rotation;
    //        }
    //    }
    //    else if(num == 1)
    //    {
    //        GameObject obj = Instantiate(bigProjectile);
    //        obj.transform.position = createPoints[0].transform.position;
    //        obj.transform.rotation = createPoints[0].transform.rotation;
    //    }

    //    yield return new WaitForSeconds(1.0f);

    //    curMonster.ChangeToMove();
    //}

    #endregion

    #region 3.Big Throw

    void BigThrow()
    {
        switch ((int)monsterName)
        {
            case 1:
                SoundManager_E.Instance.MonsterEffectSoundPlay(1);
                break;
            case 6:
                SoundManager_E.Instance.MonsterEffectSoundPlay(7);
                break;
        }

        print("big Throw");
        StartCoroutine(CreateObject(bigThrowObject, 1, 1.0f));
    }

    #endregion

    #region 4.Punch

    void Punch()
    {
        print("punch");
        StartCoroutine(SoundDelaying(0.5f, true));
        StartCoroutine(CreateObject(punchObject, 1, 1.0f));
    }

    #endregion

    #region 5.Sound

    void Sound()
    {
        print("sound");
        StartCoroutine(SoundDelaying(0.5f, false));
    }

    IEnumerator SoundDelaying(float delayTime, bool frontDelay)
    {
        anim.SetTrigger("idle");

        if (frontDelay)
        {
            yield return new WaitForSeconds(delayTime);
        }

        // 동물에 따른 효과음 on
        switch ((int)monsterName)
        {
            case 2:
                // 오리
                SoundManager_E.Instance.MonsterEffectSoundPlay(3);
                break;
            case 3:
                // 닭
                SoundManager_E.Instance.MonsterEffectSoundPlay(4);
                break;
            case 4:
                // 고양이
                SoundManager_E.Instance.MonsterEffectSoundPlay(5);
                break;
            case 7:
                // 양
                SoundManager_E.Instance.MonsterEffectSoundPlay(8);
                break;
        }

        if (!frontDelay)
        {
            yield return new WaitForSeconds(delayTime);
            StartCoroutine(CreateObject(soundObject, 1, 1.0f));
        }
    }

    #endregion

    #region 6.Push

    void Push()
    {
        print("push");
        StartCoroutine(Pushing(3));
    }
    
    IEnumerator Pushing(int num)
    {
        GravityArea_E gravityArea = gravityObject.GetComponent<GravityArea_E>();
        gravityArea.GetComponent<CircleCollider2D>().radius = 3.3f; // 밀어낼 범위 설정

        // 가운데로 이동
        while (true)
        {
            anim.SetTrigger("walk");
            Vector2 dir = GameManager_E.Instance.monsterSpawner.center - (Vector2)this.transform.position; // 가운데로 향하는 방향

            float dist = dir.magnitude; // 남은거리
            float delta = Time.fixedDeltaTime * 20.0f; // 이동할 거리

            delta = delta > dist ? dist : delta;

            this.transform.Translate(dir.normalized * delta);

            dist -= delta;

            yield return new WaitForFixedUpdate();

            if (dist <= 0.0f) break;
        }

        gravityObject.SetActive(true); // 범위 오브젝트 on
        gravityArea.gravityForce = -30; // 밀어내는 힘  

        // 점프 애니메이션
        for (int i = 0; i < num; i++)
        {
            anim.SetTrigger("jump");       
            yield return new WaitForSeconds(1.0f);
            
            anim.SetTrigger("idle");
            yield return new WaitForSeconds(0.2f);
        }

        gravityObject.SetActive(false); // 범위 오브젝트 off

        // 다시 원래대로 돌아감
        yield return new WaitForSeconds(0.5f);
        curMonster.ChangeToMove();
    }

    #endregion

    #region 7.Pull

    void Pull()
    {
        print("pull");
        StartCoroutine(Pulling(3));
    }

    IEnumerator Pulling(int num)
    {
        GravityArea_E gravityArea = gravityObject.GetComponent<GravityArea_E>();

        gravityObject.SetActive(true); // 범위 오브젝트 on
        gravityArea.gravityForce = 30; // 끌어당기는 힘
        gravityArea.GetComponent<CircleCollider2D>().radius = 9999.9f; // 당길 범위 설정(맵 전체)

        // 점프 애니메이션
        for (int i = 0; i < num; i++)
        {
            anim.SetTrigger("jump");            
            yield return new WaitForSeconds(1.0f);

            anim.SetTrigger("idle");
            yield return new WaitForSeconds(0.2f);
        }

        gravityObject.SetActive(false); // 범위 오브젝트 off

        // 다시 원래대로 돌아감
        yield return new WaitForSeconds(0.5f);
        curMonster.ChangeToMove();
    }

    #endregion

    #region 8.Laser

    void Laser()
    {
        print("laser");
        StartCoroutine(EyeLaser());
    }

    IEnumerator EyeLaser()
    {
        Transform eyeLaserAxis = this.transform.GetChild(1); // eyelaser 오브젝트에 접근

        eyeLaserAxis.transform.GetChild(0).gameObject.SetActive(true); // 오브젝트 활성화

        Vector2 dir = GameManager_E.Instance.Player.transform.position - eyeLaserAxis.position; // 공격 방향

        // 각도 계산
        RotData moveData;
        CalculateAngle(this.transform.up, this.transform.right, dir, out moveData);

        // 축 회전
        eyeLaserAxis.rotation = Quaternion.Euler(new Vector3(0, 0, moveData.angle * moveData.rotDir));

        // 효과음 on
        SoundManager_E.Instance.MonsterEffectSoundPlay(6);

        yield return new WaitForSeconds(1.5f); // 애니메이션 발동 시간

        eyeLaserAxis.transform.GetChild(0).gameObject.SetActive(false); // 오브젝트 비활성화

        // 다시 원래대로 돌아감
        yield return new WaitForSeconds(0.5f);
        curMonster.ChangeToMove();
    }

    #endregion

    #region 9.Cycle

    void Cycle()
    {
        print("cycle");
        StartCoroutine(Rotating());
    }

    IEnumerator Rotating()
    {
        Vector3 baseScale = this.transform.localScale; // 원래 방향
        Vector3 leftScale = new Vector3(Mathf.Abs(baseScale.x), baseScale.y, baseScale.z);
        Vector3 rightScale = new Vector3(-Mathf.Abs(baseScale.x), baseScale.y, baseScale.z);

        anim.SetTrigger("idle");
        yield return new WaitForSeconds(0.3f);
        anim.SetTrigger("walk");

        GameObject centerAxis;

        // 중심 축 생성
        if (this.transform.parent.childCount == 1 || GameManager_E.Instance.finalStage)
        {
            // 중심 축이 아직 생성이 되지 않은 경우
            centerAxis = new GameObject();
            centerAxis.name = "centerAxis";
            centerAxis.transform.position = GameManager_E.Instance.monsterSpawner.center;
            centerAxis.transform.parent = this.transform.parent;
        }
        else
        {
            // 중심 축이 이미 생성된 경우 -> 축이 이미 부모로 설정되어 있으므로 대입만 해줌
            centerAxis = this.transform.parent.gameObject;
        }

        // 몬스터의 부모를 중심 축으로 변경
        this.transform.parent = centerAxis.transform;

        float radius = 50.0f; // 회전 반지름
        float moveSpeed = 50.0f; // 몬스터 이동속도
        float dist = 0.0f; // 중심축에서 몬스터까지의 거리

        // 몬스터가 이동하는 방향의 각도
        float MoveAngle = Vector2.SignedAngle(this.transform.up, this.transform.position - centerAxis.transform.position);

        // 몬스터 이동방향 설정
        if (MoveAngle > 0 && MoveAngle < 180)
        {
            // 왼쪽
            this.transform.localScale = leftScale;
        }
        else
        {
            // 오른쪽
            this.transform.localScale = rightScale;
        }

        // 몬스터가 중심축으로부터 반지름만큼 멀어질때 까지 이동
        while (dist < radius)
        {
            Vector3 dir = this.transform.position - centerAxis.transform.position; // 이동방향
            dist = dir.magnitude;
            dir.Normalize();

            this.GetComponent<Rigidbody2D>().MovePosition(this.transform.position + dir * Time.fixedDeltaTime * moveSpeed); // 이동

            yield return new WaitForFixedUpdate();

        }

        float cycleSpeed = 180; // 회전 속도
        float cycleNum = 3; // 회전 횟수
        float cycleAngle = cycleNum * 360; // 회전 각도

        // 효과음 on
        SoundManager_E.Instance.MonsterEffectSoundPlay(11);

        // 회전축을 돌림
        while (cycleAngle > 0)
        {
            anim.speed = 2; // 애니메이션 속도 2배

            // 프레임당 회전속도
            float delta = Time.fixedDeltaTime * cycleSpeed; 
            delta = delta > cycleAngle ? cycleAngle : delta;

            centerAxis.transform.Rotate(this.transform.forward, delta); // 회전
            this.transform.up = Vector3.up;

            Vector2 monsterDir = this.transform.position - centerAxis.transform.position; // 몬스터 방향
            float monsterAngle = Vector2.SignedAngle(this.transform.up, monsterDir); // 축과 몬스터의 각도

            // 몬스터의 방향 설정
            if (monsterAngle < 90 || monsterAngle > 270)
            {
                // 왼쪽
                this.transform.localScale = leftScale;
            }
            else
            {
                // 오른쪽 -> 플립
                this.transform.localScale = rightScale;
            }

            cycleAngle -= delta; // 남은 회전 각도

            yield return new WaitForFixedUpdate();
        }

        anim.speed = 1; // 애니메이션 속도 복원

        // 다시 원래대로 돌아감
        curMonster.ChangeToMove();
    }

    #endregion

    #region 10.Roll

    void Roll()
    {
        print("roll");
        StartCoroutine(Rolling(7.0f));
    }

    IEnumerator Rolling(float rollTime)
    {
        anim.SetTrigger("roll");
        Rigidbody2D rigid = this.GetComponent<Rigidbody2D>();

        SoundManager_E.Instance.MonsterEffectSoundPlay(12);

        while (rollTime > 0)
        {
            rollTime -= Time.fixedDeltaTime;

            Vector2 dir = GameManager_E.Instance.Player.transform.position - this.transform.position;
            dir.Normalize();

            rigid.MovePosition(rigid.position + dir * 30 * Time.fixedDeltaTime);

            yield return new WaitForFixedUpdate();
        }

        // 다시 원래대로 돌아감
        yield return new WaitForSeconds(0.1f);
        curMonster.ChangeToMove();
    }

    #endregion

    #region 11.Scatter

    void Scatter()
    {
        print("scatter");
        StartCoroutine(Scattering());
    }

    IEnumerator Scattering()
    {
        anim.SetTrigger("eat");
        yield return new WaitForSeconds(1.5f);

        // 효과음 on
        SoundManager_E.Instance.MonsterEffectSoundPlay(13);

        for (int i = 0; i < createPoints.Length; i++)
        {
            GameObject obj = Instantiate(scatterObject); // 나중에 오브젝트 풀링 해야될듯
            obj.transform.position = createPoints[i].transform.position;
            obj.transform.rotation = Quaternion.Euler(Vector3.up);
        }

        // 다시 원래대로 돌아감
        yield return new WaitForSeconds(0.5f);
        curMonster.ChangeToMove();        
    }

    #endregion
}
