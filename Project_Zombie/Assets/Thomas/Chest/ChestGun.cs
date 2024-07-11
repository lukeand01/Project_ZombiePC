using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChestGun : ChestBase
{

    //we open another ui
    //we slow the game to bascially stop.
    //we can animate stuff here and
    //i can use time.unscaledtime to not deal with the mess of scale time.

    //the player can get any gun from this place
    //but the guns arent as unique as they might seen
    //you will have a bunch of guns here that look alike but with different stats.
    //there are different levels of gun and skill tier. and you can increase your chances in the base.
    //there is 4 tiers.
    //and at the first level:
    //1 tier -> 80%
    //2 tier -> 15%
    //3 tier -> 5%
    //4 tier -> 0%

    //every level improves the chance of higher tier and lowers the chance of lower tiers.

    [Separator("SHOWING")]
    [SerializeField] GameObject gunHolder; //then we will spawn all guns moedels. but we will check if we have repeating stuff so we dont need to remove
    
    bool isShowing;
    bool isDone;

    ItemGunData chosenGun;

    float currentPrice;
    [SerializeField] float basePrice; 
    public override void Interact()
    {
        base.Interact();


        if (isShowing)
        {
            GetGun();
        }
        else
        {
            bool canSpend = PlayerHandler.instance._playerResources.HasEnoughPoints((int)currentPrice);

            if (!canSpend) return;

            PlayerHandler.instance._playerResources.SpendPoints((int)currentPrice);
            List<ItemGunData> spinningGunList = GameHandler.instance.cityDataHandler.cityArmory.GetGunSpinningList();
            ItemGunData chosenGun = GameHandler.instance.cityDataHandler.cityArmory.GetGunChosen();
            this.chosenGun = chosenGun;
            StartCoroutine(PresentationProcess(chosenGun, spinningGunList));
        }
        //we get a bunch of random fellas. but those random fellas need to be from places we can get them
        //then we roll for the 
       

        //instead of getting from there
        //instead the player will also have acess to the dataarmory.
    }

    public override void InteractUI(bool isVisible)
    {
        base.InteractUI(isVisible);
        

        //we are constantly calling this.
        float modifier = PlayerHandler.instance._entityStat.GetTotalEspecialConditionValue(EspecialConditionType.GunBoxPriceModifier);
        float reduction = basePrice * modifier;
        currentPrice = basePrice - reduction;
        currentPrice = Mathf.Clamp(reduction, 0, 9999);


    }

    public override bool IsInteractable()
    {

       int freeSpaceIndex = PlayerHandler.instance._playerCombat.GetGunEmptySlot();
        

        if(freeSpaceIndex == -1)
        {
            //it means we dont have a gun
            int currentIndex = PlayerHandler.instance._playerCombat.GetCurrentGunIndex;

            if (currentIndex == 0) return false; //this means you are holding the perma when you are not space.
        }


        if (isShowing && !isDone)
        {
            return false;
        }

        return base.IsInteractable();
    }

    //when yoiu interact you start the effect;
    //the effect opens the box and then the 



    IEnumerator PresentationProcess(ItemGunData chosenGun, List<ItemGunData> spinningGunList)
    {
        isShowing = true;
        isDone = false;

        //we can delete everything but trying to not delete everything is the best.



        for (int i = 0; i < gunHolder.transform.childCount; i++)
        {
            //honestly i will just destroy everything for now. not good. if there is need for perfoamnce i will take a lookok
            Destroy(gunHolder.transform.GetChild(i).gameObject);
        }

        foreach (var item in spinningGunList)
        {
            //spawn the model and put it here.
            GameObject newObject = Instantiate(item.gunModel, transform.position, Quaternion.identity);
            newObject.name = item.name;
            newObject.transform.SetParent(gunHolder.transform);
            newObject.transform.localPosition = Vector3.zero;
            newObject.SetActive(false);
        }


        gunHolder.transform.localPosition = Vector3.zero;

        float timer = 4;

        gunHolder.transform.DOKill();
        gunHolder.transform.DOLocalMoveY(gunHolder.transform.localPosition.y + 4, timer).SetUpdate(true).SetEase(Ease.Linear);

        int safeBreak = 0;

        int currentIndex = 0;

        while (gunHolder.transform.localPosition != Vector3.zero + new Vector3(0,4,0))
        {
            //we keep cycling through the options
            gunHolder.transform.GetChild(currentIndex).gameObject.SetActive(false);
            currentIndex++;            

            if(currentIndex >= gunHolder.transform.childCount)
            {
                currentIndex = 0;
            }
            gunHolder.transform.GetChild(currentIndex).gameObject.SetActive(true);


            safeBreak++;
            if (safeBreak > 1000) break;

            yield return new WaitForSecondsRealtime(0.3f);
        }

        Debug.Log("never done");
        isDone = true;
        StartCoroutine(CancelProcess());
    }

    IEnumerator CancelProcess()
    {
        //we wait a bit
        Debug.Log("cancel process called");
        yield return new WaitForSecondsRealtime(5); //after a time. and i need to inform the player of that time. it starts going down when its over

        float timer = 2;

        gunHolder.transform.DOKill();
        var gunMovemetTween = gunHolder.transform.DOLocalMoveY(0, timer).SetUpdate(true).SetEase(Ease.Linear);

        int safeBreak = 0;

        while (!gunMovemetTween.IsComplete())
        {
            

            safeBreak++;
            if (safeBreak > 1000) break;

            yield return new WaitForSecondsRealtime(0.01f);
        }

        gunHolder.SetActive(false);

        isShowing = false;
        isDone = false;


    }

    void GetGun()
    {

        Debug.Log("get gun");
        //we stop all process.
        //give gun
        StopAllCoroutines();
        gunHolder.SetActive(false);
        gunHolder.transform.DOKill();
        gunHolder.transform.localPosition = Vector3.zero;

        //then we play animation.

        int indexCurrentlyUsing = PlayerHandler.instance._playerCombat.GetCurrentGunIndex;
        PlayerHandler.instance._playerCombat.ReceiveTempGunToReplace(chosenGun, indexCurrentlyUsing);
    }

    public override void ProgressChest()
    {
        if(LocalHandler.instance != null)
        {
            LocalHandler.instance.ChestGunUse();
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
