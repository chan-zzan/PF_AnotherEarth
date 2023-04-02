using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile_E : MonoBehaviour
{
    #region 오브젝트 풀
    IObjectPool<Projectile_E> myPool;

    public void SetManagedPool(IObjectPool<Projectile_E> pool)
    {
        // 풀을 PoolManager에 저장
        myPool = pool;
    }

    public void DestroyProjectile()
    {
        // 트레일 끔
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

    TrailRenderer[] tr; // 트레일 렌더러
    float flyTime = 0.0f; // 날아간 시간
    float destroyTime = 2.0f; // 사라지는 시간

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

        // 생성한 후 트레일을 켜서 잔상이 남지 않게 함
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
        this.transform.Translate(this.transform.up * WeaponSkillManager.Instance.curLweaponPts * Time.deltaTime, Space.World); // 투사체 발사

        flyTime += Time.deltaTime;

        // 일정시간 이후에 투사체를 없앰
        if (flyTime > destroyTime)
        {
            DestroyProjectile();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 몬스터와 부딪혔을 경우
        if (collision.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            collision.GetComponent<Monster_E>().OnDamage(WeaponSkillManager.Instance.curLweaponAtk, 0); // 몬스터 피격 데미지

            if (!this.gameObject.CompareTag("Penetration"))
            {
                // 관통x
                DestroyProjectile();
            }
            
        }
    }
   
}
