using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    [SerializeField] SoundUnit soundTemplate;
    [SerializeField] SoundRefData _soundRefData;

    AudioClip _letterAudioClip;
    private void Start()
    {
        _letterAudioClip = _soundRefData.GetAudioClip(SoundType.AudioClip_DialogueLetter);
    }

    public void CreateSfx(SoundType _soundType, Transform pos = null, float modifier = 1)
    {

        AudioClip _clip = _soundRefData.GetAudioClip(_soundType);

        CreateSfx_WithAudioClip(_clip, pos, modifier);

    }

    public void CreateSfx_WithAudioClip(AudioClip _clip, Transform pos = null, float modifier = 1)
    {

        if (_clip == null)
        {
            return;
        }

        if (pos != null)
        {
            SoundUnit newObject = GameHandler.instance._pool.GetSound(pos);
            newObject.SetUp(_clip, true, modifier);
            newObject.transform.position = pos.position;
        }
        else
        {
            SoundUnit newObject = GameHandler.instance._pool.GetSound(transform);
            newObject.SetUp(_clip, false, modifier);
        }
    }

    public void CreateSFX_DialogueLetter()
    {
        CreateSfx_WithAudioClip(_letterAudioClip);
    }


    #region BGM

    [Separator("BGM")]
    [SerializeField] AudioSource _bgmSource;


    //what i should do?
    //


    public void StartBackgroundMusic(AudioClip clip)
    {
        //its always gradual
        StartCoroutine(FullTransition(clip));
      
    }
    public void StopBackgroundMusic()
    {
        //i want it to be gradual.

        StartCoroutine(BGMTransitionProcess_LowerIt());
    }


    IEnumerator FullTransition(AudioClip clip)
    {
        yield return StartCoroutine(BGMTransitionProcess_LowerIt());

        //cahnge the clip.
        _bgmSource.clip = clip;
        _bgmSource.Play();

        yield return StartCoroutine(BGMTransitionProcess_RaiseIt());
    }

    IEnumerator BGMTransitionProcess_RaiseIt()
    {
        float bgmVolume = GameHandler.instance._settingsData.Get_Audio(Setting_AudioType.BackgroundMusic);
        float masterVolume = GameHandler.instance._settingsData.Get_Audio(Setting_AudioType.Master);
        float actualVolume = bgmVolume * masterVolume;

        while(_bgmSource.volume < actualVolume)
        {
            _bgmSource.volume += Time.deltaTime;
            yield return new WaitForSeconds(0.1f);
        }


    }

    IEnumerator BGMTransitionProcess_LowerIt()
    {
        //we get the sound 
       
        while(_bgmSource.volume > 0)
        {
            _bgmSource.volume -= Time.deltaTime;
            yield return new WaitForSeconds(0.1f);
        }
        
    }

    #endregion


}
