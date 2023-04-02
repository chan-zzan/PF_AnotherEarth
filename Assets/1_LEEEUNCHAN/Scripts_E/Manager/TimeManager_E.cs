using UnityEngine;

public class TimeManager_E : MonoBehaviour
{
    public float testMultipleTime = 1.0f; // ��� ����

    public float playTime = 0.0f;

    public TMPro.TMP_Text timer; // �ð� �ؽ�Ʈ

    private void Update()
    {
        // ������ ���������� �ʰ� ������ ���� ��쵵 �ƴ� ��쿡�� ����
        if (!GameManager_E.Instance.bossSpawn && !GameManager_E.Instance.gameover)
        {
            playTime += Time.deltaTime * testMultipleTime;

            int sec = ((int)(playTime * 100) / 100) % 60;
            int min = ((int)(playTime * 100) / 100) / 60;

            string single_digit_sec = "";
            string single_digit_min = "";

            if (sec < 10)
            {
                single_digit_sec = "0";
            }

            if (min < 10)
            {
                single_digit_min = "0";
            }

            timer.text = single_digit_min + min + ":" + single_digit_sec + sec;
        }
    }
}
