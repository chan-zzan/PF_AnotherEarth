using UnityEngine;

public class CameraMove_E : MonoBehaviour
{
    [SerializeField] Transform Target;

    private void LateUpdate()
    {
        switch(GameManager_E.Instance.Map.curMapType)
        {
            case MapType_E.Infinite:
                {
                    // ���� ���� ���
                    Vector3 pos = new Vector3(Target.position.x, Target.position.y, this.transform.position.z);
                    this.transform.position = pos;
                }
                break;
            case MapType_E.FixedVertical:
                {
                    // ���� ���� ���� ���
                    Vector3 pos = new Vector3(Target.position.x, this.transform.position.y, this.transform.position.z);
                    this.transform.position = pos;
                }
                break;
            case MapType_E.FixedHorizontal:
                {
                    // ���� ���� ���� ���
                    Vector3 pos = new Vector3(this.transform.position.x, Target.position.y, this.transform.position.z);
                    this.transform.position = pos;
                }
                break;
        }

    }

}
