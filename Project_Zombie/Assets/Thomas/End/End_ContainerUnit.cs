using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class End_ContainerUnit : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] Transform _container;

    public void SetText(string stringValue)
    {
        _text.text = stringValue;
    }
    public void SetContainer(List<Transform> unitList)
    {
        foreach (var item in unitList)
        {
            item.SetParent(_container);
        }
    }

}
