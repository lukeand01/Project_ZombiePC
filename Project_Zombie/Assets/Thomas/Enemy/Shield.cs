using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Shield : MonoBehaviour, IDamageable
{
    [SerializeField] EnemyBase enemyHoldingShield;
    [SerializeField] float howMuchPenIsRequiredToGoThrough;
    string id;

    private void Awake()
    {
        id = Guid.NewGuid().ToString();
    }
    private void Start()
    {
        enemyHoldingShield.ControlEnemyImmunityToExplosion(true);
    }

    public void ApplyBD(BDClass bd)
    {
        
    }

    public string GetID()
    {
        return id;
    }

    public GameObject GetObjectRef()
    {
        return gameObject;
    }

    public float GetTargetCurrentHealth()
    {
        return 0;
    }

    public float GetTargetMaxHealth()
    {
        return -5;
    }

    public bool IsDead()
    {
        return false;
    }

    public void RestoreHealth(float value)
    {
        //
    }

    public void TakeDamage(DamageClass damage)
    {
        //if it has pen enough it attacks the other target.
        //it cnanot be damaged by explosionm damage.
        //the problem is that it may reach

        if(damage.pen > howMuchPenIsRequiredToGoThrough)
        {
            enemyHoldingShield.TakeDamage(damage);
            return;
        }

        if (damage.isExplosion)
        {
            return;
        }

        enemyHoldingShield.CallShieldPopUp();
    }

    
}


//but how the shield enemy works?
//but they have very slow turning speed. and you can easilçy shoot them. can the shoot break?