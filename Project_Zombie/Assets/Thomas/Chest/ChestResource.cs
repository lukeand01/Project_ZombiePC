using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestResource : ChestBase
{
    [Separator("RESOURCE")]
    [SerializeField] ItemClass item;
    [SerializeField] ItemFollowTillEnd itemFollowTillEndTemplate;

    public override void Interact()
    {
        base.Interact();

        //thye go directly.

        isLocked = true;
        
        CreateItem();
    }

    //every 10 it gives the stuff

    void CreateItem()
    {

        //float modifier = 1.5f;

        PlayerHandler.instance._playerInventory.AddItemInUI(new ItemClass(item.data, item.quantity));

        while (item.quantity > 0)
        {
            //float x = Random.Range(-modifier, modifier);
            //float z = Random.Range(-modifier, modifier);


            if(item.quantity >= 10)
            {
                //we remove those items adn give 10
                CreateObject(item.data, 10);
                item.RemoveQuantity(10);

            }else
            {
                CreateObject(item.data, item.quantity);
                item.RemoveQuantity(item.quantity);
            }


        }



    }

    void CreateObject(ItemData data, int quantity)
    {
        ItemFollowTillEnd newObject = Instantiate(itemFollowTillEndTemplate);

        newObject.SetUp(new ItemClass(data, quantity), PlayerHandler.instance.transform);
        newObject.transform.position = transform.position;

    }



}
