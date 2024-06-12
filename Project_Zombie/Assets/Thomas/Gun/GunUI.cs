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
    [SerializeField] Image gunPortrait;
    [SerializeField] TextMeshProUGUI gunTitleText;
    public void UpdateGunPortrait(Sprite icon)
    {
        gunPortrait.sprite = icon;  
    }

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

        reloadFillImage.gameObject.SetActive(total > 0);
        reloadFillImage.fillAmount = current / total;
    }


    [SerializeField] OwnedGunShowUnit[] ownedGunShowUnits;
    OwnedGunShowUnit currentOwnedGunShowUnit;


    private void Update()
    {
        //UIHandler.instance.debugui.UpdateDEBUGUI("This is visible " + ownedGunShowUnits[1].gameObject.activeInHierarchy);
    }

    public void SetOwnedGunUnit(GunClass gun, int index)
    {
        ownedGunShowUnits[index].gameObject.SetActive(true);
        ownedGunShowUnits[index].SetUp(gun);
    }

    public void ClearOwnedGunUnit(int index)
    {
        ownedGunShowUnits[index].gameObject.SetActive(false);
    }

    public void ShowOwnedGunUnit(int index)
    {
        ownedGunShowUnits[index].gameObject.SetActive(true);
    }

    public void ChangeOwnedGunShowUnit(int index)
    {

        if(currentOwnedGunShowUnit != null)
        {
            currentOwnedGunShowUnit.Unselect();
        }

        currentOwnedGunShowUnit = ownedGunShowUnits[index];
        currentOwnedGunShowUnit.Select();

    }

    public void UpdateAmmoInOwnedGunShowUnit(int index, int ammo)
    {
        ownedGunShowUnits[index].UpdateAmmo(ammo);
    }

}
