
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

[System.Serializable]
public class GunClass 
{
    public ItemGunData data { get; private set; }
    [SerializeField] ItemGunData debugShowData;
    string ownerId;

   

    public GunClass()
    {
        data = null;
        debugShowData = null;
    }
    public GunClass(PlayerHandler handler, ItemGunData data, GameObject spawnedGunModel)
    {
        this.data = data;
        SetGunModel(spawnedGunModel);

        ammoCurrent = (int)data.GetValue(StatType.Magazine);
        ammoTotal = ammoCurrent;
        ammoReserve = ammoCurrent * 5;

        cooldownTotal = data.GetValue(StatType.FireRate);

        bulletBehaviorList = data.bulletBehaviorList;

        AssignEvents(handler._entityEvents);
        SetInitialValues(handler._entityStat);

        debugShowData = data;

        isInUpgradeStation = false;
    }

   

    public GunClass(ItemGunData data)
    {
        //we actually need to pass all the information we can
    }


    void SetInitialValues(EntityStat _stat)
    {
        //we have to update the values here

        _DamageClass = new DamageClass(0);

        //DAMAGE
        float modifier = data.GetValue(StatType.Damage) * _stat.GetTotalValue(StatType.Damage);
        _DamageClass.MakeDamage(data.GetValue(StatType.Damage) + modifier);

        //PEN
        _DamageClass.MakePen(data.GetValue(StatType.Pen) + _stat.GetTotalValue(StatType.Pen));


        //CRIT CHANCE
        _DamageClass.MakeCritChance(data.GetValue(StatType.CritChance) + _stat.GetTotalValue(StatType.CritChance) + (_stat.GetTotalValue(StatType.Luck) * 1.5f )) ;

        //CRIT DAMAGE
        _DamageClass.MakeCritDamage(data.GetValue(StatType.CritDamage) + _stat.GetTotalValue(StatType.CritDamage));

    }

    void AssignEvents(EntityEvents _events)
    {
        //update the 
        _events.eventUpdateStat += UpdateStat;
    }


    public GunClass(GunClass refClass)
    {

    }

    public void ResetGunClass()
    {
        data = null;
    }

    #region AMMO

    public int ammoCurrent { get; private set; }
    public int ammoTotal { get; private set; }
    public int ammoReserve { get; private set; }

    float ammoRefundProgress;

    public void RefundAmmo(float modifier)
    {
        float valueRestored = ammoTotal * modifier;
        float valueForAmmo = 0;

        if (valueRestored >= 1)
        {
            valueForAmmo = Mathf.FloorToInt(valueRestored);
        }
        else
        {
            
            ammoRefundProgress += valueRestored;
            Debug.Log("here? " + ammoRefundProgress);

            if (ammoRefundProgress >= 1)
            {
                valueForAmmo = 1;
                ammoRefundProgress = 0;
            }
        }



        Debug.Log(valueRestored + " " + valueForAmmo + " " + ammoRefundProgress);

        ammoCurrent += (int)valueForAmmo;
        ammoCurrent = Mathf.Clamp(ammoCurrent, 0, ammoTotal);
    }
    

    public void MakeAmmoInfinite()
    {
        ammoReserve = -1;
    }
    public void UpdateReserveAmmo(int value)
    {
        ammoReserve = value;
    }

    public void ReloadGun()
    {

        int amountRequired = ammoTotal - ammoCurrent;
        int amountCost = 0;


        //you have 5 bullets and require 9
        //the different is what you need: 9 - 5 = 4;
        //


        if (ammoReserve != -1)
        {
            //then its infiite ammot.
            amountCost = amountRequired;
            amountCost = Mathf.Clamp(amountCost, 0, ammoReserve);
            ammoReserve -= amountCost;
            ammoCurrent += amountCost;
        }
        else
        {
            ammoCurrent = ammoTotal;
        }




    }

    public void ReloadGunForFree()
    {
        ammoCurrent = ammoTotal;
    }


    public bool HasReserveAmmo()
    {
        return ammoReserve == -1 || ammoReserve > 0;
    }
    public bool CanReload()
    {
        return ammoTotal > ammoCurrent;
    }
    public bool MustReload()
    {
        return ammoCurrent <= 0;
    }

    #endregion

    #region COOLDOWN

    public float cooldownCurrent {  get; private set; }
    public float cooldownTotal {  get; private set; }

    public void HandleCooldown()
    {
        if(cooldownCurrent > 0)
        {
            cooldownCurrent -= Time.deltaTime;
        }
    }

