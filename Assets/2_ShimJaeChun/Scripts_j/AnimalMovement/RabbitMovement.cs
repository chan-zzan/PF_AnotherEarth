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
        // ���� ���� �̵��� ���
        if(isMoveLeft)
        {
            // �̵� ������ ��� ���
            if (parentRect.anchoredPosition.x - movePos <= minXpos)
            {
                // ���������� ȸ��
                parentRect.rotation = Quaternion.Euler(0, 180, 0);
                parentRect.localScale = new Vector3(parentRect.localScale.x, parentRect.localScale.y, -1);
                
                // ������ �̵�
                isMoveLeft = false;

                // ������ �̵� ������ ����
                destinationPos = new Vector3(parentRect.anchoredPosition.x + movePos, parentRect.anchoredPosition.y, 0);
            }
            else
            {
                // ���� �̵� ������ ����
                destinationPos = new Vector3(parentRect.anchoredPosition.x - movePos, parentRect.anchoredPosition.y, 0);
            }
        }
        // ������ ���� �̵��� ���
        else
        {
            // �̵� ������ ��� ���
            if (parentRect.anchoredPosition.x + movePos >= maxXpos)
            {
                // �������� ȸ��
                parentRect.rotation = Quaternion.Euler(0, 0, 0);
                parentRect.localScale = new Vector3(parentRect.localScale.x, parentRect.localScale.y, 1);

                // ���� �̵�
                isMoveLeft = true;

                // ������ �̵� ������ ����
                destinationPos = new Vector3(parentRect.anchoredPosition.x - movePos, parentRect.anchoredPosition.y, 0);
            }
            else
            {
                // ���� �̵� ������ ����
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
