using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CityStoreUnit : ButtonBase
{
    //this will be able to take an ability, gun_Perma or a command for either upgrading itself or the player´s roll level.

    [SerializeField] GameObject hover;
    [SerializeField] GameObject selected;
    [SerializeField] GameObject isLocked;
    [SerializeField] GameObject isBought;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Image icon;
    [SerializeField] GameObject unknownImage;

    CityCanvas handler;

    public string id { get; private set; }
    bool cannotClick;

    private void Awake()
    {
        id = Guid.NewGuid().ToString();
    }
    #region GUN
    public ItemGunData gunData {  get; private set; }
    int index;
    public void SetUpGun(ItemGunData gunData, int index, CityCanvas handler, CityDataArmory armoryData)
    {
     
        this.gunData = gunData;
        this.index = index;

        nameText.text = gunData.itemName;
        icon.sprite = gunData.itemIcon;

        this.handler = handler;

        bool hasGun = armoryData.HasGun(gunData.storeIndex);
        isBought.SetActive(hasGun && !gunData.isTemp);

        if (hasGun)
        {
            gunData.SetHasBeenFound(true);
        }

        SetUnknown(gunData.HasBeenFound);

       //if(!gunData.isTemp) Debug.Log("GunUnit " + gunData.name + " hasgun: " + hasGun + "; HasBeenFound: " + gunData.HasBeenFound);


    }
    #endregion

    #region Ability
    public AbilityActiveData activeData { get; private set; }
    public AbilityPassiveData passiveData {  get; private set; }
    //public CityStoreLabClass labClass { get; private set; }
    public CityDataLab labData {  get; private set; }

    public void SetUpAbility(AbilityBaseData abilityData, int index, CityCanvas handler, CityDataLab labData)
    {
        this.index = index;

        if (abilityData.GetActive())
        {
            activeData = abilityData.GetActive();
        }
        if (abilityData.GetPassive())
        {
            passiveData = abilityData.GetPassive();
        }


        this.handler = handler;
        this.labData = labData;


        nameText.text = abilityData.abilityName;
        icon.sprite = abilityData.abilityIcon;

        bool hasGun = labData.HasAbility_Active(index);
        isBought.SetActive(hasGun);

        SetUnknown(abilityData.HasBeenFound);
    }

    #endregion

    #region DROP
    DropData dropData;
    public void SetDrop(DropData data, CityCanvas handler)
    {
        dropData = data;
        this.handler = handler;

        nameText.text = data.dropName;
        icon.sprite = data.dropSprite;

        isBought.SetActive(false);

        SetUnknown(true);
    }

    #endregion

    public void SetUnknown(bool isUnkown)
    {
        unknownImage.SetActive(!isUnkown);
    }

    public void SetCannotClick(bool cannotClick)
    {
        this.cannotClick = cannotClick;
    }

    public void SetAsBought()
    {
        isBought.SetActive(true);
    }

    #region BUTTON
    public override void OnPointerEnter(PointerEventData eventData)
    {

        if (unknownImage.activeInHierarchy) return;

        base.OnPointerEnter(eventData);
        hover.SetActive(true);
        //inform the optin

        if (gunData != null)
        {
            UIHandler.instance._DescriptionWindow.DescribeGunData(gunData, transform);
            UIHandler.instance._DescriptionWindow.StoreDescribeGun(gunData, isBought.activeInHierarchy);
            return;
        }

    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        if(unknownImage.activeInHierarchy) return;
        base.OnPointerExit(eventData);
        hover.SetActive(false);
        UIHandler.instance._DescriptionWindow.StopDescription();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (cannotClick || unknownImage.activeInHierarchy) return;
        base.OnPointerClick(eventData);
        if (isBought.activeInHierarchy) return;

        handler.SelectStoreUnit(this);
        
    }


    public void Select()
    {
        selected.SetActive(true);
    }
    public void Unselect()
    {
        selected.SetActive(false);
    }

    private void OnDisable()
    {
        hover.SetActive(false);
    }

    #endregion

}
