using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAxis_E : MonoBehaviour
{
    float rotSpeed = 1.0f;

    private void Start()
    {
        // ¹ÛÀ¸·Î »©³¿
        this.transform.parent = this.transform.parent.parent;
    }

    private void Update()
    {
        //float curRWeaponDPS = 0;

        //switch (GameManager_E.Instance.Weapon.CurSWeapon.tag)
        //{
        //    case "Idle":
        //        break;
        //    case "Sword":
        //        curRWeaponDPS = StatManager.Instance.Dps_Sword;
        //        break;
        //    case "Hammer":
        //        curRWeaponDPS = StatManager.Instance.Dps_Hammer;
        //        break;
        //    case "Sycthe":
        //        curRWeaponDPS = StatManager.Instance.Dps_Scythe;
        //        break;
        //    case "Fire":
        //        curRWeaponDPS = StatManager.Instance.Dps_Flamethrower;
        //        break;
        //}

        rotSpeed = WeaponSkillManager.Instance.curSweaponDps / 100;

        this.transform.position = GameManager_E.Instance.Player.transform.position;
        this.transform.Rotate(Vector3.forward, Time.deltaTime * 360 * rotSpeed, Space.World); // 1¹ÙÄû µµ´Âµ¥  1/rotSpeed ÃÊ ¸¸Å­ °É¸²
    }
}
