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
            // ������ ȹ���
            Destroy(this.gameObject);
            ApplyItem(this.myType); // ������ ����
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
                GameManager_E.Instance.Player.CurHP += GameManager_E.Instance.Player.MaxHP * 0.3f; // �ִ� ü���� 30���� ȸ��
                break;
        }
    }

    void FindMinerals()
    {
        // �ڼ��� �Ծ��� �� ���� ���� ���� �̳׶��� ã��
        Collider2D[] cols = Physics2D.OverlapCircleAll(GameManager_E.Instance.Player.transform.position, 9999.9f);

        for (int i = 0; i < cols.Length; i++)
        {
            // �ڼ� ȿ�� �߻�
            MagnetEffect_E curCoin = cols[i].GetComponent<MagnetEffect_E>();

            if (curCoin != null && curCoin.CompareTag("coin"))
            {
                curCoin.MagnetEffect();
            }
        }

    }
}
