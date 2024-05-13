using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityStore_Lab : CityStore
{
    [SerializeField] CityDataLab labData;



    private void Start()
    {
        _cityCanvas.SetAbilities(labData);
    }
}
