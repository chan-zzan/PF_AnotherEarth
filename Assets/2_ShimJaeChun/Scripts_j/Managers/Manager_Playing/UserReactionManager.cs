using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ReactionType
{
    Plant,
    Quest,
    Ency,
    Battle,
    BattleHard
}

public class UserReactionManager : MonoBehaviour
{
    #region �̱���
    private static UserReactionManager instance;
    public static UserReactionManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<UserReactionManager>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<UserReactionManager>();
                    instance = newObj;
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        var objs = FindObjectsOfType<UserReactionManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        // �� ����ÿ��� �ı����� �ʴ´�.
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    [Header("�Ĺ� ���׼� ������Ʈ")]
    public GameObject plantReact;
    [Header("����Ʈ ���׼� ������Ʈ")]
    public GameObject questReact;
    [Header("���� ���׼� ������Ʈ")]
    public GameObject encyReact;
    [Header("���� ���׼� ������Ʈ")]
    public GameObject battleReact;
    [Header("�ϵ��� ���׼� ������Ʈ")]
    public GameObject battleHardReact;

    public void OnReactObject(ReactionType r_Type, bool isOn)
    {
        switch(r_Type)
        {
            case ReactionType.Plant:
                {
                    // ���׼� �ѱ�
                    if(isOn)
                    {
                        // �Ĺ� �����ҿ� �ٸ� ��ũ���� ���� ��츸 ���׼� ǥ��
                        if(!plantReact.activeSelf 
                            && PopUpUIManager.Instance.CurrentScreen != PopUpUIManager.Instance.screenGroups[(int)ScreenType.PlantLab])
                        {
                            plantReact.SetActive(true);
                        }
                    }
                    // ���׼� ����
                    else
                    {
                        if(plantReact.activeSelf)
                        {
                            plantReact.SetActive(false);
                        }
                    }
                    break;
                }
            case ReactionType.Quest:
                {
                    // ���׼� �ѱ�
                    if (isOn)
                    {
                        if (!questReact.activeSelf)
                        {
                            questReact.SetActive(true);
                        }
                    }
                    // ���׼� ����
                    else
                    {
                        if (questReact.activeSelf)
                        {
                            questReact.SetActive(false);
                        }
                    }
                    break;
                }
            case ReactionType.Ency:
                {
                    // ���׼� �ѱ�
                    if (isOn)
                    {
                        if (!encyReact.activeSelf)
                        {
                            encyReact.SetActive(true);
                        }
                    }
                    // ���׼� ����
                    else
                    {
                        if (encyReact.activeSelf)
                        {
                            encyReact.SetActive(false);
                        }
                    }
                    break;
                }
            case ReactionType.Battle:
                {
                    // ���׼� �ѱ�
                    if (isOn)
                    {
                        if (!battleReact.activeSelf)
                        {
                            battleReact.SetActive(true);
                        }
                    }
                    // ���׼� ����
                    else
                    {
                        if (battleReact.activeSelf)
                        {
                            battleReact.SetActive(false);
                        }
                    }
                    break;
                }
            case ReactionType.BattleHard:
                {
                    // ���׼� �ѱ�
                    if (isOn)
                    {
                        if (!battleHardReact.activeSelf)
                        {
                            battleHardReact.SetActive(true);
                        }
                    }
                    // ���׼� ����
                    else
                    {
                        if (battleHardReact.activeSelf)
                        {
                            battleHardReact.SetActive(false);
                        }
                    }
                    break;
                }
        }
    }
}
