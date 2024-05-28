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
        MakeDamage(baseDamage);
    }

    public DamageClass(float baseDamage, float basePen)
    {
        MakeDamage(baseDamage);
        MakePen(basePen);
    }

    public DamageClass(DamageClass refClass)
    {
        MakeDamage(refClass.baseDamage);
        MakePen(refClass.pen);
        if (refClass.alwaysCrit)
        {
            MakeAlwaysCrit();
        }
        MakeCritChance(refClass.critChance);
        MakeCritDamage(refClass.critDamage);
        MakeDamageType(refClass.damageType);
        if (refClass.shoudNotShowPopUp)
        {
            MakeUIInvisible();
        }
        MakeAttacker(refClass.attacker);

        MakeBulletQuantityDamageModifier(additionalDamageBasedInBulletQuantity);
        MakePureDamageModifier(pureDamageModifier);
        if(refClass.cannotBeDodged) MakeCannotDodge();
    }
    

    public string damageableID {  get; private set; }


    DamageType damageType;
    public IDamageable attacker {  get; private set; }

    public float baseDamage { get; private set; }
    public float currentDamage { get; private set; } 
    public float pen {  get; private set; }

    public float critChance;
    public float critDamage { get; private set; }
    public float damageBasedInHealth {  get; private set; }

    public float pureDamageModifier { get; private set; }

    public float additionalDamageBasedInBulletQuantity { get; private set; }

    bool alwaysCrit;

    public bool cannotBeDodged {  get; private set; }
    public float explosionRadius {  get; private set; }
    //we get the targetHealth scaling.


    public bool cannotFinishEntity { get; private set;}


    public bool shoudNotShowPopUp;
    

    #region MAKE

    public void MakeCannotDodge()
    {
        cannotBeDodged = true;
    }
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

    public void MakeStack(int stacks)
    {
        currentDamage = baseDamage * stacks;
    }

    public void MakeDamage(float damage)
    {      
        baseDamage = damage;
        currentDamage = damage;
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
    public void MakeUIInvisible()
    {
        shoudNotShowPopUp = true;
    }

    public void MakeAttacker(IDamageable attacker)
    {
        this.attacker = attacker;
    }

    public void MakeBulletQuantityDamageModifier(float value)
    {
        additionalDamageBasedInBulletQuantity = value;
    }
    public void MakePureDamageModifier(float value)
    {
        pureDamageModifier = value;
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

        float totalDamage = currentDamage;


        

        
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



        float BulletQuantityDamageModifier = additionalDamageBasedInBulletQuantity * totalDamage;
        totalDamage += BulletQuantityDamageModifier;

        float pureAdditional = totalDamage * pureDamageModifier;

        return totalDamage + pureAdditional;
    }


}

public enum DamageType 
{ 
    Physical,
    Pure

}



//what does fire do?