using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    // 씬 전환 매니저
    #region SigleTon
    private static GameSceneManager instance;
    public static GameSceneManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<GameSceneManager>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<GameSceneManager>();
                    instance = newObj;
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        var objs = FindObjectsOfType<GameSceneManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    #endregion
    
    [Header("씬 로드 진행도")]
    public float sceneLoadProgress;

    [Header("페이크 로딩 타임")]
    public float fakeLoadingTime;

    [Header("로드한 게임인지 체크")]
    public bool isLoadGame;

    [HideInInspector]
    public AsyncOperation oper;

    private void Start()
    {
        sceneLoadProgress = 0;
    }

    public void LoadGameScene(string sceneName)
    {
        StartCoroutine(AsyncSceneLoad(sceneName));
    }

    IEnumerator AsyncSceneLoad(string _sceneName)
    {
        yield return null;

        oper = SceneManager.LoadSceneAsync(_sceneName);

        oper.allowSceneActivation = false;

        // 씬이 로드되지 않은 경우 반복
        while(!oper.isDone)
        {
            yield return null;
            
            // 씬이 90퍼센트 이상 로드가 된 경우
            if(oper.progress >= 0.9f)
            {

                StartCoroutine(FakeLoading(_sceneName));

                Debug.Log("Fake Loading");

                break;
            }
        }
    }

    IEnumerator FakeLoading(string __sceneName)
    {
        yield return new WaitForSeconds(fakeLoadingTime);

        oper.allowSceneActivation = true;

        // 전투 -> 메인
        if (__sceneName == "Main_j" && SceneManager.GetActiveScene().name != "Start_j")
        {
            MainCameraSingleton.Instance.gameObject.SetActive(true);
            CanvasSingleton.Instance.gameObject.SetActive(true);
        }
        // 메인 -> 전투
        else if (__sceneName == "InGame_E")
        {
            MainCameraSingleton.Instance.gameObject.SetActive(false);
            CanvasSingleton.Instance.gameObject.SetActive(false);
        }
    }
}
