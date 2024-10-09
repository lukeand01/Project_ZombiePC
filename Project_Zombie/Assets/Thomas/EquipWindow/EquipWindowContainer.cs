using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EquipWindowContainer : MonoBehaviour
{
    //this controls the actual that will hold the options.

    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Transform container;
    [SerializeField] EquipWindowEquipUnit equipUnitTemplate;
    EquipWindowUI handler;



    public void SetUp(string name, EquipWindowUI handler)
    {
        nameText.text = name;   
        this.handler = handler;
    }


    public void UpdateContainerGun(List<ItemGunData> gunList)
    {
        DestroyChildren();
        foreach (var gun in gunList)
        {
            EquipWindowEquipUnit newObject = Instantiate(equipUnitTemplate);
            newObject.SetGun(gun, handler);
            newObject.transform.SetParent(container);
        }
        
       
    }

    public void UpdateContainerAbility(List<AbilityActiveData> abilityLIst)
    {
        DestroyChildren();

        foreach (var item in abilityLIst)
        {
            EquipWindowEquipUnit newObject = Instantiate(equipUnitTemplate);
            newObject.SetAbility(item, handler);
            newObject.transform.SetParent(container);
        }
    }

    public void UpdateContainerDrop(List<DropData> dropList)
    {
        DestroyChildren();

        foreach (var item in dropList)
        {
            EquipWindowEquipUnit newObject = Instantiate(equipUnitTemplate);
            newObject.SetDrop(item, handler);
            newObject.transform.SetParent(container);
        }
    }


    [Separator("QUEST")]
    [SerializeField] QuestUnit questUnitTemplate;
    
    public void UpdateContainerQuest(List<QuestClass> questList)
    {
        DestroyChildren();

        foreach (var item in questList)
        {
            QuestUnit newObject = Instantiate(questUnitTemplate);
            newObject.SetUp(item);
            newObject.SetUp_Story();
            newObject.transform.SetParent(container);
        }
    }
   
    
    void DestroyChildren()
    {
        for (int i = 0; i < container.childCount; i++)
        {
            Destroy(container.GetChild(i).gameObject);
        }
    }
}
