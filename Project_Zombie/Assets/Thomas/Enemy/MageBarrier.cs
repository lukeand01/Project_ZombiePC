using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageBarrier : MonoBehaviour, IDamageable
{

    float health_Current;
    float health_Total;
    [SerializeField] EnemyData _enemyData;
    string id;

    private void Awake()
    {
        id = MyUtils.GetRandomID();
        ResetMageBarrier();
    }

    public void ResetMageBarrier()
    {
        health_Current = health_Total;
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
        return health_Current;
    }

    public float GetTargetMaxHealth()
    {
        return health_Total;
    }

    public bool IsDead()
    {
        return false;
    }

    public void RestoreHealth(float value)
    {
        
    }

    public void TakeDamage(DamageClass damage)
    {

        DamageClass _damage = new DamageClass(damage);
        _damage.UpdateDamageList_Enemy(_enemyData);
        float totalDamage = _damage.GetTotalDamage();

        health_Current -= totalDamage;

        if(health_Current <= 0)
        {
            DestroyBarrier();
        }

    }


    void DestroyBarrier()
    {
        //spawn a ps that shows the barrier destruction
        //sound
        //and make it invisible


    }
}
