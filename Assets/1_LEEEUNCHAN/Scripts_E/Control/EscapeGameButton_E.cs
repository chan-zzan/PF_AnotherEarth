using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EscapeGameButton_E : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    public void Yes()
    {
        GameDataManager.Instance.AutoSave();
        Application.Quit();
    }

    public void No()
    {
        this.gameObject.SetActive(false);
    }
}
