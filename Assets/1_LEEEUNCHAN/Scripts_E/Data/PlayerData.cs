using UnityEngine;

[System.Serializable]
public class PlayerData // 길어져서 따로 스크립트를 만들었음
{
    #region 사용자 정보
    [Header("User")]
    // 닉네임
    [SerializeField] private string _name = "";
    public string Name
    {
        get => _name;
    }

    // 레벨
    [SerializeField] private int _level = 1;
    public int Level
    {
        get => _level;
        set
        {
            // 레벨이 증가하는 경우
            if (value >= 0)
            {
                _level = value;

                // 레벨업시 스텟 증가(예시)
                HP += 1;
                ATK += 1;
                DEF += 1;
                SPEED += 1;
            }
        }
    }

    // 경험치
    [SerializeField] private float _exp = 0;
    public float EXP
    {
        get => _exp;
        set
        {
            _exp = value;

            while (_exp >= 100)
            {
                // 경험치 꽉참 -> 레벨업
                Level += 1;
                _exp -= 100;
            }

            DataManager_E.Instance.SaveData(); // 스탯 적용
        }
    }

    // 우주선 레벨
    [SerializeField] private int _spaceShipLevel = 1;
    public int SpaceShipLevel
    {
        get => _spaceShipLevel;
        set
        {
            // 우주선 레벨이 증가하는 경우
            if (value >= 0)
            {
                _spaceShipLevel = value;

                // 초당 미네랄 획득량 증가
                //

                // 스테이지 해금
                //
            }

            DataManager_E.Instance.SaveData(); // 스탯 적용
        }
    }
    #endregion

    [Space(30)]

    #region 재화 관련
    [Header("Money")]
    // 보유 미네랄
    [SerializeField] private int _mineral = 0;
    public int Mineral
    {
        get => _mineral;
        set => _mineral = value;
    }

    // 보유 다이아
    [SerializeField] private int _diamond = 0;
    public int Diamond
    {
        get => _diamond;
        set => _diamond = value;
    }

    // 보유 에너지
    [SerializeField] private int _energy = 0;
    public int Energy
    {
        get => _energy;
        set => _energy = value;
    }

    // 환경점수
    [SerializeField] private int _score = 0;
    public int Score
    {
        get => _score;
        set => _score = value;
    }
    #endregion

    [Space(30)]

    #region 스텟관련
    [Header("Status")]
    // 체력
    [SerializeField] private float _hp = 100;
    public float HP
    {
        get => _hp;
        set => _hp = value;
    }

    // 공격력
    [SerializeField] private float _atk = 10;
    public float ATK
    {
        get => _atk;
        set => _atk = value;
    }

    // 방어력
    [SerializeField] private float _def = 10;
    public float DEF
    {
        get => _def;
        set => _def = value;
    }

    // 이동속도
    [SerializeField] private float _speed = 15;
    public float SPEED
    {
        get => _speed;
        set => _speed = value;
    }
    #endregion

    [Space(30)]

    #region 무기 관련
    [Header("Weapon")]
    // 보유 무기별 정보
    public WeaponData weaponDatas;

    #endregion
}

[System.Serializable]
public struct WeaponData
{
    public LWeaponType LWeaponType; // 원거리무기 타입
    public float L_STR; // 투사체 공격력
    public float L_DEX; // 투사체 발사속도(연사 속도)
    public float L_SPEED; // 투사체 스피드(날아가는 속도)


    public SWeaponType SWeaponType; // 근접무기 타입
    public int S_LEVEL; // 근접무기 레벨 -> 레벨에 따라 애니메이션이 달라짐
    public float S_STR; // 근접무기 공격력
    public float S_SPEED; // 근접무기 스피드    
}