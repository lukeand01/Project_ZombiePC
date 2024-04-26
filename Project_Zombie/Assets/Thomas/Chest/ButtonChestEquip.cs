using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonChestEquip : ButtonBase
{
    //it needs to handle certain things
    //when you have no selected fella but need one it needs to warn
    //ask for confirm when you are about to swap
    //when you dont need to replace a gun and you have one selected it just ignores.


    //i will think that wiull create more problems i will be doing through here.

    ChestUI handler;

    private void Start()
    {
        handler = UIHandler.instance.ChestUI;
    }


    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        //first if you can just equip.
        handler.GunEquip(this);
    }

}
