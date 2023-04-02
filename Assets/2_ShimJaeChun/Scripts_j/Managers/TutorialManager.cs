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
    [Header("Ʃ�丮�� Ÿ��")]
    public TutorialType t_Type;

    [Header("Ʃ�丮�� ������ ����Ʈ")]
    public GameObject[] totorialList;

    [Header("���� �������� ������")]
    public GameObject curPhase;

    [Header("����ȭ�� ĳ���� ������Ʈ : Ʃ�丮�� ���� �� ��Ȱ��ȭ")]
    public GameObject mainCharacter;

    [Header("�ƾ� �׷�")]
    public GameObject cut;
    public GameObject events;

    private int index;  // �������� ������ �ε���

    private void Awake()
    {
        // Ʃ�丮�� �ʱ⼼��
        index = 0;
        curPhase = totorialList[index];
    }

    public void OnEnable()
    {
        if (t_Type == TutorialType.FirstUser)
        { // Ʃ�丮���� Ȱ��ȭ �� ��� ����� ���� (ó�� �����ϴ� ������ ���)
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
