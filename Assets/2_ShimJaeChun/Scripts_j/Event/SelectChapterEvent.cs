using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectChapterEvent : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(AnimTimer());
    }

    IEnumerator AnimTimer()
    {
        yield return new WaitForSeconds(1.8f);

        this.gameObject.SetActive(false);
    }
}

