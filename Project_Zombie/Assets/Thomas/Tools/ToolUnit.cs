using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolUnit : MonoBehaviour
{
    //


    public ToolClass _tool {  get; private set; }

    [SerializeField] Image _iconImage;
    [SerializeField] IngredientUnit _ingredientUnitTemplate;
    [SerializeField] Transform _container;
    [SerializeField] ToolUnit_Hover _hover;
    List<IngredientUnit> _ingredientList = new();
    public void SetUp(ToolClass tool)
    {
        _tool = tool;
        _tool.SetUnit(this);

        _iconImage.sprite = tool._data._icon;

        SetIngredients();
        UpdateUnit();

        _hover.SetUp(this);
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

            if(item.data == null)
            {
                Debug.Log("1");
            }

            newObject.UpdateValue((int)_tool.GetIngredientValue(item.data));
            _ingredientList.Add(newObject);
        }

    }


    public void UpdateUnit()
    {
        for (int i = 0; i < _ingredientList.Count; i++)
        {
            var item = _ingredientList[i];
            if(item._data == null)
            {
                Debug.Log("2");
            }
            item.UpdateValue((int)_tool.GetIngredientValue(item._data));
        }
    }

    private void OnDisable()
    {
        UIHandler.instance._DescriptionWindow.StopDescription();
    }

 
}
