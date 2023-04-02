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

    public int value; // 값

    #region 오브젝트 풀
    IObjectPool<Coin_E> myPool;

    public void SetManagedPool(IObjectPool<Coin_E> pool)
    {
        // 풀을 PoolManager에 저장
        myPool = pool;
    }

    public void DestroyCoin()
    {
        // 투사체를 다시 풀에 돌려놓음
        myPool.Release(this);
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // 플레이어와 부딫힌 경우
            DestroyCoin();
            GameManager_E.Instance.totalMineral += value; // 미네랄 획득
            GameManager_E.Instance.coinText.text = ScoreManager.Instance.ScoreToString(GameManager_E.Instance.totalMineral); // 미네랄 텍스트 적용

            SoundManager_E.Instance.EffectSoundPlay(4);
        }
    }
}
