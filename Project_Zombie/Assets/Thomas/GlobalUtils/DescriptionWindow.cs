using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DescriptionWindow : MonoBehaviour
{

    GameObject descriptionHolder;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI typeText;
    [SerializeField] TextMeshProUGUI tierText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI damageText;
    [SerializeField] TextMeshProUGUI cooldownText;
    

    private void Awake()
    {
        descriptionHolder = transform.GetChild(0).gameObject;
    }

    public void StopDescription()
    {
        descriptionHolder.SetActive(false);
    }

    public void DescribeBD(BDClass bd, Transform posRef)
    {
        //we do the same thing but we also 

        descriptionHolder.SetActive(true);
        descriptionHolder.transform.position = posRef.position + GetScreenOffset(posRef.position);
        CloseStoreDescribe();

        nameText.text = bd.bdType.ToString();
        typeText.text = bd.GetTypeForDescription();

        tierText.text = "";


        string bdDescription = MyUtils.GetDescriptionForBD(bd);
        descriptionText.text = bdDescription;


        damageText.text = bd.GetDamageDescription();


        cooldownText.text = "Total Cooldown: " + bd.GetTempDurationForDescription();
    }

    public void DescribeAbiliy(AbilityClass ability, Transform posRef)
    {
        descriptionHolder.SetActive(true);
        descriptionHolder.transform.position = posRef.position + GetScreenOffset(posRef.position);
        CloseStoreDescribe();

        nameText.text = ability.GetNameForDescription();
        typeText.text = ability.GetTypeForDescription();
        tierText.text = ability.GetTierForDescription();
        descriptionText.text = ability.GetDescriptionForDescription();
        damageText.text = ability.GetDamageForDescription();
        cooldownText.text = ability.GetCooldownForDescription();
    }
   public void DescribeAbilityData(AbilityActiveData ability, Transform posRef)
    {
        descriptionHolder.SetActive(true);
        descriptionHolder.transform.position = posRef.position + GetScreenOffset(posRef.position);
        CloseStoreDescribe();

        nameText.text = ability.abilityName;
        descriptionText.text = ability.abilityDescription;
        typeText.text = "Ability";
        damageText.text = ability.GetDamageDescription(new AbilityClass(ability));
        cooldownText.text = "Cooldown: " + ability.abilityCooldown.ToString();
    }

    public void DescribeGun(GunClass gun, Transform posRef)
    {
        descriptionHolder.SetActive(true);
        descriptionHolder.transform.position = posRef.position + GetScreenOffset(posRef.position);
        CloseStoreDescribe();

        nameText.text = gun.data.itemName;
        typeText.text = "Gun";
        tierText.text = "";
        descriptionText.text = gun.data.itemDescription;

        float damagePerShot = gun.data.GetValue(StatType.Damage);
        int bulletPerShot = gun.data.bulletPerShot;

        damageText.text = $"Damage Per Bullet({damagePerShot} ; Bullet per shot ({bulletPerShot}))";
        cooldownText.text = "Ammo: " + gun.ammoCurrent.ToString() + " / " + gun.ammoReserve.ToString();

    }

    public void DescribeGunData(ItemGunData gunData, Transform posRef)
    {
        descriptionHolder.SetActive(true);
        descriptionHolder.transform.position = posRef.position + GetScreenOffset(posRef.position);
        CloseStoreDescribe();

        nameText.text = gunData.itemName;
        typeText.text = "Gun";
        tierText.text = "";
        descriptionText.text = gunData.itemDescription;

        float damagePerShot = gunData.GetValue(StatType.Damage);
        int bulletPerShot = gunData.bulletPerShot;

        damageText.text = $"Damage Per Bullet({damagePerShot} ; Bullet per shot ({bulletPerShot}))";
        cooldownText.text = "";

    }


    public void DescribeStat(StatClass stat, Transform posRef)
    {
        descriptionHolder.SetActive(true);
        descriptionHolder.transform.position = posRef.position + GetScreenOffset(posRef.position);
        CloseStoreDescribe();

        nameText.text = stat.stat.ToString();
        typeText.text = "Stat";
        tierText.text = "";
        descriptionText.text = MyUtils.GetStatDescription(stat.stat);
        damageText.text = "Value is: " + stat.value.ToString();
        cooldownText.text = "";
    }

    public void DescribeDash(Transform posRef)
    {
        descriptionHolder.SetActive(true);
        descriptionHolder.transform.position = posRef.position + GetScreenOffset(posRef.position);
        CloseStoreDescribe();

        nameText.text = "Dash";
        typeText.text = "Unique Ability";
        tierText.text = "";
        descriptionText.text = "Once you dahs you become immune to all forms of damage for a short amount of time";
        damageText.text = "";
        cooldownText.text = "";
    }

    public void DescribeResource(ItemClass item, Transform posRef)
    {
        descriptionHolder.SetActive(true);
        descriptionHolder.transform.position = posRef.position + GetScreenOffset(posRef.position);
        CloseStoreDescribe();

        nameText.text = item.data.itemName;
        typeText.text = "Resource";
        tierText.text = item.data.tierType.ToString();
        descriptionText.text = item.data.itemDescription;
        damageText.text = "";
        cooldownText.text = item.quantity.ToString();
    }


    Vector3 GetScreenOffset(Vector3 posRef)
    {
        float screenHeight = Screen.height;
        float screenWidth = Screen.width;
        Vector3 newOffset = new Vector3(screenWidth * 0.1f, screenHeight * 0.08f, 0);

        if (posRef.y > screenHeight * 0.7f)
        {
            newOffset = new Vector3(0, -screenHeight * 0.25f, 0);
        }

        if (posRef.y < screenHeight * 0.05f)
        {
            //newOffset += new Vector3(0, 30, 0);
        }

        if (posRef.x > screenWidth * 0.7f)
        {
            newOffset += new Vector3(-screenWidth * 0.15f, 0, 0);
        }
        if (posRef.x < screenWidth * 0.3f)
        {
            newOffset += new Vector3(screenWidth * 0.07f, 0, 0);
        }

        return newOffset;
    }


    [Separator("DESCRIPTION FOR STORE")]
    [SerializeField] GameObject storeHolder;
    [SerializeField] GameObject storeBoughtHolder;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] TextMeshProUGUI requirementText;

    void CloseStoreDescribe()
    {
        storeHolder.SetActive(false);
    }

    public void StoreDescribeGun(CityStoreArmoryClass armory, bool isBought)
    {
        storeHolder.SetActive(true);
        storeBoughtHolder.SetActive(isBought);
        List<string> stringList = armory.GetStringPriceList();

        priceText.text = "Price: ";

        foreach (var item in stringList)
        {
            priceText.text += item + "; ";
        }

    }
    public void StoreDescribeAbility()
    {

    }

    public void StoreDescribeStage(CityStageClass stage)
    {

    }

}
