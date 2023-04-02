using System.Collections.Generic;
using UnityEngine;

public enum ZombieType { Normal = 0, Power, Tanker, Speed }

public class ZombieSpawner : MonoBehaviour
{
    [Tooltip("Normal(0), Power(1), Tanker(2), Speed(3)")]
    [SerializeField]
    private List<ZombieData> zombieDatas;

    [SerializeField]
    private GameObject zombiePrefab;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < zombieDatas.Count; i++)
        {
            Zombie zombie = SpawnZombie((ZombieType)i);
            zombie.PrintZombieData(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Zombie SpawnZombie(ZombieType type)
    {
        Zombie newZombie = Instantiate(zombiePrefab).GetComponent<Zombie>();
        newZombie.zombieData = zombieDatas[(int)type];
        newZombie.name = newZombie.zombieData.ZombieName;
        return newZombie;
    }
}
