using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public EnemyBase enemyModel;
    public EnemyBoss bossModel;

    public float attackRange; //the only one we can use here. every single fella will have just one attack so thats fine.
    public float AttackAnimationSpeed = 1;
    public float attackRest;
    [field:SerializeField] public DamageType damageType {  get; private set; }

    [Separator("STATS")]
    public List<StatClass> initialStatList = new();
    public List<StatClass> scaleStatList = new();
    public List<DamageTypeClass> enemyResistanceList = new();

    [Separator("SOUND")]
    public AudioClip audio_Hit;
    public AudioClip audio_Dead;
    public AudioClip audio_Spawned;
    public AudioClip audio_Attack;

    [Separator("INFO ABOUT GAME SPAWN")]
    public bool shouldNotDespawnBecauseOfDistance;


    [Separator("INFO ABOUT ALLY")]
    [SerializeField] bool canIgnoreAlly;
    [SerializeField] bool canTurnAlly; //

    public bool CanIgnoreAlly { get { return canIgnoreAlly; } }
    public bool CanTurnAlly { get { return canTurnAlly; } }

    private void OnEnable()
    {
        if(enemyResistanceList.Count == 0)
        {
            enemyResistanceList.Add(new DamageTypeClass(DamageType.Physical, 1));
            enemyResistanceList.Add(new DamageTypeClass(DamageType.Magical, 1));
            enemyResistanceList.Add(new DamageTypeClass(DamageType.Plasma, 1));
            enemyResistanceList.Add(new DamageTypeClass(DamageType.Corrupt, 1));
        }
    }


}

