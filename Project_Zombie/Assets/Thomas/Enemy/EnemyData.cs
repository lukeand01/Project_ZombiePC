using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public EnemyBase enemyModel;

    public float attackRange; //the only one we can use here. every single fella will have just one attack so thats fine.
    public float attackSpeed;

    

    [Separator("STATS")]
    public List<StatClass> initialStatList = new();
    public List<StatClass> scaleStatList = new();

    [Separator("SOUND")]
    public AudioClip audio_Hit;
    public AudioClip audio_Dead;
    public AudioClip audio_Spawned;
    public AudioClip audio_Attack;

    [Separator("INFO ABOUT GAME SPAWN")]
    public bool shouldNotDespawnBecauseOfDistance;

}
