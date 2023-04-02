using System.IO;
using System.Text;
using UnityEngine;

// JsonUtility
// ��ųʸ��� ����x
// Ŭ������ [System.Serializable]�� �ٿ��־�� �۵�
// Vector3 Ÿ�� ��ȯ ����
// MonoBehaviour�� ��ӹ޴� Ŭ������ ������Ʈ�� �ø�������� ����

// NewtonSoft Json
// ��ųʸ� ����
// Ŭ������ ����
// VectorŸ�� ������ �������� ������� ���� ����
// MonoBehaviour�� ��ӹ޴� Ŭ���� ����x

public class DataManager_E : MonoBehaviour
{
    #region �̱���
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

    public PlayerData playerData; // ������ ������� ������

    private void Awake()
    {
        var objs = FindObjectsOfType<StageManager_E>();

        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        SaveData(); // �ν����ͻ� �ٲ� ������ ����
        LoadData();
    }

    [ContextMenu("Save Data")]
    public void SaveData()
    {
        // ������ ����(��ȣȭ)
        string jsonData = DataToJson(playerData);
        byte[] bytes = Encoding.UTF8.GetBytes(jsonData);
        string code = System.Convert.ToBase64String(bytes);

        // dataPath�� �ƴ� persistentDataPath�� ����ؾ� �����ͻӸ��ƴ϶� pc�� �ڵ��������� ������ ������
        string path = Path.Combine(Application.persistentDataPath, "playerData.json");
        File.WriteAllText(path, code);

        print(path);

        #region ��ȣȭ ����(Ȥ�� ���� ����)
        // ������ ����
        //string jsonData = DataToJson(playerData);
        //string path = Path.Combine(Application.dataPath, "playerData.json");
        //File.WriteAllText(path, jsonData);        
        #endregion

    }

    [ContextMenu("Load Data")]
    void LoadData()
    {
        // ������ �ҷ�����(��ȣȭ)
        string path = Path.Combine(Application.persistentDataPath, "playerData.json");

        if (!File.Exists(path))
        {
            // ������ �������� ���� ���
            playerData = new PlayerData();
            SaveData();
            LoadData();
        }
        else
        {
            // ������ �̹� ������ ���
            string code = File.ReadAllText(path);
            byte[] bytes = System.Convert.FromBase64String(code);
            string jsonData = Encoding.UTF8.GetString(bytes);
            playerData = JsonToData(jsonData);
        }

        #region ��ȣȭ ����(Ȥ�� ���� ����)
        /*
        // ������ �ҷ�����
        string path = Path.Combine(Application.dataPath, "playerData.json");

        if (!File.Exists(path))
        {
            // ������ �������� ���� ���
            playerData = new PlayerData();
            SaveData();
            LoadData();
        }
        else
        {
            // ������ �̹� ������ ���
            string jsonData = File.ReadAllText(path);
            playerData = JsonToData(jsonData);
        }
        */
        #endregion

    }

    public string DataToJson(PlayerData data)
    {
        // �����͸� json���� ��ȯ
        return JsonUtility.ToJson(data, true);
    }

    public PlayerData JsonToData(string jsonData)
    {
        // json�� �����ͷ� ��ȯ
        return JsonUtility.FromJson<PlayerData>(jsonData);
    }
}
