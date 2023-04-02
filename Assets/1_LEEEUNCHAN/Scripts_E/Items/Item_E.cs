using UnityEngine;
public enum ItemType
{
    Magnet, Heal
}

public class Item_E : MagnetEffect_E
{
    [SerializeField]
    private ItemType myType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // 아이템 획득시
            Destroy(this.gameObject);
            ApplyItem(this.myType); // 아이템 적용
        }
    }

    void ApplyItem(ItemType type)
    {
        switch (type)
        {
            case ItemType.Magnet:
                FindMinerals();
                break;
            case ItemType.Heal:
                GameManager_E.Instance.Player.CurHP += GameManager_E.Instance.Player.MaxHP * 0.3f; // 최대 체력의 30프로 회복
                break;
        }
    }

    void FindMinerals()
    {
        // 자석을 먹었을 때 일정 범위 내의 미네랄을 찾음
        Collider2D[] cols = Physics2D.OverlapCircleAll(GameManager_E.Instance.Player.transform.position, 9999.9f);

        for (int i = 0; i < cols.Length; i++)
        {
            // 자석 효과 발생
            MagnetEffect_E curCoin = cols[i].GetComponent<MagnetEffect_E>();

            if (curCoin != null && curCoin.CompareTag("coin"))
            {
                curCoin.MagnetEffect();
            }
        }

    }
}
