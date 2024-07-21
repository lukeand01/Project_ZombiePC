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
    public void OnHardInput() => eventHardInput?.Invoke();



    public Action<ChestType> eventOpenChest; //movement, dash, shooting
    public void OnOpenChest(ChestType _chest) => eventOpenChest?.Invoke(_chest);

    public Action eventHasDodged; 
    public void OnHasDodged()
    {
        eventHasDodged?.Invoke();
    }


    public Action<EnemyBase, bool> eventKilledEnemy; 
    public void OnKillEnemy(EnemyBase enemy, bool wasPlayer)
    {
        eventKilledEnemy?.Invoke(enemy, wasPlayer);
    }
    

    public Action eventCrit; 
    public void OnCrit()
    {
        eventCrit?.Invoke();
    }

    public Action<ItemGunData> eventReloadedGun;
    public void OnReloadedGun(ItemGunData data)
    {
        eventReloadedGun?.Invoke(data);
    }

    public Action eventMinedResource;
    public void OnMinedResource()
    {
        eventMinedResource?.Invoke();
    }

    public Action<int> eventChangedPoints;
    public void OnChangedPoints(int value) => eventChangedPoints?.Invoke(value);



    private void OnDestroy()
    {
        eventUpdateStat = delegate { }; 
    }


}
public enum ChestType
{
    ChestResource,
    ChestAbility,
    ChestGun,
    ChestShrine
}