using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StatDescriptionGroupHolder : MonoBehaviour
{
    //i will handle the calculation for each fella right here so i dont need to keep doing that.



    [SerializeField] StatType gunStat;
    [SerializeField] StatDescriptionUnit chosenUnit;
    [SerializeField] Image arrow; //we just rotate the arrow.
    [SerializeField] StatDescriptionUnit selectedUnit;

    float valueTotalForChoseGun;
    public void ChoseGun(ItemGunData data)
    {
        
        chosenUnit.gameObject.SetActive(true);
        arrow.gameObject.SetActive(false);
        selectedUnit.gameObject.SetActive(false);

        //how to get the information here?
        //we need to calculate information regarding the stats.

        float valueBase = GetFlatValue(data);
        List<ModifierClass> modifiers = GetAllModifiers(valueBase);
        float valueTotal = GetTotal(valueBase, modifiers);
        string info = GetInfo(valueBase, valueTotal );

        valueTotalForChoseGun = valueTotal;

        chosenUnit.UpdateStat(info, modifiers);


        selectedGunDictionary.Clear();
    }

    string GetInfo(float valueBase, float valueTotal)
    {
        string info = gunStat.ToString() + ": " + valueBase + $"({valueTotal})";

        return info;
    }

    float GetFlatValue(ItemGunData data)
    {
        return data.GetValue(gunStat);
    }

    float GetTotal(float valueBase, List<ModifierClass> modifierList)
    {
        float total = valueBase;

        for (int i = 1; i < modifierList.Count; i++)
        {
            var item = modifierList[i];

            float percentBase = valueBase * item.modifierPercentValue;
            total += percentBase;

            total += item.modifierFlatValue;
        }



        return total;
    }


    List<ModifierClass> GetAllModifiers(float valueBase)
    {
        //first we have the base
        //second we have the upgrade.
        //third we check the bds affecting it.

        List<ModifierClass> modifierList = new()
        {
            new ModifierClass("BaseValue", "GunValue", valueBase, 0)
        };

        List<ModifierClass> bdModifierList = new();

        foreach (var item in bdModifierList)
        {
            modifierList.Add(item);
        }


        return modifierList;
    }



    //i should save information here in case i need to use then again
    //so i create 

    Dictionary<int, GunStatDescriptionSavedInfoClass> selectedGunDictionary = new();

    public void SelectGun(GunClass gun, int unitID)
    {
        //the gunclass is for checking update but for now i will ignore it.


        float valueBase = 0;
        List<ModifierClass> modifiers = new();
        float valueTotal = 0;
        string info = "";

        if (!selectedGunDictionary.ContainsKey(unitID))
        {
            //then we create new info.
            valueBase = GetFlatValue(gun.data);
            modifiers = GetAllModifiers(valueBase);
            valueTotal = GetTotal(valueBase, modifiers);
            info = GetInfo(valueBase, valueTotal);

            selectedGunDictionary.Add(unitID, new GunStatDescriptionSavedInfoClass(valueBase, modifiers, valueTotal, info));
        }
        else
        {
            //then we use the new info.
            GunStatDescriptionSavedInfoClass savedInfo = selectedGunDictionary[unitID];

            valueBase = savedInfo.valueBase;
            modifiers = savedInfo.modifiers;
            valueTotal = savedInfo.valueTotal;
            info = savedInfo.info;

        }

        arrow.gameObject.SetActive(true);
        selectedUnit.gameObject.SetActive(true);

        //how to get the information here?
        //we need to calculate information regarding the stats.



        selectedUnit.UpdateStat(info, modifiers);

        //we rotate the 
        if(valueTotal < valueTotalForChoseGun)
        {
            arrow.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if( valueTotal > valueTotalForChoseGun)
        {
            arrow.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            arrow.gameObject.SetActive(false);
        }

    }

    public void UnselectGun()
    {

        arrow.gameObject.SetActive(false);
        selectedUnit.gameObject.SetActive(false);
    }

}

public class GunStatDescriptionSavedInfoClass
{
    //i just want to keep stuff here to not recalculate 

    public float valueBase {  get; private set; }
    public List<ModifierClass> modifiers { get; private set; } = new();
    public float valueTotal { get; private set; } = 0;
    public string info { get; private set; } = "";


    public GunStatDescriptionSavedInfoClass(float valueBase, List<ModifierClass> modifiers, float valueTotal, string info)
    {
        this.valueBase = valueBase;
        this.modifiers = modifiers;
        this.valueTotal = valueTotal;
        this.info = info;
    }
}

public class ModifierClass
{
    //this class shows

    public string modifierName;
    public string modifierType;
    public float modifierFlatValue; //it might be percent or 
    public float modifierPercentValue;

    public ModifierClass(string modifierName, string modifierType, float modifierFlatValue, float modifierPercentValue)
    {
        this.modifierName = modifierName;
        this.modifierType = modifierType;
        this.modifierFlatValue = modifierFlatValue;
        this.modifierPercentValue = modifierPercentValue;
    }
}