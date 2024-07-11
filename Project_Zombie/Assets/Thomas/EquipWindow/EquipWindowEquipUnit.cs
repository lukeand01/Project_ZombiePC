using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipWindowEquipUnit : ButtonBase
{

    [SerializeField] GameObject selected;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI inputText;
    [SerializeField] GameObject empty;

    [SerializeField] bool isPlayer;



    [field: SerializeField] public bool cannotDrag {  get; private set; }


    EquipWindowUI handler;

    private void Awake()
    {
        
    }

    public ItemGunData gunData { get; private set; }
    public void SetGun(ItemGunData gunData, EquipWindowUI handler)
    {
        this.handler = handler;

        if (gunData == null)
        {
            empty.SetActive(true);
            return;
        }
        empty.SetActive(false);

        this.gunData = gunData;


        icon.sprite = gunData.itemIcon;
        inputText.text = "";
 
    }
       

    public AbilityActiveData abilityData { get; private set; }

    public int abilityIndex { get; private set; } = -1;
    public void SetAbility(AbilityActiveData abilityData, EquipWindowUI handler)
    {

        this.handler = handler;

        if (abilityData == null)
        {
            empty.SetActive(true);
            return;
        }

        empty.SetActive(false);
        this.abilityData = abilityData;


        icon.sprite = abilityData.abilityIcon;


    }
    public void SetAbilitySlotIndex(int index)
    {
        //Log("index slot " + index.ToString() + " was called " + gameObject.name);

        inputText.gameObject.SetActive(true);
        inputText.text = (index + 1).ToString();
        abilityIndex = index;

    }

    public void UseRef(EquipWindowEquipUnit equipUnit)
    {
        Sprite rightIcon = null;
        string rightName = "";
        if(equipUnit.gunData != null)
        {
            rightIcon = equipUnit.gunData.itemIcon;
            rightName = equipUnit.gunData.itemName;
        }

        if(equipUnit.abilityData != null)
        {

            rightIcon = equipUnit.abilityData.abilityIcon;
            rightName = equipUnit.abilityData.abilityName;
        }

        empty.SetActive(false);
        icon.sprite = rightIcon;
        

    }


    public void SetDrop(DropData data, EquipWindowUI handler)
    {

    }


    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        selected.SetActive(true);

        //call pause and the stats 

        if(handler == null)
        {
            Debug.Log("handler was called");
            return;
        }

        if (!handler.isOpen) return;


        handler.StartHover(this);

        if (handler.isDragging) return;

        if (gunData != null)
        {
            handler.descriptionWindow.DescribeGunData(gunData, transform);
            return;
        }

        if(abilityData != null)
        {
            handler.descriptionWindow.DescribeAbilityData(abilityData, transform);
            return;
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        selected.SetActive(false);

        if(handler == null)
        {
            return;
        }

        handler.EndHover();

        if (handler.isDragging) return;

        handler.descriptionWindow.StopDescription();
    }


    public void ReceiveInfo(EquipWindowEquipUnit draggingUnit)
    {

        if (!isPlayer) return;


        if(draggingUnit.gunData != null && gunData != null)
        {

            //when we swap we pass this new information to the player and we tell him to equip this new gun
            PlayerHandler.instance._playerCombat.ReceivePermaGun(draggingUnit.gunData);
            ItemGunData newPermaGun = PlayerHandler.instance._playerCombat.GetCurrentPermaGun();
            Debug.Log("this is the new perma gun " + newPermaGun.itemName);
            SetGun(newPermaGun, handler);


            return;
        }     

        if (draggingUnit.abilityData != null && abilityIndex != -1) 
        {

            //this is teh hover unit
            //i want the dragging unit to give stuff to this and then disappear
            PlayerHandler.instance._playerAbility.ReplaceActiveAbility(draggingUnit.abilityData, abilityIndex);
            SetAbility(draggingUnit.abilityData, handler);
            draggingUnit.RemoveAbilityFromPlayer();
            
            return;

        }


    }


    public void RemoveAbilityFromPlayer()
    {
        if (!isPlayer) return;
        empty.SetActive(true);
        abilityData = null;

        PlayerHandler.instance._playerAbility.ReplaceActiveAbility(null, abilityIndex);
    }

}
