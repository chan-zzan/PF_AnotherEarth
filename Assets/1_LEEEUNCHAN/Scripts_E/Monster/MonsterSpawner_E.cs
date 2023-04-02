using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;


public class MonsterSpawner_E : MonoBehaviour
{
    public Transform[] spawnPoint; // ���� ���� ��ġ
    public Transform bossSpawnPoint; // ���� ���� ��ġ

    [Space(10)]

    public Transform[] crossWave; // ũ�ν� ���� ���� ��ġ
    public GameObject[] crossMonsters;// ũ�ν� ���� ������
    public Transform crossAxis; // ũ�ν� ���� ��

    [Space(10)]

    public Transform[] circleWave; // ����� ������ ���� ���� ��ġ
    public GameObject[] circleMonsters;// ����� ������ ���� ������

    [Space(10)]

    // �������
    public Transform[] specialWaves; // Ư�� ���� ���̺� ����
    public GameObject[] speialMonsters; // Ư�� ���� �����յ�

    [Space(10)]

    // �ϵ���
    public Transform[] specialWaves_H; // Ư�� ���� ���̺� ����
    public GameObject[] speialMonsters_H; // Ư�� ���� �����յ�

    [Space(10)]

    // Ÿ�� ����
    public Transform[] tileWaves_H; // Ư�� ���� ���̺� ����
    public GameObject[] tileMonsters_H; // Ÿ�� ���� �����յ�

    [Space(10)]

    // ���̳� ���̺�
    public Transform[] finalWaves;
    public Transform finalMonsterParent; // ������ �θ� ��ġ
    public int finalPhaseNum; // ���̳� ������ ��ȣ

    [Space(10)]

    public Transform[] AddtionalWaves; // �߰� ���̺�

    [Space(10)]

    // ���� ������ ���ѵǴ� ��
    public GameObject limitedMap;
    public Vector2 center; // ���� �߾� ��ġ

    float timer;
    
    bool HardMode = false;

    [SerializeField] List<StageDB_Entity> Phases;
    public List<StageDB_Entity> _Phases { get => Phases; }

    public StageDB_Entity curPhase;

    [Space(30)]

    public GameObject[] monsterType;

    private void Start()
    {       
        // List<StageDB_Entity>
        Phases = StageManager.Instance.CurStageData;

        /// �׽�Ʈ ��

        //curPhase = Phases[Phases.Count - 1];
        //BossSpawn();
        //return;

        /// �׽�Ʈ �� 



        // ���� �������� ��ȣ
        int curStageNum = StageManager.Instance.curStageNum;

        if (GameManager_E.Instance.finalStage)
        {
            // ���̳� ���������� ���
            StartCoroutine(FinalStageStart());
            center = this.transform.position;
            return;
        }

        // �ϵ������� �Ǵ�
        if (curStageNum < 0) HardMode = true;

        // ������ ī���� �� ���� ����
        StartCoroutine(PhaseCounting());

        // ���� �� �⺻ ������ ����(��, ��) ���� �ڷ�ƾ ����
        StartCoroutine(PhaseMonsterSpawnStart());                

        SpecialMonster_E curModeSpecialMonster;

        if (!HardMode)
        {
            // �������
            if (speialMonsters[curStageNum - 1] == null) return;
            curModeSpecialMonster = speialMonsters[curStageNum - 1].GetComponent<SpecialMonster_E>();
        }
        else
        {
            // �ϵ���
            if (speialMonsters_H[-curStageNum - 1] == null) return;
            curModeSpecialMonster = speialMonsters_H[-curStageNum - 1].GetComponent<SpecialMonster_E>();

            // Ÿ�ϸ��� ���� �ڷ�ƾ ����
            StartCoroutine(SpecialSpawnStart(tileMonsters_H[(-curStageNum - 1)].GetComponent<SpecialMonster_E>(), curStageNum));
        }

        // Ư������ ���� �ڷ�ƾ ����
        StartCoroutine(SpecialSpawnStart(curModeSpecialMonster, curStageNum));
    }

