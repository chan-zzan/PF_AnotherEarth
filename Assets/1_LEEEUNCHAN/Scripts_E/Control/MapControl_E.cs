using UnityEngine;

public class MapControl_E : MonoBehaviour
{
    float MoveDist;

    private void Start()
    {
        MoveDist = Mathf.Abs(this.transform.position.x) * 4; // 타일맵의 이동거리 = 맵의 반지름의 4배
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Area"))
        {
            return;
        }

        Vector3 playerPos = GameManager_E.Instance.Player.transform.position; // 플레이어의 위치
        Vector3 curPos = this.transform.position; // 현재 맵의 위치

        //// 방향
        //Vector2 playerDir = GameManager_E.Instance.Player.JoyInput;

        //// 맵과 플레이어의 거리차
        //float diff_X = Mathf.Abs(curPos.x - playerPos.x);
        //float diff_Y = Mathf.Abs(curPos.y - playerPos.y);

        //dir_X = playerDir.x < 0 ? -1 : 1;
        //dir_Y = playerDir.y < 0 ? -1 : 1;

        float dir_X = playerPos.x - curPos.x;
        float dir_Y = playerPos.y - curPos.y;

        float diff_X = Mathf.Abs(dir_X);
        float diff_Y = Mathf.Abs(dir_Y);

        dir_X = dir_X > 0 ? 1 : -1;
        dir_Y = dir_Y > 0 ? 1 : -1;

        if (diff_X > diff_Y)
        {
            this.transform.Translate(Vector3.right * dir_X * MoveDist); // x축으로 타일맵 하나를 건너뛰고 이동
        }
        else if (diff_X < diff_Y)
        {
            this.transform.Translate(Vector3.up * dir_Y * MoveDist); // y축으로 타일맵 하나를 건너뀌고 이동
        }
    }
}
