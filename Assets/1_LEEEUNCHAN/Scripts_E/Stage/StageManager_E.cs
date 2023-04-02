using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StageManager_E : MonoBehaviour
{
    #region 싱글톤
    private static StageManager_E instance = null;

    public static StageManager_E Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<StageManager_E>();

                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<StageManager_E>();
                    instance = newObj;
                }
            }
            return instance;
        }
    }
    #endregion

    public StageData_E curStageData; // 현재 스테이지 데이터

    private void Awake()
    {
        var objs = FindObjectsOfType<StageManager_E>();

        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

    }

    public void SelectStage()
    {
        print(EventSystem.current.currentSelectedGameObject.name);

        // 클릭한 스테이지의 스테이지 정보를 불러옴
        curStageData = EventSystem.current.currentSelectedGameObject.GetComponent<Stage_E>().myData;
    }

    public void LoadScene(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }
}
