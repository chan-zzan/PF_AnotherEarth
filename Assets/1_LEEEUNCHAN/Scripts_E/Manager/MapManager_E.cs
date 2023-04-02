using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MapType_E
{
    Infinite,
    FixedVertical, // ���� ����
    FixedHorizontal, // ���� ����       
}

public class MapManager_E : MonoBehaviour
{
    public int stageNum; /// �׽�Ʈ��

    public MapType_E curMapType; // ���� ���� Ÿ��

    [SerializeField]
    GameObject[] Maps; // �� ������Ʈ��

    [SerializeField]
    GameObject[] H_Maps; // �ϵ��� ��

    private void Start()
    {
        //curMapType = MapType_E.Infinite;
        //curMapType = MapType_E.FixedVertical;

        // �׽�Ʈ �ڵ�
        //StageManager.Instance.curStageNum = stageNum;
        //GameObject map = Instantiate(Maps[StageManager.Instance.curStageNum]);
        //map.transform.parent = this.transform;

        int curStage = StageManager.Instance.curStageNum;

        if (curStage == 10 || curStage == -10)
        {
            curMapType = MapType_E.FixedVertical; // ���� ���� ��
            GameManager_E.Instance.finalStage = true;
        }

        // ���� �ڵ�
        if (curStage < 0)
        {
            int curMapNum = 0;

            // �ϵ���
            if (curStage == -10)
            {
                curMapNum = 1; 
            }

            Instantiate(H_Maps[curMapNum], this.transform.position, this.transform.rotation);
            GameManager_E.Instance.isHardMode = true;
        }
        else
        {
            // �������
            Instantiate(Maps[curStage - 1], this.transform.position, this.transform.rotation);

            if (curStage % 4 == 0)
            {
                curMapType = MapType_E.FixedVertical;
            }
        }

        // ������� ����
        SoundManager_E.Instance.SelectBGM(Mathf.Abs(curStage) - 1);

    }

}
