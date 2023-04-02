using UnityEngine;

public class SpaceShipBattleButton : MonoBehaviour
{
    public void OnClickBattleButton()
    {
        PopUpUIManager.Instance.ClosePopUp(true);
    }
}
