using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestAbility : ChestBase
{

    [Separator("CHEST ABILITY")]
    [SerializeField] List<AbilityPassiveData> debugPassiveDataList;
    [SerializeField] int price;
    
    //create an effect for the thing instead of just disappearing.

    public override void Interact()
    {
        
        if(price != 0)
        {
            if (!PlayerHandler.instance._playerResources.HasEnoughPoints(price)) return;

            PlayerHandler.instance._playerResources.SpendPoints(price);
            List<AbilityPassiveData> dataList = GameHandler.instance.cityDataHandler.cityLab.GetPassiveAbilityList(1, "chestability");
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


       StartCoroutine(OpenProcess(_chestUI));
    }

    //

    IEnumerator OpenProcess(ChestUI _chestUI)
    {

        List<AbilityPassiveData> passiveList = GameHandler.instance.cityDataHandler.cityLab.GetPassiveAbilityList(3, "OpenProcess");
        //GameHandler.instance._soundHandler.CreateSfx(openChestClip, transform);

        //it jumps in the air and o
        transform.DOMove(transform.position + new Vector3(0, 5f, 0), 0.6f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(0.6f);


        graphic_Lid.DOLocalRotate(new Vector3(-90, 0, 0), 0.2f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(0.2f);


        _chestUI.SetChest(this);
        _chestUI.CallChestAbility(passiveList);

        PlayerHandler.instance._entityEvents.OnOpenChest(ChestType.ChestAbility);

        yield return new WaitForSeconds(1f);

        //it stays in the air and disappears.

        Destroy(gameObject);
    }



    public override void InteractUI(bool isVisible)
    {

        interactCanvas.gameObject.SetActive(isVisible);
        base.InteractUI(isVisible);

        if(price != 0)
        {
            interactCanvas.ControlPriceHolder(price);
        }
    }
}
