using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;
using System;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    #region SigleTon
    private static GameDataManager instance;
    public static GameDataManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<GameDataManager>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<GameDataManager>();
                    instance = newObj;
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        var objs = FindObjectsOfType<GameDataManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    public bool isResetGame = false;

    public void SaveData()
    {
        // 데이터 저장
        //FileStream stream = new FileStream(Application.dataPath + "/UserData.json", FileMode.OpenOrCreate);
        FileStream stream = new FileStream(Application.persistentDataPath + "/UserData_j.json", FileMode.OpenOrCreate); // dataPath가 아닌 persistentDataPath를 사용해야 에디터뿐만아니라 pc나 핸드폰에서도 파일이 생성됨

        if (stream == null)
        {
            Debug.Log(" 데이터 저장 실패 ! ");
        }
        else
        {
            // 스텟 데이터 저장
            GameData_Json statData = new GameData_Json();

            // 직렬화된 정보 저장
            string statJsonData = JsonConvert.SerializeObject(statData);
            byte[] data = Encoding.UTF8.GetBytes(statJsonData);
            stream.Write(data, 0, data.Length);
            stream.Close();
        }
    }

    public void SaveUserProductData()
    {
        FileStream stream = new FileStream(Application.persistentDataPath + "/UserProductData_j.json", FileMode.OpenOrCreate); // dataPath가 아닌 persistentDataPath를 사용해야 에디터뿐만아니라 pc나 핸드폰에서도 파일이 생성됨

        if (stream == null)
        {
            Debug.Log(" 데이터 저장 실패 ! ");
        }
        else
        {
            // 스텟 데이터 저장
            UserProductData_Json userProductData = new UserProductData_Json();

            // 직렬화된 정보 저장
            string userProductJsonData = JsonConvert.SerializeObject(userProductData);
            byte[] data = Encoding.UTF8.GetBytes(userProductJsonData);
            stream.Write(data, 0, data.Length);
            stream.Close();
        }
    }

    public void SaveProductData()
    {
        FileStream stream = new FileStream(Application.persistentDataPath + "/ProductData_j.json", FileMode.OpenOrCreate); // dataPath가 아닌 persistentDataPath를 사용해야 에디터뿐만아니라 pc나 핸드폰에서도 파일이 생성됨

        if (stream == null)
        {
            Debug.Log(" 데이터 저장 실패 ! ");
        }
        else
        {
            // 스텟 데이터 저장
            ProductData_Json productData = new ProductData_Json();

            // 직렬화된 정보 저장
            string productJsonData = JsonConvert.SerializeObject(productData);
            byte[] data = Encoding.UTF8.GetBytes(productJsonData);
            stream.Write(data, 0, data.Length);
            stream.Close();
        }

    }

    public void SaveQuestData()
    {
        FileStream stream = new FileStream(Application.persistentDataPath + "/QuestData_j.json", FileMode.OpenOrCreate); // dataPath가 아닌 persistentDataPath를 사용해야 에디터뿐만아니라 pc나 핸드폰에서도 파일이 생성됨

        if (stream == null)
        {
            Debug.Log(" 데이터 저장 실패 ! ");
        }
        else
        {
            // 스텟 데이터 저장
            QuestData_Json questData = new QuestData_Json();

            // 직렬화된 정보 저장
            string questJsonData = JsonConvert.SerializeObject(questData);
            byte[] data = Encoding.UTF8.GetBytes(questJsonData);
            stream.Write(data, 0, data.Length);
            stream.Close();
        }
    }

    public void SaveStageData()
    {
        FileStream stream = new FileStream(Application.persistentDataPath + "/StageData_j.json", FileMode.OpenOrCreate); // dataPath가 아닌 persistentDataPath를 사용해야 에디터뿐만아니라 pc나 핸드폰에서도 파일이 생성됨

        if (stream == null)
        {
            Debug.Log(" 데이터 저장 실패 ! ");
        }
        else
        {
            // 스텟 데이터 저장
            StageData_Json stageData = new StageData_Json();

            // 직렬화된 정보 저장
            string stageJsonData = JsonConvert.SerializeObject(stageData);
            byte[] data = Encoding.UTF8.GetBytes(stageJsonData);
            stream.Write(data, 0, data.Length);
            stream.Close();
        }
    }


    public GameData_Json LoadData()
    {
        FileStream stream;

        try
        // 데이터 로드
        {
            stream = new FileStream(Application.persistentDataPath + "/UserData_j.json", FileMode.Open);
        }
        catch
        {
            Debug.Log(" 로딩 된 데이터 없음 ! ");
            return null;
        }
         byte[] data = new byte[stream.Length];
         stream.Read(data, 0, data.Length);
         stream.Close();
         string statJsonData = Encoding.UTF8.GetString(data);
         GameData_Json statData = JsonConvert.DeserializeObject<GameData_Json>(statJsonData);

        print(Application.persistentDataPath + "/UserData_j.json");

        System.IO.File.Delete(Application.persistentDataPath + "/UserData_j.json");

        return statData;
    }

    public UserProductData_Json LoadUserProductData()
    {
        FileStream stream;

        try
        // 데이터 로드
        {
            stream = new FileStream(Application.persistentDataPath + "/UserProductData_j.json", FileMode.Open);
        }
        catch
        {
            Debug.Log(" 로딩 된 데이터 없음 ! ");
            return null;
        }
        byte[] data = new byte[stream.Length];
        stream.Read(data, 0, data.Length);
        stream.Close();
        string userProductJsonData = Encoding.UTF8.GetString(data);
        UserProductData_Json userProductData = JsonConvert.DeserializeObject<UserProductData_Json>(userProductJsonData);

        print(Application.persistentDataPath + "/UserProductData_j.json");

        System.IO.File.Delete(Application.persistentDataPath + "/UserProductData_j.json");

        return userProductData;
    }

    public ProductData_Json LoadProductData()
    {
        FileStream stream;

        try
        // 데이터 로드
        {
            stream = new FileStream(Application.persistentDataPath + "/ProductData_j.json", FileMode.Open);
        }
        catch
        {
            Debug.Log(" 로딩된 데이터 없음 ! ");
            return null;
        }
        byte[] data = new byte[stream.Length];
        stream.Read(data, 0, data.Length);
        stream.Close();
        string productJsonData = Encoding.UTF8.GetString(data);
        ProductData_Json productData = JsonConvert.DeserializeObject<ProductData_Json>(productJsonData);

        print(Application.persistentDataPath + "/ProductData_j.json");

        // 로딩이 끝나면 파일 삭제
        System.IO.File.Delete(Application.persistentDataPath + "/ProductData_j.json");

        return productData;
    }

    public QuestData_Json LoadQuestData()
    {
        FileStream stream;

        try
        // 데이터 로드
        {
            stream = new FileStream(Application.persistentDataPath + "/QuestData_j.json", FileMode.Open);
        }
        catch
        {
            Debug.Log(" 로딩된 데이터 없음 ! ");
            return null;
        }
        byte[] data = new byte[stream.Length];
        stream.Read(data, 0, data.Length);
        stream.Close();
        string questDataJson = Encoding.UTF8.GetString(data);
        QuestData_Json questData = JsonConvert.DeserializeObject<QuestData_Json>(questDataJson);

        print(Application.persistentDataPath + "/QuestData_j.json");

        // 로딩이 끝나면 파일 삭제
        System.IO.File.Delete(Application.persistentDataPath + "/QuestData_j.json");

        return questData;

    }

    public StageData_Json LoadStageData()
    {
        FileStream stream;

        try
        // 데이터 로드
        {
            stream = new FileStream(Application.persistentDataPath + "/StageData_j.json", FileMode.Open);
        }
        catch
        {
            Debug.Log(" 로딩된 데이터 없음 ! ");
            return null;
        }
        byte[] data = new byte[stream.Length];
        stream.Read(data, 0, data.Length);
        stream.Close();
        string stageDataJson = Encoding.UTF8.GetString(data);
        StageData_Json stageData = JsonConvert.DeserializeObject<StageData_Json>(stageDataJson);

        print(Application.persistentDataPath + "/StageData_j.json");

        // 로딩이 끝나면 파일 삭제
        System.IO.File.Delete(Application.persistentDataPath + "/StageData_j.json");

        return stageData;
    }

    public void AutoSave()
    {
        // 기존 파일 삭제
        if (File.Exists(Application.persistentDataPath + "/UserData_j.json"))
        { 
            System.IO.File.Delete(Application.persistentDataPath + "/UserData_j.json");
        }
        if (File.Exists(Application.persistentDataPath + "/ProductData_j.json"))
        {
            System.IO.File.Delete(Application.persistentDataPath + "/ProductData_j.json");
        }
        if(File.Exists(Application.persistentDataPath + "/QuestData_j.json"))
        {
            System.IO.File.Delete(Application.persistentDataPath + "/QuestData_j.json");
        }
        if (File.Exists(Application.persistentDataPath + "/StageData_j.json"))
        {
            System.IO.File.Delete(Application.persistentDataPath + "/StageData_j.json");
        }
        if (File.Exists(Application.persistentDataPath + "/UserProductData_j.json"))
        {
            System.IO.File.Delete(Application.persistentDataPath + "/UserProductData_j.json");
        }

        // 데이터 저장
        SaveData();
        SaveUserProductData();
        SaveProductData();
        SaveQuestData();
        SaveStageData();
    }

    public void DestroyData()
    {
        // 기존 파일 삭제
        if (File.Exists(Application.persistentDataPath + "/UserData_j.json"))
        {
            System.IO.File.Delete(Application.persistentDataPath + "/UserData_j.json");
        }
        if (File.Exists(Application.persistentDataPath + "/ProductData_j.json"))
        {
            System.IO.File.Delete(Application.persistentDataPath + "/ProductData_j.json");
        }
        if (File.Exists(Application.persistentDataPath + "/QuestData_j.json"))
        {
            System.IO.File.Delete(Application.persistentDataPath + "/QuestData_j.json");
        }
        if (File.Exists(Application.persistentDataPath + "/StageData_j.json"))
        {
            System.IO.File.Delete(Application.persistentDataPath + "/StageData_j.json");
        }
        if (File.Exists(Application.persistentDataPath + "/UserProductData_j.json"))
        {
            System.IO.File.Delete(Application.persistentDataPath + "/UserProductData_j.json");
        }
    }



    private async void OnApplicationQuit()
    {
        // 시작씬이 아니고 게임을 리셋하는 경우가 아닐 때
        if (SceneManager.GetActiveScene().name != "Start_j"
            && !isResetGame)
        {
            // 기존 파일 삭제
            if (File.Exists(Application.persistentDataPath + "/UserData_j.json"))
            {
                System.IO.File.Delete(Application.persistentDataPath + "/UserData_j.json");
            }
            if (File.Exists(Application.persistentDataPath + "/ProductData_j.json"))
            {
                System.IO.File.Delete(Application.persistentDataPath + "/ProductData_j.json");
            }
            if (File.Exists(Application.persistentDataPath + "/QuestData_j.json"))
            {
                System.IO.File.Delete(Application.persistentDataPath + "/QuestData_j.json");
            }
            if (File.Exists(Application.persistentDataPath + "/StageData_j.json"))
            {
                System.IO.File.Delete(Application.persistentDataPath + "/StageData_j.json");
            }
            if (File.Exists(Application.persistentDataPath + "/UserProductData_j.json"))
            {
                System.IO.File.Delete(Application.persistentDataPath + "/UserProductData_j.json");
            }

            SaveData();
            SaveUserProductData();
            SaveProductData();
            SaveQuestData(); 
            SaveStageData();
        }
    }
}