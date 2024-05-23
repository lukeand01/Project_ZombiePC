using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Shield : MonoBehaviour, IDamageable
{
    [SerializeField] EnemyBase enemyHoldingShield;
    [SerializeField] float howMuchPenIsRequiredToGoThrough;

    public void ApplyBD(BDClass bd)
    {
        
    }

    public string GetID()
    {
        return "";
    }

    public float GetTargetCurrentHealth()
    {
        return 0;
    }

    public float GetTargetMaxHealth()
    {
        return 0;
    }

    public bool IsDead()
    {
        return false;
    }

    public void TakeDamage(DamageClass damage)
    {
        //if it has pen enough it attacks the other target.
        
    }

    
}


//but how the shield enemy works?
//but they have very slow turning speed. and you can easilçy shoot them. can the shoot break?