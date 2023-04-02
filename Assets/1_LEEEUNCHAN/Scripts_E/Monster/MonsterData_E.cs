using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "MonsterData", order = int.MinValue)] // 스크립터블 오브젝트를 만들 수 있는 경로 생성
public class MonsterData_E : ScriptableObject
{
    [SerializeField]
    private float hp; // 체력

    [SerializeField]
    private float damage; // 공격력

    [SerializeField]
    private float moveSpeed; // 이동속도


    public float Hp
    {
        get { return hp; }
    }

    public float Damage
    {
        get { return damage; }
    }

    public float MoveSpeed
    {
        get { return moveSpeed; }
    }
}
