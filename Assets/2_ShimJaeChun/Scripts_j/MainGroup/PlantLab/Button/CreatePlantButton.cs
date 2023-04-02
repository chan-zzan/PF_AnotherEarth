using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
public class CreatePlantButton : MonoBehaviour
{
    [SerializeField]
    private PlantInfo plantInfo;

    private void Start()
    {
        plantInfo = this.transform.parent.GetComponent<PlantInfo>();

        if (!plantInfo)
        {
            Debug.Log("식물 정보가 등록되지 않은 버튼입니다.");
        }
    }

    public void OnClickCreateButton()
    {
        StartCoroutine(WebChk());
    }

    public void Create()
    {
        SlotManager.Instance.CreatePlant(plantInfo.PlantData.PlantNumber);
    }

    IEnumerator WebChk()
    {
        // bool 형 isStart 변수 사용
        // true : 앱이 중지된 시간 할당
        // false : 앱이 재실행된 시간 할당

        string url = "www.naver.com";

        UnityWebRequest request = new UnityWebRequest();

        using (request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
                SlotManager.Instance.ErrorCreating();
            }
            else
            {
                Create();
            }
        }

    }
}