    IEnumerator FinalStageStart()
    {
        for (int phaseNum = 0; phaseNum < 10; phaseNum++)
        {
            curPhase = Phases[phaseNum]; // ������ ����
            finalPhaseNum = phaseNum;

            if (phaseNum == 0)
            {
                // ù��° ������ -> ��� ���̺� ���͵��� 2�ʰ������� 5������ ��ȯ
                for (int i = 0; i < 26; i += 3)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        GameObject mon = Instantiate(monsterType[i]);
                        mon.transform.parent = finalMonsterParent; // �θ���

                        int randomNum = Random.Range(0, finalWaves[1].childCount);
                        mon.transform.position = finalWaves[1].GetChild(randomNum).position; // ���� ��ġ ����
                    }

                    yield return new WaitForSeconds(2.0f);
                }

                print("1 : " + GameManager_E.Instance.Time.playTime);
                yield return new WaitForSeconds(20 - GameManager_E.Instance.Time.playTime);
            }
            else
            {
                // �ι�° ������ ���� -> �븻 ���͵��� �ѹ��� ��ȯ
                for (int i = 0; i < curPhase.number; i++)
                {
                    GameObject mon = Instantiate(monsterType[(phaseNum - 1) * 3 + 1]);
                    mon.transform.parent = finalMonsterParent; // �θ���

                    int waveNum;

                    if (phaseNum > 3) waveNum = 0; // 20������ ��ȯ
                    else waveNum = 1; // 30������ ��ȯ

                    mon.transform.position = finalWaves[waveNum].GetChild(i + 1).position; // ���� ��ġ ����
                }

                // ���� ��ȯ
                BossSpawn(phaseNum);

                if (phaseNum == 9) yield break; // ������ ������ ���

                yield return FinalStageTimeCounting(phaseNum); // ī��Ʈ

                print("2 : " + GameManager_E.Instance.Time.playTime);
            }

