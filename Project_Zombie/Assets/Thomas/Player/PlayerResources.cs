using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour, IDamageable
{

    string id;
    bool isDead;
    private void Awake()
    {
        id  =Guid.NewGuid().ToString();
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
        

    }

    public void ApplyBD(BDClass bd)
    {
        
    }
}
