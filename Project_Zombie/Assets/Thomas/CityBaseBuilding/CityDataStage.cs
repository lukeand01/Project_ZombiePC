using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CityData / Stage")]
public class CityDataStage : CityData
{
    //this controls what missions you have and what you play now
    [SerializeField] List<CityStageClass> cityStageClassList = new();


}

[System.Serializable]
public class CityStageClass
{
    public StageData stageData;
    //for now i will have no conditions
}
