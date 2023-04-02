using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpControl : MonoBehaviour
{
    public GameObject[] popUpObject;

    private void OnDisable()
    {
       for(int i=0; i< popUpObject.Length; i++)
        {
            popUpObject[i].SetActive(false);
        }
    }
}
