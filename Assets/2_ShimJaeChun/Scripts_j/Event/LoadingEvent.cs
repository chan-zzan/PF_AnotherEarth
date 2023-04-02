using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingEvent : MonoBehaviour
{
    [Header("전투 로딩 화면에 출력할 랜덤 텍스트")]
    [TextArea(1,2)]
    public string[] randomText;

    [Header("메인 로딩 화면에 출력할 랜덤 텍스트")]
    [TextArea(1, 2)]
    public string[] m_randomText;

    [Header("Text")]
    [SerializeField]
    private TextMeshProUGUI loadingText;

    [Header("이동할 씬이 전투씬인지?")]
    public bool isBattleScene;

    private void OnEnable()
    {
        if (isBattleScene)
        {
            int rand = Random.Range(0, randomText.Length);

            loadingText.text = randomText[rand];
        }
        else
        {
            int rand = Random.Range(0, m_randomText.Length);

            loadingText.text = m_randomText[rand];
        }
    }
}
