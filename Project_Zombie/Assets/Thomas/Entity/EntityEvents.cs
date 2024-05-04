using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityEvents : MonoBehaviour
{

    public Action<StatType, float> eventUpdateStat;
    public void OnUpdateStat(StatType type, float value) => eventUpdateStat?.Invoke(type, value);

    public Action<IDamageable> eventDamagedEntity;
    public void OnDamaged(IDamageable damageable) => eventDamagedEntity?.Invoke(damageable);


    private void OnDestroy()
    {
        eventUpdateStat = delegate { }; 
    }


}
