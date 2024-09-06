using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UIElements;

public class EntityEvents : MonoBehaviour
{
    //playerhandler will also 
    public Action<StatType, float> eventUpdateStat;
    public void OnUpdateStat(StatType type, float value) => eventUpdateStat?.Invoke(type, value);


    public Action<IDamageable, DamageClass> eventDamagedEntity;
    public void OnDamagedEntity(IDamageable damageable, DamageClass damageClassBeingUsed) => eventDamagedEntity?.Invoke(damageable, damageClassBeingUsed);

    public Action eventDamageTaken;
    public void OnDamageTaken() => eventDamageTaken?.Invoke();


    //so this is the fella




    public Action eventHealed;
    public void OnHealed()
    {

        eventHealed?.Invoke();
    }

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


    public Action eventPassedRound;
    public void OnPassedRound() => eventPassedRound?.Invoke();

    public Action<bool> eventLockEntity;
    public void OnLockEntity(bool isLocked) => eventLockEntity?.Invoke(isLocked);


    public Action eventEntityStunned;
    public void OnEntityStunned() => eventEntityStunned?.Invoke();

    private void OnDestroy()
    {
        eventUpdateStat = delegate { }; 
    }

    #region DELEGATES 

    public delegate void DelegateDamageTaken<T>(ref T modifier);

    public DelegateDamageTaken<DamageClass> eventDelegate_DamageTaken;

     public void CallDelegate_DamageTaken(ref DamageClass damage)
    {
        if (eventDelegate_DamageTaken != null)
        {
            eventDelegate_DamageTaken(ref damage);
        }
    }


    public delegate void DelegateDealDamageToEntity<T>(ref T totalValue);

    public DelegateDealDamageToEntity<DamageClass> eventDelegate_DealDamageToEntity;

     public void CallDelegate_DealDamageToEntity(ref DamageClass modifier)
    {
        if (eventDelegate_DealDamageToEntity != null)
        {
            eventDelegate_DealDamageToEntity(ref modifier);
        }
    }



    public delegate void DelegateHealed<T>(ref T totalValue);

    public DelegateHealed<float> eventDelegate_Healed;

    public void CallDelegate_Healed(ref float modifier)
    {
        if (eventDelegate_Healed != null)
        {
            eventDelegate_Healed(ref modifier);
        }
    }


    public delegate void DelegateChangedPoints<T>(ref T totalValue);

    public DelegateChangedPoints<float> eventDelegate_ChangedPoints;

    public void CallDelegate_ChangedPoints(ref float modifier)
    {
        if (eventDelegate_ChangedPoints != null)
        {
            eventDelegate_ChangedPoints(ref modifier);
        }
    }

    #endregion
}
public enum ChestType
{
    ChestResource,
    ChestAbility,
    ChestGun,
    ChestShrine
}