using UnityEngine;

public class PopupButton : MonoBehaviour
{
    [Header("�˾� Ÿ�� ����")]
    [Space(10)]

    public PopUpType myType;

    public void OnClickPopUpButton()
    {
        if ((int)myType == 0)
        {
            // �ݱ� ���� ��ư�� ���
            PopUpUIManager.Instance.ClosePopUp(false);
        }
        else
        {
            PopUpUIManager.Instance.OpenPopUp(myType);
        }
    }
}
