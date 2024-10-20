using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TrapBase : MonoBehaviour
{

    protected bool alreadyCalled;

    private void OnTriggerEnter(Collider other)
    {
        if (alreadyCalled) return;
        if (other.tag != "Player") return;

        CallTrap();

    }

    float _cooldown_Total;
    float _cooldown_Current;

    private void Update()
    {
        if (_cooldown_Total == 0) return;

        if(_cooldown_Total > _cooldown_Current)
        {
            _cooldown_Current += Time.deltaTime;
        }
        else
        {
            ReleaseTrap();
        }
    }

    protected abstract void ReleaseTrap();

    public void SetDestroy(float duration)
    {
        _cooldown_Total = duration;
    }

    public abstract void ResetForPool();
    


    public abstract void CallTrap();


}

public enum TrapType
{
    BearTrap
}

//trap 