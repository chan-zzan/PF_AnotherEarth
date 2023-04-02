using UnityEngine;
using System.Collections;

public class FlameThrower_E : MonoBehaviour
{
    [SerializeField] Transform P; // �θ� ��ġ
    [SerializeField] Transform fire; // ȭ�� ũ�� ����   

    [SerializeField] float durationTime; // ��ų ���ӽð�
    [SerializeField] float coolTime; // ��ų ��Ÿ��


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
    //        // ������ ����
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
        if (level > 6) level = 6; // ���� ����(0 ~ 6)

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
    //    GameManager_E.Instance.CoolTimeStart(coolTime); // ��Ÿ�� ����

    //    yield return new WaitForSeconds(durationTime); // ��ų ��� �ð�
        
    //    this.gameObject.SetActive(false);
        
    //}
}
