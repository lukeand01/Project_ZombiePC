using MyBox;
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
    int currentGunIndex;

    GunUI _gunUI;
    bool keepShootingIfCanHold;
    bool trueBlockForShooting; //this is used to block to hold fire while swapping

    [Separator("SOUND")]
    [SerializeField] AudioSource audioSrc_Reload;
    [SerializeField] AudioClip audio_Swap;


    private void Awake()
    {
        handler = GetComponent<PlayerHandler>();

    }

    private void Start()
    {

        SetUp();
        _gunUI = UIHandler.instance.gunUI;
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

    void SetUp()
    {
        //we can never change this stuff so its an array.
        gunList = new GunClass[] { new GunClass(), new GunClass(), new GunClass() };
        currentGunIndex = 0;

        if(debugGunDataPerma != null)
        {
            ReceivePermaGun(debugGunDataPerma);
        }
        if(debugGunDataTemp1 != null)
        {
            ReceiveTempGunIfEmptySlot(debugGunDataTemp1);
        }
        if(debugGunDataTemp2 != null)
        {
            ReceiveTempGunIfEmptySlot(debugGunDataTemp2);
        }


       UIHandler.instance._EquipWindowUI.GetEquipForPermaGun(gunList[0]);

        if(CityHandler.instance != null)
        {
            CityHandler.instance.UpdateGunListUsingCurrentPermaGun(gunList[0].data);
        }

    }


    public void ResetPlayerCombat()
    {
        for (int i = 1; i < gunList.Length; i++)
        {
            if (gunList[i].data == null) continue;
            Destroy(gunList[i].gunModel);
            UIHandler.instance.gunUI.ClearOwnedGunUnit(i);
            gunList[i].ResetGunClass();
        }

        gunList[0].data.RemoveGunPassives();
        gunList[0].data.AddGunPassives();

        ShieldRemove(-1);
    }

    public void ControlGunHolderVisibility(bool isVisible)
    {
        gunSpawnPos.gameObject.SetActive(isVisible);
    }


    #region GETTER
    public ItemGunData GetCurrentPermaGun()
    {
        return gunList[0].data;
    }
    public GunClass GetCurrentGun()
    {
        return gunList[currentGunIndex];
    }

    #endregion

    #region RECEIVE
    public void ReceivePermaGun(ItemGunData gun)
    {
        //we replace the first one.
        if (gunList[0].data != null)
        {
            //then we just adad the gun to the first
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

        gunList[0] = new GunClass(handler, gun, spawnedModel);       
        gunList[0].MakeAmmoInfinite();
        gunList[0].data.AddGunPassives();

        UpdateSecretValues();

        UIHandler.instance.gunUI.SetOwnedGunUnit(gunList[0], 0);
        UIHandler.instance.gunUI.ChangeOwnedGunShowUnit(0);


        if(currentGunIndex == 0)
        {
            SwapGunModel();
        }

        if (CityHandler.instance != null)
        {
            CityHandler.instance.UpdateGunListUsingCurrentPermaGun(gunList[0].data);
        }


    }

    public void ReceiveTempGunIfEmptySlot(ItemGunData gun)
    {
        //if there is no empty space the player shouldKeepChecking choose.
        int emptyIndex = GetGunEmptySlot();

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


        GameObject spawnedModel = CreateGunModel(gun);
        gunList[emptyIndex] = new GunClass(handler, gun, spawnedModel);
        UIHandler.instance.gunUI.SetOwnedGunUnit(gunList[emptyIndex], emptyIndex);

        UpdateSecretValues();

        if (emptyIndex == currentGunIndex)
        {
            SwapGunModel();
        }

    }
    public void ReceiveTempGunToReplace(ItemGunData data, int index)
    {



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
        //Debug.Log("to replace " + ownedGunList[index].data.itemName);

        GameObject spawnedModel = CreateGunModel(data);
        spawnedModel.transform.rotation = Quaternion.Euler(0,-90,0);


        gunList[index] = new GunClass(handler, data, spawnedModel);
        UIHandler.instance.gunUI.SetOwnedGunUnit(gunList[index], index);

        UpdateSecretValues();

        if (index == currentGunIndex)
        {
            SwapGunModel();
        }

    }

    
    GameObject CreateGunModel(ItemGunData data)
    {
        

        GameObject newObject = Instantiate(data.gunModel, gunSpawnPos.transform.position, data.gunModel.transform.localRotation);
        
        newObject.SetActive(false);
        newObject.transform.SetParent(gunSpawnPos);

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


    public void OrderSwapGun()
    {


        UnselectCurrentGunModel();

        bool done = false;

        int lastGun = currentGunIndex;

        while (!done)
        {
            int safeBreak = 0;

            safeBreak++;

            if (safeBreak > 100)
            {
                Debug.LogError("COUDLTN FIND ANY AVAIALBE GUN " + currentGunIndex);
                break;
            }

            currentGunIndex++;


           
            if (currentGunIndex >= gunList.Length)
            {
                currentGunIndex = 0;
            }
            if (gunList[currentGunIndex] == null)
            {
                continue;
            }
            if (gunList[currentGunIndex].data == null)
            {
                continue;
            }
            if (gunList[currentGunIndex].isInUpgradeStation)
            {
                continue;
            }


            done = true;
        }

        if(lastGun != currentGunIndex)
        {
            GameHandler.instance._soundHandler.CreateSfx(audio_Swap);
        }

        CancelReload();

        _gunUI.ChangeOwnedGunShowUnit(currentGunIndex);
        SwapGunModel();
        ResetHoldShoot();
        MakeTrueBlock();


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
            
        

        gunList[currentGunIndex].gunModel.SetActive(false);
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

        UIHandler.instance.gunUI.UpdateGunPortrait(gunList[currentGunIndex].data.itemIcon);
        UIHandler.instance.gunUI.UpdateGunTitle(gunList[currentGunIndex].data.itemName);
        UIHandler.instance.gunUI.UpdateAmmoGun(gunList[currentGunIndex].ammoCurrent, gunList[currentGunIndex].ammoReserve);

        gunList[currentGunIndex].gunModel.SetActive(true);
    }

    

    #endregion

    #region RELOAD

    bool isReloading;

    public void Reload()
    {
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
            Debug.Log("not allowed to reload");
            return;
        }

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

        while (total > current)
        {
            current += Time.deltaTime;
            _gunUI.UpdateReloadFill(current, total);
            yield return new WaitForSecondsRealtime(Time.deltaTime);
        }

        gunList[currentGunIndex].ReloadGun();

         handler._entityEvents.OnReloadedGun(gunList[currentGunIndex].data);
        _gunUI.UpdateReloadFill(0, 0);
        _gunUI.UpdateAmmoGun(gunList[currentGunIndex].ammoCurrent, gunList[currentGunIndex].ammoReserve);
        _gunUI.UpdateAmmoInOwnedGunShowUnit(currentGunIndex, gunList[currentGunIndex].ammoCurrent);

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

    public void FullInstantReload()
    {
        gunList[currentGunIndex].ReloadGunForFree();
        _gunUI.UpdateAmmoGun(gunList[currentGunIndex].ammoCurrent, gunList[currentGunIndex].ammoReserve);
        _gunUI.UpdateAmmoInOwnedGunShowUnit(currentGunIndex, gunList[currentGunIndex].ammoCurrent);

    }

    #endregion

    #region SHIELD
    //define the shield system here.

    float shieldTotal;
    float shieldCurrent;

    float shieldModifier; //the value we will use here. 

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

    public float ShieldReduceDamage(float damage)
    {
        if (shieldTotal == 0)
        {

            return damage;
        }

        float valueToReturn = 0;
        float valueToReduce = 0;

        if(shieldCurrent > damage)
        {
            valueToReduce = damage;
            valueToReturn = 0;
        }
        else
        {
            valueToReduce = shieldCurrent;
            valueToReturn = damage - shieldCurrent ;

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
        //if shield total is higher

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

        //then we move to the next gun
        OrderSwapGun();

    }
    public void ReceiveTempGun_FromUpgradeStation()
    {
        //then we check if there is space. if there isnt we cannot pick it up.
        //we put them in either 1 or 2. whatever is 


        //the gun never left.
        //but now we are going to call it back
        //but i need to know what index i did this.

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
        gunList[indexToUse].SetGunModel(spawnedModel);
        //UIHandler.instance.gunUI.SetOwnedGunUnit(gunList[indexToUse], indexToUse);
        UIHandler.instance.gunUI.ShowOwnedGunUnit(indexToUse);
        currentGunIndex = indexToUse;   
        SwapGunModel();
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


        gunList[currentGunIndex].Shoot(shootDir, forcedBulletBehaviorList);
        _gunUI.UpdateAmmoGun(gunList[currentGunIndex].ammoCurrent, gunList[currentGunIndex].ammoReserve);
        _gunUI.UpdateAmmoInOwnedGunShowUnit(currentGunIndex, gunList[currentGunIndex].ammoCurrent);
        
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


   
}

