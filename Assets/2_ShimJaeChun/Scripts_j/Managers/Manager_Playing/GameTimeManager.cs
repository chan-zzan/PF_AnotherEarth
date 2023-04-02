using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
using System.Runtime.CompilerServices;

public class GameTimeManager : MonoBehaviour
{
    #region SigleTon
    private static GameTimeManager instance;
    public static GameTimeManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<GameTimeManager>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<GameTimeManager>();
                    instance = newObj;
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        var objs = FindObjectsOfType<GameTimeManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public int countMaxTime;    // 에너지 카운트 최대 시간 (10분)

    public int countCurrentTime;// 에너지 카운트 현재 시간

    public int gamePlayTime;    // 게임 플레이 중 시간

    public int startWebTime;    // 게임시작 서버시간

    public int firstLogInTime;  // 유저 첫 로그인 시간

    public int nextDayTime;     // 다음날 접속체크 시간

    private int daySeconds = 86400; // 24시간 - 86400초

    public int dayCounting;

    public int pauseStartTime;      // 일시정지 시작 시간

    public int pauseEndTime;        // 일시정지 풀린 시간

    public bool isEnergyTimer;      // 에너지 타이머가 동작중인지?
    public int energyTimerStartTime;

    public TextMeshProUGUI energyM;
    public TextMeshProUGUI energyS;

    [Header("에너지 타이머 게임 오브젝트")]
    public GameObject energyTimer;

