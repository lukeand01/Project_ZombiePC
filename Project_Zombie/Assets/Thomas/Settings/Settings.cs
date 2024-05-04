using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    //it will have different categories.
    //General
    //Key bindings
    //Graphics
    //Language
    //Audio
    //Credits

    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;
    }
    private void Start()
    {
        //PlayerHandler.instance._playerController.block.AddBlock("TESTE", BlockClass.BlockType.Complete);
    }
    GameObject holder;
    [SerializeField] GameObject[] selectButtonArray;
    [SerializeField] GameObject[] optionHolderArray;
    [SerializeField] GameObject selector;
    int index;

    public void OpenSetting()
    {
        holder.SetActive(true);
        OpenCategory();
    }

    public void SelectCategory(int index)
    {


        this.index = index;
        OpenCategory();
        
    }

    void OpenCategory()
    {
        Debug.Log("Current index " + index);

        selector.transform.DOKill();
        selector.transform.DOMoveY(selectButtonArray[index].transform.position.y, 0.15f).SetUpdate(true);
        foreach (var item in optionHolderArray)
        {
            item.SetActive(false);
        }
        optionHolderArray[index].SetActive(true);
    }

}

public enum SettingsType 
{ 
    General = 0,
    KeyBindings = 1,
    Graphic = 2,
    Language = 3,
    Audio = 4,
    Credits = 5

}
