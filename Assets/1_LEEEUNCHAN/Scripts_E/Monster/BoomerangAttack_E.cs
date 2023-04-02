using System.Collections;
using System.Collections.Generic;
//using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BoomerangAttack_E : MonoBehaviour
{
    Transform P; // �θ��� ��ġ

    [SerializeField]
    float throwDist; // ������ �Ÿ�

    [SerializeField]
    float throwSpeed; // ������ �ӵ�

    private void OnEnable()
    {
        P = this.transform.parent.transform; // �θ� ����
        this.transform.parent = P.parent; // ������ ����
        StartCoroutine(BoomerangAttack());
    }

    private void OnDisable()
    {
        this.transform.parent = P; // �θ޶� ȸ��
    }

    private void Update()
    {
        if (!P.gameObject.activeSelf)
        {
            this.transform.parent = P; // �θ޶� ȸ��            
        }
    }

    IEnumerator BoomerangAttack()
    {
        // �����Ÿ�
        float dist = throwDist;

        // ���� ����
        Vector2 throwDir = (GameManager_E.Instance.Player.transform.position - P.position).normalized; // ���� -> �÷��̾�

        // ������
        while (dist > 0)
        {
            float delta = Time.deltaTime * throwSpeed; // �����Ӵ� ���� �Ÿ�

            delta = delta > dist ? dist : delta;

            this.transform.Translate(delta * throwDir); // ����

            dist -= delta; 

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        Vector2 catchDir = P.position - this.transform.position; // �θ޶� -> ����
        float catchDist = catchDir.magnitude; // �÷��̾�� ������ ��ü ������ �Ÿ�  
        
        // �ޱ�
        while (catchDist > 0)
        {
            // ����,�Ÿ� ��� ����
            catchDir = P.position - this.transform.position; 
            catchDist = catchDir.magnitude;
            catchDir.Normalize();
                                                   
            float delta = Time.fixedDeltaTime * throwSpeed;
            delta = delta > catchDist ? catchDist : delta;

            this.transform.Translate(catchDir * delta); // ����            

            catchDist -= delta;

            yield return new WaitForFixedUpdate();
        }

        yield return BoomerangAttack(); // �ݺ�
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

