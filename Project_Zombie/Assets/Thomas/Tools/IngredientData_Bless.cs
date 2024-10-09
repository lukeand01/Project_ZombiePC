using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Tool / IngredientBless")]
public class IngredientData_Bless : IngredientData
{
    [Separator("BLESS")]
    [SerializeField] int blessValue;

    public override void OnHarvested(ToolData data)
    {
        base.OnHarvested(data);

        PlayerHandler.instance._playerResources.Bless_Gain(blessValue);
    }


}
