using TMPro;
using UnityEngine;
public class SpaceShipManager : MonoBehaviour
{
    public TextMeshProUGUI levelText;   // 우주선 레벨 텍스트
    public TextMeshProUGUI cashPSText;  // 초당 미네랄 획득량 텍스트

    // 우주선 스텟 관련
    private int currentLevel;       // 현재 우주선 레벨
    private int getCashPerSeconds;  // 현재 초당 미네랄 획득량
    private int levelUpCash;        // 레벨업 시 필요한 재화


    // 싱글톤
    private static SpaceShipManager instance;
    public static SpaceShipManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<SpaceShipManager>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<SpaceShipManager>();
                    instance = newObj;
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        var objs = FindObjectsOfType<SpaceShipManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }

        // -----초기 스텟 저장 (제이슨 연동)
        // 레벨
        currentLevel = 1;
        levelText.text = currentLevel.ToString();
        // 초당 미네랄 획득
        getCashPerSeconds = 5;
        cashPSText.text = getCashPerSeconds.ToString();
    }

    // 우주선 레벨 관련 함수
    public void AddLevel()
    {
        currentLevel += 1;
        levelText.text = currentLevel.ToString();
        // 레벨 관련 스텟
        UpdateCashPerSeconds();
    }

    public void SetLevel(int value)
    {
        currentLevel = value;
        levelText.text = currentLevel.ToString();
        // 레벨 관련 스텟
        UpdateCashPerSeconds();
    }
    public int GetLevel()
    {
        return currentLevel;
    }

    // 초당 미네랄 관련 함수
    public void UpdateCashPerSeconds()
    {
        getCashPerSeconds = currentLevel * 5;
        cashPSText.text = getCashPerSeconds.ToString();
    }

    public int GetCashPerSeconds()
    {
        return getCashPerSeconds;
    }


}
