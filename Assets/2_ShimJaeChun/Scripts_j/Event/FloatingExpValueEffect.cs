using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingExpValueEffect : MonoBehaviour
{
    private float moveSpeed;
    private float alphaSpeed;
    private float destroyTime;

    [Header("exp 표시 텍스트")]
    public TextMeshProUGUI text;
    Color alpha;

    public int expValue;

    void Start()
    {
        moveSpeed = 1.0f;   // 텍스트 이동속도
        alphaSpeed = 2.0f;  // 텍스트 알파 값 변경 속도
        destroyTime = 2.0f; // 텍스트 유지 시간

        text = GetComponent<TextMeshProUGUI>();
        alpha = text.color;

        StartCoroutine(ExpTextTimer());
    }

    void Update()
    {
        // 텍스트 이동
        transform.Translate(new Vector2(0, moveSpeed * Time.deltaTime));

        // 텍스트 알파 값 변경
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        text.color = alpha;
    }

    IEnumerator ExpTextTimer()
    {
        yield return new WaitForSeconds(destroyTime);

        Destroy(this.gameObject);
    }
}
