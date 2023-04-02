using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetEffect_E : MonoBehaviour
{
    public void MagnetEffect()
    {
        if (this.gameObject.activeSelf)
        {
            StartCoroutine(MagnetEffectGenerating());
        }
    }

    IEnumerator MagnetEffectGenerating()
    {
        while (true) // Ȱ��ȭ ������ ��쿡�� ����
        {
            Vector3 dir = (GameManager_E.Instance.Player.transform.position - this.transform.position).normalized; // ���� ����

            this.transform.Translate(dir * Time.fixedDeltaTime * 150); // �̵�

            yield return new WaitForFixedUpdate();
        }
    }
}
