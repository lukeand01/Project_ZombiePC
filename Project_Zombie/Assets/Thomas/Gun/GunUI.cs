using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunUI : MonoBehaviour
{
    GameObject holder;

    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;
    }

    public void ControlUI(bool isVisible)
    {
        holder.SetActive(isVisible);
    }


    [SerializeField] TextMeshProUGUI currentAmmoText;
    [SerializeField] TextMeshProUGUI reserveAmmoText;
    [SerializeField] Image reloadFillImage;
    [SerializeField] GameObject reloadHolder;
    [SerializeField] TextMeshProUGUI gunTitleText;

    [Separator("SWAP")]
    [SerializeField] GameObject swapHolder;
    [SerializeField] Image swapFillImage;


    public void UpdateGunTitle(string name)
    {
        gunTitleText.text = name;
    }

    public void UpdateAmmoGun(int current, int reserve)
    {
        currentAmmoText.text = current.ToString();
        reserveAmmoText.text = reserve.ToString();


        if(reserve == -1)
        {
            reserveAmmoText.text = "?";
        }

        if(reserve == 0)
        {
            reserveAmmoText.color = Color.red;
        }
        else
        {
            reserveAmmoText.color = Color.white;
        }

        if (current == 0)
        {
            currentAmmoText.color = Color.red;
        }
        else
        {
            currentAmmoText.color = Color.white;
        }


    }

    public void UpdateReloadFill(float current, float total)
    {
        reloadHolder.SetActive(total > 0);
        swapHolder.SetActive(false);

        reloadFillImage.fillAmount = current / total;
    }

    public void UpdateSwapFill(float current, float total)
    {
        reloadHolder.SetActive(false);
        swapHolder.SetActive(total > 0);

        swapFillImage.fillAmount = current / total;
    }

    [SerializeField] OwnedGunShowUnit[] ownedGunShowUnits;

    private void Start()
    {
        for (int i = 0; i < ownedGunShowUnits.Length; i++)
        {
            var item = ownedGunShowUnits[i];
            item.UpdateKeyText(i + 1);
        }
    }

    private void Update()
    {
        //UIHandler.instance.debugui.UpdateDEBUGUI("This is visible " + ownedGunShowUnits[1].gameObject.activeInHierarchy);
    }

    public void UpdateGunShowUnit()
    {
        foreach (var item in ownedGunShowUnits)
        {
            item.UpdateUI();
        }
    }

    public void SetOwnedGunUnit(GunClass gun_Perma, GunClass gun_Temp, int index)
    {


        ownedGunShowUnits[index].gameObject.SetActive(true);
        ownedGunShowUnits[index].SetUp(gun_Perma, gun_Temp);
    }

    public void ClearOwnedGunUnit(int index)
    {      

        ownedGunShowUnits[index].gameObject.SetActive(false);
    }

    public void ShowOwnedGunUnit(int index)
    {
        ownedGunShowUnits[index].gameObject.SetActive(true);
    }

    public void UpdateAmmoInOwnedGunShowUnit(int index, int ammo_Current, int ammo_Reserve)
    {
        //ownedGunShowUnits[index].UpdateAmmo(ammo_Current, ammo_Reserve);
    }

    public void UpdateChargeInOwnedGunShowUnit(int index, float current, float total)
    {
       //we will show a differnt ui here.


    }

}


//