using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatButton : MonoBehaviour
{
    

    public void OnClickCoinButton()
    {
        StatManager.Instance.AddMineral(1000000);
    }
    public void OnClickDiaButton()
    {
        StatManager.Instance.AddDia(1000);
    }
    public void OnClickEnergyButton()
    {
        StatManager.Instance.SubCurrentEnergy(5);
    }
}
