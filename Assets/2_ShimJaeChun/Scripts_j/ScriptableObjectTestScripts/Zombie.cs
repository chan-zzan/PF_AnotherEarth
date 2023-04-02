using UnityEngine;

public class Zombie : MonoBehaviour
{
    public ZombieData zombieData;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PrintZombieData(bool enable)
    {
        // 생성 시 enable = true   (호출:ZombieSpawner)
        // 파괴 시 enable = false  (호출:this)
        if (enable)
        {
            Debug.Log("-------------- 생성 ----------------");
            Debug.Log("좀비이름 :: " + zombieData.ZombieName);
            Debug.Log("체력 :: " + zombieData.Hp);
            Debug.Log("공격력 :: " + zombieData.Damage);
            Debug.Log("이동속도 :: " + zombieData.MoveSpeed);
            Debug.Log("-------------------------------------");
        }
        else
        {
            Debug.Log("-------------- 파괴 ----------------");
            Debug.Log("좀비이름 :: " + zombieData.ZombieName);
            Debug.Log("-------------------------------------");
        }
    }
    private void OnDestroy()
    {
        PrintZombieData(false);
    }
}
