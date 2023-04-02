using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GravityArea_E : MonoBehaviour
{
    [SerializeField]
    GameObject fireRing;

    [SerializeField]
    GameObject magnetEffet;

    Vector2 gravityDir; // 끌어당기는 방향
    public float gravityForce; // 끌어당길 힘

    private SkillName curSkill;

    private void OnEnable()
    {
        if (GameManager_E.Instance.curBoss.GetComponent<MonsterSkill_E>().curUseSkill == SkillName.Push)
        {
            // Push
            curSkill = SkillName.Push;
            fireRing.SetActive(true); // 자기장 on
            magnetEffet.SetActive(false); // 자석 이미지 off
        }
        else
        {
            // Pull
            curSkill = SkillName.Pull;
            fireRing.SetActive(false); // 자기장 off
            magnetEffet.SetActive(true); // 자석 이미지 on
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // 플레이어가 범위 내에 있는 경우 끌어당거나 밀어냄
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // 끌어당기는 방향 설정
            gravityDir = this.transform.position - collision.transform.position;
            gravityDir.Normalize();

            collision.transform.Translate(gravityDir * Time.fixedDeltaTime * gravityForce);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (curSkill == SkillName.Push)
            {
                if (!this.gameObject.activeSelf) return; // 오브젝트가 꺼질때 데미지 체크x

                // 자기장 표면 닿을 경우 데미지를 입힘(push의 경우에만)
                GameManager_E.Instance.Player.CurHP -= this.transform.parent.GetComponent<Monster_E>().CurDamage / 2;
            }
        }            
    }
}
