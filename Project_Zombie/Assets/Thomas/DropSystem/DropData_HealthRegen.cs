using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Drop / HealthRegen")]
public class DropData_HealthRegen : DropData
{
    [Separator("Health Regen")]
    [SerializeField] [Range(0,1)]float healthRecoverValue = 0;
    public override void CallDrop()
    {
        //just recover health
        PlayerHandler.instance._playerResources.RestoreHealthBasedInPercent(healthRecoverValue);
    }
}
