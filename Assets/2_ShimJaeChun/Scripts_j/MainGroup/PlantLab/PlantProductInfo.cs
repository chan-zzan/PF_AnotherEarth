using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlantProductInfo : MonoBehaviour
{
    [Header("식물 슬롯 이미지")]
    public Image plantImage;

    [Header("식물 버튼")]
    public Button productButton;

    [Header("생산 완료 버튼")]
    public GameObject harvestButton;    // ��Ȯ ��ư ������Ʈ (��ǰ ������ �Ϸ�Ǹ� Ȱ��ȭ)

    [Header("생산 취소 버튼")]
    public GameObject cancleButton;

    [Header("생산중 바")]
    public Slider slider;

    [Space(30)]

    [Header("식물 데이터")]
    public PlantData _plantData;

    // 제작 시작 시간
    public int startTime;

    // 제작중 흐른 시간
    public int currentTime;

    // 제작 종료 시간
    public int endTime;

    [Header("기존 제품의 제작시작 시간")]
    public int loadStartTime = 0;   // 생산중 게임을 종료한 경우 

    [Header("기존 제품의 제작 중 시간")]
    public int loadCurrentTime=0;   // 생산중 게임을 종료한 경우 

    [Header("수확 버튼 클릭 시 경험치 표시 텍스트")]
    public GameObject floatingText;

    [Header("경험치 표시 텍스트 띄울 좌표")]
    public GameObject floatingTextPos;

    [Header("식물 생산 코루틴")]
    public Coroutine myCorutine;

    public bool isCreateDone= false;

    private void Start()
    {
        plantImage.sprite = _plantData.PlantSprite;
        isCreateDone = false;
    }

    private void Update()
    {
        if (currentTime < endTime)
        {
            slider.value = (currentTime - startTime) / (float)(endTime - startTime);
        }
        else
        {
            // 생산이 완료된 경우
            if (!isCreateDone)
            {
                // 한 번만 실행을 위해 true로 변경
                isCreateDone = true;

                if(slider.IsActive() == false)
                {
                    slider.enabled = true;
                }
                
                slider.value = 1f;

                StopCoroutine(myCorutine);

                // 식물 버튼 비활성화
                productButton.enabled = false;
                // 취소 버튼 비활성화
                cancleButton.SetActive(false);
                // 생산 버튼 활성화
                harvestButton.SetActive(true);
            }
        }
    }

    // 식물 버튼 클릭 시 
    public void OnClickProductButton()
    {
        // 취소 버튼이 활성화되어 있는 경우
        if(!cancleButton.activeSelf)
        {
            cancleButton.SetActive(true);
        }
    }

    // 취소 버튼 클릭 시 
    public void OnClickCancleButton()
    {
        SlotManager.Instance.CancleCreate(this.gameObject);
    }

    // 생산 완료 버튼 클릭 시 
    public void OnClickHarvestButton()
    {
        Debug.Log("수확");

        GameObject temp = Instantiate(floatingText, floatingTextPos.transform.position, Quaternion.identity, CanvasSingleton.Instance.transform);
        temp.GetComponent<TextMeshProUGUI>().text = "+"+ScoreManager.Instance.ScoreToString(_plantData.GetExpValue);
        SlotManager.Instance.HarvestPlant(this.gameObject, temp);
    }
}
