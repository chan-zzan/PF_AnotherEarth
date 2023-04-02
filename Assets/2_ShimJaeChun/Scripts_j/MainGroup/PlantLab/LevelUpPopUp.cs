using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelUpPopUp : MonoBehaviour
{
    public TextMeshProUGUI curLevelText;
    public TextMeshProUGUI curHpText;

    public TextMeshProUGUI nextLevelText;
    public TextMeshProUGUI nextHpText;

    public GameObject nextTutorial;

    private void OnEnable()
    {
        if(PopUpUIManager.Instance.tutorialManager.isActiveAndEnabled)
        {
            PopUpUIManager.Instance.tutorialManager.StopTutorial();
            nextTutorial.SetActive(true);
        }
        else
        {
            if(nextTutorial.activeSelf)
            {
                nextTutorial.SetActive(false);
            }
        }

        curLevelText.text = (StatManager.Instance.Level_Player - 1).ToString();
        nextLevelText.text = (StatManager.Instance.Level_Player).ToString();

        curHpText.text = (StatManager.Instance.Hp_Player - 20).ToString();
        nextHpText.text = (StatManager.Instance.Hp_Player).ToString();
    }
}
