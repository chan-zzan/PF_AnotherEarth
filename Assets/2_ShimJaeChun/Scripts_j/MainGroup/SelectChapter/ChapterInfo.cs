using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChapterInfo : MonoBehaviour
{
    [Header("é�� Ÿ�� �� �̹��� ����Ʈ(0:1_���� 1:2_���� 2:3_���� 3:1_�ϵ� 4:2_�ϵ� 5:3_�ϵ�)")]
    public Sprite[] chapterSpriteList;

    [Header("������� �༺ ����Ʈ")]
    public GameObject[] easyPlanetList;

    [Header("�ϵ��� �༺ ����Ʈ")]
    public GameObject[] hardPlanetList;

    public GameObject curPlanetButtonList;

    [Header("Chapter BackGround Image")]
    public Image backgroundImage;

    [Header("�������� ���� ȭ�� ���")]
    public MapDragAndMove selectStageBackground;

    private void OnEnable()
    {
    }

    public void ChapterSetting(int chapterNumber, BattleType chapterType)
    {
        // ���� é�� ���� �ʱ�ȭ
        if (curPlanetButtonList)
        {
            curPlanetButtonList.SetActive(false);
        }

        selectStageBackground.ResetPosition();

        switch (chapterType)
        {
            case BattleType.Easy:
                {
                    // ������� �̹��� ����
                    backgroundImage.sprite = chapterSpriteList[chapterNumber];
                    // ������� �������� ��ư ����
                    curPlanetButtonList = easyPlanetList[chapterNumber];
                    curPlanetButtonList.SetActive(true);
                    break;
                }
                case BattleType.Hard:
                {
                    // �ϵ��� �̹��� ����
                    backgroundImage.sprite = chapterSpriteList[chapterNumber+4];
                    // �ϵ��� �������� ��ư ����
                    curPlanetButtonList = hardPlanetList[chapterNumber];
                    curPlanetButtonList.SetActive(true);
                    break;
                }
            default: break;
        }
    }
}
