using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHomeGameManager : MonoBehaviour
{
    private void OnEnable()
    {
        // ������ Ÿ�� ������Ʈ�� ���� �۵��� ���
        if(GameTimeManager.Instance.startWebTime>0)
        {
            if(GameTimeManager.Instance.CheckDayTime())
            {
                // ��¥ ���� ���� -> ����Ʈ ����

            }
        }
    }
}
