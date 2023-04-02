using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlantButton : MonoBehaviour
{
    [Header("�Ĺ� ��ȣ")]
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

        // �̹��� ����
        mainImage.sprite = SlotManager.Instance.plantDataList[plantNumber - 1].PlantSprite;

        // �ؽ�Ʈ ����
        buttonText.text = SlotManager.Instance.plantDataList[plantNumber - 1].PlantName;
    }

    public void UnLockThisButton()
    {
        lockGroup.SetActive(false);
        button.enabled = true;
    }

    public void OnClickPlantButton()
    {
            // �˾� ����
            PopUpUIManager.Instance.OpenPopUp(PopUpType.PlantInfo);

            // �˾� �������� ���� �ε�
            plantInfoGroup.GetComponent<PlantInfo>().InitialSetting(plantNumber);
    }
}
