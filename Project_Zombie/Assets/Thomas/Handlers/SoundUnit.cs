using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundUnit : MonoBehaviour
{
    [SerializeField] AudioSource _source;
    float timeForDestruction_Current;
    float timeForDestruction_Total;
    public void SetUp(AudioClip clip, bool isSpatial)
    {

        if(isSpatial)
        {
            _source.spatialBlend = 1;
        }
        else
        {
            _source.spatialBlend = 0;
        }

        timeForDestruction_Total = clip.length + 0.1f;
        timeForDestruction_Current = timeForDestruction_Total;
        _source.clip = clip;
        _source.Play();

        _source.volume = 0.05f;

    }

    private void FixedUpdate()
    {
        if(timeForDestruction_Current > 0)
        {
            timeForDestruction_Current -= Time.fixedDeltaTime;

        }
        else if(!_source.isPlaying)
        {
            GameHandler.instance._pool.Sound_Release(this);
        }
    }

    public void ReturnToPool()
    {
        _source.Stop();
        gameObject.SetActive(false);
    }


}
