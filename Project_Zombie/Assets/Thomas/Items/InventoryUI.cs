using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{

    //we will use this only for resource.

    #region UPDATE INVENTORY LIST



    #endregion

    #region UI FOR NEW ITEM
    [SerializeField] Transform gridContainer;
    [SerializeField] ItemNotificationUnit _itemNotificationUnit;
    [SerializeField] Transform itemNotificationSpawnPos;
    List<ItemNotificationUnit> itemNotifactionList = new();
    List<ItemClass> itemNotificationWaitingList = new(); //this is foir when we exceed the limit of the first list.

    float nextNotificationCurrent;
    float nextNotificationTotal;

    //this thing will keep focring the right fellas into the right positions always.
    //


    //the first one is the list is always the one in the botton in the list and the first to disappear.
    //the limit list ios 4 so if there is more then we put in the waiting list.
    //the distance is based in the unit size. so its consistent.

    //i could create two fellas. one that will stick and the other that will remain in the grid.


    private void Update()
    {
        
        


    }


    public void CallItemNotification(ItemClass item)
    {

        ItemNotificationUnit fakeObject = Instantiate(_itemNotificationUnit);
        fakeObject.transform.localScale = Vector3.one;
        fakeObject.MakeFake();
        fakeObject.transform.SetParent(gridContainer);

        ItemNotificationUnit newObject = Instantiate(_itemNotificationUnit);
        newObject.transform.SetParent(itemNotificationSpawnPos);
        newObject.transform.position = itemNotificationSpawnPos.position;
        newObject.transform.localScale = Vector3.one;
        newObject.SetUp(this, item, fakeObject.transform);


        itemNotifactionList.Add(newObject);
    }


    public void RemoveFirstInList()
    {
        if(itemNotifactionList.Count > 0)
        {
            itemNotifactionList.RemoveAt(0);
        }
        else
        {
            Debug.Log("couldnt remove");
        }
        
    }

    #endregion



}
