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

    public abstract void ResetForPool();
    


    public abstract void CallTrap();

}

public enum TrapType
{
    BearTrap
}

//trap 