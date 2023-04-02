using System.Collections.Generic;
using UnityEngine;

public class MonsterDetect_E : MonoBehaviour
{
    public List<GameObject> monsters = new List<GameObject>();

    public Transform nearestMonster; // 현재 가장 가까이에 있는 몬스터

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 감지된 몬스터를 리스트에 넣음
        if (collision.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            monsters.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 감지 범위 밖으로 나간 몬스터는 리스트에서 삭제
        monsters.Remove(collision.gameObject);
    }

    private void Update()
    {
        if (monsters.Count > 0)
        {
            FindNearestMonster();
            Debug.DrawLine(GameManager_E.Instance.Player.transform.position, nearestMonster.position, Color.red);
        }
        else
        {
            nearestMonster = null;
        }
    }

    void FindNearestMonster()
    {
        float nearestDist = 9999.9f; // 최소거리

        // 리스트 내에서 가장 가까운 몬스터를 찾음
        for (int i = 0; i < monsters.Count; i++)
        {
            float curDist = (monsters[i].transform.position - GameManager_E.Instance.Player.transform.position).magnitude; // 현재 몬스터와 캐릭터 사이의 거리

            nearestDist = nearestDist > curDist ? curDist : nearestDist; // 최솟값 갱신

            if (nearestDist == curDist)
            {
                // 갱신이 일어난 경우
                nearestMonster = monsters[i].transform;
            }
        }


        //foreach (GameObject mon in monsters)
        //{
        //    float curDist = (mon.transform.position - GameManager_E.Instance.Player.transform.position).magnitude; // 현재 몬스터와 캐릭터 사이의 거리

        //    nearestDist = nearestDist > curDist ? curDist : nearestDist; // 최솟값 갱신

        //    if (nearestDist == curDist)
        //    {
        //        // 갱신이 일어난 경우
        //        nearestMonster = mon.transform;
        //    }
        //}
    }
}
