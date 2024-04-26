using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamageClass 
{
    //this thing needs to know if its from t eh player or not.
    //because otherwise traps will heal the player.
    //i worrry it later

    public DamageClass(float baseDamage)
    {
        this.baseDamage = baseDamage;
    }


    public string damageableID {  get; private set; }


    DamageType damageType;


    public float baseDamage { get; private set; }
    public float pen {  get; private set; }
    public float critChance;
    public float critDamage { get; private set; }
    public float damageBasedInHealth {  get; private set; }

    bool alwaysCrit;

    public float explosionRadius {  get; private set; }
    //we get the targetHealth scaling.


    public bool cannotFinishEntity { get; private set;}

    
   
    

    #region MAKE

    public void MakeBlockFromFinishingEntity()
    {
        cannotFinishEntity = true;
    }

    public void MakeCritDamage(float critDamage)
    {
        
        this.critDamage = critDamage;
    }

    public void MakeCritChance(float value)
    {

        critChance = value;
    }


    public void MakeDamage(float damage)
    {      
        baseDamage = damage;    
    }
    public void MakePen(float pen)
    {
        this.pen = pen; 
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

    public bool CheckForCrit()
    {

        if (alwaysCrit)
        {
            return true;
        }

       int roll = Random.Range(0, 100);


        return critChance > roll;

    }
    public float GetDamage(float totalReducition, float targetMaxHealth,  bool isCrit)
    {
        //i need to know how strong he is. we roll for crit.

        float totalDamage = baseDamage;

        
        totalReducition -= pen;
        totalReducition = Mathf.Clamp(totalReducition, 0, 100);

        float reduction = baseDamage * (totalReducition * 0.01f);

        totalDamage -= reduction;
        totalDamage = Mathf.Clamp(totalDamage, 1, totalDamage);

        float healthbasedDamageIncrement = targetMaxHealth * damageBasedInHealth;
        totalDamage += healthbasedDamageIncrement;


        //the pen ignores an amount. its always flat. each value


        if (isCrit)
        {
            totalDamage *= 1.5f + critDamage;
        }



        return totalDamage;
    }


}

public enum DamageType 
{ 
    Physical,
    Pure

}



//what does fire do?