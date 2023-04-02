using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager_E : MonoBehaviour
{
    #region 싱글톤
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
    public AudioSource bgmSource; // 배경음 출력 source
    public AudioSource effectSource; // 효과음 출력 source
    public AudioSource effectSource2; // 효과음 출력 source2
    public AudioSource effectSource3; // 효과음 출력 source3

    [Space(10)]

    [Header("BGM Clips")]
    public List<AudioClip> bgmClips; // 배경음 리스트
    public List<AudioClip> bgmClips_Boss; // 보스 배경음 리스트

    [Space(10)]

    [Header("Clips")]
    public List<AudioClip> effectClips; // 효과음 리스트
    public List<AudioClip> effectClips2; // 효과음 리스트
    public List<AudioClip> monsterEffectClips; // 몬스터 효과음 리스트

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
        // 겹치는 효과음 방지
        effectSource2.clip = effectClips2[clipNum];
        effectSource2.Play();
    }

    public void MonsterEffectSoundPlay(int clipNum)
    {
        // 몬스터 효과음 재생
        effectSource3.clip = monsterEffectClips[clipNum];
        effectSource3.Play();
    }

    public void SelectBGM(int clipNum)
    {
        // 스테이지 별 다른 배경음악 재생
        bgmSource.clip = bgmClips[clipNum];
        bgmSource.Play();
    }
    
    public void ChangeBGM(int clipNum)
    {
        // 보스 등장시 배경음악 빠르게 재생
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
