using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SoundRefData : ScriptableObject
{
    [SerializeField] List<SoundClass> soundList = new();
    Dictionary<SoundType, AudioClip> soundDictionary = new();


    private void OnEnable()
    {
        soundDictionary.Clear();

        for (int i = 0; i < soundList.Count; i++)
        {
            var item = soundList[i];
            soundDictionary.Add(item._soundType, item._soundClip);
        }

    }

    public AudioClip GetAudioClip(SoundType _soundType)
    {
        if (!soundDictionary.ContainsKey(_soundType))
        {
            Debug.LogError("No Sound ref for this " + _soundType);
            return null;
        }

        return soundDictionary[_soundType];
    }
}
[System.Serializable]
public class SoundClass
{
    [SerializeField] string title; //this is just for help with the editor
    [field: SerializeField] public SoundType _soundType { get; private set; }
    [field: SerializeField] public AudioClip _soundClip { get; private set; }

}

public enum SoundType
{
    AudioClip_ThunderTeleporter = 0,
    AudioClip_BlessFailure = 1,
    AudioClip_BlessSuccess = 2,
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

}