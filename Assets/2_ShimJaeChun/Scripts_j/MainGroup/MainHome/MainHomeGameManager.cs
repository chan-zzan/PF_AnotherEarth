using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHomeGameManager : MonoBehaviour
{
    private void OnEnable()
    {
        // 웹서버 타임 리퀘스트가 정상 작동한 경우
        if(GameTimeManager.Instance.startWebTime>0)
        {
            if(GameTimeManager.Instance.CheckDayTime())
            {
                // 날짜 갱신 성공 -> 퀘스트 갱신

            }
        }
    }
}
