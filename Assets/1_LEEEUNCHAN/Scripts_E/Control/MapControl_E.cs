using UnityEngine;

public class MapControl_E : MonoBehaviour
{
    float MoveDist;

    private void Start()
    {
        MoveDist = Mathf.Abs(this.transform.position.x) * 4; // Ÿ�ϸ��� �̵��Ÿ� = ���� �������� 4��
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Area"))
        {
            return;
        }

        Vector3 playerPos = GameManager_E.Instance.Player.transform.position; // �÷��̾��� ��ġ
        Vector3 curPos = this.transform.position; // ���� ���� ��ġ

        //// ����
        //Vector2 playerDir = GameManager_E.Instance.Player.JoyInput;

        //// �ʰ� �÷��̾��� �Ÿ���
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
            this.transform.Translate(Vector3.right * dir_X * MoveDist); // x������ Ÿ�ϸ� �ϳ��� �ǳʶٰ� �̵�
        }
        else if (diff_X < diff_Y)
        {
            this.transform.Translate(Vector3.up * dir_Y * MoveDist); // y������ Ÿ�ϸ� �ϳ��� �ǳʲ�� �̵�
        }
    }
}
