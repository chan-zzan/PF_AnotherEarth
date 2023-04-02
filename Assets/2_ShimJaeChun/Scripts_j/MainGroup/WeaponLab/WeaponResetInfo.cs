using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponResetInfo : MonoBehaviour
{
    [Header("돌려받는 코인량 텍스트")]
    public TextMeshProUGUI getCoinText;

    [Header("Layout_WeaaponInfo")]
    public GameObject layout_WeaponInfo;

    [HideInInspector]
    public bool isShort;
    [HideInInspector]
    public int weaponNum;

    [HideInInspector]
    public int level;          // 무기 레벨
    [HideInInspector]
    public float defaultCoinReq; // 무기 레벨 업 기본 재화
    [HideInInspector]
    public float perCoinReq;     // 무기 레벨 당 재화 증가량

    private float getCoinAmount;    // 얻는 재화

    private int subDiaAmount = 10;       // 소모하는 다이아



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

        Debug.Log("enable 후 coin:" + getCoinAmount);
    }


    public void OnClickYesButton()
    {
        if(StatManager.Instance.Own_Dia >= subDiaAmount)
        {
            StatManager.Instance.SubDia(subDiaAmount);
            StatManager.Instance.AddMineral(getCoinAmount);
            StatManager.Instance.ResetWeapon(isShort, weaponNum);

            // 팝업 스테이지 정보 로드
            layout_WeaponInfo.GetComponent<WeaponInfo>().UpdateUI(isShort, weaponNum);

            // 재화 초기화
            getCoinAmount = 0;

            this.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log(" 다이아가 부족합니다. ");
        }
    }
    public void OnClickNoButton()
    {
        SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponUnEquipSound);

        // 재화 초기화
        getCoinAmount = 0;

        this.gameObject.SetActive(false);
    }
}
