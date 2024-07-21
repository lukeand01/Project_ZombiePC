using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestAbility : ChestBase
{

    [SerializeField] List<AbilityPassiveData> debugPassiveDataList;


    public override void Interact()
    {
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
}
