using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SoundRefData : ScriptableObject
{
    [SerializeField] List<SoundClass> soundList_Clip = new();
    Dictionary<SoundType, AudioClip> soundDictionary_Clip = new();

    [Separator("BGM")]
    [SerializeField] List<SoundClass_Bgm> soundList_Bgm = new();
    Dictionary<BGMType, AudioClip> soundDictionary_Bgm = new();

    private void OnEnable()
    {
        soundDictionary_Clip.Clear();

        for (int i = 0; i < soundList_Clip.Count; i++)
        {
            var item = soundList_Clip[i];
            soundDictionary_Clip.Add(item._soundType, item._soundClip);
        }

        soundDictionary_Bgm.Clear();
        for (int i = 0; i < soundList_Bgm.Count; i++)
        {
            var item = soundList_Bgm[i];
            soundDictionary_Bgm.Add(item._soundType, item._soundClip);
        }

    }

    public AudioClip GetAudioClip(SoundType _soundType)
    {
        if (!soundDictionary_Clip.ContainsKey(_soundType))
        {
            Debug.LogError("No Sound ref for this " + _soundType);
            return null;
        }

        return soundDictionary_Clip[_soundType];
    }

    public AudioClip GetAudioBgm(BGMType _bgmType)
    {
        if (!soundDictionary_Bgm.ContainsKey(_bgmType))
        {
            Debug.LogError("No Sound ref for this " + _bgmType);
            return null;
        }

        return soundDictionary_Bgm[_bgmType];
    }
}
[System.Serializable]
public class SoundClass
{
    [SerializeField] string title; //this is just for help with the editor
    [field: SerializeField] public SoundType _soundType { get; private set; }
    [field: SerializeField] public AudioClip _soundClip { get; private set; }

}

[System.Serializable]
public class SoundClass_Bgm
{
    [SerializeField] string title; //this is just for help with the editor
    [field: SerializeField] public BGMType _soundType { get; private set; }
    [field: SerializeField] public AudioClip _soundClip { get; private set; }

}


public enum SoundType
{
    AudioClip_ThunderTeleporter = 0,
    AudioClip_Failure = 1, //general failure.
    AudioClip_UseBless = 2,
    AudioClip_Scrap = 3,
    AudioClip_ButtonClick = 4,
    AudioClip_ButtonHover = 5,
    AudioClip_ShrineBuff = 6,
    AudioClip_ShrineHeal = 7,
    AudioClip_OpenChestResource = 8,
    AudioClip_OpenChestAbility = 9,
    AudioClip_OpenChestGun = 10,
    AudioClip_GateOpen = 11,
    AudioClip_GateClose = 12,
    AudioClip_SpendMoney = 13,
    AudioClip_BearTrap = 14,
    AudioClip_PlayerStarDialogue = 15,
    AudioClip_GainBless = 16,
    AudioClip_DialogueLetter = 17,
    AudioClip_ResponseChoice = 18,
    AudioClip_OutOfTimeTimer = 19,

}


public enum BGMType 
{ 
    Merchant,
}
