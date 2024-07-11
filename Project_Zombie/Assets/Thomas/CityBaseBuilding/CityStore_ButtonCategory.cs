using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CityStore_ButtonCategory : ButtonBase
{
    //

    int index;
    CityCanvas _canvas;

    public void SetUp(int index, CityCanvas _canvas)
    {
        this.index = index;
        this._canvas = _canvas;


        SetText(index.ToString());
    }


    public void Select()
    {
        ControlMouseClick(true);
    }
    public void UnSelect()
    {
        ControlMouseClick(false);
    }
    private void OnDisable()
    {
        UnSelect();
    }


    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        _canvas.SelectNewCategory(this, index);

    }
    
    

}
