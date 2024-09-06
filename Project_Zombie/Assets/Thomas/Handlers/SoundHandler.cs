using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    [SerializeField] SoundUnit soundTemplate;

   public void CreateSfx(AudioClip clip, Transform pos = null, float modifier = 1)
    {
        if(clip == null)
        {
            return;
        }

        if(pos != null)
        {
            SoundUnit newObject = GameHandler.instance._pool.GetSound(pos);
            newObject.SetUp(clip, true, modifier);
            newObject.transform.position = pos.position;
        }
        else
        {
            SoundUnit newObject = GameHandler.instance._pool.GetSound(transform);
            newObject.SetUp(clip, false, modifier);
        }

    }
}
