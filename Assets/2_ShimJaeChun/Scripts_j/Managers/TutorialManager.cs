using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialType
{
    FirstUser = 0,
    Encyclopedia
}

public class TutorialManager : MonoBehaviour
{
    [Header("튜토리얼 타입")]
    public TutorialType t_Type;

    [Header("튜토리얼 페이즈 리스트")]
    public GameObject[] totorialList;

    [Header("현재 진행중인 페이즈")]
    public GameObject curPhase;

    [Header("메인화면 캐릭터 오브젝트 : 튜토리얼 진행 시 비활성화")]
    public GameObject mainCharacter;

    [Header("컷씬 그룹")]
    public GameObject cut;
    public GameObject events;

    private int index;  // 진행중인 페이즈 인덱스

    private void Awake()
    {
        // 튜토리얼 초기세팅
        index = 0;
        curPhase = totorialList[index];
    }

    public void OnEnable()
    {
        if (t_Type == TutorialType.FirstUser)
        { // 튜토리얼이 활성화 된 경우 배경음 변경 (처음 시작하는 유저의 경우)
            SoundManager.Instance.PlayBackGroundSound(MainSoundType.MainStageSound);
            cut.SetActive(false);
        }

        mainCharacter.SetActive(false);
    }
    public void OnDisable()
    {
        mainCharacter.SetActive(true);
    }

    public void PlayNextToturial()
    {
        if (t_Type == TutorialType.FirstUser)
        {
            if (events.activeSelf)
            {
                events.SetActive(false);
            }
        }

        ++index;
        curPhase.SetActive(false);
        curPhase = totorialList[index];

        curPhase.SetActive(true);
    }

    public void StopTutorial()
    {
        curPhase.SetActive(false);
    }

    public void EndTutorial()
    {
        index = 0;
        curPhase = totorialList[index];
        this.gameObject.SetActive(false);
    }

}
