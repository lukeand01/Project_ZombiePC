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
        SetGunStat();
    }

    public void StopDescription()
    {
        descriptionHolder.SetActive(false);

        gun_Stat_Holder.SetActive(false);
        gun_Upgrade_Holder.SetActive(false);

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
        descriptionHolder.transform.position = posRef.position + GetScreenOffset(posRef.position) + new Vector3(-50,0,0);
        CloseStoreDescribe();

        nameText.text = gun.data.itemName;
        typeText.text = "Gun";
        tierText.text = "";
        descriptionText.text = gun.data.itemDescription;

        float damagePerShot = gun.data.GetValue(StatType.Damage);
        int bulletPerShot = gun.data.bulletPerShot;

        damageText.text = $"Damage Per Bullet({damagePerShot} / Bullet per shot ({bulletPerShot}))";

        string gunReserveAmmoString = gun.ammoReserve.ToString();
        if(gun.ammoReserve == -1)
        {
            gunReserveAmmoString = "*";
        }

        cooldownText.text = "Ammo: " + gun.ammoCurrent.ToString() + " / " + gunReserveAmmoString;


        //we also show all upgrades in this fella
        //and we show the stats, the stats show the 
        DescribeGun_Stat(gun);
        DescribeGun_Upgrades(gun);
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

    public void DescribeQuest(QuestClass quest, Transform posRef)
    {
        //we only are going to describe story quests.
        //quests have a name
        descriptionHolder.SetActive(true);
        descriptionHolder.transform.position = posRef.position + GetScreenOffset(posRef.position);
        CloseStoreDescribe();

        nameText.text = quest.GetQuestName;
        typeText.text = "Story Quest";
        tierText.text = quest.GetQuestGiverName;
        descriptionText.text = quest.GetDescription_Story;

        //i want to put the reward here.    
        damageText.text = "";
        cooldownText.text = quest.GetDescription_Reward();




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

    public void StoreDescribeGun(ItemGunData gunData, bool isBought)
    {
        storeHolder.SetActive(true);
        storeBoughtHolder.SetActive(isBought);
        List<string> stringList = gunData.GetStringPriceList();

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


    [Separator("DESCRIPTION FOR GUN")]
    [SerializeField] GameObject gun_Stat_Holder;
    [SerializeField] Transform gun_Stat_Container;
    [SerializeField] StatDescriptionUnit gun_Stat_Template;
    [SerializeField] GameObject gun_Upgrade_Holder;
    [SerializeField] Transform gun_Upgrade_Container;
    [SerializeField] GunUpgradeUnit gun_Upgrade_Template;


    List<StatDescriptionUnit> gun_Stat_UnitList = new();

    void SetGunStat()
    {
        List<StatType> refList = MyUtils.GetStatForGunListRef();

        foreach (var item in refList)
        {
           StatDescriptionUnit newObject = Instantiate(gun_Stat_Template);
            newObject.SetUp(item, 0); //we give no stats here but when we open with teh gun we change that.
            newObject.RemoveRaycast();
            newObject.transform.SetParent(gun_Stat_Container);
            gun_Stat_UnitList.Add(newObject);
        }

    }

    void DescribeGun_Stat(GunClass gun)
    {
        gun_Stat_Holder.SetActive(true);
        foreach (var item in gun_Stat_UnitList)
        {
            float value = gun.GetGunTotalStat(item.stat);
            item.UpdateWithAlteredValue(gun.data.GetValue(item.stat), value);
        }
    }
    void DescribeGun_Upgrades(GunClass gun)
    {
        //we clear the container every time.
        //
        if(gun.gunUpgradeList.Count == 0)
        {
            return;
        }

        gun_Upgrade_Holder.SetActive(true);

        for (int i = 0; i < gun_Upgrade_Container.childCount; i++)
        {
            Destroy(gun_Upgrade_Container.GetChild(i).gameObject);
        }

        foreach (var item in gun.gunUpgradeList)
        {
            GunUpgradeUnit newObject = Instantiate(gun_Upgrade_Template);
            newObject.SetUp(item.upgradeName, item.upgradeDescription);
            newObject.transform.SetParent(gun_Upgrade_Container);
        }

    }
}
