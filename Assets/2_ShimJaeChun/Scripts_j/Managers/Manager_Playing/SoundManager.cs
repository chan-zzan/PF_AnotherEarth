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

    [Header("����ȭ�� ����� ����Ʈ")]
    [Header("0:����ȭ��")]
    public AudioClip[] mainSoundList;

    [Header("ȿ���� ����Ʈ")]
    [Header(" 0:�Ĺ���ư 1:�Ĺ������ư 2:����Ϸ��ư\n 3:�����ư 4:����������ư 5:��������������ư 6:���ⷹ������ư\n 7:�˾���ư 8:ȭ����ȯ 9:����ƮŬ���� 10:������")]
    public AudioClip[] effectSoundList;

    [Space(10)]

    [Header("����� ��� �ҽ�")]
    public AudioSource bgmAudioSource;

    [Header("ȿ���� ��� �ҽ�")]
    public AudioSource effectSoundSource;

    private int curPlayBGM = 0;

    private void Start()
    {
        /// ������ �߰� -> �������� �Ŵ����� �����ص� ���� ���� ������
        bgmAudioSource.volume = StageManager.Instance.bgmSoundVolume;
        effectSoundSource.volume = StageManager.Instance.effectSoundVolume;

        // ���� �� ����� ���
        PlayBackGroundSound(MainSoundType.MainStageSound);
    }


    public void PlayBackGroundSound(MainSoundType myType)
    {
        // ����� Ŭ�� �Ҵ�
        bgmAudioSource.clip = mainSoundList[(int)myType];

        // ����� ���
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
        // ����� Ŭ�� �Ҵ�
        effectSoundSource.clip = effectSoundList[(int)myType];

        // ����� ���
        effectSoundSource.Play();
    }


    // �˾� â ���� ����
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
