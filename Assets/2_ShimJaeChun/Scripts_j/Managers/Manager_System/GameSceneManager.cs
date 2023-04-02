using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    // �� ��ȯ �Ŵ���
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
    
    [Header("�� �ε� ���൵")]
    public float sceneLoadProgress;

    [Header("����ũ �ε� Ÿ��")]
    public float fakeLoadingTime;

    [Header("�ε��� �������� üũ")]
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

        // ���� �ε���� ���� ��� �ݺ�
        while(!oper.isDone)
        {
            yield return null;
            
            // ���� 90�ۼ�Ʈ �̻� �ε尡 �� ���
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

        // ���� -> ����
        if (__sceneName == "Main_j" && SceneManager.GetActiveScene().name != "Start_j")
        {
            MainCameraSingleton.Instance.gameObject.SetActive(true);
            CanvasSingleton.Instance.gameObject.SetActive(true);
        }
        // ���� -> ����
        else if (__sceneName == "InGame_E")
        {
            MainCameraSingleton.Instance.gameObject.SetActive(false);
            CanvasSingleton.Instance.gameObject.SetActive(false);
        }
    }
}
