using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamageClass 
{
    //this thing needs to know if its from t eh player or not.
    //because otherwise traps will heal the player.
    //i worrry it later

    //i should be carry multiple types of damage here
    //or just a basic one.

    
    public DamageClass(List<DamageTypeClass> damageList)
    {
        CreateDamageList(damageList);
    }
    public DamageClass(float baseDamage, DamageType _damageType, float pen)
    {
        Add_Damage(_damageType, baseDamage);
        Make_Pen(pen);
    }

    public DamageClass(DamageClass refClass)
    {
        
        Make_Pen(refClass.pen);
        if (refClass.alwaysCrit)
        {
            Make_AlwaysCrit();
        }
        Make_CritChance(refClass.critChance);
        Make_CritDamage(refClass.critDamage);

        if (refClass.shoudNotShowPopUp)
        {
            Make_UIInvisible();
        }

        CreateDamageList(refClass.damageList);
        Make_Attacker(refClass.attacker);


        if(refClass.cannotBeDodged) Make_CannotDodge();
    }







    //GENERAL. THIS STUFF SHOULD BE IN THE CLASS
    public string damageableID { get; private set; }
    public IDamageable attacker { get; private set; }
    public Transform projectilTransform { get; private set; }
    public Vector3 lastPos { get; private set; }

    public float critChance { get; private set; }
    public float critDamage { get; private set; } //crit works by increasing the crit for the largest of all damage.
    bool alwaysCrit;

    public float totalDamageModifier { get; private set; } = 1;

    public bool cannotBeDodged { get; private set; }

    public bool cannotFinishEntity { get; private set; }

    public bool shoudNotShowPopUp { get; private set; }

    public bool isExplosion { get; private set; }

    //PER STUFF. THIS SHOULD BE TAKEN TO A SMALLER CLASS AND PUT IN A LIST
    public float pen {  get; private set; } //how does pen work?

    public List<DamageTypeClass> damageList { get; private set; } = new(); //this is teh damage

    // 

    #region MAKE
    //we will refill this as we require stuff.

    public void Make_Attacker(IDamageable attacker) =>
        this.attacker = attacker;


    public void Make_UIInvisible() =>    
        shoudNotShowPopUp = true;
    
    public void Make_CannotDodge() =>   
        cannotBeDodged = true;
    
    public void Make_PlayerPosWhenCreated(Vector3 pos) => lastPos = pos;

    public void Make_CritChance(float critChance) =>   
        this.critChance = critChance;
    
    public void Make_CritDamage(float critDamage)  => 
       this.critDamage = critDamage;
    
    public void Make_AlwaysCrit() => alwaysCrit = true;

    public void Make_Pen(float pen) => this.pen = pen;

    public void Make_Explosion() => isExplosion = true;

    public void Add_Damage(DamageType _type, float _value)
    {        
        damageList.Add(new DamageTypeClass(_type, _value));
    }

    public void Make_TotalDamageModifier(float totalDamageModifier) =>    
        this.totalDamageModifier = totalDamageModifier;

    public void Make_Projectil(Transform projectil) => projectilTransform = projectil;

    public void Make_Stack(int stack)
    {
        //we always goiong to assume this to be only the first.
        damageList[0].UpdateValue(damageList[0]._baseValue * stack);
    }
    #endregion


    //if the damage is 10
    //we increase it bt 100%
    //so its 20
    //now we want to increase to 200%
    //so subtract it by 100% this would not working

    public void Update_DamageListUsingDamageStat(float damageStatValue, float valueToRemove = 0)
    {
        foreach (var item in damageList)
        {
            float modifierToRemove = item._baseValue * valueToRemove;
            item.UpdateValue(item._value - modifierToRemove);

            float modifier = item._value* damageStatValue;
            item.UpdateValue(item._value + modifier);        
        }
    }

    public void UpdateDamageList_Player()
    {
       
    }

    public void UpdateDamageList_Enemy(EnemyData data)
    {

        foreach (var item in damageList)
        {
            //we will check for crit
            //we will modify by resistance
            int critRoll = Random.Range(0, 101);

            if(critChance > critRoll)
            {
                item.Make_Crit(critDamage);
            }



        }

    }
    
    public bool AtLeastOneDamageCrit()
    {
        foreach (var item in damageList)
        {
            if (item.isCrit) return true;
        }
        return false;
    }

    public float GetTotalDamage(bool onlyNotPure = false)
    {
        float damage = 0;

        foreach (var item in damageList)
        {
            if(onlyNotPure && item._damageType == DamageType.Pure)
            {
                continue;
            }

            damage += item._value;
        }
        return damage;
    }

    public float GetTotalDamage_Pure()
    {
        float damage = 0;

        foreach (var item in damageList)
        {
            if (item._damageType == DamageType.Pure)
            {
                damage += item._value;
            }

           
        }
        return damage;
    }

    void CreateDamageList(List<DamageTypeClass> newDamageList)
    {
        damageList.Clear();

        foreach (var item in newDamageList)
        {
            damageList.Add(new DamageTypeClass(item._damageType, item._value));
        }

    }
}

[System.Serializable]
public class DamageTypeClass
{
    [field: SerializeField] public DamageType _damageType { get; private set; }
    [field: SerializeField] public float _value { get; private set; } = 1; //
     public float _modifierValue { get; private set; } = 0;
     public float _baseValue {  get; private set; } = 1;
    public bool isCrit {  get; private set; } //we inform this for events and for ui down the line.

    public void Make_Crit(float critDamage)
    {
        _value *= 1.5f + critDamage;
        isCrit = true;
    }

    public void UpdateValue(float _value) => this._value = _value;

    public DamageTypeClass(DamageType damageType, float value)
    {
        _damageType = damageType;
        _value = value;
        _baseValue = _value;
        _modifierValue = 0;
    }
}


//enemies have weakness to certain damages.
//certain enemies have weakness to certain damages and strenght against other damages
//they are on a rarity scale.
//on player: phy is normal; Mag deals more damage to shield; Plasma reduces armor for a duration; Corrupt silences the player for a short duration; Pure ignores everything

public enum DamageType 
{ 
    Physical, //
    Magical,
    Plasma, //
    Corrupt,
    Pure

}



//what does fire do?