    private void Start()
    {
        // 0초
        countCurrentTime = 0;

        // 10분
        countMaxTime = 600;

        // 게임 진행시간 : 최초 0
        gamePlayTime = 0;

        // 서버 시간 : 최초 0
        startWebTime = 0;

        daySeconds = 86400;

        // 에너지 타이머 동작 false
        isEnergyTimer = false;

        energyTimer.SetActive(false);

        StartCoroutine(GamePlayTimer());
    }
    // 게임이 일시정지된 경우 체크
    private void OnApplicationPause(bool pause)
    {
        // 앱이 중지된 경우
        if (pause)
        {
            // 데이터 저장
            GameDataManager.Instance.AutoSave();

            // 일시정지 시간 저장
            StartCoroutine(WebChk_GamePauseTime(pause));
        }
        // 앱이 재실행 된 경우
        else
        {
            // 재실행시간 할당
            StartCoroutine(WebChk_GamePauseTime(pause));
        }
    }
    IEnumerator WebChk_GamePauseTime(bool isStart)
    {
        // bool 형 isStart 변수 사용
        // true : 앱이 중지된 시간 할당
        // false : 앱이 재실행된 시간 할당

        string url = "www.naver.com";

        UnityWebRequest request = new UnityWebRequest();

        using (request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                string date = request.GetResponseHeader("date");

                DateTime dateTime = DateTime.Parse(date);

                TimeSpan timestamp = dateTime - new System.DateTime(1970, 1, 1, 0, 0, 0);

                // 앱을 중지시킨 시간
                if (isStart)
                {
                    pauseStartTime = (int)timestamp.TotalSeconds;
                    Debug.Log("일시정지 시작 시간 할당 : " + pauseStartTime);
                }
                // 앱을 재실행 시킨 시간
                else
                {
                    pauseEndTime = (int)timestamp.TotalSeconds;

                    // 생산중인 식물에 흐른 시간 추가
                    SlotManager.Instance.AddPauseTime(pauseEndTime - pauseStartTime);

                    // 중지 타이머 초기화
                    pauseStartTime = 0;
                    pauseEndTime = 0;
                }
            }
        }

    }

    public void InitialSetting(GameData_Json _data)
    {
        if (_data != null)  // 기존 유저
        {            
            StartCoroutine(WebChk_GameStartTime(_data));
        }
        else  // 새로 시작한 유저
        {
            StartCoroutine(WebChk_GameStartTime(null));
        }
    }
    IEnumerator WebChk_GameStartTime(GameData_Json _data_)
    {
        string url = "www.naver.com";

        UnityWebRequest request = new UnityWebRequest();

        using (request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                string date = request.GetResponseHeader("date");

                DateTime dateTime = DateTime.Parse(date);

                TimeSpan timestamp = dateTime - new System.DateTime(1970, 1, 1, 0, 0, 0);

                startWebTime = (int)timestamp.TotalSeconds;

                // 기존의 유저
                if (_data_ != null)
                {

                   // 해당 유저 첫 접속 시간 그대로 할당
                   firstLogInTime = _data_.iFirstLogInTime;
                   // 다음날 기준 시간 할당
                   nextDayTime = _data_.iNextDayTime;
                   // 누적 접속일 할당
                   dayCounting = _data_.iDayCount;

                   // 접속 날짜 확인
                   RenewDayCounting(startWebTime);

                   Debug.Log("유저 첫 접속 시간: " + firstLogInTime);
                   Debug.Log("유저 현재 접속 시간: " + startWebTime);
                   Debug.Log("다음 날 출석 체크 시간: " + nextDayTime);
                   Debug.Log("누적 접속일: " + dayCounting);

                   // 기존에 에너지 카운팅 중 이었다면 카운트를 가져옴
                   if (StatManager.Instance.Max_Energy > StatManager.Instance.Own_Energy)
                   {
                        energyTimerStartTime = _data_.iEnergyTimerStartTime;
                        countCurrentTime = startWebTime - energyTimerStartTime;
                        isEnergyTimer = _data_.bIsEnergyTimer;
                        energyTimer.SetActive(true);
                        StartCoroutine(GetEnergyCounting());
                   }
                   else
                   {
                        countCurrentTime = 0;
                   }

                }
                else
                {
                    // 유저 첫 접속 시간 저장
                    firstLogInTime = startWebTime;

                    // 다음날 접속 시간 저장
                    nextDayTime = firstLogInTime + daySeconds;

                    // 누적 접속일 수 할당
                    dayCounting = 1;

                    Debug.Log("유저 첫 접속 시간: " + firstLogInTime);
                    Debug.Log("다음 날 출석 체크 시간: " + nextDayTime);
                    Debug.Log("누적 접속일: " + dayCounting);

                    // 퀘스트 데이터 업데이트 - 초기세팅
                    QuestManager.Instance.InitialSetting(false);
                }
                
            }
        }
    }

    // 게임이 시작된 순간부터 흐른 시간을 계산함.
    IEnumerator GamePlayTimer()
    {
        yield return new WaitForSeconds(1f);

        gamePlayTime++;
        StartCoroutine(GamePlayTimer());
    }

    // 최대 에너지보다 현재 에너지가 적을 경우 실행
    public void EnergyTimer()
    {
       // 에너지 타이머가 동작 중이 아니고 에너지가 부족한 상태일 경우 수행
       if (!isEnergyTimer && StatManager.Instance.Max_Energy > StatManager.Instance.Own_Energy)
       {
           StartCoroutine(WebChk_EnergyTimerStartTime());
       }
    }

    IEnumerator WebChk_EnergyTimerStartTime()
    {
        string url = "www.naver.com";

        UnityWebRequest request = new UnityWebRequest();

        using (request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                string date = request.GetResponseHeader("date");

                DateTime dateTime = DateTime.Parse(date);

                TimeSpan timestamp = dateTime - new System.DateTime(1970, 1, 1, 0, 0, 0);

                energyTimerStartTime = (int)timestamp.TotalSeconds;

                energyTimer.SetActive(true);

                isEnergyTimer = true;

                StartCoroutine(GetEnergyCounting());

                Debug.Log("에너지 타이머 실행");
            }
        }
    }


    IEnumerator GetEnergyCounting()
    {
        yield return new WaitForSecondsRealtime(1f);

        if (countCurrentTime >= countMaxTime)
        {
            int repeat = countCurrentTime / countMaxTime;

            for (int i = 0; i < repeat; i++)
            {
                // 에너지 추가
                StatManager.Instance.AddCurrentEnergy(5);
            }

            countCurrentTime = countCurrentTime % countMaxTime;

            isEnergyTimer = false;
            energyTimer.SetActive(false);

            EnergyTimer();
        }
        else
        {
            // 추가로 에너지 획득한 경우 타이머 종료
            if (StatManager.Instance.Max_Energy <= StatManager.Instance.Own_Energy)
            {
                isEnergyTimer = false;
                energyTimer.SetActive(false);
                countCurrentTime = 0;
            }
            else
            {
                ++countCurrentTime;

                TimeHMS e_time = ScoreManager.Instance.TimeToString(countMaxTime - countCurrentTime);
                energyM.text = e_time.minute.ToString();
                energyS.text = e_time.second.ToString();
                StartCoroutine(GetEnergyCounting());
            }
        }
    }

    // 날짜 확인이 필요한 부분에서 호출
    // ex) 일일 퀘스트 갱신 여부 
    public bool CheckDayTime()
    {
        // 플레이 중인 시간
        int playingDaytime = startWebTime + gamePlayTime;

        // 하루가 지난 경우
        if (playingDaytime >= nextDayTime)
        {
            RenewDayCounting(playingDaytime);
            return true;
        }
        else
        {
            return false;
        }
    }


    // 날짜 갱신 함수
    public void RenewDayCounting(int currentTime)
    {
        // 현재 시간이 출석 체크 기준 시간 이상인 경우
        if (currentTime >= nextDayTime)
        {
            // 지난 날짜 카운트 
            int lastDaycount = startWebTime / nextDayTime;

            // 하루가 넘어 접속을 한 경우 ( ex) 바로 다음날이 아닌 다다음날)
            // 다음날 출석 시간 할당
            nextDayTime += daySeconds * lastDaycount;

            // 접속일 수 카운트 갱신
            dayCounting++;

            StatManager.Instance.adsDiaCount = 3;
            StatManager.Instance.adsEnergyCount = 3;
            StatManager.Instance.adsRevivalCount = 3;

            // 퀘스트 데이터 업데이트 - 로드세팅 (다음날)
            QuestManager.Instance.InitialSetting(true);
        }
        else
        {
            Debug.Log("재접속한 유저입니다.");
            // 퀘스트 데이터 업데이트 - 로드세팅 (재접속)
            QuestManager.Instance.InitialSetting(false);
        }
    }
}
