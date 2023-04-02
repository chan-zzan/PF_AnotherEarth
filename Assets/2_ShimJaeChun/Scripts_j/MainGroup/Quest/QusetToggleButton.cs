using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestToggleType
{
    Daily = 0,
    All
}

public class QusetToggleButton : MonoBehaviour
{
    [Header("��� ��ư Ÿ��")]
    public QuestToggleType myType;

    public GameObject dailyQuest;
    public GameObject allQuest;

    public void OnClickToggleButton()
    {
        SoundManager.Instance.PlayEffectSound(EffectSoundType.PopUpButtonSound);

        switch(myType)
        {
            case QuestToggleType.Daily:
                {
                    dailyQuest.SetActive(true);
                    allQuest.SetActive(false);
                    break;
                }
                case QuestToggleType.All:
                {
                    allQuest.SetActive(true);
                    dailyQuest.SetActive(false);
                    break;
                }
            default: break;
        }
    }
}
