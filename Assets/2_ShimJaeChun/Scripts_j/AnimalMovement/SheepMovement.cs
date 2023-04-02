using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepMovement : MonoBehaviour
{
    public RectTransform parentRect;

    public Animator sheepAnim;

    public bool isWalk = false;
    public bool isEat = false;
    public bool isMoveLeft = true;

    public float movePos = 300f;

    public Vector3 destinationPos;

    Coroutine sheepIdle;
    Coroutine sheepWalk;
    Coroutine sheepEat;

    private void OnEnable()
    {
        sheepIdle = StartCoroutine(SheepIdle());
    }

    private void OnDisable()
    {
        if (sheepIdle != null)
        {
            StopCoroutine(sheepIdle);
        }
        if (sheepWalk != null)
        {
            StopCoroutine(sheepWalk);
        }
        if (sheepEat != null)
        {
            StopCoroutine(sheepEat);
        }
    }

    public void Update()
    {
        if (isWalk)
        {
            parentRect.anchoredPosition = Vector3.Lerp(
                new Vector3(parentRect.anchoredPosition.x, parentRect.anchoredPosition.y, 0)
                , destinationPos
                , Time.deltaTime * 0.5f);
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

    IEnumerator SheepIdle()
    {
        yield return new WaitForSeconds(1.0f);

        sheepAnim.SetBool("IsWalk", true);
        SetDestination();
        isWalk = true;
        sheepWalk = StartCoroutine(SheepWalk());
    }


    IEnumerator SheepWalk()
    {
        yield return new WaitForSeconds(3.0f);

        isWalk = false;
        sheepAnim.SetBool("IsEat", true);
        sheepAnim.SetBool("IsWalk", false);

        sheepEat = StartCoroutine(SheepEat());
    }

    IEnumerator SheepEat()
    {
        yield return new WaitForSeconds(3.0f);

        isEat = false;
        sheepAnim.SetBool("IsEat", false);
        sheepIdle = StartCoroutine(SheepIdle());
    }

}
