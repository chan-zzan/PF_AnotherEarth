using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitMovement : MonoBehaviour
{
    public RectTransform parentRect;

    public Animator rabbitAnim;

    public float minXpos = -1600f;
    public float maxXpos = 1600f;

    public bool isJump = false;
    public bool isMoveLeft = true;

    public float movePos = 300f;

    public Vector3 destinationPos;

    Coroutine rabbitIdle;
    Coroutine rabbitJump;

    private void OnEnable()
    {
        rabbitIdle = StartCoroutine(RabbitIdle());
    }

    private void OnDisable()
    {
        if(rabbitIdle != null)
        {
            StopCoroutine(rabbitIdle);
        }
        if (rabbitIdle != null)
        {
            StopCoroutine(rabbitJump);
        }
    }

    public void Update()
    {
        if(isJump)
        {
            parentRect.anchoredPosition = Vector3.Lerp(
                new Vector3(parentRect.anchoredPosition.x, parentRect.anchoredPosition.y,0)
                , destinationPos
                , Time.deltaTime);
        }
    }
    public void SetDestination()
    {
        // 왼쪽 방향 이동일 경우
        if(isMoveLeft)
        {
            // 이동 범위에 벗어난 경우
            if (parentRect.anchoredPosition.x - movePos <= minXpos)
            {
                // 오른쪽으로 회전
                parentRect.rotation = Quaternion.Euler(0, 180, 0);
                parentRect.localScale = new Vector3(parentRect.localScale.x, parentRect.localScale.y, -1);
                
                // 오른쪽 이동
                isMoveLeft = false;

                // 오른쪽 이동 목적지 설정
                destinationPos = new Vector3(parentRect.anchoredPosition.x + movePos, parentRect.anchoredPosition.y, 0);
            }
            else
            {
                // 왼쪽 이동 목적지 설정
                destinationPos = new Vector3(parentRect.anchoredPosition.x - movePos, parentRect.anchoredPosition.y, 0);
            }
        }
        // 오른쪽 방향 이동일 경우
        else
        {
            // 이동 범위에 벗어난 경우
            if (parentRect.anchoredPosition.x + movePos >= maxXpos)
            {
                // 왼쪽으로 회전
                parentRect.rotation = Quaternion.Euler(0, 0, 0);
                parentRect.localScale = new Vector3(parentRect.localScale.x, parentRect.localScale.y, 1);

                // 왼쪽 이동
                isMoveLeft = true;

                // 오른쪽 이동 목적지 설정
                destinationPos = new Vector3(parentRect.anchoredPosition.x - movePos, parentRect.anchoredPosition.y, 0);
            }
            else
            {
                // 왼쪽 이동 목적지 설정
                destinationPos = new Vector3(parentRect.anchoredPosition.x + movePos, parentRect.anchoredPosition.y, 0);
            }

        }
    }

    IEnumerator RabbitJump()
    {
        yield return new WaitForSeconds(1.0f);

        isJump = false;
        rabbitAnim.SetBool("IsJumping",false);

        rabbitIdle = StartCoroutine(RabbitIdle());
    }

    IEnumerator RabbitIdle()
    {
        yield return new WaitForSeconds(1.5f);

        rabbitAnim.SetBool("IsJumping", true);
        SetDestination();
        isJump = true;
        rabbitJump = StartCoroutine(RabbitJump());
    }
}
