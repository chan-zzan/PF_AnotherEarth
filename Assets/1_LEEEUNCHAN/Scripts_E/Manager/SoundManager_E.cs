using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager_E : MonoBehaviour
{
    #region �̱���
    private static SoundManager_E instance;
    public static SoundManager_E Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<SoundManager_E>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<SoundManager_E>();
                    instance = newObj;
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        instance = this;
    }
    #endregion

    [Header("Sources")] 
    public AudioSource bgmSource; // ����� ��� source
    public AudioSource effectSource; // ȿ���� ��� source
    public AudioSource effectSource2; // ȿ���� ��� source2
    public AudioSource effectSource3; // ȿ���� ��� source3

    [Space(10)]

    [Header("BGM Clips")]
    public List<AudioClip> bgmClips; // ����� ����Ʈ
    public List<AudioClip> bgmClips_Boss; // ���� ����� ����Ʈ

    [Space(10)]

    [Header("Clips")]
    public List<AudioClip> effectClips; // ȿ���� ����Ʈ
    public List<AudioClip> effectClips2; // ȿ���� ����Ʈ
    public List<AudioClip> monsterEffectClips; // ���� ȿ���� ����Ʈ

    private void Start()
    {
        bgmSource.Play();

        bgmSource.volume = StageManager.Instance.bgmSoundVolume;

        float curEffectSoundVolume = StageManager.Instance.effectSoundVolume < 0.5f ? StageManager.Instance.effectSoundVolume : 0.5f;

        effectSource.volume = curEffectSoundVolume;
        effectSource2.volume = curEffectSoundVolume;
        effectSource3.volume = curEffectSoundVolume >= 0 ? curEffectSoundVolume + 0.5f : 0.0f;
    }

    public void EffectSoundPlay(int clipNum)
    {
        effectSource.clip = effectClips[clipNum];
        effectSource.Play();
    }

    public void EffectSoundPlay2(int clipNum)
    {
        // ��ġ�� ȿ���� ����
        effectSource2.clip = effectClips2[clipNum];
        effectSource2.Play();
    }

    public void MonsterEffectSoundPlay(int clipNum)
    {
        // ���� ȿ���� ���
        effectSource3.clip = monsterEffectClips[clipNum];
        effectSource3.Play();
    }

    public void SelectBGM(int clipNum)
    {
        // �������� �� �ٸ� ������� ���
        bgmSource.clip = bgmClips[clipNum];
        bgmSource.Play();
    }
    
    public void ChangeBGM(int clipNum)
    {
        // ���� ����� ������� ������ ���
        bgmSource.clip = bgmClips_Boss[clipNum];
        bgmSource.Play();
    }
    
    public void AllSoundOff()
    {
        bgmSource.Stop();
        effectSource.Stop();
        effectSource2.Stop();
        effectSource3.Stop();

        this.gameObject.SetActive(false);
    }

}
