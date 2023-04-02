using System.IO;
using System.Text;
using UnityEngine;

// JsonUtility
// 딕셔너리는 지원x
// 클래스는 [System.Serializable]을 붙여주어야 작동
// Vector3 타입 변환 가능
// MonoBehaviour를 상속받는 클래스의 오브젝트도 시리얼라이즈 가능

// NewtonSoft Json
// 딕셔너리 지원
// 클래스도 지원
// Vector타입 지원시 쓸데없는 값들까지 같이 나옴
// MonoBehaviour를 상속받는 클래스 지원x

public class DataManager_E : MonoBehaviour
{
    #region 싱글톤
    private static DataManager_E instance = null;

    public static DataManager_E Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<DataManager_E>();

                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<DataManager_E>();
                    instance = newObj;
                }
            }
            return instance;
        }
    }
    #endregion

    public PlayerData playerData; // 저장할 사용자의 데이터

    private void Awake()
    {
        var objs = FindObjectsOfType<StageManager_E>();

        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        SaveData(); // 인스펙터상 바뀐 데이터 저장
        LoadData();
    }

    [ContextMenu("Save Data")]
    public void SaveData()
    {
        // 데이터 저장(암호화)
        string jsonData = DataToJson(playerData);
        byte[] bytes = Encoding.UTF8.GetBytes(jsonData);
        string code = System.Convert.ToBase64String(bytes);

        // dataPath가 아닌 persistentDataPath를 사용해야 에디터뿐만아니라 pc나 핸드폰에서도 파일이 생성됨
        string path = Path.Combine(Application.persistentDataPath, "playerData.json");
        File.WriteAllText(path, code);

        print(path);

        #region 암호화 이전(혹시 몰라서 보존)
        // 데이터 저장
        //string jsonData = DataToJson(playerData);
        //string path = Path.Combine(Application.dataPath, "playerData.json");
        //File.WriteAllText(path, jsonData);        
        #endregion

    }

    [ContextMenu("Load Data")]
    void LoadData()
    {
        // 데이터 불러오기(암호화)
        string path = Path.Combine(Application.persistentDataPath, "playerData.json");

        if (!File.Exists(path))
        {
            // 파일이 존재하지 않을 경우
            playerData = new PlayerData();
            SaveData();
            LoadData();
        }
        else
        {
            // 파일이 이미 존재할 경우
            string code = File.ReadAllText(path);
            byte[] bytes = System.Convert.FromBase64String(code);
            string jsonData = Encoding.UTF8.GetString(bytes);
            playerData = JsonToData(jsonData);
        }

        #region 암호화 이전(혹시 몰라서 보존)
        /*
        // 데이터 불러오기
        string path = Path.Combine(Application.dataPath, "playerData.json");

        if (!File.Exists(path))
        {
            // 파일이 존재하지 않을 경우
            playerData = new PlayerData();
            SaveData();
            LoadData();
        }
        else
        {
            // 파일이 이미 존재할 경우
            string jsonData = File.ReadAllText(path);
            playerData = JsonToData(jsonData);
        }
        */
        #endregion

    }

    public string DataToJson(PlayerData data)
    {
        // 데이터를 json으로 변환
        return JsonUtility.ToJson(data, true);
    }

    public PlayerData JsonToData(string jsonData)
    {
        // json을 데이터로 변환
        return JsonUtility.FromJson<PlayerData>(jsonData);
    }
}
