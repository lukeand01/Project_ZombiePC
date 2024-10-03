using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tool / Tool")]
public class ToolData : ScriptableObject
{
    public string _toolName;
    public Sprite _icon;
    public IngredientClass[] _ingredientArray;


}

[System.Serializable]
public class IngredientClass
{
    public IngredientData data;
    public int chance;
}
public class ToolClass
{
    public ToolData _data { get; private set;}



    Dictionary<IngredientData, int> ingredientDictionary = new();

    public ToolClass(ToolData data)
    {
        _data = data;

        ingredientDictionary.Clear();
        for (int i = 0; i < _data._ingredientArray.Length; i++)
        {
            var item = _data._ingredientArray[i];

            ingredientDictionary.Add(item.data, 0);

        }
    }


    public float GetIngredientValue(IngredientData data)
    {
        if (ingredientDictionary.ContainsKey(data)) return 0;

        return ingredientDictionary[data];
    }

    public void AddIngredient(IngredientData data, int quantity = 1)
    {
        if (ingredientDictionary.ContainsKey(data))
        {
            ingredientDictionary[data] += quantity;
            UpdateUnit();
        }
        else
        {

        }
    }
    public void RemoveIngredient(IngredientData data, int quantity = 1)
    {
        if (ingredientDictionary.ContainsKey(data))
        {
            ingredientDictionary[data] -= quantity;
            UpdateUnit();
        }
        else
        {
            Debug.Log("there was nothing here");
        }
    }
    public bool HasIngredient(IngredientData data, int quantity)
    {
        if (!ingredientDictionary.ContainsKey(data)) return false;

        return ingredientDictionary[data] >= quantity;
    }


    ToolUnit _unit;

    public void SetUnit(ToolUnit unit)
    {
        _unit = unit;
    }

    public void UpdateUnit()
    {
        if(_unit != null)
        {
            _unit.UpdateUnit();
        }
    }

}