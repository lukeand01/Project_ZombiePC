using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSScript : MonoBehaviour
{

    [SerializeField] ParticleSystem _particleSystem;
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
