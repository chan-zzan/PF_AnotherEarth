using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraSingleton : MonoBehaviour
{
    #region SigleTon
    private static MainCameraSingleton instance;
    public static MainCameraSingleton Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<MainCameraSingleton>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<MainCameraSingleton>();
                    instance = newObj;
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        var objs = FindObjectsOfType<MainCameraSingleton>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    #endregion


    private void OnEnable()
    {
        gameObject.GetComponent<AudioListener>().enabled = true;
    }

    private void OnDisable()
    {
        gameObject.GetComponent<AudioListener>().enabled = false;
    }
}
