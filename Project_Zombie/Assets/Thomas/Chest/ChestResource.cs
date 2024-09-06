using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChestResource : ChestBase
{
    [Separator("RESOURCE")]
    [SerializeField] ItemClass item;
    [SerializeField] ItemFollowTillEnd itemFollowTillEndTemplate;
    [SerializeField] ParticleSystem dustExplosionPS;
    int index;

    private void Awake()
    {
        
    }

    public override void ResetForPool()
    {
        base.ResetForPool();
        dustExplosionPS.Clear();
        dustExplosionPS.gameObject.SetActive(false);
    }

    public void SetUp(ItemClass item, int index)
    {
        //we need to decided. and we can just decide it here instead of receiving something.
        if(item != null)
        {
            //there is already an item so its for testing
            return;
        }

        this.item = new ItemClass(item.data, item.quantity);
        this.index = index;
    }

    public override void Interact()
    {
        base.Interact();

        //thye go directly.

        isLocked = true;

        //CreateItem(); //this is to create an item that follows the player

        PlayerHandler.instance._playerInventory.AddItemForStage(item);
        PlayerHandler.instance._entityEvents.OnOpenChest(ChestType.ChestResource);
        StartCoroutine(DestroyCrateProcess());
        
    }

    //every 10 it gives the stuff

    IEnumerator DestroyCrateProcess()
    {
        GameHandler.instance._soundHandler.CreateSfx(openChestClip, transform);
        graphic_Body.gameObject.SetActive(false);
        dustExplosionPS.gameObject.SetActive(true);
        dustExplosionPS.Play();

        //create hte itens and make them go above, and then move to the player
        //actually i will do nothing of this i will just add.

        yield return new WaitForSeconds(1.2f);



        Destroy(gameObject);
    }


    //instead of relying on the itens to send we will instantly do it and 
    void CreateItem()
    {

        //float _value = 1.5f;

        PlayerHandler.instance._playerInventory.AddItemInUI(new ItemClass(item.data, item.quantity));

        while (item.quantity > 0)
        {
            //float x = Random.Range(-_value, _value);
            //float z = Random.Range(-_value, _value);
            CreateObject(item.data, item.quantity);
            item.RemoveQuantity(item.quantity);

            continue;

            if (item.quantity >= 10)
            {
                //we remove those items adn give 10
                CreateObject(item.data, 10);
                item.RemoveQuantity(10);

            }else
            {
                
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
