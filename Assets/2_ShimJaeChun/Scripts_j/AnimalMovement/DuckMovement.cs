using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckMovement : MonoBehaviour
{
    public RectTransform parentRect;

    public Animator duckAnim;

    public bool isWalk = false;
    public bool isEat = false;
    public bool isMoveLeft = false;

    public float movePos = 300f;

    public Vector3 destinationPos;

    Coroutine duckIdle;
    Coroutine duckWalk;
    Coroutine duckEat;

    public ParentsBaseAnim[] myChilds;

    private void OnEnable()
    {
        duckIdle = StartCoroutine(DuckIdle());
    }

    private void OnDisable()
    {
        if(duckIdle != null)
        {
            StopCoroutine(duckIdle);
        }
        if (duckWalk != null)
        {
            StopCoroutine(duckWalk);
        }
        if (duckEat != null)
        {
            StopCoroutine(duckEat);
        }
    }

    public void Update()
    {
        if (isWalk)
        {
            parentRect.anchoredPosition = Vector3.Lerp(
                new Vector3(parentRect.anchoredPosition.x, parentRect.anchoredPosition.y, 0)
                , destinationPos
                , Time.deltaTime*0.3f);
        }
    }
    public void SetDestination()
    {
        // 왼쪽 방향 이동일 경우
        if (isMoveLeft)
        {
            // 왼쪽으로 회전
            parentRect.rotation = Quaternion.Euler(0, 0, 0);
            parentRect.localScale = new Vector3(parentRect.localScale.x, parentRect.localScale.y, 1);
            
            // 왼쪽 이동 목적지 설정
            destinationPos = new Vector3(parentRect.anchoredPosition.x - movePos, parentRect.anchoredPosition.y, 0);

            // 다음 이동 = 오른쪽 이동
            isMoveLeft = false;
        }
        // 오른쪽 방향 이동일 경우
        else
        {
            // 오른쪽으로 회전
            parentRect.rotation = Quaternion.Euler(0, 180, 0);
            parentRect.localScale = new Vector3(parentRect.localScale.x, parentRect.localScale.y, -1);

            // 왼쪽 이동 목적지 설정
            destinationPos = new Vector3(parentRect.anchoredPosition.x + movePos, parentRect.anchoredPosition.y, 0);

            // 다음 이동 = 왼쪽이동
            isMoveLeft = true;
        }
    }

    IEnumerator DuckIdle()
    {
        yield return new WaitForSeconds(1.0f);

        duckAnim.SetBool("IsWalk", true);
        for(int i=0; i< myChilds.Length; i++)
        {
            myChilds[i].SetParentState(CurrentAnimType.Walk);
        }
        SetDestination();
        isWalk = true;
        duckWalk = StartCoroutine(DuckWalk());
    }


    IEnumerator DuckWalk()
    {
        yield return new WaitForSeconds(3.0f);

        isWalk = false;
        duckAnim.SetBool("IsEat", true); 
        duckAnim.SetBool("IsWalk", false);

        for (int i = 0; i < myChilds.Length; i++)
        {
            myChilds[i].SetParentState(CurrentAnimType.Eat);
        }

        duckEat = StartCoroutine(DuckEat());
    }

    IEnumerator DuckEat()
    {
        yield return new WaitForSeconds(3.0f);

        isEat = false;
        duckAnim.SetBool("IsEat", false);

        for (int i = 0; i < myChilds.Length; i++)
        {
            myChilds[i].SetParentState(CurrentAnimType.Idle);
        }

        duckIdle = StartCoroutine(DuckIdle());
    }
}
