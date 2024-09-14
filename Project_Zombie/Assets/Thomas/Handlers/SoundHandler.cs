using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    [SerializeField] SoundUnit soundTemplate;
    [SerializeField] SoundRefData _soundRefData;

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

}
