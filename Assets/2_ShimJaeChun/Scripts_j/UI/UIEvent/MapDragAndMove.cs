using UnityEngine;
using UnityEngine.EventSystems;

public class MapDragAndMove : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public RectTransform rect_Background;

    [Header("ȭ�� x �ػ�")]
    public float game_X_Resolution = 1440f;
    [Header("ȭ�� y �ػ�")]
    public float game_Y_Resolution = 3200f;

    [Header("ȭ�� �̵� �ִ� �� (x ����)")]
    [SerializeField]
    private float maxWidth;
    [Header("ȭ�� �̵� �ּ� �� (x ����)")]
    [SerializeField]
    private float minWidth;

    [Header("ȭ�� �巡�� �� ��Ȱ��ȭ �� UI �׷�")]
    public GameObject[] hideUIGroups;

    [Header("��ũ�� Ÿ��")]
    public ScreenType myType;

    [Header("�������� ���� â ȭ��ǥ")]
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
        // �巡�� ���� �� �¿� UI ��� ����
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

        // �巡�� ���� �� �¿� UI �ٽ� ǥ��
        if (hideUIGroups[0])
        {
            for (int i = 0; i < hideUIGroups.Length; i++)
            {
                hideUIGroups[i].SetActive(true);
            }
        }
        Debug.Log("End Drag");
        Debug.Log("Xpos : " + Camera.main.ScreenToViewportPoint(new Vector2(eventData.position.x, 0)));

        // �� ������ ����� ���
        if (rect_Background.anchoredPosition.x > maxWidth)
        {
            rect_Background.anchoredPosition = new Vector2(maxWidth, rect_Background.anchoredPosition.y);
        }

        if (rect_Background.anchoredPosition.x < minWidth)
        {
            rect_Background.anchoredPosition = new Vector2(minWidth, rect_Background.anchoredPosition.y);
            // ȭ��ǥ ����
            if (myType == ScreenType.SelectStage)
            {
                rightArrow.SetActive(false);
            }
        }
        else
        {
            // ȭ��ǥ �ٽ� ǥ��
            if (myType == ScreenType.SelectStage)
            {
                rightArrow.SetActive(true);
            }
        }
    }
}
