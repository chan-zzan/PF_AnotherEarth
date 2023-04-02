using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenChange : MonoBehaviour
{

    private void OnEnable()
    {
        this.gameObject.GetComponent<Animator>().Play("ScreenChange");

        StartCoroutine("SetActiveCorutine");
    }
 
    IEnumerator SetActiveCorutine()
    {
        yield return new WaitForSeconds(1.1f);

        this.gameObject.SetActive(false);
    }
}
