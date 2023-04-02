using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class OptionInfo : MonoBehaviour
{

    public void OnClickOptionButton()
    {

    }

    public void OnClickIntializeButton()
    {
        //System.IO.File.Delete(Application.persistentDataPath + "/UserData.json");
        //System.IO.File.Delete(Application.persistentDataPath + "/ProductData.json");

        //GameSceneManager.Instance.LoadGameScene("Main_j");
    }
}
