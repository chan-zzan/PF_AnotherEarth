using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FloatingLevelUpEffect : MonoBehaviour
{
    private float moveSpeed;
    private float alphaSpeed;
    private float destroyTime;

    [Header("exp ǥ�� �ؽ�Ʈ")]
    public TextMeshProUGUI text;

    Color alpha;

    void Start()
    {
        moveSpeed = 1.0f;   // �ؽ�Ʈ �̵��ӵ�
        alphaSpeed = 1.0f;  // �ؽ�Ʈ ���� �� ���� �ӵ�
        destroyTime = 3.5f; // �ؽ�Ʈ ���� �ð�

        text = GetComponent<TextMeshProUGUI>();
        alpha = text.color;

        StartCoroutine(LevelUpTextTimer());
    }
    void Update()
    {
        // �ؽ�Ʈ �̵�
        transform.Translate(new Vector2(0, moveSpeed * Time.deltaTime));

        // �ؽ�Ʈ ���� �� ����
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        text.color = alpha;
    }

    IEnumerator LevelUpTextTimer()
    {
        yield return new WaitForSeconds(destroyTime);

        Destroy(this.gameObject);
    }
}
