using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButton : MonoBehaviour
{
    public TutorialManager manager;

    [Header("���� Ʃ�丮�� ����")]
    public int nextCount;
    public int count = 0;

    public void OnClickNextButton()
    {
        manager.PlayNextToturial();
    }

    public void OnClickCountButton()
    {
        ++count;
        if(count == nextCount)
        {
            manager.PlayNextToturial();
        }
    }
}
