using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;


public class MonsterSpawner_E : MonoBehaviour
{
    public Transform[] spawnPoint; // 몬스터 생성 위치
    public Transform bossSpawnPoint; // 보스 생성 위치

    [Space(10)]

    public Transform[] crossWave; // 크로스 몬스터 생성 위치
    public GameObject[] crossMonsters;// 크로스 몬스터 프리팹
    public Transform crossAxis; // 크로스 몬스터 축

    [Space(10)]

    public Transform[] circleWave; // 원모양 떼거지 몬스터 생성 위치
    public GameObject[] circleMonsters;// 원모양 떼거지 몬스터 프리팹

    [Space(10)]

    // 이지모드
    public Transform[] specialWaves; // 특수 몬스터 웨이브 종류
    public GameObject[] speialMonsters; // 특수 몬스터 프리팹들

    [Space(10)]

    // 하드모드
    public Transform[] specialWaves_H; // 특수 몬스터 웨이브 종류
    public GameObject[] speialMonsters_H; // 특수 몬스터 프리팹들

    [Space(10)]

    // 타일 몬스터
    public Transform[] tileWaves_H; // 특수 몬스터 웨이브 종류
    public GameObject[] tileMonsters_H; // 타일 몬스터 프리팹들

    [Space(10)]

    // 파이널 웨이브
    public Transform[] finalWaves;
    public Transform finalMonsterParent; // 몬스터의 부모 위치
    public int finalPhaseNum; // 파이널 페이즈 번호

    [Space(10)]

    public Transform[] AddtionalWaves; // 추가 웨이브

    [Space(10)]

    // 보스 생성시 제한되는 맵
    public GameObject limitedMap;
    public Vector2 center; // 맵의 중앙 위치

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

        /// 테스트 용

        //curPhase = Phases[Phases.Count - 1];
        //BossSpawn();
        //return;

        /// 테스트 용 



        // 현재 스테이지 번호
        int curStageNum = StageManager.Instance.curStageNum;

        if (GameManager_E.Instance.finalStage)
        {
            // 파이널 스테이지인 경우
            StartCoroutine(FinalStageStart());
            center = this.transform.position;
            return;
        }

        // 하드모드인지 판단
        if (curStageNum < 0) HardMode = true;

        // 페이즈 카운팅 및 몬스터 생성
        StartCoroutine(PhaseCounting());

        // 보스 및 기본 페이즈 몬스터(새, 뱀) 생성 코루틴 시작
        StartCoroutine(PhaseMonsterSpawnStart());                

        SpecialMonster_E curModeSpecialMonster;

        if (!HardMode)
        {
            // 이지모드
            if (speialMonsters[curStageNum - 1] == null) return;
            curModeSpecialMonster = speialMonsters[curStageNum - 1].GetComponent<SpecialMonster_E>();
        }
        else
        {
            // 하드모드
            if (speialMonsters_H[-curStageNum - 1] == null) return;
            curModeSpecialMonster = speialMonsters_H[-curStageNum - 1].GetComponent<SpecialMonster_E>();

            // 타일몬스터 생성 코루틴 시작
            StartCoroutine(SpecialSpawnStart(tileMonsters_H[(-curStageNum - 1)].GetComponent<SpecialMonster_E>(), curStageNum));
        }

