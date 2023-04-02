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

    public int countMaxTime;    // ������ ī��Ʈ �ִ� �ð� (10��)

    public int countCurrentTime;// ������ ī��Ʈ ���� �ð�

    public int gamePlayTime;    // ���� �÷��� �� �ð�

    public int startWebTime;    // ���ӽ��� �����ð�

    public int firstLogInTime;  // ���� ù �α��� �ð�

    public int nextDayTime;     // ������ ����üũ �ð�

    private int daySeconds = 86400; // 24�ð� - 86400��

    public int dayCounting;

    public int pauseStartTime;      // �Ͻ����� ���� �ð�

    public int pauseEndTime;        // �Ͻ����� Ǯ�� �ð�

    public bool isEnergyTimer;      // ������ Ÿ�̸Ӱ� ����������?
    public int energyTimerStartTime;

    public TextMeshProUGUI energyM;
    public TextMeshProUGUI energyS;

    [Header("������ Ÿ�̸� ���� ������Ʈ")]
    public GameObject energyTimer;

    private void Start()
    {
        // 0��
        countCurrentTime = 0;

        // 10��
        countMaxTime = 600;

        // ���� ����ð� : ���� 0
        gamePlayTime = 0;

        // ���� �ð� : ���� 0
        startWebTime = 0;

        daySeconds = 86400;

        // ������ Ÿ�̸� ���� false
        isEnergyTimer = false;

        energyTimer.SetActive(false);

        StartCoroutine(GamePlayTimer());
    }
    // ������ �Ͻ������� ��� üũ
    private void OnApplicationPause(bool pause)
    {
        // ���� ������ ���
        if (pause)
        {
            // ������ ����
            GameDataManager.Instance.AutoSave();

            // �Ͻ����� �ð� ����
            StartCoroutine(WebChk_GamePauseTime(pause));
        }
        // ���� ����� �� ���
        else
        {
            // �����ð� �Ҵ�
            StartCoroutine(WebChk_GamePauseTime(pause));
        }
    }
    IEnumerator WebChk_GamePauseTime(bool isStart)
    {
        // bool �� isStart ���� ���
        // true : ���� ������ �ð� �Ҵ�
        // false : ���� ������ �ð� �Ҵ�

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

                // ���� ������Ų �ð�
                if (isStart)
                {
                    pauseStartTime = (int)timestamp.TotalSeconds;
                    Debug.Log("�Ͻ����� ���� �ð� �Ҵ� : " + pauseStartTime);
                }
                // ���� ����� ��Ų �ð�
                else
                {
                    pauseEndTime = (int)timestamp.TotalSeconds;

                    // �������� �Ĺ��� �帥 �ð� �߰�
                    SlotManager.Instance.AddPauseTime(pauseEndTime - pauseStartTime);

                    // ���� Ÿ�̸� �ʱ�ȭ
                    pauseStartTime = 0;
                    pauseEndTime = 0;
                }
            }
        }

    }

    public void InitialSetting(GameData_Json _data)
    {
        if (_data != null)  // ���� ����
        {            
            StartCoroutine(WebChk_GameStartTime(_data));
        }
        else  // ���� ������ ����
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

                // ������ ����
                if (_data_ != null)
                {

                   // �ش� ���� ù ���� �ð� �״�� �Ҵ�
                   firstLogInTime = _data_.iFirstLogInTime;
                   // ������ ���� �ð� �Ҵ�
                   nextDayTime = _data_.iNextDayTime;
                   // ���� ������ �Ҵ�
                   dayCounting = _data_.iDayCount;

                   // ���� ��¥ Ȯ��
                   RenewDayCounting(startWebTime);

                   Debug.Log("���� ù ���� �ð�: " + firstLogInTime);
                   Debug.Log("���� ���� ���� �ð�: " + startWebTime);
                   Debug.Log("���� �� �⼮ üũ �ð�: " + nextDayTime);
                   Debug.Log("���� ������: " + dayCounting);

                   // ������ ������ ī���� �� �̾��ٸ� ī��Ʈ�� ������
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
                    // ���� ù ���� �ð� ����
                    firstLogInTime = startWebTime;

                    // ������ ���� �ð� ����
                    nextDayTime = firstLogInTime + daySeconds;

                    // ���� ������ �� �Ҵ�
                    dayCounting = 1;

                    Debug.Log("���� ù ���� �ð�: " + firstLogInTime);
                    Debug.Log("���� �� �⼮ üũ �ð�: " + nextDayTime);
                    Debug.Log("���� ������: " + dayCounting);

                    // ����Ʈ ������ ������Ʈ - �ʱ⼼��
                    QuestManager.Instance.InitialSetting(false);
                }
                
            }
        }
    }

    // ������ ���۵� �������� �帥 �ð��� �����.
    IEnumerator GamePlayTimer()
    {
        yield return new WaitForSeconds(1f);

        gamePlayTime++;
        StartCoroutine(GamePlayTimer());
    }

    // �ִ� ���������� ���� �������� ���� ��� ����
    public void EnergyTimer()
    {
       // ������ Ÿ�̸Ӱ� ���� ���� �ƴϰ� �������� ������ ������ ��� ����
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

                Debug.Log("������ Ÿ�̸� ����");
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
                // ������ �߰�
                StatManager.Instance.AddCurrentEnergy(5);
            }

            countCurrentTime = countCurrentTime % countMaxTime;

            isEnergyTimer = false;
            energyTimer.SetActive(false);

            EnergyTimer();
        }
        else
        {
            // �߰��� ������ ȹ���� ��� Ÿ�̸� ����
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

    // ��¥ Ȯ���� �ʿ��� �κп��� ȣ��
    // ex) ���� ����Ʈ ���� ���� 
    public bool CheckDayTime()
    {
        // �÷��� ���� �ð�
        int playingDaytime = startWebTime + gamePlayTime;

        // �Ϸ簡 ���� ���
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


    // ��¥ ���� �Լ�
    public void RenewDayCounting(int currentTime)
    {
        // ���� �ð��� �⼮ üũ ���� �ð� �̻��� ���
        if (currentTime >= nextDayTime)
        {
            // ���� ��¥ ī��Ʈ 
            int lastDaycount = startWebTime / nextDayTime;

            // �Ϸ簡 �Ѿ� ������ �� ��� ( ex) �ٷ� �������� �ƴ� �ٴ�����)
            // ������ �⼮ �ð� �Ҵ�
            nextDayTime += daySeconds * lastDaycount;

            // ������ �� ī��Ʈ ����
            dayCounting++;

            StatManager.Instance.adsDiaCount = 3;
            StatManager.Instance.adsEnergyCount = 3;
            StatManager.Instance.adsRevivalCount = 3;

            // ����Ʈ ������ ������Ʈ - �ε弼�� (������)
            QuestManager.Instance.InitialSetting(true);
        }
        else
        {
            Debug.Log("�������� �����Դϴ�.");
            // ����Ʈ ������ ������Ʈ - �ε弼�� (������)
            QuestManager.Instance.InitialSetting(false);
        }
    }
}
