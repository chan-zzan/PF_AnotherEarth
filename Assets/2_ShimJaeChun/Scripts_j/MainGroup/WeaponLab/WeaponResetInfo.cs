using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponResetInfo : MonoBehaviour
{
    [Header("�����޴� ���η� �ؽ�Ʈ")]
    public TextMeshProUGUI getCoinText;

    [Header("Layout_WeaaponInfo")]
    public GameObject layout_WeaponInfo;

    [HideInInspector]
    public bool isShort;
    [HideInInspector]
    public int weaponNum;

    [HideInInspector]
    public int level;          // ���� ����
    [HideInInspector]
    public float defaultCoinReq; // ���� ���� �� �⺻ ��ȭ
    [HideInInspector]
    public float perCoinReq;     // ���� ���� �� ��ȭ ������

    private float getCoinAmount;    // ��� ��ȭ

    private int subDiaAmount = 10;       // �Ҹ��ϴ� ���̾�



    private void Awake()
    {

    }

    private void OnEnable()
    {
        for(int i=1; i<level; i++)
        {
            getCoinAmount += defaultCoinReq + (i - 1) * perCoinReq; 
        }

        getCoinText.text = ScoreManager.Instance.ScoreToString(getCoinAmount);

        Debug.Log("enable �� coin:" + getCoinAmount);
    }


    public void OnClickYesButton()
    {
        if(StatManager.Instance.Own_Dia >= subDiaAmount)
        {
            StatManager.Instance.SubDia(subDiaAmount);
            StatManager.Instance.AddMineral(getCoinAmount);
            StatManager.Instance.ResetWeapon(isShort, weaponNum);

            // �˾� �������� ���� �ε�
            layout_WeaponInfo.GetComponent<WeaponInfo>().UpdateUI(isShort, weaponNum);

            // ��ȭ �ʱ�ȭ
            getCoinAmount = 0;

            this.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log(" ���̾ư� �����մϴ�. ");
        }
    }
    public void OnClickNoButton()
    {
        SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponUnEquipSound);

        // ��ȭ �ʱ�ȭ
        getCoinAmount = 0;

        this.gameObject.SetActive(false);
    }
}
