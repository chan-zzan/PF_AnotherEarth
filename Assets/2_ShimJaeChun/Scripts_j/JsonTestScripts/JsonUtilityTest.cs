using UnityEngine;

public class JsonUtilityTest : MonoBehaviour
{
    // 유틸리티 사용시
    // 기본적인 데이터타입, 리스트, 배열 등에 대한 시리얼라이즈만 허용
    // 딕셔너리, 내부 클래스 등 불허
    // System.Serializable 등 외부 라이브러리를 통해 시리얼라이즈 (딕셔너리는 아예 지원하지 않음)

    // Start is called before the first frame update
    void Start()
    {
        //// 유틸리티 테스트
        //JsonExample jtest = new JsonExample();
        //string jsondata = JsonUtility.ToJson(jtest);
        //Debug.Log(jsondata);

        //JsonExample jtest2 = JsonUtility.FromJson<JsonExample>(jsondata);
        //jtest2.print();

        //// 오브젝트 시리얼라이즈 테스트
        GameObject obj = new GameObject();
        var test1 = obj.AddComponent<TestMono>();
        test1.v3 /= 10;       // 디시리얼라이즈 이전 변수에 값을 미리 초기화 해야함.
        obj.AddComponent<TestMono>();
        string jsondata = JsonUtility.ToJson(obj.GetComponent<TestMono>());
        Debug.Log(jsondata);

        //// 디시리얼라이즈 테스트
        GameObject obj2 = new GameObject();
        JsonUtility.FromJsonOverwrite(jsondata, obj2.AddComponent<TestMono>());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
