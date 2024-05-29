using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundUnit : MonoBehaviour
{
    [SerializeField] AudioSource _source;
    float timeForDestruction;

    public void SetUp(AudioClip clip)
    {
        timeForDestruction = clip.length + 0.1f;
        _source.clip = clip;
        _source.Play();

        Invoke(nameof(DestroyThis), timeForDestruction);
    }

    void DestroyThis()
    {
        Destroy(gameObject);
    }
}