    void PutInCooldown()
    {
        cooldownCurrent = cooldownTotal;
    }
    #endregion

    #region MODELS
    public GameObject gunModel {  get; private set; }
    public Transform gunPoint {  get; private set; }
    public void SetGunModel(GameObject gunModel)
    {
        this.gunModel = gunModel;
        gunPoint = gunModel.transform.GetChild(0).transform;


        gunModel.transform.localRotation = data.gunModel.transform.localRotation;
    }


    #endregion


    #region SHOOT

    List<BulletBehavior> bulletBehaviorList = new();

    public void Shoot(Vector3 gunDir,List<BulletBehavior> forcedBulletBehaviorList)
    {
        if (!CanShoot())
        {
            return;
        }
        List<BulletBehavior> newList = new List<BulletBehavior>();
        newList.AddRange(forcedBulletBehaviorList);
        newList.AddRange(bulletBehaviorList);

        

        data.Shoot(this, ownerId, data.bulletTemplate, gunDir, newList);
        UIHandler.instance._playerUI.CallMouseIconAnimation();
        PutInCooldown();
        ammoCurrent -= 1;
    }


    public void AddBulletBehavior(BulletBehavior newBulletBehavior)
    {
        bulletBehaviorList.Add(newBulletBehavior);
    }
    public void RemoveBulletBehavior(BulletBehavior newBulletBehavior) 
    {
        for (int i = 0; i < bulletBehaviorList.Count; i++)
        {
            if (bulletBehaviorList[i].id == newBulletBehavior.id)
            {
                bulletBehaviorList.RemoveAt(i);
            }
        }
    }

    bool CanShoot()
    {
        return cooldownCurrent <= 0;
    }

    #endregion

    #region DAMAGE
    public DamageClass _DamageClass { get;private set; }

    
    void UpdateStat(StatType stat, float value)
    {

        if(stat == StatType.Damage)
        {
            float modifier = data.GetValue(StatType.Damage) * value;
            _DamageClass.MakeDamage(data.GetValue(StatType.Damage) + modifier);
        }
        if(stat == StatType.Pen)
        {
            _DamageClass.MakePen(data.GetValue(StatType.Pen) + value);
            return;
        }
        if (stat == StatType.CritChance)
        {
            _DamageClass.MakeCritChance(data.GetValue(StatType.Pen) + value);
            return;
        }
        if (stat == StatType.CritDamage)
        {
            _DamageClass.MakeCritDamage(data.GetValue(StatType.Pen) + value);
            return;
        }


    }

    #endregion

    #region UPGRADE STATION
    public bool isInUpgradeStation { get; private set; }
    public void AddUpgradeStation()
    {
        isInUpgradeStation = true;
    }
    public void RemoveUpgradeStation()
    {
        isInUpgradeStation = false;
    }



    #endregion


    #region SECRET STATS
    //i am puitting here variable that i dont where to put yet.

    public int bulletPerShot { get; private set; } //this is the actual number.
    public float bulletQuantityDamageModifier { get; private set; } //we are simply going to add this to the damage.


    public float secretStatMultipleBulletFlat { get; private set; }

    public float secretStatMultipleBulletPercent { get; private set; }

    public float secretStatMultipleBulletDamageModifier { get; private set; }


    public void MakeSecretStats(float multipleBulletFlat, float multipleBulletPercent, float multipleBulletDamage)
    {
        secretStatMultipleBulletFlat = multipleBulletFlat;
        secretStatMultipleBulletPercent = multipleBulletPercent;
        secretStatMultipleBulletDamageModifier = multipleBulletDamage;


        bulletPerShot = data.bulletPerShot;
        bulletPerShot += (int)secretStatMultipleBulletFlat;
        float bulletPerShotModifier = bulletPerShot * secretStatMultipleBulletPercent;
        bulletPerShot += (int)bulletPerShotModifier;



        _DamageClass.MakeBulletQuantityDamageModifier(secretStatMultipleBulletDamageModifier * (bulletPerShot - 1));

    }



    #endregion

    EquipWindowEquipUnit gunEquipUnit;

    public void SetGunEquip(EquipWindowEquipUnit gunEquipUnit)
    {
        this.gunEquipUnit = gunEquipUnit;
    }

    public void ChangeGunbyEquipWindow(ItemGunData newGun)
    {
        //we are going tell player to change the model.


    }
    //this can be changed only inside.

}

