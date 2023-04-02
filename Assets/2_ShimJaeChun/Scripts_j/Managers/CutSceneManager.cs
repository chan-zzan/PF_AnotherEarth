using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneManager : MonoBehaviour
{
    public GameObject onObject;


    private void OnDisable()
    {
        if (!onObject.activeSelf)
        {
            onObject.SetActive(true);
        }
    }
}
