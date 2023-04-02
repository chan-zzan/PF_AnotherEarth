using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Coin_E : MagnetEffect_E
{
    public enum CoinType
    {
        None, Kilo, Million, Billion
    }
    public CoinType myType;

    public int value; // ��

    #region ������Ʈ Ǯ
    IObjectPool<Coin_E> myPool;

    public void SetManagedPool(IObjectPool<Coin_E> pool)
    {
        // Ǯ�� PoolManager�� ����
        myPool = pool;
    }

    public void DestroyCoin()
    {
        // ����ü�� �ٽ� Ǯ�� ��������
        myPool.Release(this);
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // �÷��̾�� �΋H�� ���
            DestroyCoin();
            GameManager_E.Instance.totalMineral += value; // �̳׶� ȹ��
            GameManager_E.Instance.coinText.text = ScoreManager.Instance.ScoreToString(GameManager_E.Instance.totalMineral); // �̳׶� �ؽ�Ʈ ����

            SoundManager_E.Instance.EffectSoundPlay(4);
        }
    }
}
