using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SettingsData : ScriptableObject
{
    //this data here will control all the stuff. it will also be used for showing all the stuff. 
    //even for. but i dont have the desire of settings stuff here through the editor. not for the keycode at least.

    //we will get this information to decide on music. maybe we should set events as well to trigger in case on


    [Separator("KEYCODE")]
    public List<KeyClass_Individual_ForShow> keyList = new();

    [Separator("AUDIO")]
    [SerializeField][Range(0, 100)] float audio_Master = 100;
    [SerializeField][Range(0, 100)] float audio_SFX = 100;
    [SerializeField][Range(0, 100)] float audio_Music = 100;

    //

    public void Initialize()
    {
        //once we are done initalizing we send infomration to the settings fellas. so they must subscribe to an event.
        //here we will first ask if there is saved data

        bool isSaveData = false;

        if (isSaveData)
        {

        }
        else
        {
            //in here we set all original values.
            Initalize_Audio();
            Initalize_KeyBinding();

        }
    }

    //

    #region KEY BINDING
    void Initalize_KeyBinding()
    {
        //if there is no savedata we will 

        //i should ge the info here 
        //what i will do instead is to create a keyclass here and then pass to the 
        KeyClass key =  new KeyClass();
        keyList = key.GetListForSettings();

       


    }

    public void ChangeKey(KeyType _keyType, KeyCode _keyCode)
    {

        foreach (var item in keyList)
        {
            if(item.key_Id == _keyType)
            {

            }
        }


        if(PlayerHandler.instance != null)
        {
            PlayerHandler.instance._playerController.key.ChangeKey(_keyType, _keyCode);
        }

    }

    public void SetKeycodeBackToDefault()
    {
        //we can just initalize a new and get the list.
        KeyClass key = new KeyClass();
        keyList = key.GetListForSettings();
    }

    #endregion

    #region AUDIO

    public Action event_Audio_Update;
    public void On_Audio_Update() => event_Audio_Update?.Invoke(); //we tell them to look at the settings with this.

    void Initalize_Audio()
    {
        audio_Master = 100;
        audio_Music = 100;
        audio_SFX = 100;
    }

    public float Get_Audio(Setting_AudioType audioType)
    {
        if (audioType == Setting_AudioType.Master)
        {
            return audio_Master;
        }
        if (audioType == Setting_AudioType.Sfx)
        {
            return audio_SFX;
        }
        if (audioType == Setting_AudioType.BackgroundMusic)
        {
            return audio_Music;
        }

        return 0;
    }

    public void Set_Audio(Setting_AudioType audioType, float value)
    {
        if (audioType == Setting_AudioType.Master)
        {
            audio_Master = value;
        }
        if (audioType == Setting_AudioType.Sfx)
        {
            audio_SFX = value;
        }
        if (audioType == Setting_AudioType.BackgroundMusic)
        {
            audio_Music = value;
        }

        On_Audio_Update();
    }

    #endregion
}
