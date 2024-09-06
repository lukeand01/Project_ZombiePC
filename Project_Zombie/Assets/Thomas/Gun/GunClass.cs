
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

[System.Serializable]
public class GunClass 
{
    public ItemGunData data { get; private set; }
    [SerializeField] ItemGunData debugShowData;
    string ownerId;

    public bool isEquipped { get; private set; } 




    public GunClass()
    {
        data = null;
        debugShowData = null;
    }
    public GunClass(PlayerHandler handler, ItemGunData data, GameObject spawnedGunModel, Transform holsterHolder)
    {
        this.data = data;

        SetUp();

        SetGunModel(spawnedGunModel, holsterHolder);

        ammoCurrent = (int)data.GetValue(StatType.Magazine);
        ammoTotal = ammoCurrent;
        RefreshReserveAmmo();



        cooldownTotal = data.GetValue(StatType.FireRate);

        bulletBehaviorList = data.bulletBehaviorList;

        AssignEvents(handler._entityEvents);
        SetInitialValues(handler._entityStat);

        debugShowData = data;

        isInUpgradeStation = false;


        ItemGunDataCharge itemGunCharge = data.GetGunCharge();

        if(itemGunCharge != null )
        {
            gunCharge_Total = itemGunCharge.chargeDuration;
        }
    }

  
    void SetInitialValues(EntityStat _stat)
    {
        //we have to update the values here

        _DamageClass = new DamageClass(data.GetDamageList);
        
        //damage will increase every stat.

        //DAMAGE
        float modifier = GetGunTotalStat(StatType.Damage) * _stat.GetTotalValue(StatType.Damage);
        

        //PEN
        _DamageClass.Make_Pen(GetGunTotalStat(StatType.Pen) + _stat.GetTotalValue(StatType.Pen));

        //CRIT CHANCE
        _DamageClass.Make_CritChance(GetGunTotalStat(StatType.CritChance) + _stat.GetTotalValue(StatType.CritChance) + (_stat.GetTotalValue(StatType.Luck) * 1.5f )) ;

        //CRIT DAMAGE
        _DamageClass.Make_CritDamage(GetGunTotalStat(StatType.CritDamage) + _stat.GetTotalValue(StatType.CritDamage));

    }

    void AssignEvents(EntityEvents _events)
    {
        //update the 
        _events.eventUpdateStat += UpdateStat;
    }

    public void ResetGunClass()
    {
        data = null;
    }


    #region STAT

    //i will create the

    //create a bd here.
    public List<BDClass> gun_bdList { get; private set; } = new();

    List<float> UpgradeStackList = new();

    public Dictionary<StatType, float> statBaseDictionary { get; private set; } = new Dictionary<StatType, float>();
    public Dictionary<StatType, float> statAlteredDictionary { get; private set; } = new Dictionary<StatType, float>();


    //I WONT USE THIS BECAUSE THE UPGRADE MACHINE WONT TAKE IT.
    void SetWithRef(GunClass refClass)
    {
        gun_bdList = refClass.gun_bdList;

        statBaseDictionary = refClass.statBaseDictionary;
        statAlteredDictionary = refClass.statAlteredDictionary;




        if(statBaseDictionary.Count == 0)
        {
            Debug.Log("we need to set up this");
            SetUp();
        }
    }

    void SetUp()
    {
        List<StatType> refList = MyUtils.GetStatForGunListRef();

        foreach (var item in refList)
        {
            if (!statBaseDictionary.ContainsKey(item))
            {
                statBaseDictionary.Add(item, data.GetValue(item));
            }

        }


    }

    public void Gun_AddBD(BDClass bd)
    {
        //if its not stat then we shoudnt care about it
        //this bd is only for the unique buffs.

        if(bd.bdType != BDType.Stat)
        {
            Debug.Log("this is not stat so should not be used in a gun");
            return;
        }

        gun_bdList.Add(bd);

        OrganizeBD();
    }

    public void Gun_RemoveBD(string id)
    {
        for (int i = 0; i < gun_bdList.Count; i++)
        {
            var item = gun_bdList[i];

            if (item.id == id)
            {
                gun_bdList.RemoveAt(i);
                OrganizeBD();
                return;
            }
        }


        Debug.Log("failed to find gun bd");
    }

    void OrganizeBD()
    {
        statAlteredDictionary.Clear();



        foreach (var item in gun_bdList)
        {
            float value = item.statValueFlat;
            value += statBaseDictionary[item.statType] * item.statValue_PercentbasedOnBaseValue;


            if (statAlteredDictionary.ContainsKey(item.statType))
            {
                statAlteredDictionary[item.statType] += value;
            }
            else
            {
                statAlteredDictionary.Add(item.statType, value);    
            }

            
        }
    }





