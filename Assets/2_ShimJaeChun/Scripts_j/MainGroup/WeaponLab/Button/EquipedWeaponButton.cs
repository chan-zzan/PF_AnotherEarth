using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipedWeaponButton : MonoBehaviour
{
    /// <summary>
    /// ������ ���� Ŭ�� ��ư
    /// </summary>
    [Header("�������� ��ư")]
    public GameObject unEquipButton;

    public void OnClickEquipedWeaponButton()
    {
        unEquipButton.SetActive(true);
    }
}
