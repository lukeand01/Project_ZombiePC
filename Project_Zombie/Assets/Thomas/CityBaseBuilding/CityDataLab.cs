using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "City Data / Lab")]
public class CityDataLab : CityData
{

    [SerializeField] List<AbilityCostClass> abilityCostList = new();
    [SerializeField] List<int> liberatedAbilityList = new();

}
[System.Serializable]
public class AbilityCostClass
{
    public string nameClass;
    public AbilityActiveData abilityData;
    public int levelRequired; //at what level can you do stuff?
    public List<CityCostClass> costList = new();
}


//how do i put stuff
//for now i will just put as blocks in a grid. thats all.