using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFollowTillEnd : MonoBehaviour
{
    //this will follow the target till the end.
    //once it arrives it gives the item to the player. 

    ItemClass item;
    Transform target;

    float current;
    float total;

    //its 2d and it needs to be always facing the camera.

    public void SetUp(ItemClass item, Transform target)
    {
        this.item = new ItemClass(item.data, item.quantity);
        this.target = target;

        total = 0.1f;
        current = 0;
    }

    private void Update()
    {
        if (target == null) return;

        if(total > current)
        {
            current += Time.deltaTime;
            return;
        }



        //float distance = Vector3.Distance(transform.position, target.position);

       
        if (transform.position != target.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * 50);
        }
        else
        {
            Act();
        }
    }

    void Act()
    {
        target = null;
        PlayerHandler.instance._playerInventory.AddItemForStage(item);

        transform.DOScale(0, 0.15f);
        Invoke(nameof(CallDestroy), 0.2f);
    }

    void CallDestroy()
    {
        Destroy(gameObject);
    }


}
