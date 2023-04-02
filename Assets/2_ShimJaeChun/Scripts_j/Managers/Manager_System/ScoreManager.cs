using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TimeHMS
{
    public int hour;
    public int minute;
    public int second;

    public TimeHMS(int h, int m, int s)
    {
        hour = h;
        minute = m;
        second = s;
    }
}

public class ScoreManager : MonoBehaviour
{
    // 스코어 매니저 : 재화, 시간, 점수등을 계산하여 string 타입으로 반환
    // ex) 1100미네랄 -> 1.1k 미네랄
    //     80s -> 1m 20s

    #region SigleTon
    private static ScoreManager instance;
    public static ScoreManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<ScoreManager>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<ScoreManager>();
                    instance = newObj;
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        var objs = FindObjectsOfType<ScoreManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    [HideInInspector]
    public int countMaxTime;    // 에너지 카운트 최대 시간 (10분)

    [HideInInspector]
    public int countCurrentTime;// 에너지 카운트 현재 시간

    [HideInInspector]
    public int gamePlayTime;    // 게임 플레이 중 시간

    [HideInInspector]
    public int startWebTime;    // 게임시작 서버시간

    // 시간 변환
    public TimeHMS TimeToString(int time)
    {
        TimeHMS mytime = new TimeHMS(0, 0, 0);

        // 0초는 그대로 반환
        if (time == 0f)
        {
            return mytime;
        }

        int temp_time = time;

        if (temp_time >= 3600f)
        {
            mytime.hour = temp_time / 3600;
            temp_time -= (int)mytime.hour * 3600;
        }
        if (temp_time >= 60f)
        {
            mytime.minute = temp_time / 60;
            temp_time -= (int)mytime.minute * 60;
        }
        if (temp_time > 0f)
        {
            mytime.second = temp_time;
        }

        return mytime;
    }

    // 점수 변환
    public string ScoreToString(double score)
    {
        string str = score.ToString();
        //Debug.Log("금액:" + str);
        // 정수 표현 : 해당 단위
        // 소수 표현 : 해당 단위/10

        // 0 단위
        if (0 <= str.Length && str.Length < 4)
        {
            return str;
        }
        // k 단위 (1,000)
        else if (4 <= str.Length && str.Length < 7)
        {
            string _integer = "";

            for (int i = 0; i < (str.Length - 3); i++)
            {
                _integer += str[i];
            }

            str = _integer + "." + str[str.Length - 3] + "k";

            return str;
        }
        // M 단위 (1,000,000)
        else if (7 <= str.Length && str.Length < 10)
        {
            string _integer = "";

            for (int i = 0; i < (str.Length - 6); i++)
            {
                _integer += str[i];
            }

            str = _integer + "." + str[str.Length - 6] + "M";

            return str;
        }
        // G 단위 최대 (1,000,000,000)
        else if (str.Length >= 10)   // G 단위 (최대)
        {
            string _integer = "";

            for (int i = 0; i < (str.Length - 9); i++)
            {
                _integer += str[i];
            }

            str = _integer + "." + str[str.Length - 9] + "G";

            return str;
        }
        else
        {
            str = "0";
            return str;
        }
    }

}
