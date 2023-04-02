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

    Vector2 gravityDir; // ������� ����
    public float gravityForce; // ������ ��

    private SkillName curSkill;

    private void OnEnable()
    {
        if (GameManager_E.Instance.curBoss.GetComponent<MonsterSkill_E>().curUseSkill == SkillName.Push)
        {
            // Push
            curSkill = SkillName.Push;
            fireRing.SetActive(true); // �ڱ��� on
            magnetEffet.SetActive(false); // �ڼ� �̹��� off
        }
        else
        {
            // Pull
            curSkill = SkillName.Pull;
            fireRing.SetActive(false); // �ڱ��� off
            magnetEffet.SetActive(true); // �ڼ� �̹��� on
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // �÷��̾ ���� ���� �ִ� ��� �����ų� �о
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // ������� ���� ����
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
                if (!this.gameObject.activeSelf) return; // ������Ʈ�� ������ ������ üũx

                // �ڱ��� ǥ�� ���� ��� �������� ����(push�� ��쿡��)
                GameManager_E.Instance.Player.CurHP -= this.transform.parent.GetComponent<Monster_E>().CurDamage / 2;
            }
        }            
    }
}
