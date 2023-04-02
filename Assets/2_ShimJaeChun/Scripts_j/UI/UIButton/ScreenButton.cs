using UnityEngine;
using UnityEngine.UI;

public class ScreenButton : MonoBehaviour
{
    [Header("��ũ�� Ÿ�� ����")]
    [Space(10)]

    public ScreenType myType;

    public Animator anim;

    [Header("������ é�� ��ȣ (���� ���������� ���)")]
    [SerializeField]
    private int chapterNum;
    public int ChapterNum { get { return chapterNum; } }

    [Header("������ é�� Ÿ�� (���� ���������� ���)")]
    [SerializeField]
    private BattleType chapterType;

    public void ClickedScreenButton(bool isClicked)
    {
        // �ش� ��ư�� Ŭ���� ���
        if(isClicked)
        {
            anim.Play("ScreenButtonDown");
        }
        // �ٸ� ��ư�� Ŭ���� ���
        else 
        {
            anim.Play("ScreenButtonUp");
        }
    }


    public void OnClickScreenButton()
    {
        // ���� ���������� ���
        if(myType == ScreenType.SelectStage)
        {
            SoundManager.Instance.PlayEffectSound(EffectSoundType.PopUpButtonSound);

            // é�� ���� �Ҵ�
            PopUpUIManager.Instance.screenGroups[(int)ScreenType.SelectStage].GetComponent<ChapterInfo>().ChapterSetting(chapterNum, chapterType);
        }

        PopUpUIManager.Instance.ChangeScreenType(myType);

        PopUpUIManager.Instance.ClosePopUp(true);
    }
}
