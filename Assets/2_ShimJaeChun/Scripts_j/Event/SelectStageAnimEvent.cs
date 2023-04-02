using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectStageAnimEvent : MonoBehaviour
{
    public AudioSource myAudio;

    private void OnEnable()
    {
        //StartCoroutine(SetActiveTimer());

        SoundManager.Instance.StopBGM();

        myAudio.Play();
    }

    //private void OnDisable()
    //{
    //    StopCoroutine(SetActiveTimer());    
    //}

    //IEnumerator SetActiveTimer()
    //{
    //    yield return new WaitForSeconds(activeTime);

    //    this.gameObject.SetActive(false);
    //}
}
