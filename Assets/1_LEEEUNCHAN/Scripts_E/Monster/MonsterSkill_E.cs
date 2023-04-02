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

public class MonsterSkill_E : MonoBehaviour // ������ Ư�� �̵� ����
{
    Monster_E curMonster;
    Animator anim;

    public MonsterName monsterName;

    [Space(10)]

    public SkillName curUseSkill; // ���� ������� ��ų
    public List<SkillName> UseSkills; // ����� ��ų��

    [Space(10)]

    public float skillCycle; // ��ų �ֱ�
    public Transform createAxis; // ���� ��
    public Transform[] createPoints; // ���� ��ġ

    [Space(10)]

    // ���� ��ü 
    [Header("CreateObjects")]
    public GameObject attackSignal; // ���� ���� ��ȣ
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
        // idle �ִϸ��̼����� ����
        anim.SetTrigger("idle");

        // �ߵ��� ��ų
        curUseSkill = (SkillName)num;

        // ���� ������ ���� �� ��ų ����
        yield return PreviewSignal();

        switch (num)
        {
            case 0:
                // ����
                curMonster.ChangeToMove(); // �ٽ� �ٷ� �׳� �̵� ���·� ���ư�
                break;
            case 1:
                // �뽬
                Dash();
                break;
            case 2:
                // ������
                Throw();
                break;
            case 3:
                // ū ��ü ������
                BigThrow();
                break;
            case 4:
                // ��ġ
                Punch();
                break;
            case 5:
                // �Ҹ�
                Sound();
                break;
            case 6:
                // �б�
                Push();
                break;
            case 7:
                // ����
                Pull();
                break;
            case 8:
                // ������
                Laser();
                break;
            case 9:
                // ����
                Cycle();
                break;
            case 10:
                // ������
                Roll();
                break;
            case 11:
                // �Ѹ���
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

    void CalculateAngle(Vector2 baseDir, Vector2 flipDir, Vector2 inputDir, out RotData data) // ���� ���
    {
        float rad = Mathf.Acos(Vector2.Dot(baseDir, inputDir.normalized)); // �̵��� ���������� ������ ����
        data.angle = 180 * (rad / Mathf.PI); // degree ������ �ٲ�
        data.rotDir = 1.0f; // ȸ�� ���Ⱚ => ������

        if (Vector2.Dot(flipDir, inputDir.normalized) > 0.0f)
        {
            data.rotDir = -1.0f; // ���ʹ���
        }
    }

    IEnumerator CreateObject(GameObject createObject, int num, float waitingTime) // ������Ʈ ���� �ڷ�ƾ(������ü, ��������, ������ ���ð�)
    {
        // ������Ʈ ���� ���� -> ����(�÷��̾� ����)
        Vector2 dir = GameManager_E.Instance.Player.transform.position - createAxis.position; 

        // ���� ���
        RotData createData;
        CalculateAngle(this.transform.up, this.transform.right, dir, out createData);

        // ���� ���� ����
        createAxis.rotation = Quaternion.Euler(new Vector3(0, 0, createData.angle * createData.rotDir));

        // ���� ���� ����
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
        // ���� ���� ����ϸ鼭 �������� �˸�(���� ��� ǥ��)
        anim.SetTrigger("idle");

        Vector2 dir = GameManager_E.Instance.Player.transform.position - this.transform.position; // �뽬 ����
        dir.Normalize();

        // ���� ��� ǥ��
        Transform dangerRotate = this.transform.GetChild(0); // ���� ��� ȸ�� ���
        dangerRotate.gameObject.SetActive(true); // ��� ǥ��

        // ���� ���
        RotData moveData;
        CalculateAngle(-this.transform.up, -this.transform.right, dir, out moveData);

        // ���� ��� ȸ��
        dangerRotate.transform.rotation = Quaternion.Euler(new Vector3(0, 0, moveData.angle * moveData.rotDir));

        Transform dangerLine = dangerRotate.transform.GetChild(0); // ���� ��� ������ ���
        dangerLine.localScale = new Vector3(1, 0, 1); // ������ �⺻�� ����

        float curScale = 0.0f;
        float maxScale = 2.5f;

        // ���� ��� �ȳ� �ִϸ��̼�
        while (curScale < maxScale)
        {
            float delta = Time.deltaTime * maxScale;
            delta = delta > (maxScale - curScale) ? (maxScale - curScale) : delta;

            dangerLine.localScale += new Vector3(0, delta, 0);

            curScale += delta;

            yield return null;
        }

        // ���� ��� ǥ�� ��
        dangerRotate.gameObject.SetActive(false);

        // �뽬
        float dist = GameManager_E.Instance.dashInfo.dashDist; // ���� �̵� �Ÿ�

        anim.SetTrigger("walk");

        while (dist > 0)
        {
            float delta = Time.fixedDeltaTime * GameManager_E.Instance.dashInfo.dashSpeed;  // ������ �Ÿ�
            delta = delta > dist ? dist : delta; // ������ �Ÿ��� ���� �Ÿ����� ū ��� ���� �Ÿ���ŭ�� �̵�

            Rigidbody2D rigid = this.GetComponent<Rigidbody2D>();
            rigid.MovePosition(rigid.position + (Vector2)(dir * delta)); // �̵�

            dist -= delta; // ���� �Ÿ� ����

            yield return new WaitForFixedUpdate();
        }

        // �ٽ� ������� ���ư�
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

    //    Vector2 dir = GameManager_E.Instance.Player.transform.position - createAxis.position; // ����ü ���� ����

    //    RotData moveData;

    //    // ���� ���
    //    CalculateAngle(this.transform.up, this.transform.right, dir, out moveData);

    //    // �� ȸ�� -> ���� �������� ����ü�� ����
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

        // ������ ���� ȿ���� on
        switch ((int)monsterName)
        {
            case 2:
                // ����
                SoundManager_E.Instance.MonsterEffectSoundPlay(3);
                break;
            case 3:
                // ��
                SoundManager_E.Instance.MonsterEffectSoundPlay(4);
                break;
            case 4:
                // �����
                SoundManager_E.Instance.MonsterEffectSoundPlay(5);
                break;
            case 7:
                // ��
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
        gravityArea.GetComponent<CircleCollider2D>().radius = 3.3f; // �о ���� ����

        // ����� �̵�
        while (true)
        {
            anim.SetTrigger("walk");
            Vector2 dir = GameManager_E.Instance.monsterSpawner.center - (Vector2)this.transform.position; // ����� ���ϴ� ����

            float dist = dir.magnitude; // �����Ÿ�
            float delta = Time.fixedDeltaTime * 20.0f; // �̵��� �Ÿ�

            delta = delta > dist ? dist : delta;

            this.transform.Translate(dir.normalized * delta);

            dist -= delta;

            yield return new WaitForFixedUpdate();

            if (dist <= 0.0f) break;
        }

        gravityObject.SetActive(true); // ���� ������Ʈ on
        gravityArea.gravityForce = -30; // �о�� ��  

        // ���� �ִϸ��̼�
        for (int i = 0; i < num; i++)
        {
            anim.SetTrigger("jump");       
            yield return new WaitForSeconds(1.0f);
            
            anim.SetTrigger("idle");
            yield return new WaitForSeconds(0.2f);
        }

        gravityObject.SetActive(false); // ���� ������Ʈ off

        // �ٽ� ������� ���ư�
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

        gravityObject.SetActive(true); // ���� ������Ʈ on
        gravityArea.gravityForce = 30; // ������� ��
        gravityArea.GetComponent<CircleCollider2D>().radius = 9999.9f; // ��� ���� ����(�� ��ü)

        // ���� �ִϸ��̼�
        for (int i = 0; i < num; i++)
        {
            anim.SetTrigger("jump");            
            yield return new WaitForSeconds(1.0f);

            anim.SetTrigger("idle");
            yield return new WaitForSeconds(0.2f);
        }

        gravityObject.SetActive(false); // ���� ������Ʈ off

        // �ٽ� ������� ���ư�
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
        Transform eyeLaserAxis = this.transform.GetChild(1); // eyelaser ������Ʈ�� ����

        eyeLaserAxis.transform.GetChild(0).gameObject.SetActive(true); // ������Ʈ Ȱ��ȭ

        Vector2 dir = GameManager_E.Instance.Player.transform.position - eyeLaserAxis.position; // ���� ����

        // ���� ���
        RotData moveData;
        CalculateAngle(this.transform.up, this.transform.right, dir, out moveData);

        // �� ȸ��
        eyeLaserAxis.rotation = Quaternion.Euler(new Vector3(0, 0, moveData.angle * moveData.rotDir));

        // ȿ���� on
        SoundManager_E.Instance.MonsterEffectSoundPlay(6);

        yield return new WaitForSeconds(1.5f); // �ִϸ��̼� �ߵ� �ð�

        eyeLaserAxis.transform.GetChild(0).gameObject.SetActive(false); // ������Ʈ ��Ȱ��ȭ

        // �ٽ� ������� ���ư�
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
        Vector3 baseScale = this.transform.localScale; // ���� ����
        Vector3 leftScale = new Vector3(Mathf.Abs(baseScale.x), baseScale.y, baseScale.z);
        Vector3 rightScale = new Vector3(-Mathf.Abs(baseScale.x), baseScale.y, baseScale.z);

        anim.SetTrigger("idle");
        yield return new WaitForSeconds(0.3f);
        anim.SetTrigger("walk");

        GameObject centerAxis;

        // �߽� �� ����
        if (this.transform.parent.childCount == 1 || GameManager_E.Instance.finalStage)
        {
            // �߽� ���� ���� ������ ���� ���� ���
            centerAxis = new GameObject();
            centerAxis.name = "centerAxis";
            centerAxis.transform.position = GameManager_E.Instance.monsterSpawner.center;
            centerAxis.transform.parent = this.transform.parent;
        }
        else
        {
            // �߽� ���� �̹� ������ ��� -> ���� �̹� �θ�� �����Ǿ� �����Ƿ� ���Ը� ����
            centerAxis = this.transform.parent.gameObject;
        }

        // ������ �θ� �߽� ������ ����
        this.transform.parent = centerAxis.transform;

        float radius = 50.0f; // ȸ�� ������
        float moveSpeed = 50.0f; // ���� �̵��ӵ�
        float dist = 0.0f; // �߽��࿡�� ���ͱ����� �Ÿ�

        // ���Ͱ� �̵��ϴ� ������ ����
        float MoveAngle = Vector2.SignedAngle(this.transform.up, this.transform.position - centerAxis.transform.position);

        // ���� �̵����� ����
        if (MoveAngle > 0 && MoveAngle < 180)
        {
            // ����
            this.transform.localScale = leftScale;
        }
        else
        {
            // ������
            this.transform.localScale = rightScale;
        }

        // ���Ͱ� �߽������κ��� ��������ŭ �־����� ���� �̵�
        while (dist < radius)
        {
            Vector3 dir = this.transform.position - centerAxis.transform.position; // �̵�����
            dist = dir.magnitude;
            dir.Normalize();

            this.GetComponent<Rigidbody2D>().MovePosition(this.transform.position + dir * Time.fixedDeltaTime * moveSpeed); // �̵�

            yield return new WaitForFixedUpdate();

        }

        float cycleSpeed = 180; // ȸ�� �ӵ�
        float cycleNum = 3; // ȸ�� Ƚ��
        float cycleAngle = cycleNum * 360; // ȸ�� ����

        // ȿ���� on
        SoundManager_E.Instance.MonsterEffectSoundPlay(11);

        // ȸ������ ����
        while (cycleAngle > 0)
        {
            anim.speed = 2; // �ִϸ��̼� �ӵ� 2��

            // �����Ӵ� ȸ���ӵ�
            float delta = Time.fixedDeltaTime * cycleSpeed; 
            delta = delta > cycleAngle ? cycleAngle : delta;

            centerAxis.transform.Rotate(this.transform.forward, delta); // ȸ��
            this.transform.up = Vector3.up;

            Vector2 monsterDir = this.transform.position - centerAxis.transform.position; // ���� ����
            float monsterAngle = Vector2.SignedAngle(this.transform.up, monsterDir); // ��� ������ ����

            // ������ ���� ����
            if (monsterAngle < 90 || monsterAngle > 270)
            {
                // ����
                this.transform.localScale = leftScale;
            }
            else
            {
                // ������ -> �ø�
                this.transform.localScale = rightScale;
            }

            cycleAngle -= delta; // ���� ȸ�� ����

            yield return new WaitForFixedUpdate();
        }

        anim.speed = 1; // �ִϸ��̼� �ӵ� ����

        // �ٽ� ������� ���ư�
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

        // �ٽ� ������� ���ư�
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

        // ȿ���� on
        SoundManager_E.Instance.MonsterEffectSoundPlay(13);

        for (int i = 0; i < createPoints.Length; i++)
        {
            GameObject obj = Instantiate(scatterObject); // ���߿� ������Ʈ Ǯ�� �ؾߵɵ�
            obj.transform.position = createPoints[i].transform.position;
            obj.transform.rotation = Quaternion.Euler(Vector3.up);
        }

        // �ٽ� ������� ���ư�
        yield return new WaitForSeconds(0.5f);
        curMonster.ChangeToMove();        
    }

    #endregion
}
