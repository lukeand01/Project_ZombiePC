using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TreeSeed : MonoBehaviour
{
    //it signals where its going to land
    //it does an arc like a grenade
    //when it falls it decides on either spawning a plant or a trap.

    //it actually is decide on launch. the color changes depending on the kind of plant it will be spawned.

    [SerializeField] Material[] _materialArray;
    [SerializeField] MeshRenderer _meshRend;
    [SerializeField] EnemyData _plantData;
    SeedSpawnType _spawnType;

    int _treePhase;

    public void SetSeed(SeedSpawnType spawnType, int treePhase)
    {
        _meshRend.material = _materialArray[(int)spawnType];

        _spawnType = spawnType;
        _treePhase = treePhase;
    }

    public void Explode(Vector3 rightPos)
    {
        //spawn at the right position the item.

        if (_spawnType == SeedSpawnType.Nothing)
        {
            Debug.Log("nothing");
            return;
        }

        if(_spawnType == SeedSpawnType.Trap)
        {
            SpawnTrap(rightPos);
        }
        if(_spawnType == SeedSpawnType.Plant)
        {
            SpawnPlant(rightPos);
        }

        GameHandler.instance._pool.GetPS(PSType.Explosion_01, transform);
        gameObject.SetActive(false);

    }

    void SpawnTrap(Vector3 rightPos)
    {
       TrapBase trap =  GameHandler.instance._pool.GetTrap(TrapType.BearTrap, transform);
       trap.transform.position = rightPos;
       trap.SetDestroy(15);
    }
    void SpawnPlant(Vector3 rightPos)
    {
        EnemyBase plant = GameHandler.instance._pool.GetEnemy(_plantData, transform.position);
        plant.transform.position = rightPos;
        plant.SetStats(LocalHandler.instance.round + (_treePhase * 5));
        if(plant == null)
        {
            Debug.Log("base of plant");
            return;
        }


    }

}

public enum SeedSpawnType
{
    Nothing = 0, 
    Trap = 1,
    Plant = 2,
    StrongPlant = 3
}