using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IDamageable
{
    private void Awake()
    {
        id = Guid.NewGuid().ToString();
    }

    #region  DAMAGEABLE
    string id;
    bool isDead;

    public void ApplyBD(BDClass bd)
    {
        
    }

    public string GetID()
    {
        return id;
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void TakeDamage(DamageClass damage)
    {
        Debug.Log("took damage");
    }
    #endregion
}
