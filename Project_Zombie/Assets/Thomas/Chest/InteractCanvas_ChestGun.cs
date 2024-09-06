using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractCanvas_ChestGun : InteractCanvas
{
    //what i will do is get two fellas

    [Separator("Chest Gun")]
    [SerializeField] GameObject holder;
    [SerializeField] GameObject[] textHolderArray; //this 
    [SerializeField] TextMeshProUGUI text_OwnedGun;
    [SerializeField] Image image_OwnedGun;
    [SerializeField] TextMeshProUGUI text_ChestGun;
    [SerializeField] Image image_ChestGun;
    [SerializeField] Transform[] arrowsArray;
    [SerializeField] GameObject warningHolder;


    //actually we will constantly check it because fuck it, its not that much of a big deal

    ItemGunData chestItemGunData;

    bool isRunning;

    private void OnDisable()
    {
        //we stop checking
        StopChestGunInteraction();
    }


    

    private void Update()
    {
        if (isRunning)
        {
            GunClass _gunClass = PlayerHandler.instance._playerCombat.GetCurrentGun;
            bool hasSpace = PlayerHandler.instance._playerCombat.GetGunEmptySlot() != -1;
            //first we need to check if there is space here.


            if(_gunClass == null)
            {
                Debug.Log("the gun class is null");
            }

            if (hasSpace)
            {
                //we just show the gun 
                //if there is no current gun then we will simply show nothing 
                holder.SetActive(true);
                image_OwnedGun.sprite = null;
                text_OwnedGun.text = "Empty";

                titleHolder.SetActive(false);

            }
            else
            {
                if (_gunClass.data.isTemp)
                {
                    //if it is temp then we show the two thins and put the two fellas in the right places.
                    holder.SetActive(true);
                    image_OwnedGun.sprite = _gunClass.data.itemIcon;
                    text_OwnedGun.text = _gunClass.data.itemName;
                    Debug.Log("2");
                }
                else
                {
                    //if its not we will place it in both sides.
                    holder.SetActive(false);
                    titleHolder.SetActive(false);
                    ControlWarn(true);
                    Debug.Log("3");
                }
            }

        }

        
        
    }

    public void StartChestGunInteraction(ItemGunData chestItemGunData)
    {
        //we allow the animation to start runnning, and to check for it.
        //very importantly.

        if (!isRunning)
        {
            StartCoroutine(ArrowProcess());
        }


        image_ChestGun.sprite = chestItemGunData.itemIcon;
        text_ChestGun.text = chestItemGunData.itemName;
        ControlNameHolder("Pick " + chestItemGunData.itemName);

        this.chestItemGunData = chestItemGunData;
        isRunning = true;

        
    }

    IEnumerator ArrowProcess()
    {
        //arrow just keep going from one side to another
        float timer = 2;

        arrowsArray[0].transform.DOKill();
        arrowsArray[0].transform.DOLocalMove(new Vector3(-20, -100, 0), 2).SetEase(Ease.Unset);

        arrowsArray[1].transform.DOKill();
        arrowsArray[1].transform.DOLocalMove(new Vector3(-20, -160, 0), 2).SetEase(Ease.Unset);

        yield return new WaitForSeconds(timer);

        arrowsArray[0].transform.DOKill();
        arrowsArray[0].transform.DOLocalMove(new Vector3(20, -100, 0), 2).SetEase(Ease.Unset);

        arrowsArray[1].transform.DOKill();
        arrowsArray[1].transform.DOLocalMove(new Vector3(20, -160, 0), 2).SetEase(Ease.Unset);

        yield return new WaitForSeconds(timer);

        StartCoroutine(ArrowProcess());

    }


    public void StopChestGunInteraction()
    {
        if (isRunning)
        {

            arrowsArray[0].transform.DOKill();
            arrowsArray[0].transform.localPosition = new Vector3(0, -100, 0);

            arrowsArray[1].transform.DOKill();
            arrowsArray[1].transform.localPosition = new Vector3(0, -160, 0);


        }


        isRunning = false;
        ControlWarn(false);

       

        StopAllCoroutines();
    }


    void ControlWarn(bool isVisible)
    {
        warningHolder.SetActive(isVisible);
    }

    public override InteractCanvas_ChestGun GetInteractChestGun()
    {
        return this;

    }


}
