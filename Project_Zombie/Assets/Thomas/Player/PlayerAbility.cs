using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
    [SerializeField] int initialAbilitySlot;
    [SerializeField]int currentAbilitySlot;
    [SerializeField] List<AbilityClass> abilityActiveList = new();
    [SerializeField] List<AbilityClass> abilityPassiveList = new();
    public List<AbilityClass> abilityCurseList { get; private set; } = new();

    [SerializeField] List<AbilityActiveData> debugAbilityActiveStartingList = new();
    [SerializeField] List<AbilityPassiveData> debugPassiveStartingList = new();

    PlayerHandler handler;

    [field: SerializeField] public List<AbilityPassiveData> abilityList_CannotStack { get; private set; } = new();
    [field: SerializeField] public  List<AbilityPassiveData> abilityList_HigherChance { get; private set; } = new();

    private void Awake()
    {
        handler = GetComponent<PlayerHandler>();
    }

    private void Start()
    {
        //this should only call once,






    }
    private void FixedUpdate()
    {
        HandleAbilityActiveCooldown();
    }


    void AddStartingAbilities()
    {
        //we might have a sitatuion where we alawyas have a new 


        

        foreach (var item in debugPassiveStartingList)
        {
            AddAbility(item);
        }
    }


    #region SET ABILITIES

    [ContextMenu("DEBUG ADD ABILITY SLOW")]
    public void AddAbilitySlot()
    {
        if (currentAbilitySlot >= 3) return;


        currentAbilitySlot++;

        AbilityClass newActiveAbilitySlot = new AbilityClass(currentAbilitySlot - 1);


        abilityActiveList.Add(newActiveAbilitySlot);
        UIHandler.instance._EquipWindowUI.UpdateAbilitySlot(currentAbilitySlot);
        UIHandler.instance._EquipWindowUI.GetEquipForAbility(newActiveAbilitySlot);

        GameHandler.instance._saveHandler.CaptureStateUsingCurrentSaveSlot();
    }


    public void SetAbility(int startingSlot)
    {
        
        if(startingSlot == 0)
        {
            currentAbilitySlot = initialAbilitySlot;
        }
        else
        {
            currentAbilitySlot = startingSlot;
        }



        for (int i = 0; i < currentAbilitySlot; i++)
        {
            abilityActiveList.Add(new AbilityClass(i));
        }

        UIHandler.instance._EquipWindowUI.UpdateAbilitySlot(currentAbilitySlot);
        UIHandler.instance._AbilityUI.SetActiveAbilityUnits(abilityActiveList);

            

        foreach (var item in abilityActiveList)
        {
            UIHandler.instance._EquipWindowUI.GetEquipForAbility(item);
        }

        foreach (var item in debugAbilityActiveStartingList)
        {
            GameHandler.instance.cityDataHandler.cityLab.BuyAbilityWithIndex_Active(item.storeIndex);
            AddAbility(item);
        }

        if (CityHandler.instance != null)
        {
            CityHandler.instance.UpdateAbilityListUsingCurrentAbilities();
        }
    }

    public void AddAbility(AbilityBaseData data)
    {

        //should create them all but without data so i can use the ui

        AbilityActiveData dataActive = data.GetActive();

        if(dataActive != null)
        {
            AddAbilityActive(dataActive);
            return;
        }


        AbilityPassiveData dataPassive = data.GetPassive();

        if (dataPassive != null)
        {

            if (dataPassive.IsCursed())
            {

                AddAbilityPassive_Cursed(dataPassive);
            }
            else
            {
                AddAbilityPassive(dataPassive);
            }

            
            return;
        }

    }

    void AddAbilityActive(AbilityActiveData data)
    {
        int index = GetAbilityFreeSlot(abilityActiveList);

        if(index != -1)
        {
            //should not be new
            abilityActiveList[index].SetActive(data);
            
            if(CityHandler.instance != null)
            {
                CityHandler.instance.UpdateAbilityListUsingCurrentAbilities();
            }
        }
        else
        {
            Debug.Log("couldnt add");
        }

    }
    void AddAbilityPassive(AbilityPassiveData data)
    {
        //we simply add
        //but we have to check if there is already one and if we need to add.
        int index = GetTargetAbilityIndex(data, abilityPassiveList);

        data.SetFound(true);

        handler._playerResources.AddAbilityCoin(data._coinType, 1);


        if(index != -1)
        {
            ReplaceSameAbilityByIndex(index, data);    
        }
        else
        {
            //we s imply add to teh list
            AbilityClass passiveAbility = new AbilityClass(data);
            passiveAbility.AddPassive();
            abilityPassiveList.Add(passiveAbility);

            UIHandler.instance._pauseUI.AddPassive(passiveAbility);

            index = abilityPassiveList.Count - 1;

        }

        //everytime we add this fella we send it to the 

        bool canStack = abilityPassiveList[index].CanStackMore();

        if(canStack) 
        {
            if(!abilityList_HigherChance.Contains(data))
            {
                abilityList_HigherChance.Add(data);
            }

        }
        else
        {
            if (!abilityList_HigherChance.Contains(data))
            {
                abilityList_HigherChance.Add(data);
            }
            if (abilityList_HigherChance.Contains(data))
            {
                abilityList_HigherChance.Remove(data);
            }
        }

    }

    void AddAbilityPassive_Cursed(AbilityPassiveData data)
    {
        data.SetFound(true);
        AbilityClass passiveAbility = new AbilityClass(data);
        passiveAbility.AddPassive();
        abilityCurseList.Add(passiveAbility);
        UIHandler.instance._pauseUI.AddCurse(passiveAbility);
    }


    public int GetAbilityFreeSlot(List<AbilityClass> abilityList)
    {
        for (int i = 0; i < abilityList.Count; i++)
        {
            if (abilityList[i].IsEmpty())
            {
                return i;
            }
        }
        return -1;
    }

    int GetTargetAbilityIndex(AbilityBaseData target, List<AbilityClass> abilityList)
    {
        for (int i = 0; i < abilityList.Count; i++)
        {
            if (abilityList[i].IsEmpty()) continue;
            if (abilityList[i].dataActive == target)
            {
                return i;
            }
            if (abilityList[i].dataPassive.abilityName == target.abilityName)
            {
                return i;
            }

        }
        return -1;
    }


    public void ReplaceActiveAbility(AbilityActiveData data, int index)
    {
        if(index > abilityActiveList.Count - 1)
        {
            Debug.Log("are not calling this");
            return;
        }
        abilityActiveList[index].SetActive(data);

        if (CityHandler.instance != null)
        {
            CityHandler.instance.UpdateAbilityListUsingCurrentAbilities();
        }
    }

    public void UpdateActiveAbility()
    {
        foreach (var item in abilityActiveList)
        {
            item.UpdateActiveUI();
        }
    }
    public void ReplaceSameAbility(AbilityClass ability)
    {

        if(ability.dataPassive == null)
        {
            Debug.Log("ability sent has nothing");
            return;
        }

        //we look for someone to replace this.
        foreach (var item in abilityPassiveList)
        {
            if (item.IsEmpty()) continue;
            if (item.dataPassive == null) continue;
            if(item.dataPassive == ability.dataPassive)
            {
                item.IncreaseLevel(null);
                return;
            }

        }
    }

    public void ReplaceSameAbilityByIndex(int index, AbilityPassiveData dataForStacking)
    {
        var item = abilityPassiveList[index];
        if (item.IsEmpty()) return;
        if (item.dataPassive == null) return;
        item.IncreaseLevel(dataForStacking);
       

    }


    public void ClearPassiveList()
    {
        foreach (var item in abilityPassiveList)
        {
            item.RemovePassive();
            item.DestroyUI();
        }

        abilityPassiveList.Clear();
    }


    public void DebugIncreaseLevel()
    {
        if(abilityPassiveList.Count > 0)
        {
            abilityPassiveList[0].IncreaseLevel(null);
        }
    }

    public AbilityClass GetTargetAbilityClass(AbilityPassiveData data)
    {
        foreach(var item in abilityPassiveList)
        {
            if (item.IsEmpty()) continue;
            if (item.dataPassive == data) return item;
        }

        return new AbilityClass(0);
    }


    #endregion


    public void ResetPassiveAbilities()
    {
        //we clear all passive abilities.

        foreach (var item in abilityCurseList)
        {
            if (item.dataPassive == null) continue;

            if (item._abilityUnit != null)
            {
                item._abilityUnit.DestroyItself();
            }

            item.RemovePassive();
        }

        abilityCurseList.Clear();

        foreach (var item in abilityPassiveList)
        {
            if (item.dataPassive == null) continue;

            if(item._abilityUnit != null)
            {
                item._abilityUnit.DestroyItself();
            }


            item.RemovePassive();
        }

        abilityPassiveList.Clear();

        abilityList_CannotStack.Clear();
        abilityList_HigherChance.Clear();

        AddStartingAbilities();
    }

    
    public void PassedRound()
    {
        //we reset the ability if its timer
        //otherwise we progress the thing

        foreach (var item in abilityActiveList)
        {
            item.ProgressRound();
        }


    }

    int chargeIndex = -1;

    public void UseAbilityActive(int index)
    {
        if (chargeIndex != -1) return;

        if (!abilityActiveList[index].IsEmpty() && !abilityActiveList[index].ShouldCharge())
        {
            //it first needs to be empty.
            Debug.Log("use");
            abilityActiveList[index].UseActive();
                    
        }
    }

    public void StartChargeAbilityActive(int index)
    {
        if (handler._entityStat.isStunned)
        {
            //of if the character dies.
            Debug.Log("stunned");
            StopChargeAbilityActive(index);
            return;
        }
        if (handler._playerResources.IsDead())
        {
            Debug.Log("dead");
            StopChargeAbilityActive(index);
            return;
        }

        if (chargeIndex != -1 && chargeIndex != index)
        {
            return;
        }


        if (!abilityActiveList[index].IsEmpty() && abilityActiveList[index].ShouldCharge())
        {
            //it first needs to be empty.
            abilityActiveList[index].Charge();
            chargeIndex = index;
        }
    }

    public void StopChargeAbilityActive(int index)
    {
        if (chargeIndex != index) return;

        if (!abilityActiveList[index].IsEmpty() && abilityActiveList[index].ShouldCharge())
        {
            //it first needs to be empty.
            abilityActiveList[index].StopCharge();
            chargeIndex = -1;
        }
    }


    void HandleAbilityActiveCooldown()
    {
        foreach (var item in abilityActiveList)
        {
            item.HandleActiveCooldown();
        }
    }

    public void DebugAddPassive()
    {

    }

    //
    public List<AbilityClass> GetActiveAbiltiyList()
    {
        return abilityActiveList;
    }
    public List<AbilityClass> GetPassiveAbilityList()
    {
        return abilityPassiveList;
    }

    #region DROP
    [Separator("DROP")]
    [SerializeField] int initialDropSlot;
    int currentDropSlot;

    public void AddDropSlot()
    {
        if (currentDropSlot >= 5) return;

        currentDropSlot++;

        UIHandler.instance._EquipWindowUI.UpdateDropSlot(currentDropSlot);

    }
    void SetDropSlot(int dropSlot)
    {
        if(dropSlot == 0)
        {
            currentDropSlot = initialDropSlot;
        }
        else
        {
            currentDropSlot = dropSlot;
        }

        //thenw e do the ui thing.,

        UIHandler.instance._EquipWindowUI.UpdateDropSlot(currentDropSlot);

        for (int i = 0; i < currentDropSlot; i++)
        {
            UIHandler.instance._EquipWindowUI.GetEquipForDrop(null, i);
        }


        GameHandler.instance._saveHandler.CaptureStateUsingCurrentSaveSlot();
    }

    public void SetNewDrop(DropData dropData, int index)
    {
        //when you do this we it and we form a new list and we send to the data. 




    }

    #endregion


    #region SAVE SYSTEM

    public void RestoreState(SaveClass saveClass)
    {
        SetDropSlot(saveClass._dropSlot);
        SetAbility(saveClass._abilitySlot);

    }
    public void CaptureState(SaveClass saveClass)
    {      
        saveClass.MakeEquipAbilitySlot(currentAbilitySlot);
        saveClass.MakeEquipDropSlot(currentDropSlot);

    }

    #endregion
}



//