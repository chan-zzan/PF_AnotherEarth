using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenUrl : MonoBehaviour
{
    public string urlLink;

    public void OpenUrlLink()
    {
        Application.OpenURL(urlLink);
    }
}
