using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    public string enemyName;


    public List<StatClass> initialStatList = new();
    public List<StatClass> scaleStatList = new();

}
