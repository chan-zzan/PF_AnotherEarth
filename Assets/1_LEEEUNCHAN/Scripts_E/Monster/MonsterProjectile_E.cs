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

    bool OnDamge = true; // 데미지를 주는지 여부

    [SerializeField]
    Transform[] SplitPoints; // 나눠질 위치

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
        
        // 딜레이 시간 동안은 데미지 입히지 않게 함
        while (delayTime > 0.0f)
        {
            // 데미지 off
            OnDamge = false;

            // 반투명 상태로 유지
            sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, 0.5f);

            delayTime -= Time.deltaTime;
            yield return null;
        }

        // 투명도 원상복구
        sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, 1f);

        // 데미지 on
        OnDamge = true;        

        // 몬스터에 따른 효과음 on
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

            // 투사체 발사
            this.transform.Translate(Vector2.up * Time.deltaTime * speed);

            yield return null;
        }

        // 일정시간 뒤에 투사체 삭제
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (OnDamge)
            {
                print("데미지 입기 전 : " + GameManager_E.Instance.Player.CurHP);

                // 공격력의 80퍼센트 데미지
                GameManager_E.Instance.Player.CurHP -= (GameManager_E.Instance.monsterSpawner.curPhase.damage * 0.8f);

                print("데미지 입은 후 : " + GameManager_E.Instance.Player.CurHP);
            }
        }

        if ((collision.gameObject.layer == LayerMask.NameToLayer("LimitedMap")))
        {
            // split 공격이 맵과 닿았을 경우            
            if (this.gameObject.CompareTag("split"))
            {
                // 한번도 split 되지 않은 경우에만 동작
                for (int i = 0; i < SplitPoints.Length; i++)
                {
                    // 알파카 침 공격 -> 벽에 튕기면 작은 방울들로 나눠짐
                    GameObject obj = Instantiate(this.gameObject, SplitPoints[i].transform.position, SplitPoints[i].transform.rotation);
                    obj.GetComponent<Animator>().enabled = false; // 애니메이션 해제
                    obj.transform.parent = this.transform.parent;
                    obj.gameObject.tag = "Idle"; // 태그 변경
                    obj.transform.localScale *= 0.2f; // 크기 80퍼센트 감소
                    obj.transform.GetComponent<MonsterProjectile_E>().speed *= 1.5f; // 속도 1.5배 증가

                    BoundaryReflect(collision, obj.transform); // 입사각 반사각 구현
                }
            }
            else if (this.gameObject.CompareTag("bounce"))
            {
                // 벽에 튕길때마다 효과음 on
                switch ((int)monsterName)
                {
                    case 7:
                        SoundManager_E.Instance.MonsterEffectSoundPlay(9); // 양털
                        this.transform.localScale *= 0.8f; // 크기 감소
                        speed += 30; // 속도 증가
                        break;
                    case 8:
                        SoundManager_E.Instance.MonsterEffectSoundPlay(10); // 강아지 테니스 공
                        break;

                }

                // 경계에 닿은 경우
                BoundaryReflect(collision, this.transform);

                return;
            }

            Destroy(this.gameObject); // 투사체 삭제

        }
    }

    private void BoundaryReflect(Collider2D collision, Transform tr)
    {
        // 경계에 부딫힌 경우
        if (collision.transform.localScale.z == 1)
        {
            // 좌, 우 경계에 부딫힌 경우
            tr.up = Vector2.Reflect(tr.up, collision.transform.right);
        }
        else
        {
            // 위, 아래 경계에 부딫힌 경우
            tr.up = Vector2.Reflect(tr.up, collision.transform.up);
        }
    }
}
