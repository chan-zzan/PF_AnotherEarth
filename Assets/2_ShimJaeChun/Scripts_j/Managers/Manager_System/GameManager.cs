using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
public class GameManager : MonoBehaviour
{
    [Header("�ε� ȭ�� �׷�")]
    public GameObject layout_LoadingGroup;

    [Header("�ε� ��ư ��")]
    public GameObject loadButtonLock;

    [Header("�ε� ��ư")]
    public Button loadButton;

    public TextMeshProUGUI internetText;

    public void Start()
    {
        // ������ ������ ���� ���
        if (!File.Exists(Application.persistentDataPath + "/UserData_j.json")
            && !File.Exists(Application.persistentDataPath + "/UserProductData_j.json"))
        {
            Debug.Log("������ ���� ����");
            // ���� ���� ����
            // ������ �ٿ�޾Ҵ� ���� ������ ����
            if (File.Exists(Application.persistentDataPath + "/UserData.json"))
            {
                System.IO.File.Delete(Application.persistentDataPath + "/UserData.json");
            }
            if (File.Exists(Application.persistentDataPath + "/ProductData.json"))
            {
                System.IO.File.Delete(Application.persistentDataPath + "/ProductData.json");
            }
            if (File.Exists(Application.persistentDataPath + "/QuestData.json"))
            {
                System.IO.File.Delete(Application.persistentDataPath + "/QuestData.json");
            }
            if (File.Exists(Application.persistentDataPath + "/StageData.json"))
            {
                System.IO.File.Delete(Application.persistentDataPath + "/StageData.json");
            }
            if (File.Exists(Application.persistentDataPath + "/UserProductData.json"))
            {
                System.IO.File.Delete(Application.persistentDataPath + "/UserProductData.json");
            }

            StartCoroutine(StartTimer(false));
        }
        else
        {
            StartCoroutine(StartTimer(true));
        }
    }

    // ����ȭ�� �����ֱ� ��
    IEnumerator StartTimer(bool isLoadGame)
    {
        yield return new WaitForSeconds(1f);

        StartCoroutine(WebChk(isLoadGame));
    }

    // ���ͳ� ���� üũ�� 
    // ����Ȯ�εǸ� ���ε�
    IEnumerator WebChk(bool isLoadGame)
    {
        string url = "www.naver.com";

        UnityWebRequest request = new UnityWebRequest();

        using (request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
                StartCoroutine(WebChk(isLoadGame));
            }
            else
            {
                if (!isLoadGame)
                {
                    GameSceneManager.Instance.isLoadGame = false;
                }
                else
                {
                    GameSceneManager.Instance.isLoadGame = true;
                }

                GameSceneManager.Instance.LoadGameScene("Main_j");
            }
        }
    }

//    public void OnClickStartButton()
//    {
//        // ���� ���� ����
//        if (File.Exists(Application.persistentDataPath + "/UserData.json"))
//        {
//            System.IO.File.Delete(Application.persistentDataPath + "/UserData.json");
//        }
//        if (File.Exists(Application.persistentDataPath + "/ProductData.json"))
//        {
//            System.IO.File.Delete(Application.persistentDataPath + "/ProductData.json");
//        }
//        if (File.Exists(Application.persistentDataPath + "/QuestData.json"))
//        {
//            System.IO.File.Delete(Application.persistentDataPath + "/QuestData.json");
//        }
//        if (File.Exists(Application.persistentDataPath + "/StageData.json"))
//        {
//            System.IO.File.Delete(Application.persistentDataPath + "/StageData.json");
//        }
//        if (File.Exists(Application.persistentDataPath + "/UserProductData.json"))
//        {
//            System.IO.File.Delete(Application.persistentDataPath + "/UserProductData.json");
//        }

//        GameSceneManager.Instance.isLoadGame = false;

//        layout_LoadingGroup.SetActive(true);

//        internetText.text = "���ͳ� ���� Ȯ����...";

//        StartCoroutine(WebChk(false));
//        //SceneManager.LoadScene("Main_j");
//    }
//    public void OnClickLoadButton()
//    {
//        GameSceneManager.Instance.isLoadGame = true;

//        layout_LoadingGroup.SetActive(true);

//        internetText.text = "���ͳ� ���� Ȯ����...";

//        StartCoroutine(WebChk(true));
//    }
//    public void OnClickQuitButton()
//    {
//        // �������� ��ư Ŭ�� ��

//    }
}