    public void AddUpgradeStack(float value)
    {
        UpgradeStackList.Add(value);
        OrganizeAllDamageStats();

        //we update firerate.
        //we update total_Damage magazine.
        //when it come out of the upgrade station it should have the magazine refreshed as well.

        ammoTotal = (int)GetGunTotalStat(StatType.Magazine);


        cooldownTotal = GetGunTotalStat(StatType.FireRate);

        //this value_Level we will handle everything and then we will apply these values.
    }

    
    public float GetGunTotalStat(StatType stat)
    {
        if (!statBaseDictionary.ContainsKey(stat))
        {
            return 0;
        }

        float negativeModifier = 1;

        if(stat == StatType.FireRate)
        {

            negativeModifier = -1;
        }
        

        float baseValue = data.GetValue(stat);
        float alteredValue = 0;

        if (statAlteredDictionary.ContainsKey(stat))
        {
            alteredValue = statAlteredDictionary[stat] * negativeModifier;
        }

        float upgradeStackValue = 0;
        float totalValue = baseValue + alteredValue;

        foreach (var item in UpgradeStackList)
        {
            upgradeStackValue += totalValue * item;
        }

        totalValue += upgradeStackValue * negativeModifier;



        return totalValue;
    }


    #endregion

    #region AMMO

    public int ammoCurrent { get; private set; }
    public int ammoTotal { get; private set; }
    public int ammoReserve { get; private set; }
    public int ammoReserveTotal { get; private set; }

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


