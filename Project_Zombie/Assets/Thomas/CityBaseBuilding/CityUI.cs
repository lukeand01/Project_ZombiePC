using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CityUI : MonoBehaviour
{
    //this is the city ui.
    GameObject holder;

    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;
    }

    public void ControlUI(bool isVisible)
    {

        holder.SetActive(isVisible);
    }

    [Separator("RESOURCE")]
    [SerializeField] ResourceUnit resourceUnitTemplate;
    [SerializeField] Transform resourceContainer;
    [SerializeField] ItemResourceWindow itemResourceWindow;
    List<ResourceUnit> resourceUnitList = new();
    
   
    public void CreateResourceUnitForItem(ItemClass item)
    {
        ResourceUnit newObject = Instantiate(resourceUnitTemplate);
        newObject.SetUp(item);
        newObject.transform.SetParent(resourceContainer);


    }


    [Separator("BUILDING")]
    [SerializeField] CityStoreUnit cityStoreTemplate;
    [SerializeField] Transform cityStoreContainer;

    CityData currentData;

}


//what should i do about ui?
//here we only create the thing. for it to work you have to go thte armory and equip it.