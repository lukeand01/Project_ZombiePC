using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tool / Tool")]
public class ToolData : ScriptableObject
{
    public string _toolName;
    public Sprite _icon;
    public IngredientClass[] _ingredientArray;
    public ToolType _toolType;
    [TextArea]public string _description;
    public AudioClip _harvestAudioClip;

    public virtual void OnAdded()
    {
        //here we add the event tot he knife.
    }
    public virtual void OnRemoved()
    {

    }

    public IngredientData GetRandomIngredient()
    {
        int safeBreak = 0;

        int roll = Random.Range(0, 101);

        while (true)
        {
            safeBreak++;

            if (safeBreak > 1000) break;

            
            int random = Random.Range(0, _ingredientArray.Length);
            var item = _ingredientArray[random];

            if(roll > item.chance)
            {
                return item.data;
            }
            else
            {
                roll += 5;
            }

        }

        return null;
    }

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

            if(data == null)
            {
                Debug.Log("no data");
            }

            ingredientDictionary.Add(item.data, 0);

        }
    }


    public float GetIngredientValue(IngredientData data)
    {

        if(data == null)
        {
            return 0;
        }

        if(ingredientDictionary == null)
        {
            Debug.Log("yo");
            return 0;
        }

        if (!ingredientDictionary.ContainsKey(data)) return 0;

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

public enum ToolType
{
    Fishrod,
    Bugnet,
    HuntingKnife
}