            if (ammoRefundProgress >= 1)
            {
                valueForAmmo = 1;
                ammoRefundProgress = 0;
            }
        }


        ammoCurrent += (int)valueForAmmo;
        ammoCurrent = Mathf.Clamp(ammoCurrent, 0, ammoTotal);
    }
    
    public void ReplenishReserveAmmoBasedInTotal(float modifier)
    {
        float valueRestored = ammoReserve * modifier;

        ammoReserve += (int)valueRestored;
        ammoReserve = Mathf.Clamp(ammoReserve, 0, ammoReserveTotal);
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

    public void RefreshReserveAmmo()
    {
        ammoReserve = ammoCurrent * 5;
        ammoReserveTotal = ammoTotal * 7;
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
    Transform holsterHolder;
    Transform gunHand;

    Vector3 equippedPosition;
    Vector3 equippedRotation;
    Vector3 equippedScale;
    //we need to save rotation and position of the gun_Perma

    public void SetGunModel(GameObject gunModel, Transform holsterHolder)
    {
        this.gunModel = gunModel;
        gunPoint = gunModel.transform.GetChild(0).transform;
        gunHand = gunModel.transform.parent;

        equippedPosition = gunModel.transform.localPosition;
        equippedRotation = gunModel.transform.localRotation.eulerAngles;
        equippedScale = gunModel.transform.localScale;


        this.holsterHolder = holsterHolder;

        gunModel.transform.localRotation = data.gunModel.transform.localRotation;
    }

    public void StoreWeapon()
    {
        //we put teh weapon, turned on in the right place. perphaps we reduce the 

        gunModel.transform.SetParent(holsterHolder);
        gunModel.transform.localPosition = Vector3.zero;
        gunModel.transform.localRotation = Quaternion.Euler(new Vector3(0, -90, 90));

        isEquipped = false;
    }
    public void EquipWeaponInHand()
    {
        //

        gunModel.transform.SetParent(gunHand);

        gunModel.transform.localPosition = equippedPosition;
        gunModel.transform.localRotation = Quaternion.Euler(equippedRotation);
        gunModel.transform.localScale = equippedScale;

        isEquipped = true;
    }

    public void ControlModelVisibility(bool isVisible)
    {
        if(gunModel != null) gunModel.gameObject.SetActive(isVisible);
    }

    #endregion

    #region SHOOT

    [SerializeField] List<BulletBehavior> bulletBehaviorList = new();

    public int goThroughPower_Individual; //the amount of enemies it can cut. 
    public int goThroughPower_Forced;

    public void GoThroughPower_Individual_Set(int goThroughPower)
    {
        goThroughPower_Individual = goThroughPower;
    }

    public int GoThroughPower_Total { get { return goThroughPower_Forced + goThroughPower_Individual + data.goThroughPower; } }
    //i need to get a varaible for the player as well. a variable that universalis affects everyone.


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
        EntityStat _stat = PlayerHandler.instance._entityStat;
        if (stat == StatType.Damage)
        {
            float modifier = _stat.GetTotalValue(StatType.Damage);
            _DamageClass.Update_DamageListUsingDamageStat(modifier + value, modifier);
        }
        if(stat == StatType.Pen)
        {
            _DamageClass.Make_Pen(GetGunTotalStat(StatType.Pen) + value);
            return;
        }
        if (stat == StatType.CritChance)
        {
            _DamageClass.Make_CritChance(GetGunTotalStat(StatType.Pen) + value);
            return;
        }
        if (stat == StatType.CritDamage)
        {
            _DamageClass.Make_CritDamage(GetGunTotalStat(StatType.Pen) + value);
            return;
        }


    }

    void OrganizeAllDamageStats()
    {
        EntityStat _stat = PlayerHandler.instance._entityStat;

        float modifier =  _stat.GetTotalValue(StatType.Damage);
        _DamageClass.Update_DamageListUsingDamageStat(modifier);

        //PEN
        _DamageClass.Make_Pen(GetGunTotalStat(StatType.Pen) + _stat.GetTotalValue(StatType.Pen));

        //CRIT CHANCE
        _DamageClass.Make_CritChance(GetGunTotalStat(StatType.CritChance) + _stat.GetTotalValue(StatType.CritChance) + (_stat.GetTotalValue(StatType.Luck) * 1.5f));

        //CRIT DAMAGE
        _DamageClass.Make_CritDamage(GetGunTotalStat(StatType.CritDamage) + _stat.GetTotalValue(StatType.CritDamage));
    }

    #endregion

    #region UPGRADE STATION
    public bool isInUpgradeStation { get; private set; }


    public List<GunUpgradeData> gunUpgradeList { get; private set; } = new(); //this is the list for every single fella.
    List<GunUpgradeData> gunUpgradeListForNonStackableList = new();


    public void AddUpgradeStation()
    {
        isInUpgradeStation = true;
    }
    public void RemoveUpgradeStation()
    {
        isInUpgradeStation = false;
    }

    public void AddUpgradeToList(GunUpgradeData upgradeData)
    {
        gunUpgradeList.Add(upgradeData);

        if (!upgradeData.upgradeCanStack)
        {
            gunUpgradeListForNonStackableList.Add(upgradeData);
        }
    }
    public bool HasUpgradeNonStackable(GunUpgradeData upgradeData)
    {
        foreach (var item in gunUpgradeListForNonStackableList)
        {
            if (item.name == upgradeData.name) return true;
        }

        return false;
    }

    #endregion

    #region SECRET STATS
    //i am puitting here variable that i dont where to put yet.

    public int bulletPerShot { get; private set; } //this is the actual number.

    int bulletShootFlat2;

    public float secretStatMultipleBulletFlat { get; private set; }

    public float secretStatMultipleBulletPercent { get; private set; }

    public float secretStatMultipleBulletDamageModifier { get; private set; }


    public void MakeSecretStats(float multipleBulletFlat, float multipleBulletPercent, float multipleBulletDamage)
    {
        secretStatMultipleBulletFlat = multipleBulletFlat;
        secretStatMultipleBulletPercent = multipleBulletPercent;
        secretStatMultipleBulletDamageModifier = multipleBulletDamage;


        RecaculateBulletPerShot();


    }

    public void IncreaseBulletPerShot(int value)
    {
        bulletShootFlat2 += value;
        RecaculateBulletPerShot();
    }
    public void DecreaseBulletPerShot(int value)
    {
        bulletShootFlat2 -= value;
        RecaculateBulletPerShot();
    }
    void RecaculateBulletPerShot()
    {
        bulletPerShot = data.bulletPerShot;
        int additionalValue = 0;

        additionalValue += (int)secretStatMultipleBulletFlat;
        additionalValue += bulletShootFlat2;
        float bulletPerShotModifier = (bulletPerShot + additionalValue) * secretStatMultipleBulletPercent;

        bulletPerShot += additionalValue;
    }

    #endregion

    #region UI

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
    #endregion

    #region ESPECIAL EVENTS

    public void RechargeShieldAbilty(ItemGunData data, float value)
    {
        if(this.data.name == data.name)
        {
            Debug.Log("should recharge this fella with this value " + value);
            PlayerHandler.instance._playerCombat.Shield_Recharge_Percent(value);
        }
    }

    #endregion

    #region  CHARGE WEAPONS

    float gunCharge_Total;
    float gunCharge_Current;


    public void StopChargeGun()
    {
        gunCharge_Current = 0;
    }

    public float[] GetProgressValues()
    {
        float[] floatArray = new float[2];
        floatArray[0] = gunCharge_Current;
        floatArray[1] = gunCharge_Total;
        return floatArray;
    }

    public bool IsGunChargeReady() => gunCharge_Current >= gunCharge_Total;

    public bool IsGunCharge() => gunCharge_Total > 0;

    public void HandleGunCharge()
    {
        if (data == null) return;

        gunCharge_Current += Time.fixedDeltaTime;

    }


    #endregion

}

