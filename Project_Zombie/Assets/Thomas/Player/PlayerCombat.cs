using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    PlayerHandler handler;

    [Separator("DEBUG")]
    [SerializeField] bool debugStartWithWeapon;
    [SerializeField] ItemGunData debugGunDataPerma;
    [SerializeField] ItemGunData debugGunDataTemp1;
    [SerializeField] ItemGunData debugGunDataTemp2;
    [SerializeField] Transform debugLaserStartPos;
    [SerializeField] LineRenderer debugLineRenderer;

    [Separator("REFERNCES")]
    [SerializeField] Transform gunSpawnPos;

    [SerializeField]GunClass[] gunList; //0 is always perma and the next two are temp
    public GunClass GetCurrentGun { get{ return gunList[currentGunIndex]; } }
    int currentGunIndex;

    [SerializeField] Transform[] gunHolsterHolderList;

    GunUI _gunUI;
    bool keepShootingIfCanHold;
    bool trueBlockForShooting; //this is used to block to hold fire while swapping

    [Separator("SOUND")]
    [SerializeField] AudioSource audioSrc_Reload;
    [SerializeField] AudioClip audio_Swap;


    //everytime we are supposed to turn off the model we just put it in the right place.


    private void Awake()
    {
        handler = GetComponent<PlayerHandler>();

    }

    private void Start()
    {

        //the problem is because this is starting somewhere else where there is no cityhandler. i simply must call it from the cityhandler.



        SetUp(debugGunDataPerma);
        _gunUI = UIHandler.instance.gunUI;

        if(gunList.Length <= 0)
        {
            Debug.Log("no lenght");
        }
        if (gunList[0].data == null)
        {
            Debug.Log("data is null");
        }

        UIHandler.instance._EquipWindowUI.GetEquipForPermaGun(gunList[0]);
        UIHandler.instance._MouseUI.UpdateMouseUI(gunList[0].data.mouseImageType);

    }

    private void Update()
    {
       //in a nuteshell this is the time passed.
        HandleListCooldown();
        DebugCreateLaserAim();


       



    }
    private void FixedUpdate()
    {
        HandleShield();
        HandleGunCharge();

        

        //TESTING

        //so the problem is simply the calling.

  
    }

    void DebugCreateLaserAim()
    {
        if (gunList[currentGunIndex] == null)
        {
            return;
        }
        if (gunList[currentGunIndex].data == null)
        {
            return;
        }

        return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = (transform.position - mousePos).normalized;
        Vector3 fixedDir = new Vector3(dir.x, 0, dir.z);

        debugLineRenderer.SetPosition(0, gunList[currentGunIndex].gunPoint.position);
        debugLineRenderer.SetPosition(1, fixedDir * 100 * -1);
    }

    public void SetUp(ItemGunData initialWeapon)
    {
        //we can never change this stuff so its an array.

        if (gunList.Length > 0) return;

        gunList = new GunClass[] { new GunClass(), new GunClass(), new GunClass() };
        currentGunIndex = 0;


        GameHandler.instance.cityDataHandler.cityArmory.AddGunWithIndex(initialWeapon.storeIndex);
        ReceivePermaGun(initialWeapon);

        if(debugGunDataTemp1 != null)
        {
            ReceiveTempGunIfEmptySlot(debugGunDataTemp1);
        }
        if(debugGunDataTemp2 != null)
        {
            ReceiveTempGunIfEmptySlot(debugGunDataTemp2);
        }

       

    }

    //we might give more time for everything to load in teh sceneloader.
    IEnumerator CallThisLater()
    {        
        yield return new WaitForSeconds(2);

        if (CityHandler.instance != null)
        {
            //UIHandler.instance.debugui.UpdateDEBUGUI("DID CALL THIS");
            
        }
        else
        {
            //UIHandler.instance.debugui.UpdateDEBUGUI("DIDNT CALL THIS");
        }
    }

    //why is the thing not updating. it will update in update just to test it.

    public void ResetPlayerCombat()
    {
        for (int i = 1; i < gunList.Length; i++)
        {
            if (gunList[i].data == null) continue;
            Destroy(gunList[i].gunModel);
            if(i != 0)  UIHandler.instance.gunUI.ClearOwnedGunUnit(i - 1);
            gunList[i].ResetGunClass();
        }

        gunList[0].data.RemoveGunPassives();
        gunList[0].data.AddGunPassives();

        ShieldRemove(-1);

        gunList[0].ReloadGunForFree();

        OrderSwapGun(0, true);
        UpdateRiggingAndAnimationForCurrentGun();



    }

    public void ControlGunHolderVisibility(bool isVisible)
    {

        //we tell each fella to disappear with its gun.

        foreach (var item in gunList)
        {
            item.ControlModelVisibility(isVisible);
        }

    }


    #region GETTER
    public ItemGunData GetCurrentPermaGun()
    {
        return gunList[0].data;
    }


    public int GetCurrentGunIndex { get { return currentGunIndex; } }



    #endregion

    #region RECEIVE
    public void ReceivePermaGun(ItemGunData gun)
    {

        //we replace the first one.
       

        if (gunList[0].data != null)
        {
            //then we just adad the data to the first
            gunList[0].data.RemoveGunPassives();
            Destroy(gunList[0].gunModel);
        }
        if(gunList.Length <= 0)
        {
            Debug.Log("FOUND NOTHING");
            return;
        }

        //either we willl put the data.
        GameObject spawnedModel = CreateGunModel(gun);

        gunList[0] = new GunClass(handler, gun, spawnedModel, gunHolsterHolderList[0]);       
        gunList[0].MakeAmmoInfinite();
        gunList[0].data.AddGunPassives();

        UpdateSecretValues();

        //UIHandler.instance.gunUI.SetOwnedGunUnit(gunList[0], 0);
        // UIHandler.instance.gunUI.ChangeOwnedGunShowUnit(0);
        gunList[0].StoreWeapon();

        if (currentGunIndex == 0)
        {
            SwapGunModel();
        }

        if (CityHandler.instance != null)
        {
            CityHandler.instance.UpdateGunListUsingCurrentPermaGun();
        }

    }

    public void ReceiveTempGunIfEmptySlot(ItemGunData data)
    {
        //if there is no empty space the player shouldKeepChecking choose.
        int emptyIndex = GetGunEmptySlot();

        data.SetHasBeenFound(true);

        if(emptyIndex == -1)
        {
            Debug.LogError("RECEIVE FOR EMPTY SLOT BUT THERE IS NO EMPTY SLOT");
            return;
        }

        if (gunList[emptyIndex].data != null) 
        {
            Debug.LogError("RECEIVE FOR EMPTY BUT THERE IS DATA HERE");
            return;
        }


        GameObject spawnedModel = CreateGunModel(data);
        gunList[emptyIndex] = new GunClass(handler, data, spawnedModel, gunHolsterHolderList[emptyIndex]);
        UIHandler.instance.gunUI.SetOwnedGunUnit(gunList[0], gunList[emptyIndex], emptyIndex - 1);
        gunList[emptyIndex].StoreWeapon();

        UpdateSecretValues();

        if (emptyIndex == currentGunIndex)
        {
            SwapGunModel();
        }

    }
    public void ReceiveTempGunToReplace(ItemGunData data, int index)
    {

        data.SetHasBeenFound(true);

        if (gunList.Length <= 0)
        {
            Debug.Log("FOUND NOTHING");
            return;
        }

        if (gunList[index].data != null)
        {
            Destroy(gunList[index].gunModel);
        }


        //Debug.Log("received wepaon " + data.itemName);
        //Debug.Log("to replace " + ownedGunList[currentBulletIndex].data.itemName);

        GameObject spawnedModel = CreateGunModel(data);
        spawnedModel.transform.rotation = Quaternion.Euler(0,-90,0);


        gunList[index] = new GunClass(handler, data, spawnedModel, gunHolsterHolderList[index]);
        UIHandler.instance.gunUI.SetOwnedGunUnit(gunList[0], gunList[index], index - 1);
        gunList[index].StoreWeapon();

        UpdateSecretValues();

        if (index == currentGunIndex)
        {
            SwapGunModel();
        }

    }

    
    GameObject CreateGunModel(ItemGunData data)
    {
        
        GameObject newObject = Instantiate(data.gunModel, gunSpawnPos.transform.position, data.gunModel.transform.localRotation);
        
        //newObject.SetActive(false);
        newObject.transform.SetParent(gunSpawnPos);
        newObject.transform.localPosition = data.gunModel.transform.localPosition;
        newObject.transform.localRotation = data.gunModel.transform.localRotation;
        newObject.transform.localScale = data.gunModel.transform.localScale;

        return newObject;
    }


    public int GetGunEmptySlot()
    {
        //we only care 
        for (int i = 1; i < gunList.Length; i++)
        {
            if (gunList[i].data == null) return i;
        }
        return -1;

    }

    public GunClass[] GetTempGuns()
    {
        GunClass[] tempGuns = new GunClass[] { gunList[1], gunList[2] };
        return tempGuns;
    }

    #endregion

    #region SWAP GUN


    public void OrderSwapGun(int index, bool insta = false)
    {
        //we are not going to check everyone anymore

        //we are going to do the following
        //if the requested gun is 0 and the current weapon is 0 then we do nothing
        //if the requested gun is not 0, and the requested gun is NOT equippuied then we equip the main gun
        //if the requested gun is not 0, and its equipped then we will equip 0


        if (isSwapping)
        {
            //then we simply dont care.

            return;
        }



        if (gunList[index].data == null) return; //this means this gun doesnt exist
        if(index == 0 && currentGunIndex == 0) return;

        int lastGun = currentGunIndex;
        int newIndex = 0; 


        if (!gunList[index].isEquipped)
        {
            newIndex = index;
        }

        

        CancelCharge();


        if(lastGun != currentGunIndex)
        {
            GameHandler.instance._soundHandler.CreateSfx_WithAudioClip(audio_Swap);
        }

        
        CancelReload();


        if (insta)
        {
            UnselectCurrentGunModel();
            currentGunIndex = index;
            SwapGunModel();
            ResetHoldShoot();
            MakeTrueBlock();
            UIHandler.instance.gunUI.UpdateGunShowUnit();
            UIHandler.instance._MouseUI.UpdateMouseUI(gunList[currentGunIndex].data.mouseImageType);
            CancelSwap();
        }
        else
        {
            swapProcess = StartCoroutine(SwapGunProcess(newIndex));
        }

        



    }
    void UnselectCurrentGunModel()
    {
        if (gunList.Length <= 0)
        {
            Debug.Log("FOUND NOTHING");
            return;
        }

        if(gunList[currentGunIndex].data == null)
        {
            //Debug.Log("NO DATA HERE");
            return;
        }



        gunList[currentGunIndex].StoreWeapon();
    }
    void SwapGunModel()
    {
        if (gunList.Length <= 0)
        {
            Debug.Log("FOUND NOTHING");
            return;
        }

        if (gunList[currentGunIndex].data == null)
        {
            Debug.Log("yo");
            return;
        }

        audioSrc_Reload.clip = gunList[currentGunIndex].data.Get_Audio_Reload;

        //UIHandler.instance.gunUI.UpdateGunPortrait(gunList[currentGunIndex].data.itemIcon);
        UIHandler.instance.gunUI.UpdateGunTitle(gunList[currentGunIndex].data.itemName);
        UIHandler.instance.gunUI.UpdateAmmoGun(gunList[currentGunIndex].ammoCurrent, gunList[currentGunIndex].ammoReserve);

        gunList[currentGunIndex].EquipWeaponInHand();


        UpdateRiggingAndAnimationForCurrentGun();
    }

    bool isSwapping;
    Coroutine swapProcess;
    IEnumerator SwapGunProcess(int targetIndex)
    {
        //a process like reaload.

        isSwapping = true;

        float current = 0;
        float total = 0.4f;

        while (total > current)
        {
            current += Time.deltaTime;
            _gunUI.UpdateSwapFill(current, total);
            yield return new WaitForSecondsRealtime(Time.deltaTime);
        }

        //in the end we change it.
        UnselectCurrentGunModel();
        currentGunIndex = targetIndex;
        SwapGunModel();
        ResetHoldShoot();
        MakeTrueBlock();


        UIHandler.instance.gunUI.UpdateGunShowUnit();

        UIHandler.instance._MouseUI.UpdateMouseUI(gunList[currentGunIndex].data.mouseImageType);

        CancelSwap();
    }

    public void CancelSwap()
    {

        if (!isSwapping) return;
        //audioSrc_Reload.Stop();
        StopCoroutine(swapProcess);
        _gunUI.UpdateSwapFill(0, 0);
        isSwapping = false;
    }

    #endregion

    #region RELOAD

    public bool isReloading { get; private set; }

    public void Reload()
    {
        CancelSwap();
        if (gunList[currentGunIndex].data == null)
        {
            Debug.Log("there is no gun. cannot reload");
            return;
        }
        if (isReloading)
        {
            Debug.Log("was reloading");
            return;
        }

        if (!gunList[currentGunIndex].CanReload() || !gunList[currentGunIndex].HasReserveAmmo())
        {
            return;
        }

        CancelCharge();
        

        StopCoroutine(nameof(ReloadProcess));
        reloadProcess = StartCoroutine(ReloadProcess());
    }

    Coroutine reloadProcess;
    IEnumerator ReloadProcess()
    {
        isReloading = true;

        float current = 0;
        float valueRef = gunList[currentGunIndex].GetGunTotalStat(StatType.ReloadSpeed);
        float total = valueRef;
        float reloadModifier = handler._entityStat.GetTotalValue(StatType.ReloadSpeed) * total;
        total -= reloadModifier;
        total = Mathf.Clamp(total, valueRef/ 2, valueRef);

        audioSrc_Reload.Play();


        handler._entityAnimation.CallAnimation_Reload(total);

        while (total > current)
        {
            current += Time.deltaTime;
            _gunUI.UpdateReloadFill(current, total);
            yield return new WaitForSecondsRealtime(Time.deltaTime);
        }


        handler._entityAnimation.StopAnimation_Reload();

        gunList[currentGunIndex].ReloadGun();

         handler._entityEvents.OnReloadedGun(gunList[currentGunIndex].data);
        _gunUI.UpdateReloadFill(0, 0);
        _gunUI.UpdateAmmoGun(gunList[currentGunIndex].ammoCurrent, gunList[currentGunIndex].ammoReserve);
        _gunUI.UpdateAmmoInOwnedGunShowUnit(currentGunIndex, GetCurrentGun.ammoCurrent, GetCurrentGun.ammoReserve);

        isReloading = false;
    }

    public void CancelReload()
    {
        if (!isReloading) return;
        audioSrc_Reload.Stop();
        StopCoroutine(reloadProcess);
        _gunUI.UpdateReloadFill(0, 0);
        isReloading = false;
    }

    public void RefreshAllReserveAmmo()
    {
        gunList[1].RefreshReserveAmmo();
        gunList[2].RefreshReserveAmmo();
        _gunUI.UpdateAmmoGun(gunList[currentGunIndex].ammoCurrent, gunList[currentGunIndex].ammoReserve);

    }

    public void RecoverReserveAmmoByPercent(float percent)
    {
        gunList[1].ReplenishReserveAmmoBasedInTotal(percent);
        gunList[2].ReplenishReserveAmmoBasedInTotal(percent);
        _gunUI.UpdateAmmoGun(gunList[currentGunIndex].ammoCurrent, gunList[currentGunIndex].ammoReserve);
    }

    public void FullInstantReload()
    {
        gunList[currentGunIndex].ReloadGunForFree();
        _gunUI.UpdateAmmoGun(gunList[currentGunIndex].ammoCurrent, gunList[currentGunIndex].ammoReserve);
        _gunUI.UpdateAmmoInOwnedGunShowUnit(currentGunIndex, gunList[currentGunIndex].ammoCurrent, GetCurrentGun.ammoReserve);

    }



    #endregion

    #region SHIELD
    //define the shield system here.

    float shieldTotal;
    float shieldCurrent;

    float shieldModifier; //the value_Level we will use here. 

    float shieldRegenTotal;
    float shieldRegenCurrent;   

    //we change the shield everytime the stats change? or maybe just a flat shield? for now it

    public void ShieldSet(float shieldValue)
    {
        //set the event

        shieldTotal = shieldValue;
        shieldCurrent = shieldTotal;
        UIHandler.instance._playerUI.UpdateShield(shieldCurrent, shieldTotal);

        shieldRegenCurrent = 0;
        shieldRegenTotal = 5;
        UIHandler.instance._playerUI.UpdateShieldRegen(shieldCurrent, shieldTotal);

        handler._entityEvents.eventDamageTaken += ShieldRegenReset;

        
    }

    public void Shield_Recharge_Percent(float value)
    {
        float valueToRecharge = shieldTotal * value;

        shieldCurrent += valueToRecharge;
        shieldCurrent = Mathf.Clamp(shieldCurrent, 0, shieldTotal);

        UIHandler.instance._playerUI.UpdateShield(shieldCurrent, shieldTotal);
    }

    public void ShieldRemove(float shieldValue)
    {
        if(shieldTotal == shieldValue || shieldValue == -1)
        {
            shieldTotal = 0;
            handler._entityEvents.eventDamageTaken -= ShieldRegenReset;
            UIHandler.instance._playerUI.UpdateShield(0, 0);
        }
        else
        {
            //otherwise there is another shield.
        }


    }

    public float ShieldReduceDamage(float totalDamage, List<DamageTypeClass> damageList)
    {
        if (shieldTotal == 0)
        {
            return totalDamage;
        }

        float additionalDamageOnlyToShield = 0;

        foreach (var item in damageList)
        {
            if(item._damageType == DamageType.Magical)
            {
                float additionalMagicalDamage = item._value * 0.2f;
                additionalDamageOnlyToShield += additionalMagicalDamage;    
            }
            if(item._damageType == DamageType.Pure)
            {
                Debug.Log("there should be no pure damage here");
            }


        }



    
        float valueToReturn = 0;
        float valueToReduce = 0;

        if(shieldCurrent > totalDamage + additionalDamageOnlyToShield)
        {
            valueToReduce = totalDamage + additionalDamageOnlyToShield;
            valueToReturn = 0;
        }
        else
        {
            valueToReduce = shieldCurrent;
            valueToReturn = totalDamage - shieldCurrent ;

        }

        shieldCurrent -= valueToReduce;
        shieldCurrent = Mathf.Clamp(shieldCurrent, 0, shieldTotal);

        UIHandler.instance._playerUI.UpdateShield(shieldCurrent, shieldTotal);

        return valueToReturn;
    }

    void CallShieldRegen()
    {
        shieldRegenCurrent = 0;

        shieldCurrent = shieldTotal;

        UIHandler.instance._playerUI.UpdateShield(shieldCurrent, shieldTotal);
    }

    void ShieldRegenReset()
    {
        shieldRegenCurrent = 0;
    }
    public void HandleShield()
    {
        //if shield total_Damage is higher

        if(shieldTotal > 0 && shieldTotal > shieldCurrent)
        {
            if (shieldRegenCurrent >shieldRegenTotal)
            {
                //we only check this if we havent taken damdamage.
                CallShieldRegen();              
            }
            else
            {
                shieldRegenCurrent += Time.fixedDeltaTime;
            }

            UIHandler.instance._playerUI.UpdateShieldRegen(shieldRegenCurrent, shieldRegenTotal);

        }

    }


    #endregion

    #region STUFF FOR UPGRADE STATION
    public bool CanUseGunStation()
    {
        return currentGunIndex != 0;
    }

    public void RemoveCurrentGun_ForUpgradeStation()
    {
        if(currentGunIndex == 0)
        {
            Debug.Log("this is the perma");
            return;
        }

        if (gunList[currentGunIndex].data == null)
        {
            Debug.Log("no data here");
            return;
        }

        Destroy(gunList[currentGunIndex].gunModel);
        UIHandler.instance.gunUI.ClearOwnedGunUnit(currentGunIndex);

        //then we move to the next data
        OrderSwapGun(0);

    }
    public void ReceiveTempGun_FromUpgradeStation()
    {
        //then we check if there is space. if there isnt we cannot pick it up.
        //we put them in either 1 or 2. whatever is 


        //the data never left.
        //but now we are going to call it back
        //but i need to know what currentBulletIndex i did this.

        //we need to find the fella 
        int indexToUse = 0;


        if (gunList[1].data != null)
        {
            if (gunList[1].isInUpgradeStation)
            {               
                indexToUse += 1;
            }
        }
        if (gunList[2].data != null)
        {
            if (gunList[2].isInUpgradeStation)
            {
                indexToUse += 2;
            }
        }
        if (indexToUse == 3)
        {
            Debug.Log("both guns are in upgrade station for some reason");
            return;
        }

        if (indexToUse == 0)
        {
            Debug.Log("found nothing for some reason");
            return;
        }


        //we will pass the information for teh right gunlist to copy.
        //


        GameObject spawnedModel = CreateGunModel(gunList[indexToUse].data);
        gunList[indexToUse].SetGunModel(spawnedModel, gunHolsterHolderList[indexToUse]);
        //UIHandler.instance.gunUI.SetOwnedGunUnit(dropList[indexToUse], indexToUse);
        UIHandler.instance.gunUI.ShowOwnedGunUnit(indexToUse);
        currentGunIndex = indexToUse;   
        SwapGunModel();
    }

    #endregion

    #region EQUIP RIGGING
    //instead of just rotating we have to do things here
    //we have animation for different gun_Perma holdings
    //we always place the gun_Perma at the right hand.
    //we only worry about this when 

    //so basically there is two spot

    void UpdateRiggingAndAnimationForCurrentGun()
    {
        //we detect what weapon is it
        //we tell the animation 
        //but i still want a way
        //change the animation

        handler._entityAnimation.SetStateUpperBody(GetCurrentGun.data.animationType);
    }




    #endregion


    void HandleListCooldown()
    {
        foreach (var item in gunList)
        {
            item.HandleCooldown();
        }
    }

    #region SHOOT

    [SerializeField]List<BulletBehavior> forcedBulletBehaviorList = new(); //we use this when we want to force all weapons to have a behavior.

    int forcedGoThroughPower; //everytime we change weapons we should set this.

    public void ForcedGoThroughPower_Increase(int value)
    {
        forcedGoThroughPower += value;
    }
    public void ForcedGoThroughPower_Decrease(int value)
    {
        forcedGoThroughPower -= value;
    }
    public void ForcedGoThroughPower_Set(int value)
    {
        forcedGoThroughPower = value;
    }

    public void AddForcedBulletBehavior(BulletBehavior bulletBehavior)
    {
        forcedBulletBehaviorList.Add(bulletBehavior);
        
    }
    public void RemoveForcedBulletBehavior(BulletBehavior bulletBehavior)
    {
        for (int i = 0; i < forcedBulletBehaviorList.Count; i++)
        {
            if (forcedBulletBehaviorList[i].id == bulletBehavior.id)
            {
                forcedBulletBehaviorList.RemoveAt(i);
                return;
            }
        }
       
    }

    public void Shoot(Vector3 shootDir)
    {
        if (gunList[currentGunIndex].data == null)
        {
            //something wrong 
            Debug.Log("there is no gun current. cannot shoot");
            return;
        }


        if (isReloading) return; //the player is stuck in animation


        bool mustReload = gunList[currentGunIndex].MustReload();

        if (mustReload)
        {
            //reload         
            Reload();
            return;
        }

        if (trueBlockForShooting)
        {
            return;
        }

        if (!gunList[currentGunIndex].data.canHoldDownButtonToKeepShooting && keepShootingIfCanHold)
        {
            return;
        }

        keepShootingIfCanHold = true;


        //we are also going to chgeck if this is charge. if it is we dont shoot right now.

        if (gunList[currentGunIndex].data.GetGunCharge() != null)
        {
            //we clal the thing to start charging.
            StartCharge();
        }
        else
        {

            gunList[currentGunIndex].Shoot(shootDir, forcedBulletBehaviorList);
            UIHandler.instance._MouseUI.Shoot();
            _gunUI.UpdateAmmoGun(GetCurrentGun.ammoCurrent, GetCurrentGun.ammoReserve);
            //_gunUI.UpdateAmmoInOwnedGunShowUnit(currentGunIndex, GetCurrentGun.ammoCurrent, GetCurrentGun.ammoReserve);
        }


        
    }

    public void ResetHoldShoot()
    {
        keepShootingIfCanHold = false;
        trueBlockForShooting = false;
    }


    #endregion

    public void MakeTrueBlock()
    {
        trueBlockForShooting = true;
    }

    public void RefundCurrentAmmo()
    {
        //
        gunList[currentGunIndex].RefundAmmo(0.1f);
        _gunUI.UpdateAmmoGun(gunList[currentGunIndex].ammoCurrent, gunList[currentGunIndex].ammoReserve);
    }

    #region SECRET STATS



    public float secretStatMultipleBulletFlat {  get; private set; }

    public float secretStatMultipleBulletMultiplier { get; private set; }

    public float secretStatMultipleBulletDamageModifier {  get; private set; }


    public void MakeSecretStatMultipleBulletFlat(float value)
    {
        secretStatMultipleBulletFlat = value;
        UpdateSecretValues();

    }
    public void MakeSecretStatMultipleBulletPercent(float value)
    {
        secretStatMultipleBulletMultiplier = value;
        UpdateSecretValues();
    }
    public void MakeSecretStatMultipleBulletDamageModifier(float value)
    {
        secretStatMultipleBulletDamageModifier = value;
        UpdateSecretValues();
    }

    void UpdateSecretValues()
    {
        foreach (var item in gunList)
        {
            if (item.data == null) continue;

            item.MakeSecretStats(secretStatMultipleBulletFlat, secretStatMultipleBulletMultiplier, secretStatMultipleBulletDamageModifier); ;
        }
    }

    #endregion


    #region GUN CHARGE
    bool isCharging;

    public void CallCharge()
    {
        //here we order the data charge to happen.
        //it can only be the current weapon so we will check that.
        Vector3 currentMousePos = handler._playerController.GetMouseDirection();

        gunList[currentGunIndex].Shoot(currentMousePos, forcedBulletBehaviorList);
        _gunUI.UpdateAmmoGun(gunList[currentGunIndex].ammoCurrent, gunList[currentGunIndex].ammoReserve);
        _gunUI.UpdateAmmoInOwnedGunShowUnit(currentGunIndex, gunList[currentGunIndex].ammoCurrent, GetCurrentGun.ammoReserve);

        CancelCharge();
    }

    public void StartCharge()
    {
        if (isCharging)
        {
            Debug.Log("caught 1");
            return;
        }
        if(gunList[currentGunIndex] == null)
        {
            Debug.Log("caught 1 ");
            return;
        }
        if (!gunList[currentGunIndex].IsGunCharge())
        {
            Debug.Log("caught 2");
            return;
        }

        Debug.Log("told it to start charging");
        isCharging = true;

    }

    public void CancelCharge()
    {
        if (gunList[currentGunIndex] != null)
        {

            isCharging = false;
            _gunUI.UpdateChargeInOwnedGunShowUnit(currentGunIndex, 0, 1);
            gunList[currentGunIndex].StopChargeGun();
        }
    }

    void HandleGunCharge()
    {
        //here we will call the data.
        if (!isCharging) return;

        if (gunList[currentGunIndex] != null)
        {
            Debug.Log("handle gun charge");

            gunList[currentGunIndex].HandleGunCharge();
            float[] floatArray  = gunList[currentGunIndex].GetProgressValues();

            _gunUI.UpdateChargeInOwnedGunShowUnit(currentGunIndex, floatArray[0], floatArray[1]);

            if (gunList[currentGunIndex].IsGunChargeReady())
            {
                //we call the shot using info from player controller.
                CallCharge();
            }

        }

    }



    #endregion
}

