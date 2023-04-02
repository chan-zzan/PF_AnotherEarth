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
    // ������ ������ ��ǰ ����

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

    [Header("ĳ�� ���� ���� ����Ʈ")]
    [Header("0:������ 1:�Ǵ� 2:�ڳ���")]
    public bool[] CashAnimalList;

    [Header("��Ȱ�� ����")]
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
        // ������ �ε�
        if (data != null)
        {
            UserProductData_Json p_data = GameDataManager.Instance.LoadUserProductData();

            CashAnimalList = p_data.bCashAnimalList;
        }
        // ������ ���� ���
        else
        {
            // ĳ�� ���� ����Ʈ �ʱ�ȭ
            CashAnimalList = Enumerable.Repeat(false, 3).ToArray<bool>();
        }
    }


    // ��ǰ ����
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

    // ���ŵ� ��ǰ����? ����
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
            Debug.Log("��Ȱ�� ���");
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