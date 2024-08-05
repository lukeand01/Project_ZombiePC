using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestAbility : ChestBase
{

    [Separator("CHEST ABILITY")]
    [SerializeField] List<AbilityPassiveData> debugPassiveDataList;
    [SerializeField] int price;

    public override void Interact()
    {
        
        if(price != 0)
        {
            if (!PlayerHandler.instance._playerResources.HasEnoughPoints(price)) return;

            PlayerHandler.instance._playerResources.SpendPoints(price);
            List<AbilityPassiveData> dataList = GameHandler.instance.cityDataHandler.cityLab.GetPassiveAbilityList(1);
            PlayerHandler.instance._playerAbility.AddAbility(dataList[0]);
            gameObject.SetActive(false);
            return;
        }


        base.Interact();
        ChestUI _chestUI = UIHandler.instance.ChestUI;

        if (debugPassiveDataList.Count > 0)
        {

            _chestUI.SetChest(this);
            _chestUI.CallChestAbility(debugPassiveDataList);
            return;

        }


        List<AbilityPassiveData> passiveList = GameHandler.instance.cityDataHandler.cityLab.GetPassiveAbilityList();

        _chestUI.SetChest(this);
        _chestUI.CallChestAbility(passiveList);

        PlayerHandler.instance._entityEvents.OnOpenChest(ChestType.ChestAbility);

        Destroy(gameObject);
    }

    public override void InteractUI(bool isVisible)
    {
        base.InteractUI(isVisible);

        if(price != 0)
        {
            interactCanvas.ControlPriceHolder(price);
        }
    }
}
