using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlantButton : MonoBehaviour
{
    [Header("식물 번호")]
    public int plantNumber;

    [Header("Layout_PlantInfoGroup")]
    [SerializeField]
    private GameObject plantInfoGroup;

    [Header("Layout_LockGroup")]
    public GameObject lockGroup;

    [Header("Button Image")]
    public Image mainImage;

    [Header("Button Name Text")]
    public TextMeshProUGUI buttonText;

    [Header("Plant Button")]
    public Button button;

    private void Start()
    {
        plantInfoGroup = PopUpUIManager.Instance.popUpGroups[(int)PopUpType.PlantInfo];

        // 이미지 적용
        mainImage.sprite = SlotManager.Instance.plantDataList[plantNumber - 1].PlantSprite;

        // 텍스트 적용
        buttonText.text = SlotManager.Instance.plantDataList[plantNumber - 1].PlantName;
    }

    public void UnLockThisButton()
    {
        lockGroup.SetActive(false);
        button.enabled = true;
    }

    public void OnClickPlantButton()
    {
            // 팝업 오픈
            PopUpUIManager.Instance.OpenPopUp(PopUpType.PlantInfo);

            // 팝업 스테이지 정보 로드
            plantInfoGroup.GetComponent<PlantInfo>().InitialSetting(plantNumber);
    }
}
