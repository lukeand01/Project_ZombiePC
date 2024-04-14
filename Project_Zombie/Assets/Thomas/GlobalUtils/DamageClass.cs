using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageClass 
{
    //this thing needs to know if its from t eh player or not.
    //because otherwise traps will heal the player.
    //i worrry it later


    public string damageableID {  get; private set; }


    DamageType damageType;


    public float baseDamage { get; private set; }
    float critChance;
    float critDamage;
    float damageBasedInHealth;

    bool alwaysCrit;

    public float explosionRadius {  get; private set; }
    //we get the targetHealth scaling.


    public bool cannotFinishEntity { get; private set;}


   
    

    #region MAKE

    public void MakeBlockFromFinishingEntity()
    {
        cannotFinishEntity = true;
    }

    public void MakeCrit(float critChance, float critDamage)
    {
        this.critChance = critChance;
        this.critDamage = critDamage;
    }

    public void MakeAlwaysCrit()
    {
        alwaysCrit = true;
    }
   
    public void MakeDamageType(DamageType damageType)
    {
        this.damageType = damageType;
    }

    #endregion


    
    //we need to get the a ref of 





}

public enum DamageType 
{ 
    Physical,
    Pure

}



//what does fire do?