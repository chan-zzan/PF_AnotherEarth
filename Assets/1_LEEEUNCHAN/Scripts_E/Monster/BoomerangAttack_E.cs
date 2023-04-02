using System.Collections;
using System.Collections.Generic;
//using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BoomerangAttack_E : MonoBehaviour
{
    Transform P; // 부모의 위치

    [SerializeField]
    float throwDist; // 던지는 거리

    [SerializeField]
    float throwSpeed; // 던지는 속도

    private void OnEnable()
    {
        P = this.transform.parent.transform; // 부모 저장
        this.transform.parent = P.parent; // 밖으로 빼냄
        StartCoroutine(BoomerangAttack());
    }

    private void OnDisable()
    {
        this.transform.parent = P; // 부메랑 회수
    }

    private void Update()
    {
        if (!P.gameObject.activeSelf)
        {
            this.transform.parent = P; // 부메랑 회수            
        }
    }

    IEnumerator BoomerangAttack()
    {
        // 남은거리
        float dist = throwDist;

        // 던질 방향
        Vector2 throwDir = (GameManager_E.Instance.Player.transform.position - P.position).normalized; // 몬스터 -> 플레이어

        // 던지기
        while (dist > 0)
        {
            float delta = Time.deltaTime * throwSpeed; // 프레임당 던질 거리

            delta = delta > dist ? dist : delta;

            this.transform.Translate(delta * throwDir); // 던짐

            dist -= delta; 

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        Vector2 catchDir = P.position - this.transform.position; // 부메랑 -> 몬스터
        float catchDist = catchDir.magnitude; // 플레이어와 던지는 물체 사이의 거리  
        
        // 받기
        while (catchDist > 0)
        {
            // 방향,거리 계속 갱신
            catchDir = P.position - this.transform.position; 
            catchDist = catchDir.magnitude;
            catchDir.Normalize();
                                                   
            float delta = Time.fixedDeltaTime * throwSpeed;
            delta = delta > catchDist ? catchDist : delta;

            this.transform.Translate(catchDir * delta); // 받음            

            catchDist -= delta;

            yield return new WaitForFixedUpdate();
        }

        yield return BoomerangAttack(); // 반복
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Monster_E curMonster = P.GetComponent<Monster_E>();
            GameManager_E.Instance.Player.CurHP -= curMonster.CurDamage;
        }
    }
}

