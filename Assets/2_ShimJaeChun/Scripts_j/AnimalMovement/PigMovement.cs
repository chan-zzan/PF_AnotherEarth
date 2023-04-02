using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigMovement : MonoBehaviour
{
    public Animator pigAnim;

    Coroutine pigIdle;
    Coroutine pigAction;

    int randAction;

    private void OnEnable()
    {
        pigIdle = StartCoroutine(PigIdle());
    }

    private void OnDisable()
    {
        if (pigIdle != null)
        {
            StopCoroutine(pigIdle);
        }
        if (pigAction != null)
        {
            StopCoroutine(pigAction);
        }
    }

    IEnumerator PigIdle()
    {
        yield return new WaitForSeconds(3.0f);
        randAction = Random.Range(0, 2);

        switch(randAction)
        {
            case 0:
                {
                    pigAnim.SetBool("IsEat", true);
                    break;
                }
            case 1:
                {
                    pigAnim.SetBool("IsSleep", true);
                    break;
                }
            default: break;
        }
        pigAction = StartCoroutine(PigAction());
    }


    IEnumerator PigAction()
    {
        yield return new WaitForSeconds(3.0f);

        pigAnim.SetBool("IsEat", false);
        pigAnim.SetBool("IsSleep", false);

        pigIdle = StartCoroutine(PigIdle());
    }
}
