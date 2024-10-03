using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Tool / IngredientPoints")]
public class IngredientData : ScriptableObject
{
    public string _name;
    public Sprite _icon;
    [TextArea]public string _description;
    [SerializeField] int points;

    public virtual void OnHarvested(ToolData data)
    {
        //warn that it captured this fella.

        PlayerHandler.instance._entityStat.CallToolNameUI(data._toolName);
        PlayerHandler.instance._entityStat.CallToolHarvestUI(_name);
        PlayerHandler.instance._playerResources.GainPoints(points);
    }


}
