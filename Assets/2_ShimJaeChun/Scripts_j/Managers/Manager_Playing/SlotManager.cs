using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

// ���� �رݽ� �Ҹ��ϴ� ��ȭŸ��
public enum UnLockPayType 
{
    Pay_Mineral = 0,
    Pay_Dia
}
public class SlotManager : MonoBehaviour
{
    // �Ĺ� ������ ����, ���� ������ ���� ������ ������ ���� �Ŵ���
    // 1. ���Կ��� �������� �Ĺ� ����
    // 2. ���� �ر� ����
    #region SigleTon
    private static SlotManager instance;
    public static SlotManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<SlotManager>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<SlotManager>();
                    instance = newObj;
                }
            }
            return instance;
        }
    }
    #endregion

    #region �Ĺ� ���� ����

    [Header("�Ĺ� ������ ��ũ���ͺ� ������Ʈ (������ ����)")]
    public List<PlantData> plantDataList;

    [Header("�� �Ĺ� ����Ϸ� Ƚ�� (�б� ����)")]
    public int[] harvestCount;

    [Header("�� �Ĺ� ��ư ���ӿ�����Ʈ (������ ����)")]
    public PlantButton[] plantButtonList;

    // ����
    [Header("�Ĺ����� �ر����� (�б� ����)")]
    [Space(10)]

    public bool[] info_PlantSlot;

    [Header("�Ĺ����� ����Ʈ")]
    public  PlantSlotInfo[] plantSlotList;

    [Header("�رݵ� �Ĺ� ���� ���� (������ ����)")]
    [Space(10)]
    [SerializeField]
    private int count_UnLockedPlantSlot;
    public int CountUnLockedPlantSlot { get { return count_UnLockedPlantSlot; } }

    [Space(10)]

    [Header("�Ĺ� ���� ��� (������ ����)")]
    public GameObject layout_Factory;

    [Header("�Ĺ� ��ǰ ������ (������ ����)")]
    public GameObject prefab_productPlant;

    [Header("�Ĺ� ���� ���� (�б� ����)")]
    [Space(10)]
    public List<GameObject> info_ProductPlant;

    public int slotIndex;  // �߰��� �Ĺ� ���� �ε��� ��ȣ

    [Header("�Ĺ� ������")]
    public PlantItemInfo plantItemInfo;

    #endregion

    [Space(100)]

    #region ���� ���� ����

    // ������
    [Header("���Ÿ� ���� ������ ��ũ���ͺ� ������Ʈ (������ ����)")]
    public List<WeaponInfoData> longWeaponDataList;

    [Header("�ٰŸ� ���� ������ ��ũ���ͺ� ������Ʈ (������ ����)")]
    public List<WeaponInfoData> shortWeaponDataList;

    // ���� 
    [Header("������ ���Ÿ� ���� ���� (������ ����)")]
    public GameObject[] equipedLongWeaponList;

    [Header("������ ���Ÿ� ���� ��ư")]
    public GameObject equipedLongWeaponButton;

    [Header("������ �ٰŸ� ���� ���� (������ ����)")]
    public GameObject[] equipedShortWeaponList;

    [Header("������ �ٰŸ� ���� ��ư")]
    public GameObject equipedShortWeaponButton;

    [Header("���Ÿ� ���� �ر����� (�б� ����)")]
    public bool[] info_LongWeaponUnLock;

    [Header("�ٰŸ� ���� �ر����� (�б� ����)")]
    public bool[] info_ShortWeaponUnLock;

    [Header("������ ���� ����(������ ����)")]
    public GameObject playerStat;

    #endregion

    #region ������������ ����

    [Header("���� ����")]
    public EncyclopediaInfo encyclopediaInfo;

    [Header("���� ����Ʈ")]
    public GameObject[] actionAnimalList;

    [Header("ĳ�� ���� ����Ʈ")]
    public GameObject[] actionCashAnimalList;

    [Header("��ġ�� ���� ����Ʈ")]
    public List<int> releaseStageAnimalList = new List<int>();
    public List<int> releaseCashAnimalList = new List<int>();

    [Header("Ǯ���ִ� ���� ����")]
    public int countingReleaseAnimal = 0;
    [Header("��ġ�� �� �ִ� ���� ����")]
    public int maxReleaseAnimal = 5;
    #endregion

    #region ���� ����
    public bool isRequestOn = true;
    public int plantItem = 0;
    public int shortWeaponItem = 0;
    public int longWeaponItem = 0;
    #endregion

    [Header("��Ʈ��ũ ������ �˾�")]
    public GameObject checkingInternet;

    // �ʱ� ����
    private void Awake()
    {
        var objs = FindObjectsOfType<SlotManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        // �Ĺ� ������ ��� ��� ���·� �ʱ�ȭ
        info_PlantSlot = Enumerable.Repeat(false, 8).ToArray<bool>();

        // ���Ÿ� ���Ⱑ ��� ��� ���·� �ʱ�ȭ
        info_LongWeaponUnLock = Enumerable.Repeat(false, System.Enum.GetValues(typeof(LWeaponType)).Length).ToArray<bool>();

        // �ٰŸ� ���Ⱑ ��� ��� ���·� �ʱ�ȭ
        info_ShortWeaponUnLock = Enumerable.Repeat(false, System.Enum.GetValues(typeof(SWeaponType)).Length).ToArray<bool>();

        // �Ĺ� ���� ������ŭ �ر�
        info_ProductPlant = new List<GameObject>();
    }


    #region �Ĺ����� �Լ����
    // ���� ���� �ʱ⼼��
    public void PlantSlotInitialSetting(bool isFirst, GameData_Json _data)
    {
        // 0�� ���Ժ��� ����
        slotIndex = 0;

        count_UnLockedPlantSlot = 0;

        // �ε�� ������ ���� ��� ���� �ε�
        if (!isFirst)
        {
            if (_data != null)
            {
                // �Ĺ����� �ر����� �ε�
                info_PlantSlot = _data.bArrPlantSlotUnLockInfo;

                // �Ĺ� �� ���� ���� �ε�
                harvestCount = _data.iArrHarvestCount;

                // �Ĺ� ������ ���� �ε�
                plantItem = _data.iPlantItem;

                // �Ĺ� ���� �رݵ� ���� �߰�
                for(int i=0; i< info_PlantSlot.Length; i++)
                {
                    if(info_PlantSlot[i] == true)
                    {
                        ++count_UnLockedPlantSlot;
                    }
                }

                // �Ĺ� ����Ƚ���� ���� �Ĺ� ��ư Ȱ��ȭ
                for (int i = 0; i < harvestCount.Length; i++)
                {
                    if (harvestCount[i] >= 5)
                    {
                        // ���� �Ĺ� �ر�
                        plantButtonList[i + 1].UnLockThisButton();
                    }
                }

                ProductData_Json _productData = GameDataManager.Instance.LoadProductData();
                
                for(int i=0; i< _productData.lPlantProductList.Count; i++)
                {
                    PlantProductData productData = JsonUtility.FromJson<PlantProductData>(_productData.lPlantProductList[i]);

                    LoadingCreatePlant(productData.iProductNumber,productData.iProductStartTime, productData.iProductCurrentTime);
                }
            }
        }
        // �ε�� ������ ���� ��� �ʱ� ����
        else
        {
            // �ʱ� ����� ���� ����
            count_UnLockedPlantSlot = 2;
            // �� �Ĺ� ���� Ƚ�� 0���� �ʱ�ȭ
            harvestCount = Enumerable.Repeat(0, plantDataList.Count).ToArray<int>();
            // ���� ���� �Ĺ� �ʱ�ȭ
            info_ProductPlant = new List<GameObject>();
        }

        // �رݵ� ���� ���� ��ŭ �ر�
        for (int i = 0; i < count_UnLockedPlantSlot; i++)
        {
            info_PlantSlot[i] = true;
        }
    }

    public void LoadingCreatePlant(int plantNum,int _loadStartTime, int _loadCurrentTime)
    {
        // �Ĺ� ������Ʈ ����
        GameObject product = Instantiate(prefab_productPlant, layout_Factory.transform.position, Quaternion.identity);

        product.transform.SetParent(layout_Factory.transform);

        product.transform.localScale = new Vector3(1, 1, 1);

        // �Ĺ� ���Կ� ���
        info_ProductPlant.Add(product);

        product.transform.SetParent(layout_Factory.transform);

        // ������ �Ĺ� ������ �Ҵ�
        product.GetComponent<PlantProductInfo>()._plantData = plantDataList[plantNum - 1];

        PlantProductInfo p_info = product.GetComponent<PlantProductInfo>();

        p_info.loadStartTime = _loadStartTime;
        p_info.loadCurrentTime = _loadCurrentTime;

        StartCoroutine(WebChk(p_info));
    }

    // �Ĺ� �����ư Ŭ���� ȣ��
    // �Ĺ� ������ �޾ƿͼ� �Ĺ� ������Ʈ ���� �� ���Կ� ���
    public void CreatePlant(int plantNum)
    {
        // ������ ���� �ε����� ��� �Ǿ��ִ� ���� �ε��� ���� ���� ���
        if (info_ProductPlant.Count < count_UnLockedPlantSlot)
        {
            // �Ĺ� ������Ʈ ����
            GameObject product = Instantiate(prefab_productPlant, layout_Factory.transform.position, Quaternion.identity);

            // �Ĺ� ���Կ� ���
            info_ProductPlant.Add(product);

            product.transform.SetParent(layout_Factory.transform);

            product.transform.localScale = new Vector3(1, 1, 1);

            // ������ �Ĺ� ������ �Ҵ�
            product.GetComponent<PlantProductInfo>()._plantData = plantDataList[plantNum - 1];

            PlantProductInfo p_info = product.GetComponent<PlantProductInfo>();

            // �Ĺ� ���� �ð� ����ũ Ÿ�� �Ҵ�
            p_info.startTime = 0;
            p_info.endTime = 100;

            StartCoroutine(WebChk(p_info));
        }
    }

    // ���ø����̼��� �����ϰ� �ٽ� ������ ��� �帥 �ð��� �������� �Ĺ��� ������
    public void AddPauseTime(int time)
    {
        // �������� �Ĺ��� ���� ���
        if(info_ProductPlant.Count > 0)
        {
            for(int i=0; i< info_ProductPlant.Count; i++)
            {
                info_ProductPlant[i].GetComponent<PlantProductInfo>().currentTime += time;
            }
        }
    }

    // �Ĺ� ������� ��ư Ŭ�� �� ȣ��
    public void CancleCreate(GameObject ob_plant)
    {
        Debug.Log("�������");

        SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponUnEquipSound);

        for (int i = 0; i < info_ProductPlant.Count; i++)
        {
            if (info_ProductPlant[i] == ob_plant)
            {
                // �ڷ�ƾ ����
                StopCoroutine(ob_plant.GetComponent<PlantProductInfo>().myCorutine);
                // ����Ʈ���� ��� ����
                info_ProductPlant.RemoveAt(i);
                // �Ĺ� ������Ʈ ����
                Destroy(ob_plant);
            }
        }
    }

    public void ErrorCreating()
    {
        checkingInternet.SetActive(true);
    }

    IEnumerator WebChk(PlantProductInfo _p_info)
    {
        string url = "www.naver.com";

        UnityWebRequest request = new UnityWebRequest();

        using (request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            //if (request.isNetworkError)
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("������");

                string date = request.GetResponseHeader("date");

                DateTime dateTime = DateTime.Parse(date);

                TimeSpan timestamp = dateTime - new System.DateTime(1970, 1, 1, 0, 0, 0);

                if (_p_info.loadStartTime > 0)
                {
                    // �ε�� �Ĺ� �� ���
                    _p_info.startTime = _p_info.loadStartTime;
                    _p_info.currentTime = (int)timestamp.TotalSeconds;
                }
                else 
                {
                    // �ʱ� ����
                    _p_info.startTime = (int)timestamp.TotalSeconds;
                    _p_info.currentTime = _p_info.startTime;
                }
                _p_info.endTime = _p_info.startTime + _p_info._plantData.CreateTime;

                _p_info.myCorutine = StartCoroutine(CreatePlant(_p_info));
            }
        }
    }

    IEnumerator CreatePlant(PlantProductInfo __p_info)
    {
        if (__p_info && !__p_info.slider.IsActive())
        {
            __p_info.slider.enabled = true;
        }

        while (__p_info && __p_info.currentTime < __p_info.endTime)
        {
            __p_info.currentTime += 1;

            yield return new WaitForSecondsRealtime(1f);
        }

        // ���� ���׼� Ȱ��ȭ
        UserReactionManager.Instance.OnReactObject(ReactionType.Plant, true);

        // �Ĺ� ��ư ��Ȱ��ȭ
        __p_info.productButton.enabled = false;
        // ��� ��ư ��Ȱ��ȭ
        __p_info.cancleButton.SetActive(false);
        // ���� ��ư Ȱ��ȭ
        __p_info.harvestButton.SetActive(true);

        yield return null;
    }

    // �Ĺ� ��Ȯ��ư Ŭ���� ȣ��
    public void HarvestPlant(GameObject ob_plant, GameObject textObj = null)
    {
        Debug.Log("��Ȯ");

        // �Ĺ� ��Ȯ�ϱ� ����Ʈ ����
        if (QuestManager.Instance.dailyQuestList[(int)DailyQuestType.HarvestPlant].myState == QuestButtonState.Proceed)
        {
            QuestManager.Instance.UpdateDailyQuest(DailyQuestType.HarvestPlant);
        }
        // ���� ���
        SoundManager.Instance.PlayEffectSound(EffectSoundType.HarvestPlantSound);

        // �ش� �Ĺ� ������ �Ҵ�
        PlantData data = ob_plant.GetComponent<PlantProductInfo>()._plantData;

        // �������� Ȱ��ȭ �Ǿ����� ���
        if(plantItemInfo.GetItemState())
        {
            // ����ġ �ι� ȹ��
            StatManager.Instance.AddPlayerEXP(data.GetExpValue * 2);
            textObj.GetComponent<TextMeshProUGUI>().text = "+" + ScoreManager.Instance.ScoreToString(data.GetExpValue * 2);

            // ������ ���
            --plantItem;

            // ������ ���� ������Ʈ
            plantItemInfo.UpdateUI();
        }
        else
        {
            StatManager.Instance.AddPlayerEXP(data.GetExpValue);
        }

        // �Ĺ� ���� Ƚ�� ����
        HarvestCounting(data.PlantNumber);

        for(int i=0; i<info_ProductPlant.Count; i++)
        {
            if (info_ProductPlant[i] == ob_plant)
            {
                info_ProductPlant.RemoveAt(i);
                Destroy(ob_plant);
            }
        }
        Debug.Log("�������� ����" + info_ProductPlant.Count);
    }

    // �Ĺ� �ϰ���Ȯ ��ư Ŭ�� �� ȣ��
    // <��͹��>
    public void OnClickAllHarvest(int index)
    {
        Debug.Log("�ϰ� ��Ȯ Ŭ��");
        if (info_ProductPlant.Count > index)
        {
            Debug.Log("���� �ε��� = " + index);
            // ����Ϸ� ��ư�� Ȱ��ȭ �Ǿ��ִ� ���
            if(info_ProductPlant[index].GetComponent<PlantProductInfo>().harvestButton.activeSelf)
            {
                info_ProductPlant[index].GetComponent<PlantProductInfo>().OnClickHarvestButton();
                OnClickAllHarvest(index);
            }
            // ����Ϸ���� ���� �Ĺ� �� ��� ���� ������ �Ĺ� üũ 
            else
            {
                OnClickAllHarvest(index + 1);
            }
        }
    }

    // �Ĺ� ���� Ƚ�� ������Ʈ
    public void HarvestCounting(int _plantNum)
    {
        // �ε����� 0���� �����ϱ� ������ �Ĺ� ��ȣ -1 
        harvestCount[_plantNum-1]+=1;

        // �ش� �Ĺ��� 5�� �������� ��� ���� �Ĺ� �ر�
        if(harvestCount[_plantNum - 1]==5 && _plantNum < 20)
        {
            // ���� �Ĺ� �ر�
            plantButtonList[_plantNum].UnLockThisButton();
        }
    }

    // ���̾Ʒ� ���� �ر� (Call by OnClickUnLockSlot)
    public bool UnLockPlantSlot(UnLockPayType payType, float amount)
    {
        switch(payType)
            {
                // �̳׶� �Ҹ� Ÿ��
                case UnLockPayType.Pay_Mineral:
                    {
                        // �� ���� ���� > �رݵ� ���� ������ ���
                        if (info_PlantSlot.Length > count_UnLockedPlantSlot)
                        {
                            // ������ ��ȭ�� ������ ���
                            if (StatManager.Instance.Own_Mineral >= amount)
                            { 
                                // ���̾� �Ҹ�
                                StatManager.Instance.SubMineral(amount);
                                // �رݵ� ���� ���� �߰�
                                ++count_UnLockedPlantSlot;
                                // ���� �ر�
                                info_PlantSlot[count_UnLockedPlantSlot - 1] = true;
                                return true;
                            }
                        }
                        return false;
                    }
                case UnLockPayType.Pay_Dia:
                    {
                        // �� ���� ���� > �رݵ� ���� ������ ���
                        if (info_PlantSlot.Length > count_UnLockedPlantSlot)
                        {
                            // ������ ��ȭ�� ������ ���
                            if (StatManager.Instance.Own_Dia >= amount)
                            {
                                // ���̾� �Ҹ�
                                StatManager.Instance.SubDia(amount);
                                // �رݵ� ���� ���� �߰�
                                ++count_UnLockedPlantSlot;
                                // ���� �ر�
                                info_PlantSlot[count_UnLockedPlantSlot - 1] = true;
                                return true;
                            }
                        }
                        return false;
                    }
                default: return false;
            }
    }

    #endregion

    #region ������� �Լ����
    public void WeaponSlotInitialSetting(bool isFirst, GameData_Json _data)
    {
        // �ε�� ������ ���� ��� ���� �ε�
        if (!isFirst)
        {
            if(_data != null)
            {
                info_ShortWeaponUnLock = _data.bArrShortWeaponUnLockInfo;
                info_LongWeaponUnLock = _data.bArrLongWeaponUnLockInfo;

                // ���� ������ ���� �ε�
                shortWeaponItem = _data.iShortWeaponItem;
                longWeaponItem = _data.iLongWeaponItem;

                // ������ �����ߴ� ���� �״�� ����
                // ������ ���Ⱑ ���� ��� �ֻ�� �ڵ� ����
                if (StatManager.Instance.s_Weapontype != SWeaponType.Idle)
                {
                    EquipWeapon(true, (int)StatManager.Instance.s_Weapontype);
                }
                if (StatManager.Instance.l_Weapontype == LWeaponType.Idle)
                {
                    EquipWeapon(false, (int)LWeaponType.Syringe);
                }
                else
                {
                    EquipWeapon(false, (int)StatManager.Instance.l_Weapontype);
                }
            }
        }
        // �ε�� ������ ���� ��� �ʱ� ����
        else
        { 
            // �ʱ� ���� (�ֻ�⸸ �ر�)
            info_LongWeaponUnLock[(int)LWeaponType.Syringe] = true;

            // �⺻ �ֻ�� ����
            EquipWeapon(false, (int)StatManager.Instance.l_Weapontype);
        }
    }

    public void EquipWeapon(bool isShortWeapon, int weaponNum)
    {
        // ���� ���� ����
        UnEquipWeapon(isShortWeapon, false);

        // �� ���� ���
        if (isShortWeapon)
        {
            // �ٰŸ� ���� ���
            StatManager.Instance.s_Weapontype = (SWeaponType)(weaponNum);
        }
        else
        {
            // ���Ÿ� ���� ���
            StatManager.Instance.l_Weapontype = (LWeaponType)(weaponNum);
        }

        // �� ���� ����
        if (isShortWeapon)
        {
            equipedShortWeaponButton.SetActive(true);
            equipedShortWeaponList[weaponNum].SetActive(true);
        }
        else
        {
            equipedLongWeaponButton.SetActive(true);
            equipedLongWeaponList[weaponNum].SetActive(true);
        }

        // �÷��̾� ���� ������Ʈ
        playerStat.GetComponent<EquipedWeaponInfo>().UpdateStat();
    }

    public void UnEquipWeapon(bool isShortWeapon, bool soundPlay)
    {
        if(isShortWeapon)   // �ٰŸ�
        {
            StatManager.Instance.s_Weapontype = SWeaponType.Idle;

            // ������ �ٰŸ� ���� ����
            for (int i = 1; i < equipedShortWeaponList.Length; i++)
            {
                equipedShortWeaponList[i].SetActive(false);
            }

            equipedShortWeaponButton.SetActive(false);
        }
        else  // ���Ÿ�
        {
            StatManager.Instance.l_Weapontype = LWeaponType.Idle;

            // ������ ���Ÿ� ���� ����
            for (int i = 1; i < equipedLongWeaponList.Length; i++)
            {
                Instance.equipedLongWeaponList[i].SetActive(false);
            }

            equipedLongWeaponButton.SetActive(false);
        }

        // �÷��̾� ���� ������Ʈ
        playerStat.GetComponent<EquipedWeaponInfo>().UpdateStat();
    }

    #endregion

    #region ������������ �Լ����
    
    public void AnimalSlotInitialSetting(GameData_Json data)
    {
        // �ε� �� �����Ͱ� ���� ���
        if(data != null)
        {
            maxReleaseAnimal = data.iMaxReleaseAnimal;
            releaseStageAnimalList = data.iReleaseStageAnimalList;
            releaseCashAnimalList = data.iReleaseCashAnimalList;

            for(int i=0; i< releaseStageAnimalList.Count; i++)
            {
                actionAnimalList[releaseStageAnimalList[i]-1].SetActive(true);
                ++countingReleaseAnimal;
            }
            for (int i = 0; i < releaseCashAnimalList.Count; i++)
            {
                actionCashAnimalList[releaseCashAnimalList[i]].SetActive(true);
            }
        }
    }
    
    // ������ Ǯ���ִ��� ���θ� ��ȯ
    public bool GetIsReleaseAnimal(int num, bool isCashAnimal)
    {
        // �������� ����
        if(!isCashAnimal)
        {
            for(int i=0; i<releaseStageAnimalList.Count; i++)
            {
                if (releaseStageAnimalList[i] == num)
                {
                    return true;
                }
            }
            return false;
        }
        // ĳ�õ���
        else
        {
            for (int i = 0; i < releaseCashAnimalList.Count; i++)
            {
                if (releaseCashAnimalList[i] == num)
                {
                    return true;
                }
            }
            return false;
        }
    }

    // ���� Ǯ�����
    // Ǯ����� ���� ���� ��ȯ
    // �ִ� ��ġ �� �̻����� ��ġ�ϰ��� �� ��� false ��ȯ
    public bool ReleaseAnimal(int stageNum)
    {
            // �Ϲ� ����
            if (countingReleaseAnimal < maxReleaseAnimal)
            {
                actionAnimalList[stageNum - 1].SetActive(true);
                ++countingReleaseAnimal;
                releaseStageAnimalList.Add(stageNum);
                encyclopediaInfo.UpdateUI();
                return true;
            }
            else
            {
                return false;
            }
    }
    public void ReleaseCashAnimal(int cashNum)
    {
        // ĳ�� ����
        actionCashAnimalList[cashNum].SetActive(true);
        releaseCashAnimalList.Add(cashNum);
    }

    // ���� �ҷ�����
    public void ComeAnimal(int stageNum)
    {
        actionAnimalList[stageNum - 1].SetActive(false);
        --countingReleaseAnimal;

        // �ش� ������ ã��
        for(int i=0; i< releaseStageAnimalList.Count; i++)
        {
            if(releaseStageAnimalList[i] == stageNum)
            {
                releaseStageAnimalList.RemoveAt(i);
            }
        }
        encyclopediaInfo.UpdateUI();
    }
    public void ComeCashAnimal(int cashNum)
    {
        actionCashAnimalList[cashNum].SetActive(false);

        // �ش� ������ ã��
        for (int i = 0; i < releaseCashAnimalList.Count; i++)
        {
            if (releaseCashAnimalList[i] == cashNum)
            {
                releaseCashAnimalList.RemoveAt(i);
            }
        }
    }

    #endregion
}

public class ProductData_Json
{
    public List<string> lPlantProductList = new List<string>();

    public ProductData_Json()
    {
        if (SlotManager.Instance.info_ProductPlant.Count > 0)
        {
            for (int i = 0; i < SlotManager.Instance.info_ProductPlant.Count; i++)
            {
                PlantProductData data = new PlantProductData(
                    SlotManager.Instance.info_ProductPlant[i].GetComponent<PlantProductInfo>()._plantData.PlantNumber
                    , SlotManager.Instance.info_ProductPlant[i].GetComponent<PlantProductInfo>().startTime
                    , SlotManager.Instance.info_ProductPlant[i].GetComponent<PlantProductInfo>().currentTime);

                lPlantProductList.Add(JsonUtility.ToJson(data));
            }
        }
    }


}

public class PlantProductData
{
    public int iProductNumber;
    public int iProductStartTime;
    public int iProductCurrentTime;
    public PlantProductData(int num,int startTime, int curTime)
    {
        iProductNumber = num;
        iProductStartTime = startTime;
        iProductCurrentTime = curTime;
    }
}