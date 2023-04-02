using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUnEquipButton : MonoBehaviour
{
    [Header("�ٰŸ� ��������? (üũ:true)")]
    public bool isShortWeapon;
    
    

    public void OnClickUnEquipButton()
    {
        // ��ư ���� ���
        SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponUnEquipSound);

        // ���� ����
        SlotManager.Instance.UnEquipWeapon(isShortWeapon, true);

        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        { 
            this.gameObject.SetActive(false);
        }
    }
}
