using TMPro;
using UnityEngine;

public class SpaceShipInfo : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI minepersecondsText;

    private void Update()
    {
        levelText.text = StatManager.Instance.Level_SpaceShip.ToString();
        minepersecondsText.text = StatManager.Instance.Mine_MineralPerSeconds.ToString();
    }
}
