using UnityEngine;
using UnityEngine.UI;

public class Stage_E : MonoBehaviour
{
    public int stageLevel; // 스테이지 레벨
    public StageData_E myData; // 각 스테이지별 데이터

    private void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(selectStage);
        this.GetComponent<Button>().onClick.AddListener(loadScene);
    }

    void selectStage()
    {

        StageManager_E.Instance.SelectStage();
    }

    void loadScene()
    {
        StageManager_E.Instance.LoadScene(2);
    }
}
