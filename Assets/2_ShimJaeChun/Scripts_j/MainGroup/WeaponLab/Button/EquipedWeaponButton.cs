using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipedWeaponButton : MonoBehaviour
{
    /// <summary>
    /// 장착된 무기 클릭 버튼
    /// </summary>
    [Header("장착해제 버튼")]
    public GameObject unEquipButton;

    public void OnClickEquipedWeaponButton()
    {
        unEquipButton.SetActive(true);
    }
}
