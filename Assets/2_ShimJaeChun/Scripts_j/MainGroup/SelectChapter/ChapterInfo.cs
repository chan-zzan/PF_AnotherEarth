using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChapterInfo : MonoBehaviour
{
    [Header("챕터 타입 별 이미지 리스트(0:1_이지 1:2_이지 2:3_이지 3:1_하드 4:2_하드 5:3_하드)")]
    public Sprite[] chapterSpriteList;

    [Header("이지모드 행성 리스트")]
    public GameObject[] easyPlanetList;

    [Header("하드모드 행성 리스트")]
    public GameObject[] hardPlanetList;

    public GameObject curPlanetButtonList;

    [Header("Chapter BackGround Image")]
    public Image backgroundImage;

    [Header("스테이지 선택 화면 배경")]
    public MapDragAndMove selectStageBackground;

    private void OnEnable()
    {
    }

    public void ChapterSetting(int chapterNumber, BattleType chapterType)
    {
        // 기존 챕터 세팅 초기화
        if (curPlanetButtonList)
        {
            curPlanetButtonList.SetActive(false);
        }

        selectStageBackground.ResetPosition();

        switch (chapterType)
        {
            case BattleType.Easy:
                {
                    // 이지모드 이미지 변경
                    backgroundImage.sprite = chapterSpriteList[chapterNumber];
                    // 이지모드 스테이지 버튼 세팅
                    curPlanetButtonList = easyPlanetList[chapterNumber];
                    curPlanetButtonList.SetActive(true);
                    break;
                }
                case BattleType.Hard:
                {
                    // 하드모드 이미지 변경
                    backgroundImage.sprite = chapterSpriteList[chapterNumber+4];
                    // 하드모드 스테이지 버튼 세팅
                    curPlanetButtonList = hardPlanetList[chapterNumber];
                    curPlanetButtonList.SetActive(true);
                    break;
                }
            default: break;
        }
    }
}
