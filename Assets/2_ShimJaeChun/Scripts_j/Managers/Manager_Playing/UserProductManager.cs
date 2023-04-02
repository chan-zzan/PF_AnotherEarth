using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum ProductType
{
    Animal =0,
    Resurrection
}


public class UserProductManager : MonoBehaviour
{
    // 유저가 구매한 제품 관리

    #region SigleTon
    private static UserProductManager instance;
    public static UserProductManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<UserProductManager>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<UserProductManager>();
                    instance = newObj;
                }
            }
            return instance;
        }
    }
    #endregion

    [Header("캐시 동물 구매 리스트")]
    [Header("0:아프간 1:판다 2:코끼리")]
    public bool[] CashAnimalList;

    [Header("부활권 개수")]
    public int own_Resurrection;

    private void Awake()
    {
        var objs = FindObjectsOfType<UserProductManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        own_Resurrection = 0;
    }

    public void UserProductInitialSetting(GameData_Json data)
    {
        // 데이터 로드
        if (data != null)
        {
            UserProductData_Json p_data = GameDataManager.Instance.LoadUserProductData();

            CashAnimalList = p_data.bCashAnimalList;
        }
        // 데이터 없을 경우
        else
        {
            // 캐시 동물 리스트 초기화
            CashAnimalList = Enumerable.Repeat(false, 3).ToArray<bool>();
        }
    }


    // 제품 구매
    public void BuyProduct(ProductType p_type, int p_number)
    {
        switch(p_type)
        {
            case ProductType.Animal:
                {
                    CashAnimalList[p_number] = true;
                    break;
                }
            default: break;
        }

    }

    // 구매된 제품인지? 리턴
    public bool GetIsBuyProduct(ProductType p_type, int p_number)
    {
        switch(p_type)
        {
            case ProductType.Animal:
                {
                    return CashAnimalList[p_number];
                }
            default: return false;
        }
    }

    public void UseResurrection()
    {
        if(own_Resurrection>0)
        {
            Debug.Log("부활권 사용");
            own_Resurrection--;
        }
    }

}

public class UserProductData_Json
{
    public bool[] bCashAnimalList;
    public int iOwnResurrection;


    public UserProductData_Json()
    {
        bCashAnimalList = UserProductManager.Instance.CashAnimalList;
        iOwnResurrection = UserProductManager.Instance.own_Resurrection;
    }
}