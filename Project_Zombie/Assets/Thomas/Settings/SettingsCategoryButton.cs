using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SettingsCategoryButton : ButtonBase
{
    [Separator("SETTINGS")]
    public SettingsType settingsIndex;

    

    Settings _settingsHandler;

    public void SetUp(Settings _settingsHandler)
    {
        this._settingsHandler = _settingsHandler;
        SetText(settingsIndex.ToString());

        ControlMouseClick(false);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        _settingsHandler.SelectCategory((int)settingsIndex);
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

    private void OnDisable()
    {
        ControlSelected(false);
    }

}
