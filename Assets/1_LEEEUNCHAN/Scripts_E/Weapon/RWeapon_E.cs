using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum RWeaponNumber
{
    First,
    Second, 
    Third, 
    Fourth, 
    Fifth
}

public class RWeapon_E : MonoBehaviour
{
    public RWeaponNumber weaponNumbers; // 무기 번호
    public Transform rotAxis; // 회전축

    private void Update()
    {
        if (GameManager_E.Instance.Weapon.CurSWeapon.tag == "Fire") return; // 화염방사기는 자전x

        this.transform.Rotate(Vector3.forward, Time.deltaTime * 360 * 3, Space.World); // 자전
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            collision.GetComponent<Monster_E>().OnDamage(WeaponSkillManager.Instance.curSweaponAtk, 1);
        }
    }
}
