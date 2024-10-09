using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GridBrushBase;

public class ToolUnit_Hover : ButtonBase
{
    ToolUnit _unit;

   public void SetUp(ToolUnit unit)
    {
        _unit = unit;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        UIHandler.instance._DescriptionWindow.Describe_ToolData(_unit._tool._data, transform);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        UIHandler.instance._DescriptionWindow.StopDescription();
    }


}
