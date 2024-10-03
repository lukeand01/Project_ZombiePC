using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSScript : MonoBehaviour
{

    //this can also have a psanimation
    //

    [SerializeField] ParticleSystem _particleSystem;
    [SerializeField] PSAnimationObject _psAnimation;
    [SerializeField] PSType _type;
    public void ResetForPool()
    {
        _particleSystem.Clear();
        _particleSystem.Stop();
        gameObject.SetActive(false);
    }

    public void StartPS()
    {
        _particleSystem.Play();

    }

    private void Update()
    {
        if (!_particleSystem.isPlaying)
        {
            GameHandler.instance._pool.PS_Release(_type, this);
        }

    }

}
