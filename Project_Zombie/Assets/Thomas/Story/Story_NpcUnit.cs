using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Story_NpcUnit : ButtonBase
{
    Story_NpcData npc;
    [SerializeField] GameObject warnImage;
    public void SetUp(Story_NpcData npc)
    {
        this.npc = npc;
    }

    //this needs to know if there is a new dialogue avaialable or new quest.


    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        UIHandler.instance._DialogueUI.StartDialogue(npc);

        //we
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        ControlSelected(true);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        ControlSelected(false);
    }


}