        // 특수몬스터 생성 코루틴 시작
        StartCoroutine(SpecialSpawnStart(curModeSpecialMonster, curStageNum));
    }

    IEnumerator FinalStageStart()
    {
        for (int phaseNum = 0; phaseNum < 10; phaseNum++)
        {
            curPhase = Phases[phaseNum]; // 페이즈 증가
            finalPhaseNum = phaseNum;

            if (phaseNum == 0)
            {
                // 첫번째 페이즈 -> 모든 베이비 몬스터들을 2초간격으로 5마리씩 소환
                for (int i = 0; i < 26; i += 3)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        GameObject mon = Instantiate(monsterType[i]);
                        mon.transform.parent = finalMonsterParent; // 부모설정

                        int randomNum = Random.Range(0, finalWaves[1].childCount);
                        mon.transform.position = finalWaves[1].GetChild(randomNum).position; // 생성 위치 설정
                    }

                    yield return new WaitForSeconds(2.0f);
                }

                print("1 : " + GameManager_E.Instance.Time.playTime);
                yield return new WaitForSeconds(20 - GameManager_E.Instance.Time.playTime);
            }
            else
            {
                // 두번째 페이즈 이후 -> 노말 몬스터들을 한번에 소환
                for (int i = 0; i < curPhase.number; i++)
                {
                    GameObject mon = Instantiate(monsterType[(phaseNum - 1) * 3 + 1]);
                    mon.transform.parent = finalMonsterParent; // 부모설정

                    int waveNum;

                    if (phaseNum > 3) waveNum = 0; // 20마리씩 소환
                    else waveNum = 1; // 30마리씩 소환

                    mon.transform.position = finalWaves[waveNum].GetChild(i + 1).position; // 생성 위치 설정
                }

                // 보스 소환
                BossSpawn(phaseNum);

                if (phaseNum == 9) yield break; // 마지막 보스인 경우

                yield return FinalStageTimeCounting(phaseNum); // 카운트

                print("2 : " + GameManager_E.Instance.Time.playTime);
            }

            // 보스가 죽었는지 판단
            if (GameManager_E.Instance.finalStageCount == phaseNum)
            {
                // 죽은경우 3초 카운트 후 다음 스테이지 진행
                yield return Warning(-1);
                print("3 : " + GameManager_E.Instance.Time.playTime);
            }
            else
            {
                // 보스를 못 죽인 경우 -> 패배
                GameManager_E.Instance.FinalWarningPopup2.SetActive(false);
                GameManager_E.Instance.life = 0; // 부활x
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

            // 시간내에 보스를 잡은 경우
            if (bossCount == GameManager_E.Instance.finalStageCount)
            {
                // 타이머 팝업이 켜져있는 경우 -> 팝업을 끔
                if (warningPopup.activeSelf)
                {
                    warningPopup.SetActive(false);
                }

                break;
            }

            if (timer > 15.0f)
            {
                // 팝업이 켜져있지 않은 경우 -> 팝업을 켬
                if (!warningPopup.activeSelf) warningPopup.SetActive(true);

                // 남은시간 표시
                warningPopup.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().text = "" + Mathf.Ceil(20 - timer);
            }

            yield return null;
        }
    }

    public void SpawnAllStop()
    {
        // 모든 스폰을 중지시킴 -> 패배창이 뜬 경우
        StopAllCoroutines();
    }

    IEnumerator PhaseCounting()
    {
        for (int phaseNum = 0; phaseNum < 12; phaseNum++)
        {
            print("1 : " + GameManager_E.Instance.Time.playTime);
            print("1-2 : " + phaseNum);

            curPhase = Phases[phaseNum]; // 15초마다 페이즈 증가
            yield return MonsterSpawnStart(15.0f); // 몬스터 생성
                                                   
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
        int monsterPerSecond = (int)Mathf.Ceil(curPhase.number / spawnTime); // 초당 몬스터 생성 수(올림)
        bool isLastMonster = false; // 마지막 생성인지 여부

        for (int i = 0; i < 15; i++)
        {
            if (monsterPerSecond * i > curPhase.number)
            {
                monsterPerSecond = curPhase.number - monsterPerSecond * i;
                isLastMonster = true;
            }

            // 1초에 한번씩 몬스터 소환
            for (int j = 0; j < monsterPerSecond; j++)
            {
                Spawn();
            }

            if (isLastMonster) break; // 마지막 생성 후 바로 리턴

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
                // 처음에는 27초후에 생성
                yield return new WaitForSeconds(27.0f);
            }
            else
            {
                // 30초마다 생성
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
            // 파이널 경고
            warningPopup = GameManager_E.Instance.FinalWarningPopup;
        }
        else
        {
            warningPopup = GameManager_E.Instance.warningPopup;
        }

        GameManager_E.Instance.warningAnim.SetTrigger("warning");
        SoundManager_E.Instance.EffectSoundPlay2(2); // 효과음 on

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

        // 현재 모드에 따라 스폰하는 몬스터 종류 변경
        int spawnNum = GameManager_E.Instance.isHardMode ? 1 : 0;

        switch (num)
        {
            case 0:
                curPhase = Phases[12];
                BossSpawn();
                break;
            case 1:
                CrossSpawn(spawnNum); // 일렬로 지나가는 몬스터
                break;
            case 2:
                CircleSpawn(spawnNum); // 원형 떼거지 몬스터
                break;
        }
    }

    IEnumerator SpecialSpawnStart(SpecialMonster_E spawnMonster, int curStageNum)
    {
        // 생성시간
        float[] spawnTimes = spawnMonster.mySpawnTime;

        // 생성횟수
        int spawnNum = spawnTimes.Length;

        // 생성 방향
        SpecialMonster_E.Dir curDir = spawnMonster.myDir;

        for (int i = 0; i < spawnNum; i++)
        {
            // 생성 시간까지 대기후 생성
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
        // 생성할 몬스터 설정
        GameManager_E.Instance.Pool.curSpawnMonster = spawnMonster.gameObject;

        // 생성할 지점들
        Transform[] curSpawnWavePoints = tileWaves_H[-curStage - 1].GetComponentsInChildren<Transform>();

        // 원형으로 생성
        for (int i = 1; i < curSpawnWavePoints.Length; i++)
        {
            Monster_E monster = GameManager_E.Instance.Pool._TileMonsterPools.Get();
            monster.transform.position = curSpawnWavePoints[i].position;
        }
    }

    void SpecialSpawn(SpecialMonster_E spawnMonster, int curStage, SpecialMonster_E.Dir curDir = SpecialMonster_E.Dir.Idle) // 특수 몬스터 스폰
    {
        // 생성할 몬스터 설정
        GameManager_E.Instance.Pool.curSpawnMonster = spawnMonster.gameObject;

        Transform[] curModeWaves;
        Transform[] curSpawnWavePoints;

        // 해당 웨이브의 자식들의 위치 = 스폰 위치 
        if (!HardMode)
        {
            // 이지모드
            curModeWaves = specialWaves;
        }
        else
        {
            // 하드모드
            curStage *= -1; 
            curModeWaves = specialWaves_H;
        }

        curSpawnWavePoints = curModeWaves[curStage - 1].GetComponentsInChildren<Transform>();

        // 생성 방식 설정
        switch (spawnMonster.mySpawnType)
        {
            case SpecialMonster_E.SpawnType.OneRandom:
                {
                    // 스폰 위치중 랜덤한 위치에서 하나만 생성
                    Monster_E monster = GameManager_E.Instance.Pool._SpecialMonsterPools.Get();
                    monster.transform.position = curSpawnWavePoints[Random.Range(1, curSpawnWavePoints.Length)].position; 
                }
                break;
            case SpecialMonster_E.SpawnType.ManyRamdom:
                {
                    // 생성 방향 설정 -> 랜덤
                    curModeWaves[curStage - 1].transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));

                    for (int i = 1; i < curSpawnWavePoints.Length; i++)
                    {
                        // 스폰 위치 모두에서 생성
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
                        // 스폰 위치 모두에서 생성
                        Monster_E monster = GameManager_E.Instance.Pool._SpecialMonsterPools.Get();
                        monster.transform.position = curSpawnWavePoints[i].position;

                        if (curDir != SpecialMonster_E.Dir.Idle)
                        {
                            // 방향설정
                            monster.GetComponent<SpecialMonster_E>().myDir = curDir;

                            if (curDir == SpecialMonster_E.Dir.Right)
                            {
                                monster.transform.localScale = flipScale;
                            }
                        }

                        if (i == 1 && curStage == 2 && HardMode)
                        {
                            // 하드모드 - 오리의 경우
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
        // 생성할 몬스터 설정
        GameManager_E.Instance.Pool.curSpawnMonster = circleMonsters[curModeNum];

        // 원형 스폰
        for (int i = 0; i < circleWave.Length; i++)
        {
            Monster_E monster = GameManager_E.Instance.Pool._CircleMonsterPools.Get();
            monster.transform.position = circleWave[i].position;
        }
    }

    void CrossSpawn(int curModeNum) // 이지모드 = 0, 하드모드 = 1
    {
        // 생성할 몬스터 설정
        GameManager_E.Instance.Pool.curSpawnMonster = crossMonsters[curModeNum];

        // 생성 방향 설정 (랜덤)
        crossAxis.rotation = Quaternion.Euler(0, 0, Random.Range(0, 6) * 45);

        // 크로스 스폰       
        for (int i = 0; i < crossWave.Length; i++)
        {
            Monster_E monster = GameManager_E.Instance.Pool._CrossMonsterPools.Get();
            monster.transform.position = crossWave[i].position;
        }
    }

    void Spawn()
    {
        // 생성할 몬스터 설정
        GameManager_E.Instance.Pool.curSpawnMonster = monsterType[curPhase.monsterType];

        Monster_E monster;

        if (GameManager_E.Instance.Pool.curSpawnMonster.GetComponent<Monster_E>().myType == Monster_E.MonsterType.Baby)
        {
            monster = GameManager_E.Instance.Pool._BabyMonsterPools.Get(); // baby 몬스터 생성
        }
        else
        {
            monster = GameManager_E.Instance.Pool._NormalMonsterPools.Get(); // normal 몬스터 생성
        }

        // 생성할 웨이브 설정
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

        // 랜덤 구역에서 생성
        monster.transform.position = curSpawnPoints[Random.Range(1, curSpawnPoints.Length)].position;
    }

    void BossSpawn(int phaseNum = 0)
    {
        if (phaseNum == 0)
        {
            GameManager_E.Instance.Time.timer.gameObject.SetActive(false); // 타이머 삭제
            GameManager_E.Instance.bossText.gameObject.SetActive(true); // 보스 등장 표시

            // 현재 남아 있는 몬스터
            Monster_E[] monsters = GameManager_E.Instance.Pool.P_Monsters[0].parent.GetComponentsInChildren<Monster_E>();

            for (int i = 0; i < monsters.Length; i++)
            {
                monsters[i].DestroyMonster(1); // 다시 풀로 돌려놓음
            }

            // 제한 맵 생성
            LimitedMapSpawn();

            // 생성할 몬스터 설정
            GameManager_E.Instance.Pool.curSpawnMonster = monsterType[curPhase.monsterType];

            // 보스 생성
            Monster_E boss = GameManager_E.Instance.Pool._BossMonsterPools.Get();
            boss.transform.position = bossSpawnPoint.position;
            
            // 배경음악 변경
            SoundManager_E.Instance.ChangeBGM(Mathf.Abs(StageManager.Instance.curStageNum) - 1);
        }
        else
        {
            // 보스 생성
            GameObject curBoss = Instantiate(monsterType[(phaseNum - 1) * 3 + 2]);
            curBoss.transform.position = finalWaves[0].GetChild(0).position; // 보스 생성 위치 결정
            curBoss.transform.parent = finalMonsterParent; // 부모설정
        }


        GameManager_E.Instance.bossHP.gameObject.SetActive(true); // 보스 체력 UI 표시
        GameManager_E.Instance.bossHP.value = 1; // 보스 체력 초기화
    }

    void LimitedMapSpawn()
    {
        // 제한되는 맵 생성
        GameObject map = Instantiate(limitedMap);

        if (StageManager.Instance.curStageNum == 4 || StageManager.Instance.curStageNum == 8)
        {
            // 가로 무한 맵인 경우
            center = this.transform.position;
            map.transform.position = new Vector3(center.x, center.y, -200);
        }
        else
        {
            // 무한 맵인 경우
            center = bossSpawnPoint.position;
            map.transform.position = center;
        }        
    }
}
