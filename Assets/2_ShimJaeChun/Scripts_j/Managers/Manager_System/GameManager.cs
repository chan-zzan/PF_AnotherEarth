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
    [Header("로딩 화면 그룹")]
    public GameObject layout_LoadingGroup;

    [Header("로드 버튼 락")]
    public GameObject loadButtonLock;

    [Header("로드 버튼")]
    public Button loadButton;

    public TextMeshProUGUI internetText;

    public void Start()
    {
        // 데이터 파일이 없는 경우
        if (!File.Exists(Application.persistentDataPath + "/UserData_j.json")
            && !File.Exists(Application.persistentDataPath + "/UserProductData_j.json"))
        {
            Debug.Log("데이터 파일 없음");
            // 기존 파일 삭제
            // 기존에 다운받았던 유저 데이터 삭제
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

    // 메인화면 보여주기 용
    IEnumerator StartTimer(bool isLoadGame)
    {
        yield return new WaitForSeconds(1f);

        StartCoroutine(WebChk(isLoadGame));
    }

    // 인터넷 연결 체크용 
    // 연결확인되면 씬로드
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
//        // 기존 파일 삭제
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

//        internetText.text = "인터넷 연결 확인중...";

//        StartCoroutine(WebChk(false));
//        //SceneManager.LoadScene("Main_j");
//    }
//    public void OnClickLoadButton()
//    {
//        GameSceneManager.Instance.isLoadGame = true;

//        layout_LoadingGroup.SetActive(true);

//        internetText.text = "인터넷 연결 확인중...";

//        StartCoroutine(WebChk(true));
//    }
//    public void OnClickQuitButton()
//    {
//        // 게임종료 버튼 클릭 시

//    }
}
