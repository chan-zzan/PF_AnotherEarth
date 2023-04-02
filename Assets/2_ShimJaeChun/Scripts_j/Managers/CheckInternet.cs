using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckInternet : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(DisableTimer());
    }

    IEnumerator DisableTimer()
    {
        yield return new WaitForSeconds(3f);

        this.gameObject.SetActive(false);
    }
}
