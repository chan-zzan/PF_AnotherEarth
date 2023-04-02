using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

// 슬롯 해금시 소모하는 재화타입
public enum UnLockPayType 
{
    Pay_Mineral = 0,
    Pay_Dia
}
public class SlotManager : MonoBehaviour
{
    // 식물 연구소 슬롯, 무기 연구소 슬롯 데이터 정보를 가진 매니저
    // 1. 슬롯에서 생산중인 식물 관리
    // 2. 슬롯 해금 관리
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

    #region 식물 슬롯 관련

    [Header("식물 데이터 스크립터블 오브젝트 (에디터 세팅)")]
    public List<PlantData> plantDataList;

    [Header("각 식물 생산완료 횟수 (읽기 전용)")]
    public int[] harvestCount;

    [Header("각 식물 버튼 게임오브젝트 (에디터 세팅)")]
    public PlantButton[] plantButtonList;

    // 슬롯
    [Header("식물슬롯 해금정보 (읽기 전용)")]
    [Space(10)]

    public bool[] info_PlantSlot;

    [Header("식물슬롯 리스트")]
    public  PlantSlotInfo[] plantSlotList;

    [Header("해금된 식물 슬롯 개수 (에디터 세팅)")]
    [Space(10)]
    [SerializeField]
    private int count_UnLockedPlantSlot;
    public int CountUnLockedPlantSlot { get { return count_UnLockedPlantSlot; } }

    [Space(10)]

    [Header("식물 생산 목록 (에디터 세팅)")]
    public GameObject layout_Factory;

    [Header("식물 제품 프리팹 (에디터 세팅)")]
    public GameObject prefab_productPlant;

    [Header("식물 생산 정보 (읽기 전용)")]
    [Space(10)]
    public List<GameObject> info_ProductPlant;

    public int slotIndex;  // 추가할 식물 슬롯 인덱스 번호

    [Header("식물 아이템")]
    public PlantItemInfo plantItemInfo;

    #endregion

    [Space(100)]

    #region 무기 슬롯 관련

    // 데이터
    [Header("원거리 무기 데이터 스크립터블 오브젝트 (에디터 세팅)")]
    public List<WeaponInfoData> longWeaponDataList;

    [Header("근거리 무기 데이터 스크립터블 오브젝트 (에디터 세팅)")]
    public List<WeaponInfoData> shortWeaponDataList;

    // 슬롯 
    [Header("장착된 원거리 무기 슬롯 (에디터 세팅)")]
    public GameObject[] equipedLongWeaponList;

    [Header("장착된 원거리 무기 버튼")]
    public GameObject equipedLongWeaponButton;

    [Header("장착된 근거리 무기 슬롯 (에디터 세팅)")]
    public GameObject[] equipedShortWeaponList;

    [Header("장착된 근거리 무기 버튼")]
    public GameObject equipedShortWeaponButton;

    [Header("원거리 무기 해금정보 (읽기 전용)")]
    public bool[] info_LongWeaponUnLock;

    [Header("근거리 무기 해금정보 (읽기 전용)")]
    public bool[] info_ShortWeaponUnLock;

    [Header("장착된 무기 스텟(에디터 세팅)")]
    public GameObject playerStat;

    #endregion

    #region 동물도감슬롯 관련

    [Header("동물 도감")]
    public EncyclopediaInfo encyclopediaInfo;

    [Header("동물 리스트")]
    public GameObject[] actionAnimalList;

    [Header("캐시 동물 리스트")]
    public GameObject[] actionCashAnimalList;

    [Header("배치한 동물 리스트")]
    public List<int> releaseStageAnimalList = new List<int>();
    public List<int> releaseCashAnimalList = new List<int>();

    [Header("풀려있는 동물 개수")]
    public int countingReleaseAnimal = 0;
    [Header("배치할 수 있는 동물 개수")]
    public int maxReleaseAnimal = 5;
    #endregion

    #region 상점 관련
    public bool isRequestOn = true;
    public int plantItem = 0;
    public int shortWeaponItem = 0;
    public int longWeaponItem = 0;
    #endregion

    [Header("네트워크 오류시 팝업")]
    public GameObject checkingInternet;

    // 초기 세팅
    private void Awake()
    {
        var objs = FindObjectsOfType<SlotManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        // 식물 슬롯이 모두 잠긴 상태로 초기화
        info_PlantSlot = Enumerable.Repeat(false, 8).ToArray<bool>();

        // 원거리 무기가 모두 잠긴 상태로 초기화
        info_LongWeaponUnLock = Enumerable.Repeat(false, System.Enum.GetValues(typeof(LWeaponType)).Length).ToArray<bool>();

        // 근거리 무기가 모두 잠긴 상태로 초기화
        info_ShortWeaponUnLock = Enumerable.Repeat(false, System.Enum.GetValues(typeof(SWeaponType)).Length).ToArray<bool>();

        // 식물 슬롯 개수만큼 해금
        info_ProductPlant = new List<GameObject>();
    }


