using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingEvent : MonoBehaviour
{
    [Header("���� �ε� ȭ�鿡 ����� ���� �ؽ�Ʈ")]
    [TextArea(1,2)]
    public string[] randomText;

    [Header("���� �ε� ȭ�鿡 ����� ���� �ؽ�Ʈ")]
    [TextArea(1, 2)]
    public string[] m_randomText;

    [Header("Text")]
    [SerializeField]
    private TextMeshProUGUI loadingText;

    [Header("�̵��� ���� ����������?")]
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
