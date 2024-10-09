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


    EquipWindowUI _handler;

    private void Awake()
    {
        
    }

    [field: SerializeField] public ItemGunData _gunData { get; private set; }
    public void SetGun(ItemGunData gunData, EquipWindowUI handler)
    {
        this._handler = handler;

        if (gunData == null)
        {
            empty.SetActive(true);
            return;
        }
        empty.SetActive(false);

        this._gunData = gunData;


        icon.sprite = gunData.itemIcon;
        inputText.text = "";
 
    }
       

    [field:SerializeField]public AbilityActiveData _abilityData { get; private set; }

    public int abilityIndex { get; private set; } = -1;
    public void SetAbility(AbilityActiveData abilityData, EquipWindowUI handler)
    {

        this._handler = handler;

        if (abilityData == null)
        {
            empty.SetActive(true);

            return;
        }

        Debug.Log("was this called? " + abilityData.abilityName);
        empty.SetActive(false);
        this._abilityData = abilityData;


        icon.sprite = abilityData.abilityIcon;


    }
    public void SetAbilitySlotIndex(int index)
    {
        //Log("currentBulletIndex slot " + currentBulletIndex.ToString() + " was called " + gameObject.name);

        inputText.gameObject.SetActive(true);
        inputText.text = (index + 1).ToString();
        abilityIndex = index;

    }


    [field: SerializeField] public DropData _dropData { get; private set; }
    public int _dropIndex { get; private set; }  
    public void SetDrop(DropData dropData, EquipWindowUI handler)
    {
        _handler = handler;

        if (dropData == null)
        {
            empty.SetActive(true);
            return;
        }

        empty.SetActive(false);
        _dropData = dropData;


        icon.sprite = dropData.dropIcon;
    }



    public void UseRef(EquipWindowEquipUnit equipUnit)
    {
        Sprite rightIcon = null;
        string rightName = "";
        if(equipUnit._gunData != null)
        {
            rightIcon = equipUnit._gunData.itemIcon;
            rightName = equipUnit._gunData.itemName;
        }

        if(equipUnit._abilityData != null)
        {

            rightIcon = equipUnit._abilityData.abilityIcon;
            rightName = equipUnit._abilityData.abilityName;
        }

        if(equipUnit._dropData != null)
        {
            rightIcon = equipUnit._dropData.dropIcon;
            rightName = equipUnit._dropData.dropName;
        }
        //
        empty.SetActive(false);
        icon.sprite = rightIcon;
        

    }





    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        selected.SetActive(true);

        //call pause and the stats 

        if(_handler == null)
        {
            return;
        }

        if (!_handler.isOpen) return;


        _handler.StartHover(this);

        if (_handler.isDragging) return;

        if (_gunData != null)
        {
            _handler.descriptionWindow.DescribeGunData(_gunData, transform);
            return;
        }

        if(_abilityData != null)
        {
            _handler.descriptionWindow.DescribeAbilityData(_abilityData, transform);
            return;
        }

        if(_dropData != null)
        {
            _handler.descriptionWindow.DescribeDrop(_dropData, transform);
            return;
        }

    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        selected.SetActive(false);

        if(_handler == null)
        {
            return;
        }

        _handler.EndHover();

        if (_handler.isDragging) return;

        _handler.descriptionWindow.StopDescription();
    }


    public void ReceiveInfo(EquipWindowEquipUnit draggingUnit)
    {

        if (!isPlayer) return;

        //currently its only dealing with gun. it needs to deal with drop aswell.

        if(draggingUnit._gunData != null && _gunData != null)
        {

            //when we swap we pass this new information to the player and we tell him to equip this new gun_Perma
            PlayerHandler.instance._playerCombat.ReceivePermaGun(draggingUnit._gunData);
            ItemGunData newPermaGun = PlayerHandler.instance._playerCombat.GetCurrentPermaGun();
            SetGun(newPermaGun, _handler);
            GameHandler.instance._soundHandler.CreateSfx(SoundType.AudioClip_Equip_Gun);

            return;
        }     

        if (draggingUnit._abilityData != null && abilityIndex != -1) 
        {

            //this is teh hover unit
            //i want the dragging unit to give stuff to this and then disappear
            PlayerHandler.instance._playerAbility.ReplaceActiveAbility(draggingUnit._abilityData, abilityIndex);
            SetAbility(draggingUnit._abilityData, _handler);
            draggingUnit.RemoveAbilityFromPlayer();
            GameHandler.instance._soundHandler.CreateSfx(SoundType.AudioClip_Equip_Ability);
            return;
        }
        if (draggingUnit._dropData != null && _dropIndex != -1)
        {

            Debug.Log("drop data");
            //this is teh hover unit
            //i want the dragging unit to give stuff to this and then disappear
            //PlayerHandler.instance._playerAbility.ReplaceActiveAbility(draggingUnit._abilityData, abilityIndex);
            SetDrop(draggingUnit._dropData, _handler);
            _handler.CreateListForDrop();
            //draggingUnit.RemoveAbilityFromPlayer();
            GameHandler.instance._soundHandler.CreateSfx(SoundType.AudioClip_Equip_Drop);
            return;
        }

    }


    public void RemoveAbilityFromPlayer()
    {
        if (!isPlayer) return;
        empty.SetActive(true);
        _abilityData = null;

        PlayerHandler.instance._playerAbility.ReplaceActiveAbility(null, abilityIndex);
    }

}
