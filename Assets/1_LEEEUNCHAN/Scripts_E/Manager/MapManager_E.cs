using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MapType_E
{
    Infinite,
    FixedVertical, // 세로 고정
    FixedHorizontal, // 가로 고정       
}

public class MapManager_E : MonoBehaviour
{
    public int stageNum; /// 테스트용

    public MapType_E curMapType; // 현재 맵의 타입

    [SerializeField]
    GameObject[] Maps; // 맵 오브젝트들

    [SerializeField]
    GameObject[] H_Maps; // 하드모드 맵

    private void Start()
    {
        //curMapType = MapType_E.Infinite;
        //curMapType = MapType_E.FixedVertical;

        // 테스트 코드
        //StageManager.Instance.curStageNum = stageNum;
        //GameObject map = Instantiate(Maps[StageManager.Instance.curStageNum]);
        //map.transform.parent = this.transform;

        int curStage = StageManager.Instance.curStageNum;

        if (curStage == 10 || curStage == -10)
        {
            curMapType = MapType_E.FixedVertical; // 세로 고정 맵
            GameManager_E.Instance.finalStage = true;
        }

        // 실제 코드
        if (curStage < 0)
        {
            int curMapNum = 0;

            // 하드모드
            if (curStage == -10)
            {
                curMapNum = 1; 
            }

            Instantiate(H_Maps[curMapNum], this.transform.position, this.transform.rotation);
            GameManager_E.Instance.isHardMode = true;
        }
        else
        {
            // 이지모드
            Instantiate(Maps[curStage - 1], this.transform.position, this.transform.rotation);

            if (curStage % 4 == 0)
            {
                curMapType = MapType_E.FixedVertical;
            }
        }

        // 배경음악 선택
        SoundManager_E.Instance.SelectBGM(Mathf.Abs(curStage) - 1);

    }

}
