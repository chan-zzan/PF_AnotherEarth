using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSingleton : MonoBehaviour
{
    private static CanvasSingleton instance;
    public static CanvasSingleton Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<CanvasSingleton>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<CanvasSingleton>();
                    instance = newObj;
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        var objs = FindObjectsOfType<CanvasSingleton>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        // 씬 변경시에도 파괴되지 않는다.
        DontDestroyOnLoad(gameObject);
    }

    [Header("로딩 화면 그룹")]
    [SerializeField]
    private GameObject layout_LoadingGroup;

    float particleSpawnTime;
    [Header("클릭 파티클 유지 시간")]
    public float defaultParticleSpawnTime = 0.05f;
    [Header("클릭 파티클 프리팹")]
    public GameObject starParticle;

    private void OnEnable()
    {
        // 로딩 화면 종료
        RenderLoadingScreen(false);

        this.gameObject.GetComponent<Canvas>().worldCamera = MainCameraSingleton.Instance.gameObject.GetComponent<Camera>();
        Debug.Log(this.gameObject.GetComponent<Canvas>().worldCamera.name);

        // 전투 모드에서 홈으로 돌아온 경우
        if (StatManager.Instance.isBattleMode)
        {
            // 스테이지 선택 스크린 전환 이벤트
            PopUpUIManager.Instance.screenButtonList[(int)ScreenType.MainHome].ClickedScreenButton(true);

            //PopUpUIManager.Instance.ChangeScreenType(ScreenType.MainHome);

            StatManager.Instance.isBattleMode = false;

            UIUpdateManager.Instance.UpdateAll();
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0)&& particleSpawnTime >= defaultParticleSpawnTime)
        {
            ClickParticleEffect();
            particleSpawnTime = 0;
        }
        particleSpawnTime += Time.deltaTime;
    }

    // 클릭 시 파티클 출력 이벤트
    void ClickParticleEffect()
    {
        Vector3 mPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mPosition.z = 0;
        Instantiate(starParticle, mPosition, Quaternion.identity);
    }

    // 로딩 화면 출력 함수 (isEnable = 활성화 여부)
    public void RenderLoadingScreen(bool isEnable)
    {
        layout_LoadingGroup.SetActive(isEnable);
    }
}
