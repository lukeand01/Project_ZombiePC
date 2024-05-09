using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable 
{

    public void TakeDamage(DamageClass damage);

    public void ApplyBD(BDClass bd);

    public string GetID();
    public bool IsDead();

    public float GetTargetMaxHealth();
}
