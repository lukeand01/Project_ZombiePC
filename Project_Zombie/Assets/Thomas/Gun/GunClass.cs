
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

[System.Serializable]
public class GunClass 
{
    public ItemGunData data { get; private set; }
    string ownerId;

    public GunClass()
    {
        data = null;
    }
    public GunClass(ItemGunData data, GameObject spawnedGunModel)
    {
        this.data = data;
        SetGunModel(spawnedGunModel);


        ammoCurrent = data.bulletsInMaganzine;
        ammoTotal = ammoCurrent;
        ammoReserve = ammoCurrent * 5;

        cooldownTotal = data.fireRate;

        bulletBehaviorList = data.bulletBehaviorList;   
    }
    public GunClass(GunClass refClass)
    {

    }


    #region AMMO

    public int ammoCurrent { get; private set; }
    public int ammoTotal { get; private set; }
    public int ammoReserve { get; private set; }


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
    }


    #endregion


    #region SHOOT

    List<BulletBehavior> bulletBehaviorList = new();

    public void Shoot(Vector3 gunDir)
    {
        if (!CanShoot())
        {
            return;
        }


        data.Shoot(this, ownerId, data.bulletTemplate, gunDir, bulletBehaviorList);
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
            if (bulletBehaviorList[i] == newBulletBehavior)
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
}

