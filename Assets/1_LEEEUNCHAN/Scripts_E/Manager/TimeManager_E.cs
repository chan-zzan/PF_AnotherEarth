using UnityEngine;

public class TimeManager_E : MonoBehaviour
{
    public float testMultipleTime = 1.0f; // 배속 설정

    public float playTime = 0.0f;

    public TMPro.TMP_Text timer; // 시간 텍스트

    private void Update()
    {
        // 보스가 등장하지도 않고 게임이 끝난 경우도 아닌 경우에만 진행
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
