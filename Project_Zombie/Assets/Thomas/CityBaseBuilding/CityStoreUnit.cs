using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CityStoreUnit : ButtonBase
{
    //this will be able to take an ability, gun or a command for either upgrading itself or the player´s roll level.

    [SerializeField] GameObject selected;
    [SerializeField] GameObject locked;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI nameText;

    ItemGunData gunData;
    AbilityActiveData abilityData;
    

    public void SetAbility(AbilityActiveData abilityData, int index)
    {

    }
    public void SetGUn(ItemGunData gundata, int index)
    {

    }


    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
    }

}
