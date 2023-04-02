using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner_E : MonoBehaviour
{
    public GameObject[] items; // 아이템 목록
    public List<Transform> SpawnPoints; // 생성 위치

    private void Start()
    {
        StartCoroutine(ItemDropping());
    }

    public void ItemSpawn(ItemType type)
    {
        GameObject obj = Instantiate(items[(int)type]); // 아이템 생성
        obj.transform.position = SpawnPoints[Random.Range(0, SpawnPoints.Count - 1)].position; // 랜덤으로 생성위치 설정
    }

    IEnumerator ItemDropping()
    {
        // 30초에 한번씩 아이템 생성
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(30.0f);
            ItemSpawn(ItemType.Magnet);
            ItemSpawn(ItemType.Heal);
            print("item drop");
        }
    }

    public void StopItemDrop()
    {
        StopAllCoroutines();
    }
}
