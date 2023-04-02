using UnityEngine;

public class PopupButton : MonoBehaviour
{
    [Header("팝업 타입 설정")]
    [Space(10)]

    public PopUpType myType;

    public void OnClickPopUpButton()
    {
        if ((int)myType == 0)
        {
            // 닫기 전용 버튼일 경우
            PopUpUIManager.Instance.ClosePopUp(false);
        }
        else
        {
            PopUpUIManager.Instance.OpenPopUp(myType);
        }
    }
}
