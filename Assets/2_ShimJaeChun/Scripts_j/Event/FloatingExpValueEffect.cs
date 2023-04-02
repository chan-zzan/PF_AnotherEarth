using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingExpValueEffect : MonoBehaviour
{
    private float moveSpeed;
    private float alphaSpeed;
    private float destroyTime;

    [Header("exp ǥ�� �ؽ�Ʈ")]
    public TextMeshProUGUI text;
    Color alpha;

    public int expValue;

    void Start()
    {
        moveSpeed = 1.0f;   // �ؽ�Ʈ �̵��ӵ�
        alphaSpeed = 2.0f;  // �ؽ�Ʈ ���� �� ���� �ӵ�
        destroyTime = 2.0f; // �ؽ�Ʈ ���� �ð�

        text = GetComponent<TextMeshProUGUI>();
        alpha = text.color;

        StartCoroutine(ExpTextTimer());
    }

    void Update()
    {
        // �ؽ�Ʈ �̵�
        transform.Translate(new Vector2(0, moveSpeed * Time.deltaTime));

        // �ؽ�Ʈ ���� �� ����
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        text.color = alpha;
    }

    IEnumerator ExpTextTimer()
    {
        yield return new WaitForSeconds(destroyTime);

        Destroy(this.gameObject);
    }
}
