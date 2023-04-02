using UnityEngine;
using UnityEngine.EventSystems;

public class MapDragAndMove : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public RectTransform rect_Background;

    [Header("화면 x 해상도")]
    public float game_X_Resolution = 1440f;
    [Header("화면 y 해상도")]
    public float game_Y_Resolution = 3200f;

    [Header("화면 이동 최댓 값 (x 기준)")]
    [SerializeField]
    private float maxWidth;
    [Header("화면 이동 최솟 값 (x 기준)")]
    [SerializeField]
    private float minWidth;

    [Header("화면 드래그 시 비활성화 할 UI 그룹")]
    public GameObject[] hideUIGroups;

    [Header("스크린 타입")]
    public ScreenType myType;

    [Header("스테이지 선택 창 화살표")]
    public GameObject rightArrow;

    [SerializeField]
    private float defaultMainPos_x;
    [SerializeField]
    private float defaultMainPos_y;

    [SerializeField]
    private float defaultChapterPos_x;
    [SerializeField]
    private float defaultChapterPos_y;

    private void Awake()
    {
        defaultMainPos_x = 0;
        defaultMainPos_y = 0;

        defaultChapterPos_x = -1350f;
        defaultChapterPos_y = 0;
    }

    private void Start()
    {
        rect_Background = this.gameObject.GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        if (myType == ScreenType.MainHome)
        {
            rect_Background.anchoredPosition = new Vector2(defaultMainPos_x, defaultMainPos_y);
        }
        else
        {
            rect_Background.anchoredPosition = new Vector2(defaultChapterPos_x, defaultChapterPos_y);
        }
    }

    public void ResetPosition()
    {
        if (myType == ScreenType.MainHome)
        {
            rect_Background.anchoredPosition = new Vector2(defaultMainPos_x, defaultMainPos_y);
        }
        else
        {
            rect_Background.anchoredPosition = new Vector2(defaultChapterPos_x, defaultChapterPos_y);
        }
    }


    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        // 드래그 시작 시 좌우 UI 잠시 제거
        if (hideUIGroups[0])
        {
            for (int i = 0; i < hideUIGroups.Length; i++)
            {
                hideUIGroups[i].SetActive(false);
            }
        }
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        rect_Background.anchoredPosition += new Vector2(eventData.delta.x / CanvasSingleton.Instance.GetComponent<Canvas>().scaleFactor, rect_Background.anchoredPosition.y);
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        switch (myType)
        {
            case ScreenType.MainHome:
                {
                    //maxWidth = (rect_Background.sizeDelta.x - game_X_Resolution)/2 - game_X_Resolution;
                    maxWidth = (rect_Background.sizeDelta.x-game_X_Resolution)/2;
                    minWidth = -1*maxWidth;
                    break;
                }
            case ScreenType.SelectStage:
                {
                    maxWidth = (-1 * game_X_Resolution) - ((game_X_Resolution/2f)/2f);
                    minWidth = -1 * (rect_Background.sizeDelta.x - game_X_Resolution*2.5f);
                    break;
                }
            default:
                {
                    break;
                }
        }

        // 드래그 종료 시 좌우 UI 다시 표출
        if (hideUIGroups[0])
        {
            for (int i = 0; i < hideUIGroups.Length; i++)
            {
                hideUIGroups[i].SetActive(true);
            }
        }
        Debug.Log("End Drag");
        Debug.Log("Xpos : " + Camera.main.ScreenToViewportPoint(new Vector2(eventData.position.x, 0)));

        // 맵 범위가 벗어낫을 경우
        if (rect_Background.anchoredPosition.x > maxWidth)
        {
            rect_Background.anchoredPosition = new Vector2(maxWidth, rect_Background.anchoredPosition.y);
        }

        if (rect_Background.anchoredPosition.x < minWidth)
        {
            rect_Background.anchoredPosition = new Vector2(minWidth, rect_Background.anchoredPosition.y);
            // 화살표 제거
            if (myType == ScreenType.SelectStage)
            {
                rightArrow.SetActive(false);
            }
        }
        else
        {
            // 화살표 다시 표시
            if (myType == ScreenType.SelectStage)
            {
                rightArrow.SetActive(true);
            }
        }
    }
}
