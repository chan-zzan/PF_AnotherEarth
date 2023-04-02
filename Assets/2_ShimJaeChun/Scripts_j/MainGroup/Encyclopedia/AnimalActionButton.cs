using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalActionButton : MonoBehaviour
{
    Coroutine myCor;

    private void OnEnable()
    {
        myCor = StartCoroutine(DisableTimer());
    }

    private void OnDisable()
    {
        StopCoroutine(myCor);
    }

    IEnumerator DisableTimer()
    {
        yield return new WaitForSeconds(3.0f);

        gameObject.SetActive(false);
    }
}
