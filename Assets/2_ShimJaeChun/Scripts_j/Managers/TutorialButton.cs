using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButton : MonoBehaviour
{
    public TutorialManager manager;

    [Header("다음 튜토리얼 조건")]
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
