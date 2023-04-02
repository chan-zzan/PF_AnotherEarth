using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserInputfield : MonoBehaviour
{
    public TMP_InputField inputF;

    public TextMeshProUGUI warningText;

    public TutorialManager t_manager;

    public void OnClickClearButton()
    {
        if(inputF.text.Length == 0)
        {
            warningText.text = "최소 한글자 이상 입력하세요!";
        }
        else if(inputF.text.Length > 6)
        {
            warningText.text = "최대 6글자";
        }
        else
        {
            StatManager.Instance.SetUserName(inputF.text);
            t_manager.EndTutorial();
        }
    }
}
