using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    [SerializeField] SoundUnit soundTemplate_Spatial;
    [SerializeField] SoundUnit soundTemplate_global;
   public void CreateSfx(AudioClip clip, Transform pos = null)
    {
        if(clip == null)
        {
            return;
        }

        if(pos != null)
        {
            SoundUnit newObject = Instantiate(soundTemplate_Spatial);
            newObject.SetUp(clip);
            newObject.transform.position = pos.position;
        }
        else
        {
            SoundUnit newObject = Instantiate(soundTemplate_global);
            newObject.SetUp(clip);
        }

    }
}
