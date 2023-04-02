using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancleButton : MonoBehaviour
{
    Coroutine myCorutine;

    private void OnEnable()
    {
        myCorutine = StartCoroutine(ButtonEnableTimer());
    }
    private void OnDisable()
    {
        StopCoroutine(myCorutine);
    }

    IEnumerator ButtonEnableTimer()
    {
        yield return new WaitForSeconds(2f);

        this.gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        StopCoroutine(myCorutine);
    }
}
