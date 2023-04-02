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
    #region 싱글톤
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
        // 씬 변경시에도 파괴되지 않는다.
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    [Header("식물 리액션 오브젝트")]
    public GameObject plantReact;
    [Header("퀘스트 리액션 오브젝트")]
    public GameObject questReact;
    [Header("도감 리액션 오브젝트")]
    public GameObject encyReact;
    [Header("전투 리액션 오브젝트")]
    public GameObject battleReact;
    [Header("하드모드 리액션 오브젝트")]
    public GameObject battleHardReact;

    public void OnReactObject(ReactionType r_Type, bool isOn)
    {
        switch(r_Type)
        {
            case ReactionType.Plant:
                {
                    // 리액션 켜기
                    if(isOn)
                    {
                        // 식물 연구소와 다른 스크린에 있을 경우만 리액션 표시
                        if(!plantReact.activeSelf 
                            && PopUpUIManager.Instance.CurrentScreen != PopUpUIManager.Instance.screenGroups[(int)ScreenType.PlantLab])
                        {
                            plantReact.SetActive(true);
                        }
                    }
                    // 리액션 끄기
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
                    // 리액션 켜기
                    if (isOn)
                    {
                        if (!questReact.activeSelf)
                        {
                            questReact.SetActive(true);
                        }
                    }
                    // 리액션 끄기
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
                    // 리액션 켜기
                    if (isOn)
                    {
                        if (!encyReact.activeSelf)
                        {
                            encyReact.SetActive(true);
                        }
                    }
                    // 리액션 끄기
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
                    // 리액션 켜기
                    if (isOn)
                    {
                        if (!battleReact.activeSelf)
                        {
                            battleReact.SetActive(true);
                        }
                    }
                    // 리액션 끄기
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
                    // 리액션 켜기
                    if (isOn)
                    {
                        if (!battleHardReact.activeSelf)
                        {
                            battleHardReact.SetActive(true);
                        }
                    }
                    // 리액션 끄기
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
