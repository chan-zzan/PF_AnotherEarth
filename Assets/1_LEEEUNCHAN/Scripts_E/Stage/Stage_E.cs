using UnityEngine;
using UnityEngine.UI;

public class Stage_E : MonoBehaviour
{
    public int stageLevel; // �������� ����
    public StageData_E myData; // �� ���������� ������

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
