using TMPro;
using UnityEngine;

public class DiaUnLockButton : MonoBehaviour
{
    [Header("�ʿ� ���̾�")]
    [Space(10)]

    public int value_Dia;

    [Space(10)]

    [Header("���̾� �ؽ�Ʈ")]
    [Space(10)]

    public TextMeshProUGUI text_Dia;

    public GameObject myob;

    private void Start()
    {
        text_Dia.text = value_Dia.ToString();
    }
    public void OnClickUnLockButton()
    {
        RectTransform rect = myob.GetComponent<RectTransform>();

        this.transform.SetParent(myob.transform);

        this.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

    }
}
