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
        while (true) // 활성화 상태인 경우에만 동작
        {
            Vector3 dir = (GameManager_E.Instance.Player.transform.position - this.transform.position).normalized; // 방향 설정

            this.transform.Translate(dir * Time.fixedDeltaTime * 150); // 이동

            yield return new WaitForFixedUpdate();
        }
    }
}
