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
        // �� ����ÿ��� �ı����� �ʴ´�.
        DontDestroyOnLoad(gameObject);
    }

    [Header("�ε� ȭ�� �׷�")]
    [SerializeField]
    private GameObject layout_LoadingGroup;

    float particleSpawnTime;
    [Header("Ŭ�� ��ƼŬ ���� �ð�")]
    public float defaultParticleSpawnTime = 0.05f;
    [Header("Ŭ�� ��ƼŬ ������")]
    public GameObject starParticle;

    private void OnEnable()
    {
        // �ε� ȭ�� ����
        RenderLoadingScreen(false);

        this.gameObject.GetComponent<Canvas>().worldCamera = MainCameraSingleton.Instance.gameObject.GetComponent<Camera>();
        Debug.Log(this.gameObject.GetComponent<Canvas>().worldCamera.name);

        // ���� ��忡�� Ȩ���� ���ƿ� ���
        if (StatManager.Instance.isBattleMode)
        {
            // �������� ���� ��ũ�� ��ȯ �̺�Ʈ
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

    // Ŭ�� �� ��ƼŬ ��� �̺�Ʈ
    void ClickParticleEffect()
    {
        Vector3 mPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mPosition.z = 0;
        Instantiate(starParticle, mPosition, Quaternion.identity);
    }

    // �ε� ȭ�� ��� �Լ� (isEnable = Ȱ��ȭ ����)
    public void RenderLoadingScreen(bool isEnable)
    {
        layout_LoadingGroup.SetActive(isEnable);
    }
}
