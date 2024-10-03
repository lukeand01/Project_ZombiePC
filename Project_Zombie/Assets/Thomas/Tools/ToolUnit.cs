using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolUnit : ButtonBase
{
    //


    ToolClass _tool;

    [SerializeField] Image _iconImage;
    [SerializeField] IngredientUnit _ingredientUnitTemplate;
    [SerializeField] Transform _container;
    List<IngredientUnit> _ingredientList = new();
    public void SetUp(ToolClass tool)
    {
        _tool = tool;
        _tool.SetUnit(this);

        _iconImage.sprite = tool._data._icon;

        SetIngredients();
        UpdateUnit();
    }

    void SetIngredients()
    {
        for (int i = 0; i < _container.childCount; i++)
        {
            var item = _container.GetChild(i).gameObject;
            Destroy(item);
        }

        _ingredientList.Clear();

        for (int i = 0; i < _tool._data._ingredientArray.Length; i++)
        {
            var item = _tool._data._ingredientArray[i];

            IngredientUnit newObject = Instantiate(_ingredientUnitTemplate);
            newObject.transform.SetParent(_container);
            newObject.Set(item.data);
            newObject.UpdateValue((int)_tool.GetIngredientValue(item.data));
            _ingredientList.Add(newObject);
        }

    }


    public void UpdateUnit()
    {
        for (int i = 0; i < _ingredientList.Count; i++)
        {
            var item = _ingredientList[i];

            item.UpdateValue((int)_tool.GetIngredientValue(item._data));
        }
    }
}
