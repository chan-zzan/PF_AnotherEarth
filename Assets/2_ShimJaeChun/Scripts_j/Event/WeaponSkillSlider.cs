using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WeaponSkillSlider : MonoBehaviour
{
    [Header("�����̴�")]
    public Slider mySlider;    

    public List<int> myCountList;  // ���� ���� Ÿ�� ���� �� �ʿ� ų ��ġ

    [Header("���� �ִ� ���õ� ����")]
    public int MaxSkillLevel;

    [Header("���� ���õ� ���� �� �����̴� �÷�")]
    [ColorUsage(true, true)]public Color[] levelColor;

    [Header("�÷��� ������ �̹���")]
    public Image[] imageColor;

    [Header("Floating Text Pos")]
    public GameObject floatingPos;

    [Header("Floating Text Prefab")]
    public GameObject floatingTextPrefab;

    [Header("Particle Pos")]
    public GameObject particlePos;

    [Header("Particle Prefab")]
    public GameObject particlePrefab;

    [Header("���� ĵ����")]
    public Canvas myCanvas;

    private int killCount;          // ų ī��Ʈ
    private int weaponSkillLevel;   // ���� ���õ� ����

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
        // ������ ���Ⱑ �ֻ���� ��� �ִ� ���� 3���� ����
        if (WeaponSkillManager.Instance.curLweapon == LWeaponType.Syringe)
        {
            MaxSkillLevel = 3;
        }

        if (weaponSkillLevel < MaxSkillLevel)
        { 
            // ų ī��Ʈ �߰�
            killCount += 1;

            // �߰��� ų ī��Ʈ�� �ʿ� ��ġ�� �Ѿ ���
            if (killCount >= myCountList[weaponSkillLevel - 1])
            {
                killCount = 0;
                mySlider.value = weaponSkillLevel == MaxSkillLevel ? 1 : 0;
                weaponSkillLevel++;

                //�ؽ�Ʈ ������Ʈ
                GameObject temp_t = Instantiate(floatingTextPrefab, floatingPos.transform.position, Quaternion.identity, myCanvas.transform);
                temp_t.GetComponent<TextMeshProUGUI>().text = "Level Up";

                //��ƼŬ ������Ʈ
                GameObject temp_p = Instantiate(particlePrefab, particlePos.transform.position, Quaternion.identity, particlePos.transform);

                // �����̴� �̹��� �÷� ����
                for (int i = 0; i < imageColor.Length; i++) 
                {
                    imageColor[i].color = levelColor[weaponSkillLevel - 1];
                }

                // ���� ���� ����
                WeaponSkillManager.Instance.WeaponSkillLevelUp();


                /// ������ �߰�
                GameManager_E.Instance.Pool.ChangeProjectile(weaponSkillLevel); // ����ü ����
                SoundManager_E.Instance.EffectSoundPlay2(3);

                if (GameManager_E.Instance.Player.easyMoveDir.activeSelf)
                {
                    // �ȳ����� �����ִ� ��� -> ��
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
