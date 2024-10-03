using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tool / IngredientRandomAbility")]
public class IngredientData_RandomPassive : IngredientData
{
    [SerializeField] List<AbilityPassiveData> abilityList = new();

    public override void OnHarvested(ToolData data)
    {

        base.OnHarvested(data);

        int random = Random.Range(0, abilityList.Count);
        PlayerHandler.instance._playerAbility.AddAbility(abilityList[random]);

    }


}
