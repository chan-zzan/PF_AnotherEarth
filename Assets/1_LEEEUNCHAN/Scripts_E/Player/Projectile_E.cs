using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile_E : MonoBehaviour
{
    #region ������Ʈ Ǯ
    IObjectPool<Projectile_E> myPool;

    public void SetManagedPool(IObjectPool<Projectile_E> pool)
    {
        // Ǯ�� PoolManager�� ����
        myPool = pool;
    }

    public void DestroyProjectile()
    {
        // Ʈ���� ��
        for (int i = 0; i < tr.Length; i++)
        {
            if (tr[i] == null)
            {
                Destroy(this.gameObject);
                return;
            }

            tr[i].emitting = false;
        }


        if (this.gameObject.activeSelf)
        {
            myPool.Release(this);
        }
    }
    #endregion

    TrailRenderer[] tr; // Ʈ���� ������
    float flyTime = 0.0f; // ���ư� �ð�
    float destroyTime = 2.0f; // ������� �ð�

    private void Awake()
    {
        tr = this.GetComponentsInChildren<TrailRenderer>();
    }

    private void OnEnable()
    {
        flyTime = 0.0f;

        StartCoroutine(OnTrail());
    }


    IEnumerator OnTrail()
    {
        yield return new WaitForSeconds(0.01f);

        // ������ �� Ʈ������ �Ѽ� �ܻ��� ���� �ʰ� ��
        for(int i = 0; i < tr.Length; i++)
        {
            if (tr[i] == null)
            {
                Destroy(this.gameObject);
                break;
            }

            tr[i].emitting = true;
        }
        

    }

    private void Update()
    {
        this.transform.Translate(this.transform.up * WeaponSkillManager.Instance.curLweaponPts * Time.deltaTime, Space.World); // ����ü �߻�

        flyTime += Time.deltaTime;

        // �����ð� ���Ŀ� ����ü�� ����
        if (flyTime > destroyTime)
        {
            DestroyProjectile();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���Ϳ� �ε����� ���
        if (collision.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            collision.GetComponent<Monster_E>().OnDamage(WeaponSkillManager.Instance.curLweaponAtk, 0); // ���� �ǰ� ������

            if (!this.gameObject.CompareTag("Penetration"))
            {
                // ����x
                DestroyProjectile();
            }
            
        }
    }
   
}
