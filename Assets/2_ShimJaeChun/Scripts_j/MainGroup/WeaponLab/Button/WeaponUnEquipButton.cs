using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUnEquipButton : MonoBehaviour
{
    [Header("근거리 무기인지? (체크:true)")]
    public bool isShortWeapon;
    
    

    public void OnClickUnEquipButton()
    {
        // 버튼 사운드 출력
        SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponUnEquipSound);

        // 무기 해제
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