            // ������ �׾����� �Ǵ�
            if (GameManager_E.Instance.finalStageCount == phaseNum)
            {
                // ������� 3�� ī��Ʈ �� ���� �������� ����
                yield return Warning(-1);
                print("3 : " + GameManager_E.Instance.Time.playTime);
            }
            else
            {
                // ������ �� ���� ��� -> �й�
                GameManager_E.Instance.FinalWarningPopup2.SetActive(false);
                GameManager_E.Instance.life = 0; // ��Ȱx
                GameManager_E.Instance.Player.GameEnd();
                break;
            }
            
        }

        yield return null;
    }

    IEnumerator FinalStageTimeCounting(int phaseNum)
    {
        float timer = 0.0f;
        int bossCount = phaseNum;

        GameObject warningPopup = GameManager_E.Instance.FinalWarningPopup2;

        while (timer < 20.0f)
        {
            timer += Time.deltaTime;

            // �ð����� ������ ���� ���
            if (bossCount == GameManager_E.Instance.finalStageCount)
            {
                // Ÿ�̸� �˾��� �����ִ� ��� -> �˾��� ��
                if (warningPopup.activeSelf)
                {
                    warningPopup.SetActive(false);
                }

                break;
            }

            if (timer > 15.0f)
            {
                // �˾��� �������� ���� ��� -> �˾��� ��
                if (!warningPopup.activeSelf) warningPopup.SetActive(true);

                // �����ð� ǥ��
                warningPopup.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().text = "" + Mathf.Ceil(20 - timer);
            }

            yield return null;
        }
    }

    public void SpawnAllStop()
    {
        // ��� ������ ������Ŵ -> �й�â�� �� ���
        StopAllCoroutines();
    }

    IEnumerator PhaseCounting()
    {
        for (int phaseNum = 0; phaseNum < 12; phaseNum++)
        {
            print("1 : " + GameManager_E.Instance.Time.playTime);
            print("1-2 : " + phaseNum);

            curPhase = Phases[phaseNum]; // 15�ʸ��� ������ ����
            yield return MonsterSpawnStart(15.0f); // ���� ����
                                                   
            print("2-1 : " + GameManager_E.Instance.Time.playTime);

            if (GameManager_E.Instance.Time.playTime > 15 * phaseNum) continue;

            print("2-2 : " + GameManager_E.Instance.Time.playTime % 15);

            float resTime = 15 - (GameManager_E.Instance.Time.playTime % 15);

            if (resTime > 0.0f)
            {
                yield return new WaitForSeconds(resTime); 
            }

            print("3 : " + GameManager_E.Instance.Time.playTime);
        }
    }

    IEnumerator MonsterSpawnStart(float spawnTime)
    {
        int monsterPerSecond = (int)Mathf.Ceil(curPhase.number / spawnTime); // �ʴ� ���� ���� ��(�ø�)
        bool isLastMonster = false; // ������ �������� ����

        for (int i = 0; i < 15; i++)
        {
            if (monsterPerSecond * i > curPhase.number)
            {
                monsterPerSecond = curPhase.number - monsterPerSecond * i;
                isLastMonster = true;
            }

            // 1�ʿ� �ѹ��� ���� ��ȯ
            for (int j = 0; j < monsterPerSecond; j++)
            {
                Spawn();
            }

            if (isLastMonster) break; // ������ ���� �� �ٷ� ����

            yield return new WaitForSeconds(1.0f);
        }

        //float spawnTick = spawnTime / curPhase.number;

        //while (spawnTime >= spawnTick)
        //{
            
        //    Spawn();

        //    spawnTime -= spawnTick;
           
        //    if (spawnTime < spawnTick) break;

        //    if (GameManager_E.Instance.Time.playTime + spawnTick >= 15.0f * (curPhase.phase + 1))
        //    {
        //        break;
        //    }

        //    if (spawnTime == spawnTick)
        //    {
        //        print("4 : " + GameManager_E.Instance.Time.playTime);
        //        Spawn();
        //        break;
        //    }

        //    yield return new WaitForSeconds(spawnTick);
        //}
        //print(spawnTime);
    }


    IEnumerator PhaseMonsterSpawnStart()
    {
        for (int i = 0; i < 6; i++)
        {
            if (i == 0)
            {
                // ó������ 27���Ŀ� ����
                yield return new WaitForSeconds(27.0f);
            }
            else
            {
                // 30�ʸ��� ����
                yield return new WaitForSeconds(30.0f);
            }

            if (i == 5)
            {
                PhaseMonsterSpawn(0); // boss
                break;
            }

            if (i % 2 == 0)
            {
                PhaseMonsterSpawn(1); // cross
            }
            else
            {
                PhaseMonsterSpawn(2); // circle
            }
        }

        yield return null;
    }

    void PhaseMonsterSpawn(int num)
    {
        StartCoroutine(Warning(num));
    }

    IEnumerator Warning(int num)
    {
        GameObject warningPopup;

        if (num == 0)
        {
            warningPopup = GameManager_E.Instance.BossWarningPopup;
        }
        else if (num == -1)
        {
            // ���̳� ���
            warningPopup = GameManager_E.Instance.FinalWarningPopup;
        }
        else
        {
            warningPopup = GameManager_E.Instance.warningPopup;
        }

        GameManager_E.Instance.warningAnim.SetTrigger("warning");
        SoundManager_E.Instance.EffectSoundPlay2(2); // ȿ���� on

        for (int i = 0; i < 6; i++)
        {
            if (num == -1)
            {
                if (i < 2)
                {
                    warningPopup.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().text = "3";
                }
                else if (i < 4)
                {
                    warningPopup.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().text = "2";
                }
                else
                {
                    warningPopup.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().text = "1";
                }
            }

            warningPopup.SetActive(true);

            yield return new WaitForSeconds(0.25f);

            warningPopup.SetActive(false);

            yield return new WaitForSeconds(0.25f);
        }

        if (num == -1) yield break;

        // ���� ��忡 ���� �����ϴ� ���� ���� ����
        int spawnNum = GameManager_E.Instance.isHardMode ? 1 : 0;

        switch (num)
        {
            case 0:
                curPhase = Phases[12];
                BossSpawn();
                break;
            case 1:
                CrossSpawn(spawnNum); // �Ϸķ� �������� ����
                break;
            case 2:
                CircleSpawn(spawnNum); // ���� ������ ����
                break;
        }
    }

    IEnumerator SpecialSpawnStart(SpecialMonster_E spawnMonster, int curStageNum)
    {
        // �����ð�
        float[] spawnTimes = spawnMonster.mySpawnTime;

        // ����Ƚ��
        int spawnNum = spawnTimes.Length;

        // ���� ����
        SpecialMonster_E.Dir curDir = spawnMonster.myDir;

        for (int i = 0; i < spawnNum; i++)
        {
            // ���� �ð����� ����� ����
            if (i == 0)
            {
                yield return new WaitForSeconds(spawnTimes[i]);
            }
            else
            {
                yield return new WaitForSeconds(spawnTimes[i] - spawnTimes[i - 1]);
            }

            if (spawnMonster.myType == Monster_E.MonsterType.Tile)
            {
                TileSpawn(spawnMonster.GetComponent<TileMonster_E>(), curStageNum);
                continue;
            }

            if (curDir != SpecialMonster_E.Dir.Idle)
            {
                SpecialSpawn(spawnMonster, curStageNum, curDir);
                curDir++;
            }
            else
            {
                SpecialSpawn(spawnMonster, curStageNum);
            }
        }
    }

    void TileSpawn(TileMonster_E spawnMonster, int curStage)
    {
        // ������ ���� ����
        GameManager_E.Instance.Pool.curSpawnMonster = spawnMonster.gameObject;

        // ������ ������
        Transform[] curSpawnWavePoints = tileWaves_H[-curStage - 1].GetComponentsInChildren<Transform>();

        // �������� ����
        for (int i = 1; i < curSpawnWavePoints.Length; i++)
        {
            Monster_E monster = GameManager_E.Instance.Pool._TileMonsterPools.Get();
            monster.transform.position = curSpawnWavePoints[i].position;
        }
    }

    void SpecialSpawn(SpecialMonster_E spawnMonster, int curStage, SpecialMonster_E.Dir curDir = SpecialMonster_E.Dir.Idle) // Ư�� ���� ����
    {
        // ������ ���� ����
        GameManager_E.Instance.Pool.curSpawnMonster = spawnMonster.gameObject;

        Transform[] curModeWaves;
        Transform[] curSpawnWavePoints;

        // �ش� ���̺��� �ڽĵ��� ��ġ = ���� ��ġ 
        if (!HardMode)
        {
            // �������
            curModeWaves = specialWaves;
        }
        else
        {
            // �ϵ���
            curStage *= -1; 
            curModeWaves = specialWaves_H;
        }

        curSpawnWavePoints = curModeWaves[curStage - 1].GetComponentsInChildren<Transform>();

        // ���� ��� ����
        switch (spawnMonster.mySpawnType)
        {
            case SpecialMonster_E.SpawnType.OneRandom:
                {
                    // ���� ��ġ�� ������ ��ġ���� �ϳ��� ����
                    Monster_E monster = GameManager_E.Instance.Pool._SpecialMonsterPools.Get();
                    monster.transform.position = curSpawnWavePoints[Random.Range(1, curSpawnWavePoints.Length)].position; 
                }
                break;
            case SpecialMonster_E.SpawnType.ManyRamdom:
                {
                    // ���� ���� ���� -> ����
                    curModeWaves[curStage - 1].transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));

                    for (int i = 1; i < curSpawnWavePoints.Length; i++)
                    {
                        // ���� ��ġ ��ο��� ����
                        Monster_E monster = GameManager_E.Instance.Pool._SpecialMonsterPools.Get();
                        monster.transform.position = curSpawnWavePoints[i].position;
                    }
                }
                break;
            case SpecialMonster_E.SpawnType.ManySpecific:
                {                    
                    Vector3 flipScale = new Vector3(-spawnMonster.transform.localScale.x, spawnMonster.transform.localScale.y, spawnMonster.transform.localScale.z);

                    switch (curDir)
                    {
                        case SpecialMonster_E.Dir.Up:
                            break;
                        case SpecialMonster_E.Dir.Down:
                            curModeWaves[curStage - 1].transform.rotation = Quaternion.Euler(0, 0, 180);
                            break;
                        case SpecialMonster_E.Dir.Right:
                            curSpawnWavePoints = AddtionalWaves[1].GetComponentsInChildren<Transform>();
                            break;
                        case SpecialMonster_E.Dir.Left:
                            curSpawnWavePoints = AddtionalWaves[0].GetComponentsInChildren<Transform>();
                            break;
                    }

                    for (int i = 1; i < curSpawnWavePoints.Length; i++)
                    {
                        // ���� ��ġ ��ο��� ����
                        Monster_E monster = GameManager_E.Instance.Pool._SpecialMonsterPools.Get();
                        monster.transform.position = curSpawnWavePoints[i].position;

                        if (curDir != SpecialMonster_E.Dir.Idle)
                        {
                            // ���⼳��
                            monster.GetComponent<SpecialMonster_E>().myDir = curDir;

                            if (curDir == SpecialMonster_E.Dir.Right)
                            {
                                monster.transform.localScale = flipScale;
                            }
                        }

                        if (i == 1 && curStage == 2 && HardMode)
                        {
                            // �ϵ��� - ������ ���
                            monster.transform.localScale *= 2;
                            monster.GetComponent<SpecialMonster_E>().ApplyStatus(2000, 10000, 30);
                        }
                    }
                }
                break;
        }                
    }    

    void CircleSpawn(int curModeNum)
    {
        // ������ ���� ����
        GameManager_E.Instance.Pool.curSpawnMonster = circleMonsters[curModeNum];

        // ���� ����
        for (int i = 0; i < circleWave.Length; i++)
        {
            Monster_E monster = GameManager_E.Instance.Pool._CircleMonsterPools.Get();
            monster.transform.position = circleWave[i].position;
        }
    }

    void CrossSpawn(int curModeNum) // ������� = 0, �ϵ��� = 1
    {
        // ������ ���� ����
        GameManager_E.Instance.Pool.curSpawnMonster = crossMonsters[curModeNum];

        // ���� ���� ���� (����)
        crossAxis.rotation = Quaternion.Euler(0, 0, Random.Range(0, 6) * 45);

        // ũ�ν� ����       
        for (int i = 0; i < crossWave.Length; i++)
        {
            Monster_E monster = GameManager_E.Instance.Pool._CrossMonsterPools.Get();
            monster.transform.position = crossWave[i].position;
        }
    }

    void Spawn()
    {
        // ������ ���� ����
        GameManager_E.Instance.Pool.curSpawnMonster = monsterType[curPhase.monsterType];

        Monster_E monster;

        if (GameManager_E.Instance.Pool.curSpawnMonster.GetComponent<Monster_E>().myType == Monster_E.MonsterType.Baby)
        {
            monster = GameManager_E.Instance.Pool._BabyMonsterPools.Get(); // baby ���� ����
        }
        else
        {
            monster = GameManager_E.Instance.Pool._NormalMonsterPools.Get(); // normal ���� ����
        }

        // ������ ���̺� ����
        Transform curWave;

        if (StageManager.Instance.curStageNum % 4 == 0 && !HardMode)
        {
            curWave = spawnPoint[1];
        }
        else
        {
            curWave = spawnPoint[0];
        }

        Transform[] curSpawnPoints = curWave.GetComponentsInChildren<Transform>();

        // ���� �������� ����
        monster.transform.position = curSpawnPoints[Random.Range(1, curSpawnPoints.Length)].position;
    }

    void BossSpawn(int phaseNum = 0)
    {
        if (phaseNum == 0)
        {
            GameManager_E.Instance.Time.timer.gameObject.SetActive(false); // Ÿ�̸� ����
            GameManager_E.Instance.bossText.gameObject.SetActive(true); // ���� ���� ǥ��

            // ���� ���� �ִ� ����
            Monster_E[] monsters = GameManager_E.Instance.Pool.P_Monsters[0].parent.GetComponentsInChildren<Monster_E>();

            for (int i = 0; i < monsters.Length; i++)
            {
                monsters[i].DestroyMonster(1); // �ٽ� Ǯ�� ��������
            }

            // ���� �� ����
            LimitedMapSpawn();

            // ������ ���� ����
            GameManager_E.Instance.Pool.curSpawnMonster = monsterType[curPhase.monsterType];

            // ���� ����
            Monster_E boss = GameManager_E.Instance.Pool._BossMonsterPools.Get();
            boss.transform.position = bossSpawnPoint.position;
            
            // ������� ����
            SoundManager_E.Instance.ChangeBGM(Mathf.Abs(StageManager.Instance.curStageNum) - 1);
        }
        else
        {
            // ���� ����
            GameObject curBoss = Instantiate(monsterType[(phaseNum - 1) * 3 + 2]);
            curBoss.transform.position = finalWaves[0].GetChild(0).position; // ���� ���� ��ġ ����
            curBoss.transform.parent = finalMonsterParent; // �θ���
        }


        GameManager_E.Instance.bossHP.gameObject.SetActive(true); // ���� ü�� UI ǥ��
        GameManager_E.Instance.bossHP.value = 1; // ���� ü�� �ʱ�ȭ
    }

    void LimitedMapSpawn()
    {
        // ���ѵǴ� �� ����
        GameObject map = Instantiate(limitedMap);

        if (StageManager.Instance.curStageNum == 4 || StageManager.Instance.curStageNum == 8)
        {
            // ���� ���� ���� ���
            center = this.transform.position;
            map.transform.position = new Vector3(center.x, center.y, -200);
        }
        else
        {
            // ���� ���� ���
            center = bossSpawnPoint.position;
            map.transform.position = center;
        }        
    }
}
