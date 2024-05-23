using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CityStoreUnit : ButtonBase
{
    //this will be able to take an ability, gun or a command for either upgrading itself or the player´s roll level.

    [SerializeField] GameObject hover;
    [SerializeField] GameObject selected;
    [SerializeField] GameObject isLocked;
    [SerializeField] GameObject isBought;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Image icon;

    CityCanvas handler;

    public string id { get; private set; }

    private void Awake()
    {
        id = Guid.NewGuid().ToString();
    }
    #region GUN
    public CityStoreArmoryClass armoryClass { get; private set; } = null;
    public ItemGunData gunData {  get; private set; }
    int index;
    public void SetUpGun(CityStoreArmoryClass armoryClass, int index, CityCanvas handler, CityDataArmory armoryData)
    {

        //each fella will check two thigns
        //if the player has the gun
        //if the player cannot get the gun

        this.armoryClass = armoryClass;
        gunData = armoryClass.data;
        this.index = index;

        nameText.text = gunData.itemName;
        icon.sprite = gunData.itemIcon;

        this.handler = handler;

        bool hasGun = armoryData.HasGun(index);
        isBought.SetActive(hasGun);


    }
    #endregion

    #region Ability
    public AbilityActiveData abilityData { get; private set; }
    public CityStoreLabClass labClass { get; private set; }
    public CityDataLab labData {  get; private set; }

    public void SetUpAbility(CityStoreLabClass labClass, int index, CityCanvas handler, CityDataLab labData)
    {
        this.index = index;
        abilityData = labClass.data;
        this.labClass = labClass;


        this.handler = handler;
        this.labData = labData;


        nameText.text = abilityData.abilityName;
        icon.sprite = abilityData.abilityIcon;

        bool hasGun = labData.HasAbility(index);
        isBought.SetActive(hasGun);

    }

    #endregion

    public void SetAsBought()
    {
        isBought.SetActive(true);
    }

    #region BUTTON
    public override void OnPointerEnter(PointerEventData eventData)
    {

        base.OnPointerEnter(eventData);
        hover.SetActive(true);
        //inform the optin

        if (gunData != null)
        {
            UIHandler.instance._DescriptionWindow.DescribeGunData(gunData, transform);
            UIHandler.instance._DescriptionWindow.StoreDescribeGun(armoryClass, isBought.activeInHierarchy);
            return;
        }

    }
    public override void OnPointerExit(PointerEventData eventData)
    {

        base.OnPointerExit(eventData);
        hover.SetActive(false);
        UIHandler.instance._DescriptionWindow.StopDescription();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
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
