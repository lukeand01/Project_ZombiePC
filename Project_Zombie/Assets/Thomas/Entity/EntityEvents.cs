using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityEvents : MonoBehaviour
{
    //playerhandler will also 
    public Action<StatType, float> eventUpdateStat;
    public void OnUpdateStat(StatType type, float value) => eventUpdateStat?.Invoke(type, value);

    public Action<IDamageable, DamageClass> eventDamagedEntity;
    public void OnDamagedEntity(IDamageable damageable, DamageClass damageClassBeingUsed) => eventDamagedEntity?.Invoke(damageable, damageClassBeingUsed);

    public Action eventDamageTaken;
    public void OnDamageTaken() => eventDamageTaken?.Invoke();

    public Action eventHealed;
    public void OnHealed() => eventHealed?.Invoke();

    public Action eventHardInput; //movement, dash, shooting
    public void OnHardInput()
    {
        eventHardInput?.Invoke();
    }

    public Action eventOpenChest; //movement, dash, shooting
    public void OnOpenChest()
    {
        eventOpenChest?.Invoke();
    }

    public Action eventHasDodged; 
    public void OnHasDodged()
    {
        eventHasDodged?.Invoke();
    }


    public Action<EnemyBase> eventKilledEnemy; 
    public void OnKillEnemy(EnemyBase enemy)
    {
        eventKilledEnemy?.Invoke(enemy);
    }
    

    public Action eventCrit; 
    public void OnCrit()
    {
        eventCrit?.Invoke();
    }

    private void OnDestroy()
    {
        eventUpdateStat = delegate { }; 
    }


}
