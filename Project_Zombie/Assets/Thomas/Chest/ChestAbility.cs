using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestAbility : ChestBase
{

    public override void Interact()
    {
        base.Interact();


        ChestUI _chestUI = UIHandler.instance.ChestUI;

        List<AbilityPassiveData> passiveList = PlayerHandler.instance.GetPassiveList();

        _chestUI.SetChest(this);
        _chestUI.CallChestAbility(passiveList);


        Destroy(gameObject);
    }
}
