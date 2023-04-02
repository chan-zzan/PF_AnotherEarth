using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ResetPopUp : MonoBehaviour
{
    public void OnClickResetButton()
    {
        GameDataManager.Instance.DestroyData();

        GameDataManager.Instance.isResetGame = true;

        Application.Quit();
    }
}
