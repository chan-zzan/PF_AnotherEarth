using UnityEngine;

[System.Serializable]
public struct Phase
{
    public int phase; // 단계
    public float spawnTime; // 스폰시간
    public GameObject spawnMonster; // 스폰몬스터

    public float HP; // 단계별 생성 몬스터의 체력
    public float ATK; // 단계별 생성 몬스터의 공격력
    public int monsterNum; // 단계별 생성 몬스터 갯수

}

[System.Serializable]
public struct ClearItem
{
    public int mineral; // 미네랄 갯수

    [Space(30)]

    public Sprite[] spaceMaterials; // 우주선 강화재료 이미지
    public int[] spaceMaterialsNum; // 우주선 강화재료 갯수

}

[CreateAssetMenu(fileName = "StageData", menuName = "StageData", order = int.MinValue + 1)] // 스크립터블 오브젝트를 만들 수 있는 경로 생성
public class StageData_E : ScriptableObject
{
    [SerializeField]
    private Phase[] phases; // 스테이지 별 페이즈에 담길 내용들(몬스터 스폰 주기, 몬스터 종류)

    [SerializeField]
    private ClearItem[] items; // 클리어시 얻는 아이템들 목록


    public Phase[] Phases
    {
        get => phases;
    }

    public ClearItem[] Items
    {
        get => items;
    }
}
