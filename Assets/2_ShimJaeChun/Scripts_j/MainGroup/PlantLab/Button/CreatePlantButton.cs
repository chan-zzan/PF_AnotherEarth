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
            Debug.Log("�Ĺ� ������ ��ϵ��� ���� ��ư�Դϴ�.");
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
        // bool �� isStart ���� ���
        // true : ���� ������ �ð� �Ҵ�
        // false : ���� ������ �ð� �Ҵ�

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
