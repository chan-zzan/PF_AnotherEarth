using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WeaponSkillSlider : MonoBehaviour
{
    [Header("슬라이더")]
    public Slider mySlider;    

    public List<int> myCountList;  // 현재 무기 타입 레벨 별 필요 킬 수치

    [Header("무기 최대 숙련도 레벨")]
    public int MaxSkillLevel;

    [Header("무기 숙련도 레벨 별 슬라이더 컬러")]
    [ColorUsage(true, true)]public Color[] levelColor;

    [Header("컬러를 변경할 이미지")]
    public Image[] imageColor;

    [Header("Floating Text Pos")]
    public GameObject floatingPos;

    [Header("Floating Text Prefab")]
    public GameObject floatingTextPrefab;

    [Header("Particle Pos")]
    public GameObject particlePos;

    [Header("Particle Prefab")]
    public GameObject particlePrefab;

    [Header("현재 캔버스")]
    public Canvas myCanvas;

    private int killCount;          // 킬 카운트
    private int weaponSkillLevel;   // 무기 숙련도 레벨

    private void Start()
    {
        mySlider.value = 0;
        weaponSkillLevel = 1;
        MaxSkillLevel = 5;
        myCountList = WeaponSkillManager.Instance.GetRequireKillCount(StatManager.Instance.l_Weapontype);
    }

    private void Update()
    {
    }

    public void UpdateSliderValue()
    {
        // 장착한 무기가 주사기일 경우 최대 레벨 3으로 제한
        if (WeaponSkillManager.Instance.curLweapon == LWeaponType.Syringe)
        {
            MaxSkillLevel = 3;
        }

        if (weaponSkillLevel < MaxSkillLevel)
        { 
            // 킬 카운트 추가
            killCount += 1;

            // 추가한 킬 카운트가 필요 수치를 넘어선 경우
            if (killCount >= myCountList[weaponSkillLevel - 1])
            {
                killCount = 0;
                mySlider.value = weaponSkillLevel == MaxSkillLevel ? 1 : 0;
                weaponSkillLevel++;

                //텍스트 오브젝트
                GameObject temp_t = Instantiate(floatingTextPrefab, floatingPos.transform.position, Quaternion.identity, myCanvas.transform);
                temp_t.GetComponent<TextMeshProUGUI>().text = "Level Up";

                //파티클 오브젝트
                GameObject temp_p = Instantiate(particlePrefab, particlePos.transform.position, Quaternion.identity, particlePos.transform);

                // 슬라이더 이미지 컬러 변경
                for (int i = 0; i < imageColor.Length; i++) 
                {
                    imageColor[i].color = levelColor[weaponSkillLevel - 1];
                }

                // 무기 스텟 적용
                WeaponSkillManager.Instance.WeaponSkillLevelUp();


                /// 이은찬 추가
                GameManager_E.Instance.Pool.ChangeProjectile(weaponSkillLevel); // 투사체 변경
                SoundManager_E.Instance.EffectSoundPlay2(3);

                if (GameManager_E.Instance.Player.easyMoveDir.activeSelf)
                {
                    // 안내선이 켜져있는 경우 -> 끔
                    GameManager_E.Instance.Player.easyMoveDir.SetActive(false);
                }
            }
            else
            {
                mySlider.value = killCount / (float)myCountList[weaponSkillLevel - 1];
            }
        }
        else
        {
            mySlider.value = 1.0f;
        }
    }
}
