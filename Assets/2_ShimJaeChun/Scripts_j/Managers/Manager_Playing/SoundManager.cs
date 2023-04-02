using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MainSoundType
{
    MainStageSound = 0,
    CutSceneSound
}

public enum EffectSoundType
{
    PlantButtonSound = 0,
    CreatePlantSound,
    HarvestPlantSound,
    WeaponButtonSound,
    WeaponEquipSound,
    WeaponUnEquipSound,
    WeaponLevelUpSound,
    PopUpButtonSound,
    ScreenChangeSound,
    QuestClearSound,
    PlayerLevelUpSound
}

public class SoundManager : MonoBehaviour
{
    #region SigleTon
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<SoundManager>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<SoundManager>();
                    instance = newObj;
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        var objs = FindObjectsOfType<SoundManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion

    [Header("메인화면 배경음 리스트")]
    [Header("0:메인화면")]
    public AudioClip[] mainSoundList;

    [Header("효과음 리스트")]
    [Header(" 0:식물버튼 1:식물생산버튼 2:생산완료버튼\n 3:무기버튼 4:무기장착버튼 5:무기장착해제버튼 6:무기레벨업버튼\n 7:팝업버튼 8:화면전환 9:퀘스트클리어 10:레벨업")]
    public AudioClip[] effectSoundList;

    [Space(10)]

    [Header("배경음 출력 소스")]
    public AudioSource bgmAudioSource;

    [Header("효과음 출력 소스")]
    public AudioSource effectSoundSource;

    private int curPlayBGM = 0;

    private void Start()
    {
        /// 이은찬 추가 -> 스테이지 매니저에 저장해둔 사운드 값을 가져옴
        bgmAudioSource.volume = StageManager.Instance.bgmSoundVolume;
        effectSoundSource.volume = StageManager.Instance.effectSoundVolume;

        // 시작 시 배경음 출력
        PlayBackGroundSound(MainSoundType.MainStageSound);
    }


    public void PlayBackGroundSound(MainSoundType myType)
    {
        // 오디오 클립 할당
        bgmAudioSource.clip = mainSoundList[(int)myType];

        // 오디오 재생
        bgmAudioSource.Play();
    }

    public void StopBGM()
    {
        bgmAudioSource.Stop();
    }

    public void ChangeBackGroundSound()
    {
        if(curPlayBGM < mainSoundList.Length)
        {
            curPlayBGM++;

            PlayBackGroundSound((MainSoundType)curPlayBGM);
        }
        else
        {
            curPlayBGM = 0;

            PlayBackGroundSound((MainSoundType)curPlayBGM);
        }
    }


    public void PlayEffectSound(EffectSoundType myType)
    {
        // 오디오 클립 할당
        effectSoundSource.clip = effectSoundList[(int)myType];

        // 오디오 재생
        effectSoundSource.Play();
    }


    // 팝업 창 사운드 관련
    public void PlayOpenSound()
    {
        effectSoundSource.clip = effectSoundList[(int)EffectSoundType.PopUpButtonSound];

        effectSoundSource.Play();
    }

    public void PlayCloseSound()
    {
        effectSoundSource.clip = effectSoundList[(int)EffectSoundType.WeaponUnEquipSound];

        effectSoundSource.Play();
    }
}
