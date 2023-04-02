using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MonsterProjectile_E : MonoBehaviour
{
    public MonsterName monsterName;

    [SerializeField]
    SkillName skillName;

    [SerializeField]
    float speed = 100.0f;

    [SerializeField]
    float delayTime = 1.0f;

    [SerializeField]
    float destroyTime = 5.0f;

    bool OnDamge = true; // �������� �ִ��� ����

    [SerializeField]
    Transform[] SplitPoints; // ������ ��ġ

    private void Start()
    {
        if (skillName == SkillName.Throw || skillName == SkillName.BigThrow)
        {
            StartCoroutine(Shootting(destroyTime));
        }
        else if (skillName == SkillName.Punch || skillName == SkillName.Sound || skillName == SkillName.Scatter)
        {
            StartCoroutine(DestroyProjetile(this.transform.parent.gameObject, destroyTime));
        }
    }

    IEnumerator DestroyProjetile(GameObject destroyObject, float destroyTime)
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(destroyObject);
    }

    IEnumerator Shootting(float destroyTime)
    {
        float flyTime = 0.0f;

        SpriteRenderer sp = this.gameObject.GetComponentInChildren<SpriteRenderer>();
        
        // ������ �ð� ������ ������ ������ �ʰ� ��
        while (delayTime > 0.0f)
        {
            // ������ off
            OnDamge = false;

            // ������ ���·� ����
            sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, 0.5f);

            delayTime -= Time.deltaTime;
            yield return null;
        }

        // ���� ���󺹱�
        sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, 1f);

        // ������ on
        OnDamge = true;        

        // ���Ϳ� ���� ȿ���� on
        switch ((int)monsterName)
        {
            case 1:
                if (skillName == SkillName.Throw)
                {
                    SoundManager_E.Instance.MonsterEffectSoundPlay(0);
                }
                break;
            case 2:
                SoundManager_E.Instance.MonsterEffectSoundPlay(2);
                break;
            case 3:
            case 4:
                SoundManager_E.Instance.MonsterEffectSoundPlay(0);
                break;
        }

        while (flyTime < destroyTime)
        {
            flyTime += Time.deltaTime;

            // ����ü �߻�
            this.transform.Translate(Vector2.up * Time.deltaTime * speed);

            yield return null;
        }

        // �����ð� �ڿ� ����ü ����
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (OnDamge)
            {
                print("������ �Ա� �� : " + GameManager_E.Instance.Player.CurHP);

                // ���ݷ��� 80�ۼ�Ʈ ������
                GameManager_E.Instance.Player.CurHP -= (GameManager_E.Instance.monsterSpawner.curPhase.damage * 0.8f);

                print("������ ���� �� : " + GameManager_E.Instance.Player.CurHP);
            }
        }

        if ((collision.gameObject.layer == LayerMask.NameToLayer("LimitedMap")))
        {
            // split ������ �ʰ� ����� ���            
            if (this.gameObject.CompareTag("split"))
            {
                // �ѹ��� split ���� ���� ��쿡�� ����
                for (int i = 0; i < SplitPoints.Length; i++)
                {
                    // ����ī ħ ���� -> ���� ƨ��� ���� ����� ������
                    GameObject obj = Instantiate(this.gameObject, SplitPoints[i].transform.position, SplitPoints[i].transform.rotation);
                    obj.GetComponent<Animator>().enabled = false; // �ִϸ��̼� ����
                    obj.transform.parent = this.transform.parent;
                    obj.gameObject.tag = "Idle"; // �±� ����
                    obj.transform.localScale *= 0.2f; // ũ�� 80�ۼ�Ʈ ����
                    obj.transform.GetComponent<MonsterProjectile_E>().speed *= 1.5f; // �ӵ� 1.5�� ����

                    BoundaryReflect(collision, obj.transform); // �Ի簢 �ݻ簢 ����
                }
            }
            else if (this.gameObject.CompareTag("bounce"))
            {
                // ���� ƨ�涧���� ȿ���� on
                switch ((int)monsterName)
                {
                    case 7:
                        SoundManager_E.Instance.MonsterEffectSoundPlay(9); // ����
                        this.transform.localScale *= 0.8f; // ũ�� ����
                        speed += 30; // �ӵ� ����
                        break;
                    case 8:
                        SoundManager_E.Instance.MonsterEffectSoundPlay(10); // ������ �״Ͻ� ��
                        break;

                }

                // ��迡 ���� ���
                BoundaryReflect(collision, this.transform);

                return;
            }

            Destroy(this.gameObject); // ����ü ����

        }
    }

    private void BoundaryReflect(Collider2D collision, Transform tr)
    {
        // ��迡 �΋H�� ���
        if (collision.transform.localScale.z == 1)
        {
            // ��, �� ��迡 �΋H�� ���
            tr.up = Vector2.Reflect(tr.up, collision.transform.right);
        }
        else
        {
            // ��, �Ʒ� ��迡 �΋H�� ���
            tr.up = Vector2.Reflect(tr.up, collision.transform.up);
        }
    }
}