    #region 식물관련 함수목록
    // 슬롯 정보 초기세팅
    public void PlantSlotInitialSetting(bool isFirst, GameData_Json _data)
    {
        // 0번 슬롯부터 시작
        slotIndex = 0;

        count_UnLockedPlantSlot = 0;

        // 로드된 정보가 있을 경우 먼저 로드
        if (!isFirst)
        {
            if (_data != null)
            {
                // 식물슬롯 해금정보 로드
                info_PlantSlot = _data.bArrPlantSlotUnLockInfo;

                // 식물 별 생산 개수 로드
                harvestCount = _data.iArrHarvestCount;

                // 식물 아이템 개수 로드
                plantItem = _data.iPlantItem;

                // 식물 슬롯 해금된 개수 추가
                for(int i=0; i< info_PlantSlot.Length; i++)
                {
                    if(info_PlantSlot[i] == true)
                    {
                        ++count_UnLockedPlantSlot;
                    }
                }

                // 식물 생산횟수에 따른 식물 버튼 활성화
                for (int i = 0; i < harvestCount.Length; i++)
                {
                    if (harvestCount[i] >= 5)
                    {
                        // 다음 식물 해금
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
        // 로드된 정보가 없을 경우 초기 세팅
        else
        {
            // 초기 개방된 슬롯 개수
            count_UnLockedPlantSlot = 2;
            // 각 식물 생산 횟수 0으로 초기화
            harvestCount = Enumerable.Repeat(0, plantDataList.Count).ToArray<int>();
            // 생산 중인 식물 초기화
            info_ProductPlant = new List<GameObject>();
        }

        // 해금된 슬롯 개수 만큼 해금
        for (int i = 0; i < count_UnLockedPlantSlot; i++)
        {
            info_PlantSlot[i] = true;
        }
    }

    public void LoadingCreatePlant(int plantNum,int _loadStartTime, int _loadCurrentTime)
    {
        // 식물 오브젝트 생산
        GameObject product = Instantiate(prefab_productPlant, layout_Factory.transform.position, Quaternion.identity);

        product.transform.SetParent(layout_Factory.transform);

        product.transform.localScale = new Vector3(1, 1, 1);

        // 식물 슬롯에 등록
        info_ProductPlant.Add(product);

        product.transform.SetParent(layout_Factory.transform);

        // 생산한 식물 데이터 할당
        product.GetComponent<PlantProductInfo>()._plantData = plantDataList[plantNum - 1];

        PlantProductInfo p_info = product.GetComponent<PlantProductInfo>();

        p_info.loadStartTime = _loadStartTime;
        p_info.loadCurrentTime = _loadCurrentTime;

        StartCoroutine(WebChk(p_info));
    }

    // 식물 생산버튼 클릭시 호출
    // 식물 정보를 받아와서 식물 오브젝트 생성 후 슬롯에 등록
    public void CreatePlant(int plantNum)
    {
        // 생산할 슬롯 인덱스가 잠금 되어있는 슬롯 인덱스 보다 작을 경우
        if (info_ProductPlant.Count < count_UnLockedPlantSlot)
        {
            // 식물 오브젝트 생산
            GameObject product = Instantiate(prefab_productPlant, layout_Factory.transform.position, Quaternion.identity);

            // 식물 슬롯에 등록
            info_ProductPlant.Add(product);

            product.transform.SetParent(layout_Factory.transform);

            product.transform.localScale = new Vector3(1, 1, 1);

            // 생산한 식물 데이터 할당
            product.GetComponent<PlantProductInfo>()._plantData = plantDataList[plantNum - 1];

            PlantProductInfo p_info = product.GetComponent<PlantProductInfo>();

            // 식물 생산 시간 페이크 타임 할당
            p_info.startTime = 0;
            p_info.endTime = 100;

            StartCoroutine(WebChk(p_info));
        }
    }

    // 애플리케이션을 중지하고 다시 시작한 경우 흐른 시간을 생산중인 식물에 더해줌
    public void AddPauseTime(int time)
    {
        // 생산중인 식물이 있을 경우
        if(info_ProductPlant.Count > 0)
        {
            for(int i=0; i< info_ProductPlant.Count; i++)
            {
                info_ProductPlant[i].GetComponent<PlantProductInfo>().currentTime += time;
            }
        }
    }

    // 식물 생산취소 버튼 클릭 시 호출
    public void CancleCreate(GameObject ob_plant)
    {
        Debug.Log("생산취소");

        SoundManager.Instance.PlayEffectSound(EffectSoundType.WeaponUnEquipSound);

        for (int i = 0; i < info_ProductPlant.Count; i++)
        {
            if (info_ProductPlant[i] == ob_plant)
            {
                // 코루틴 종료
                StopCoroutine(ob_plant.GetComponent<PlantProductInfo>().myCorutine);
                // 리스트에서 요소 제거
                info_ProductPlant.RemoveAt(i);
                // 식물 오브젝트 제거
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
                Debug.Log("생산중");

                string date = request.GetResponseHeader("date");

                DateTime dateTime = DateTime.Parse(date);

                TimeSpan timestamp = dateTime - new System.DateTime(1970, 1, 1, 0, 0, 0);

                if (_p_info.loadStartTime > 0)
                {
                    // 로드된 식물 일 경우
                    _p_info.startTime = _p_info.loadStartTime;
                    _p_info.currentTime = (int)timestamp.TotalSeconds;
                }
                else 
                {
                    // 초기 세팅
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

        // 유저 리액션 활성화
        UserReactionManager.Instance.OnReactObject(ReactionType.Plant, true);

        // 식물 버튼 비활성화
        __p_info.productButton.enabled = false;
        // 취소 버튼 비활성화
        __p_info.cancleButton.SetActive(false);
        // 생산 버튼 활성화
        __p_info.harvestButton.SetActive(true);

        yield return null;
    }

    // 식물 수확버튼 클릭시 호출
    public void HarvestPlant(GameObject ob_plant, GameObject textObj = null)
    {
        Debug.Log("수확");

        // 식물 수확하기 퀘스트 적용
        if (QuestManager.Instance.dailyQuestList[(int)DailyQuestType.HarvestPlant].myState == QuestButtonState.Proceed)
        {
            QuestManager.Instance.UpdateDailyQuest(DailyQuestType.HarvestPlant);
        }
        // 사운드 출력
        SoundManager.Instance.PlayEffectSound(EffectSoundType.HarvestPlantSound);

        // 해당 식물 데이터 할당
        PlantData data = ob_plant.GetComponent<PlantProductInfo>()._plantData;

        // 아이템이 활성화 되어있을 경우
        if(plantItemInfo.GetItemState())
        {
            // 경험치 두배 획득
            StatManager.Instance.AddPlayerEXP(data.GetExpValue * 2);
            textObj.GetComponent<TextMeshProUGUI>().text = "+" + ScoreManager.Instance.ScoreToString(data.GetExpValue * 2);

            // 아이템 사용
            --plantItem;

            // 아이템 정보 업데이트
            plantItemInfo.UpdateUI();
        }
        else
        {
            StatManager.Instance.AddPlayerEXP(data.GetExpValue);
        }

        // 식물 생산 횟수 증가
        HarvestCounting(data.PlantNumber);

        for(int i=0; i<info_ProductPlant.Count; i++)
        {
            if (info_ProductPlant[i] == ob_plant)
            {
                info_ProductPlant.RemoveAt(i);
                Destroy(ob_plant);
            }
        }
        Debug.Log("생산중인 개수" + info_ProductPlant.Count);
    }

    // 식물 일괄수확 버튼 클릭 시 호출
    // <재귀방식>
    public void OnClickAllHarvest(int index)
    {
        Debug.Log("일괄 수확 클릭");
        if (info_ProductPlant.Count > index)
        {
            Debug.Log("현재 인덱스 = " + index);
            // 생산완료 버튼이 활성화 되어있는 경우
            if(info_ProductPlant[index].GetComponent<PlantProductInfo>().harvestButton.activeSelf)
            {
                info_ProductPlant[index].GetComponent<PlantProductInfo>().OnClickHarvestButton();
                OnClickAllHarvest(index);
            }
            // 생산완료되지 않은 식물 일 경우 다음 슬롯의 식물 체크 
            else
            {
                OnClickAllHarvest(index + 1);
            }
        }
    }

    // 식물 생산 횟수 업데이트
    public void HarvestCounting(int _plantNum)
    {
        // 인덱스가 0부터 시작하기 때문에 식물 번호 -1 
        harvestCount[_plantNum-1]+=1;

        // 해당 식물을 5개 생산했을 경우 다음 식물 해금
        if(harvestCount[_plantNum - 1]==5 && _plantNum < 20)
        {
            // 다음 식물 해금
            plantButtonList[_plantNum].UnLockThisButton();
        }
    }

    // 다이아로 슬롯 해금 (Call by OnClickUnLockSlot)
    public bool UnLockPlantSlot(UnLockPayType payType, float amount)
    {
        switch(payType)
            {
                // 미네랄 소모 타입
                case UnLockPayType.Pay_Mineral:
                    {
                        // 총 슬롯 개수 > 해금된 슬롯 개수일 경우
                        if (info_PlantSlot.Length > count_UnLockedPlantSlot)
                        {
                            // 보유한 재화가 충족할 경우
                            if (StatManager.Instance.Own_Mineral >= amount)
                            { 
                                // 다이아 소모
                                StatManager.Instance.SubMineral(amount);
                                // 해금된 슬롯 개수 추가
                                ++count_UnLockedPlantSlot;
                                // 슬롯 해금
                                info_PlantSlot[count_UnLockedPlantSlot - 1] = true;
                                return true;
                            }
                        }
                        return false;
                    }
                case UnLockPayType.Pay_Dia:
                    {
                        // 총 슬롯 개수 > 해금된 슬롯 개수일 경우
                        if (info_PlantSlot.Length > count_UnLockedPlantSlot)
                        {
                            // 보유한 재화가 충족할 경우
                            if (StatManager.Instance.Own_Dia >= amount)
                            {
                                // 다이아 소모
                                StatManager.Instance.SubDia(amount);
                                // 해금된 슬롯 개수 추가
                                ++count_UnLockedPlantSlot;
                                // 슬롯 해금
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

    #region 무기관련 함수목록
    public void WeaponSlotInitialSetting(bool isFirst, GameData_Json _data)
    {
        // 로드된 정보가 있을 경우 먼저 로드
        if (!isFirst)
        {
            if(_data != null)
            {
                info_ShortWeaponUnLock = _data.bArrShortWeaponUnLockInfo;
                info_LongWeaponUnLock = _data.bArrLongWeaponUnLockInfo;

                // 무기 아이템 개수 로드
                shortWeaponItem = _data.iShortWeaponItem;
                longWeaponItem = _data.iLongWeaponItem;

                // 기존에 장착했던 무기 그대로 장착
                // 장착한 무기가 없을 경우 주사기 자동 장착
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
        // 로드된 정보가 없을 경우 초기 세팅
        else
        { 
            // 초기 세팅 (주사기만 해금)
            info_LongWeaponUnLock[(int)LWeaponType.Syringe] = true;

            // 기본 주사기 장착
            EquipWeapon(false, (int)StatManager.Instance.l_Weapontype);
        }
    }

    public void EquipWeapon(bool isShortWeapon, int weaponNum)
    {
        // 기존 무기 해제
        UnEquipWeapon(isShortWeapon, false);

        // 새 무기 등록
        if (isShortWeapon)
        {
            // 근거리 무기 등록
            StatManager.Instance.s_Weapontype = (SWeaponType)(weaponNum);
        }
        else
        {
            // 원거리 무기 등록
            StatManager.Instance.l_Weapontype = (LWeaponType)(weaponNum);
        }

        // 새 무기 장착
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

        // 플레이어 스텟 업데이트
        playerStat.GetComponent<EquipedWeaponInfo>().UpdateStat();
    }

    public void UnEquipWeapon(bool isShortWeapon, bool soundPlay)
    {
        if(isShortWeapon)   // 근거리
        {
            StatManager.Instance.s_Weapontype = SWeaponType.Idle;

            // 장착된 근거리 무기 해제
            for (int i = 1; i < equipedShortWeaponList.Length; i++)
            {
                equipedShortWeaponList[i].SetActive(false);
            }

            equipedShortWeaponButton.SetActive(false);
        }
        else  // 원거리
        {
            StatManager.Instance.l_Weapontype = LWeaponType.Idle;

            // 장착된 원거리 무기 해제
            for (int i = 1; i < equipedLongWeaponList.Length; i++)
            {
                Instance.equipedLongWeaponList[i].SetActive(false);
            }

            equipedLongWeaponButton.SetActive(false);
        }

        // 플레이어 스텟 업데이트
        playerStat.GetComponent<EquipedWeaponInfo>().UpdateStat();
    }

    #endregion

    #region 동물도감관련 함수목록
    
    public void AnimalSlotInitialSetting(GameData_Json data)
    {
        // 로드 한 데이터가 있을 경우
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
    
    // 동물이 풀려있는지 여부를 반환
    public bool GetIsReleaseAnimal(int num, bool isCashAnimal)
    {
        // 스테이지 동물
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
        // 캐시동물
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

    // 동물 풀어놓기
    // 풀어놓기 성공 여부 반환
    // 최대 배치 수 이상으로 배치하고자 할 경우 false 반환
    public bool ReleaseAnimal(int stageNum)
    {
            // 일반 동물
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
        // 캐시 동물
        actionCashAnimalList[cashNum].SetActive(true);
        releaseCashAnimalList.Add(cashNum);
    }

    // 동물 불러오기
    public void ComeAnimal(int stageNum)
    {
        actionAnimalList[stageNum - 1].SetActive(false);
        --countingReleaseAnimal;

        // 해당 동물을 찾음
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

        // 해당 동물을 찾음
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