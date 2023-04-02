using UnityEngine;

[System.Serializable]
public class PlayerData // ������� ���� ��ũ��Ʈ�� �������
{
    #region ����� ����
    [Header("User")]
    // �г���
    [SerializeField] private string _name = "";
    public string Name
    {
        get => _name;
    }

    // ����
    [SerializeField] private int _level = 1;
    public int Level
    {
        get => _level;
        set
        {
            // ������ �����ϴ� ���
            if (value >= 0)
            {
                _level = value;

                // �������� ���� ����(����)
                HP += 1;
                ATK += 1;
                DEF += 1;
                SPEED += 1;
            }
        }
    }

    // ����ġ
    [SerializeField] private float _exp = 0;
    public float EXP
    {
        get => _exp;
        set
        {
            _exp = value;

            while (_exp >= 100)
            {
                // ����ġ ���� -> ������
                Level += 1;
                _exp -= 100;
            }

            DataManager_E.Instance.SaveData(); // ���� ����
        }
    }

    // ���ּ� ����
    [SerializeField] private int _spaceShipLevel = 1;
    public int SpaceShipLevel
    {
        get => _spaceShipLevel;
        set
        {
            // ���ּ� ������ �����ϴ� ���
            if (value >= 0)
            {
                _spaceShipLevel = value;

                // �ʴ� �̳׶� ȹ�淮 ����
                //

                // �������� �ر�
                //
            }

            DataManager_E.Instance.SaveData(); // ���� ����
        }
    }
    #endregion

    [Space(30)]

    #region ��ȭ ����
    [Header("Money")]
    // ���� �̳׶�
    [SerializeField] private int _mineral = 0;
    public int Mineral
    {
        get => _mineral;
        set => _mineral = value;
    }

    // ���� ���̾�
    [SerializeField] private int _diamond = 0;
    public int Diamond
    {
        get => _diamond;
        set => _diamond = value;
    }

    // ���� ������
    [SerializeField] private int _energy = 0;
    public int Energy
    {
        get => _energy;
        set => _energy = value;
    }

    // ȯ������
    [SerializeField] private int _score = 0;
    public int Score
    {
        get => _score;
        set => _score = value;
    }
    #endregion

    [Space(30)]

    #region ���ݰ���
    [Header("Status")]
    // ü��
    [SerializeField] private float _hp = 100;
    public float HP
    {
        get => _hp;
        set => _hp = value;
    }

    // ���ݷ�
    [SerializeField] private float _atk = 10;
    public float ATK
    {
        get => _atk;
        set => _atk = value;
    }

    // ����
    [SerializeField] private float _def = 10;
    public float DEF
    {
        get => _def;
        set => _def = value;
    }

    // �̵��ӵ�
    [SerializeField] private float _speed = 15;
    public float SPEED
    {
        get => _speed;
        set => _speed = value;
    }
    #endregion

    [Space(30)]

    #region ���� ����
    [Header("Weapon")]
    // ���� ���⺰ ����
    public WeaponData weaponDatas;

    #endregion
}

[System.Serializable]
public struct WeaponData
{
    public LWeaponType LWeaponType; // ���Ÿ����� Ÿ��
    public float L_STR; // ����ü ���ݷ�
    public float L_DEX; // ����ü �߻�ӵ�(���� �ӵ�)
    public float L_SPEED; // ����ü ���ǵ�(���ư��� �ӵ�)


    public SWeaponType SWeaponType; // �������� Ÿ��
    public int S_LEVEL; // �������� ���� -> ������ ���� �ִϸ��̼��� �޶���
    public float S_STR; // �������� ���ݷ�
    public float S_SPEED; // �������� ���ǵ�    
}