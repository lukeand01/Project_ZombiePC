using MyBox;
using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        handler = GetComponent<PlayerHandler>();
        SetUp();
    }

    private void Start()
    {
        _gunUI = UIHandler.instance.gunUI;
    }

    private void Update()
    {
       //in a nuteshell this is the time passed.
       HandleListCooldown();
        DebugCreateLaserAim();

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
    }

    #region RECEIVE
    public void ReceivePermaGun(ItemGunData gun)
    {
        //we replace the first one.
        if (gunList[0].data != null)
        {
            //then we just adad the gun to the first
            Destroy(gunList[0].gunModel);
        }
        if(gunList.Length <= 0)
        {
            Debug.Log("FOUND NOTHING");
            return;
        }


        //either we willl put the data.
        GameObject spawnedModel = CreateGunModel(gun);

        gunList[0] = new GunClass(gun, spawnedModel);
        gunList[0].MakeAmmoInfinite();

        if(currentGunIndex == 0)
        {
            SwapGunModel();
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
        gunList[emptyIndex] = new GunClass(gun, spawnedModel);    

        if(emptyIndex == currentGunIndex)
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

        GameObject spawnedModel = CreateGunModel(data);
        gunList[index] = new GunClass(data, spawnedModel);

        if (index == currentGunIndex)
        {
            SwapGunModel();
        }

    }

    GameObject CreateGunModel(ItemGunData data)
    {
        GameObject newObject = Instantiate(data.gunModel, gunSpawnPos.transform.position, data.gunModel.transform.rotation);
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


    #endregion

    #region SWAP GUN


    public void OrderSwapGun()
    {


        UnselectCurrentGunModel();

        bool done = false;

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

            Debug.Log("call this");


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

            done = true;
        }

        
         
        SwapGunModel();
        


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
            Debug.Log("NO DATA HERE");
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


        UIHandler.instance.gunUI.UpdateGunPortrait(gunList[currentGunIndex].data.itemIcon);
        UIHandler.instance.gunUI.UpdateAmmoGun(gunList[currentGunIndex].ammoCurrent, gunList[currentGunIndex].ammoReserve);

        gunList[currentGunIndex].gunModel.SetActive(true);
    }



    void ReloadCurrentGun()
    {

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
        float total = gunList[currentGunIndex].data.reloadSpeed;
        //float reloadModifier = handler.playerStats.GetStatTotalValue(StatType.ReloadSpeed) * total;
        //total -= reloadModifier;


        while (total > current)
        {
            current += Time.deltaTime;
            _gunUI.UpdateReloadFill(current, total);
            yield return new WaitForSecondsRealtime(Time.deltaTime);
        }

        gunList[currentGunIndex].ReloadGun();


        _gunUI.UpdateReloadFill(0, 0);
        _gunUI.UpdateAmmoGun(gunList[currentGunIndex].ammoCurrent, gunList[currentGunIndex].ammoReserve);

        isReloading = false;
    }

    public void CancelReload()
    {
        if (!isReloading) return;
        StopCoroutine(reloadProcess);
        _gunUI.UpdateReloadFill(0, 0);
        isReloading = false;
    }

    #endregion


    void HandleListCooldown()
    {
        foreach (var item in gunList)
        {
            item.HandleCooldown();
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



        gunList[currentGunIndex].Shoot(shootDir);
        _gunUI.UpdateAmmoGun(gunList[currentGunIndex].ammoCurrent, gunList[currentGunIndex].ammoReserve);


    }


}
