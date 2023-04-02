using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlpacaMovement : MonoBehaviour
{
    public Animator alpacaAnim;


    Coroutine alpacaIdle;
    Coroutine alpacaEat;

    private void OnEnable()
    {
        alpacaIdle = StartCoroutine(AlpacaIdle());
    }

    private void OnDisable()
    {
        if (alpacaIdle != null)
        {
            StopCoroutine(alpacaIdle);
        }
        if (alpacaEat != null)
        {
            StopCoroutine(alpacaEat);
        }
    }

    IEnumerator AlpacaIdle()
    {
        yield return new WaitForSeconds(5.0f);

        alpacaAnim.SetBool("IsEat", true);
        alpacaEat = StartCoroutine(AlpacaEat());
    }


    IEnumerator AlpacaEat()
    {
        yield return new WaitForSeconds(6.0f);

        alpacaAnim.SetBool("IsEat", false);

        alpacaIdle = StartCoroutine(AlpacaIdle());
    }


}
