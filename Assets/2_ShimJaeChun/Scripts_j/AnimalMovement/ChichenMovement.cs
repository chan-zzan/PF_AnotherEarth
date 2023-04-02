using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChichenMovement : MonoBehaviour
{
    public Animator chickenAnim;

    Coroutine chickenIdle;
    Coroutine chickenJump;

    private void OnEnable()
    {
        chickenIdle = StartCoroutine(ChickenIdle());       
    }

    private void OnDisable()
    {
        if (chickenIdle != null)
        {
            StopCoroutine(chickenIdle);
        }
        if (chickenJump != null)
        {
            StopCoroutine(chickenJump);
        }
    }

    IEnumerator ChickenIdle()
    {
        yield return new WaitForSeconds(3.0f);

        chickenAnim.SetBool("IsJump", true);
        chickenJump = StartCoroutine(ChickenJump());
    }

    IEnumerator ChickenJump()
    {
        yield return new WaitForSeconds(1.7f);

        chickenAnim.SetBool("IsJump", false);
        chickenIdle = StartCoroutine(ChickenIdle());
    }
}
