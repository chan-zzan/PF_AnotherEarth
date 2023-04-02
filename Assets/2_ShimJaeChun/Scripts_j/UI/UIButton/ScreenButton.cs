using UnityEngine;
using UnityEngine.UI;

public class ScreenButton : MonoBehaviour
{
    [Header("스크린 타입 설정")]
    [Space(10)]

    public ScreenType myType;

    public Animator anim;

    [Header("오픈할 챕터 번호 (전투 스테이지일 경우)")]
    [SerializeField]
    private int chapterNum;
    public int ChapterNum { get { return chapterNum; } }

    [Header("오픈할 챕터 타입 (전투 스테이지일 경우)")]
    [SerializeField]
    private BattleType chapterType;

    public void ClickedScreenButton(bool isClicked)
    {
        // 해당 버튼이 클릭된 경우
        if(isClicked)
        {
            anim.Play("ScreenButtonDown");
        }
        // 다른 버튼이 클릭된 경우
        else 
        {
            anim.Play("ScreenButtonUp");
        }
    }


    public void OnClickScreenButton()
    {
        // 전투 스테이지일 경우
        if(myType == ScreenType.SelectStage)
        {
            SoundManager.Instance.PlayEffectSound(EffectSoundType.PopUpButtonSound);

            // 챕터 정보 할당
            PopUpUIManager.Instance.screenGroups[(int)ScreenType.SelectStage].GetComponent<ChapterInfo>().ChapterSetting(chapterNum, chapterType);
        }

        PopUpUIManager.Instance.ChangeScreenType(myType);

        PopUpUIManager.Instance.ClosePopUp(true);
    }
}
