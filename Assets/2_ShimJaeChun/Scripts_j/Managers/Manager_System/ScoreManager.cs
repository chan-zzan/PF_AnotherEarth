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
    // ���ھ� �Ŵ��� : ��ȭ, �ð�, �������� ����Ͽ� string Ÿ������ ��ȯ
    // ex) 1100�̳׶� -> 1.1k �̳׶�
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
    public int countMaxTime;    // ������ ī��Ʈ �ִ� �ð� (10��)

    [HideInInspector]
    public int countCurrentTime;// ������ ī��Ʈ ���� �ð�

    [HideInInspector]
    public int gamePlayTime;    // ���� �÷��� �� �ð�

    [HideInInspector]
    public int startWebTime;    // ���ӽ��� �����ð�

    // �ð� ��ȯ
    public TimeHMS TimeToString(int time)
    {
        TimeHMS mytime = new TimeHMS(0, 0, 0);

        // 0�ʴ� �״�� ��ȯ
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

    // ���� ��ȯ
    public string ScoreToString(double score)
    {
        string str = score.ToString();
        //Debug.Log("�ݾ�:" + str);
        // ���� ǥ�� : �ش� ����
        // �Ҽ� ǥ�� : �ش� ����/10

        // 0 ����
        if (0 <= str.Length && str.Length < 4)
        {
            return str;
        }
        // k ���� (1,000)
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
        // M ���� (1,000,000)
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
        // G ���� �ִ� (1,000,000,000)
        else if (str.Length >= 10)   // G ���� (�ִ�)
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
