using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    [Header("Layout_PauseGroup")]
    public GameObject layout_PauseGroup; 

    public void OnClickPauseButton()
    {
        layout_PauseGroup.SetActive(true);
    }
}
