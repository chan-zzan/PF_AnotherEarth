using UnityEngine;
using System.Collections;

public class FlameThrower_E : MonoBehaviour
{
    [SerializeField] Transform P; // 부모 위치
    [SerializeField] Transform fire; // 화염 크기 조절   

    [SerializeField] float durationTime; // 스킬 지속시간
    [SerializeField] float coolTime; // 스킬 쿨타임


    //private void Start()
    //{
    //    if (P == null)
    //    {
    //        this.transform.parent = this.transform.parent.parent;
    //    }
    //}

    //private void OnEnable()
    //{
    //    if (P == null)
    //    {
    //        // 밖으로 빼냄
    //        this.transform.parent = this.transform.parent.parent;
    //    }
    //}

    void Update()
    {
        //this.transform.position = GameManager_E.Instance.Player.transform.position;
        //this.transform.Rotate(Vector3.forward, Time.deltaTime * 360, Space.Self);

        if (P == null)
        {
            ApplyFlameThrowerLevel(StatManager.Instance.Level_Flamethrower);
        }
        else
        {
            ApplyFlameThrowerLevel(6);
        }
    }

    void ApplyFlameThrowerLevel(int level)
    {
        if (level > 6) level = 6; // 레벨 제한(0 ~ 6)

        float Level = (float)level / 2;
        fire.localScale = Vector3.one * (Level + 1);
    }

    //public void Fire()
    //{
    //    this.gameObject.SetActive(true);
    //    ParticleSystem[] particles = this.GetComponentsInChildren<ParticleSystem>();

    //    for(int i = 0; i < particles.Length; i++)
    //    {
    //        particles[i].Play();
    //    }

    //    StartCoroutine(UsingSkill());
    //}

    //IEnumerator UsingSkill()
    //{
    //    GameManager_E.Instance.CoolTimeStart(coolTime); // 쿨타임 생성

    //    yield return new WaitForSeconds(durationTime); // 스킬 사용 시간
        
    //    this.gameObject.SetActive(false);
        
    //}
}
