using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner_E : MonoBehaviour
{
    public GameObject[] items; // ������ ���
    public List<Transform> SpawnPoints; // ���� ��ġ

    private void Start()
    {
        StartCoroutine(ItemDropping());
    }

    public void ItemSpawn(ItemType type)
    {
        GameObject obj = Instantiate(items[(int)type]); // ������ ����
        obj.transform.position = SpawnPoints[Random.Range(0, SpawnPoints.Count - 1)].position; // �������� ������ġ ����
    }

    IEnumerator ItemDropping()
    {
        // 30�ʿ� �ѹ��� ������ ����
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
