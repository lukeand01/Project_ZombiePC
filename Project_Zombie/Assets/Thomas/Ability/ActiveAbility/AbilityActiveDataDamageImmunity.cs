using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability / Active / Damage_Immunity")]
public class AbilityActiveDataDamageImmunity : AbilityActiveData
{

    //immunity stuff not showing.
    
    [SerializeField] float duration = 4;
    public override bool Call(AbilityClass ability)
    {
        base.Call(ability);


        BDClass bd = new BDClass("DamageImmunity", BDType.Immune, duration); 
        bd.MakeShowInUI();
        PlayerHandler.instance._entityStat.AddBD(bd);

        return true;
    }


    public override string GetDamageDescription(AbilityClass ability)
    {

        return $"Become immune for {duration} seconds";
    }

}
