using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Portal : MonoBehaviour
{

    //it has its own cooldown

    float spawnCurrent;
    float spawnTotal;


    float blockedCurrent;
    float blockedTotal;

    bool isRoomOpen; //this is to know that it belongsg to a room that it is working.
    bool isBlocked; //this is for abilities that close the portal.

    [SerializeField] List<EnemyData> spawnQueueList = new();
    List<EnemyBase> enemyDespawnedList = new();
    [SerializeField] ChestAbility chestAbilityTemplate;
    [SerializeField] Transform spawnPoint;

    LocalHandler handler;

    private void Start()
    {
        handler = LocalHandler.instance;
    }

    private void Awake()
    {
        spawnTotal = Random.Range(2, 6);
    }

    private void FixedUpdate()
    {
        if (spawnTotal == 0)
        {
            Debug.Log("spawn total");
            return;
        }

        if(spawnCurrent > 0)
        {
            spawnCurrent -= Time.fixedDeltaTime;

        }
        else
        {
            if(enemyDespawnedList.Count > 0)
            {
                SpawnDespawned();
                return;
            }

            if(spawnQueueList.Count > 0)
            {

                Spawn(spawnQueueList[0]);
                spawnQueueList.RemoveAt(0);

                //this will hlep spawn things at different times.
            }
        }
    }

    
    public bool CanSpawn()
    {
        return isRoomOpen && !isBlocked && spawnCurrent == 0;
    }


    public void OrderSpawn(EnemyData enemy)
    {
        Debug.Log("this was ordered to spawn");

        if (spawnQueueList.Count > 0)
        {
            //then we simply add to the list.
        }
        else
        {
            spawnTotal = Random.Range(3, 5);
            spawnCurrent = spawnTotal;
        }
        spawnQueueList.Add(enemy);
    }

    public void Spawn(EnemyData enemy)
    {



        int round = LocalHandler.instance.round;
        // EnemyBase newObject = Instantiate(enemy.enemyModel, spawnPoint.transform.position + Vector3.forward, Quaternion.identity);
        EnemyBase newObject = GameHandler.instance._pool.GetEnemy(enemy, spawnPoint.transform.position + Vector3.forward);

        newObject.gameObject.name = enemy.name;
        newObject.SetStats(round);
        chestAbilityTemplate = null;

        spawnTotal = Random.Range(1, 3);
        spawnCurrent = spawnTotal;

        newObject.eventDied += handler.EnemyDied;
    }


    public void OrderRespawn(EnemyBase enemy)
    {
        enemyDespawnedList.Add(enemy);  
    }

    void SpawnDespawned()
    {
        Debug.Log("portal Respawned");

        enemyDespawnedList[0].transform.position = spawnPoint.transform.position + Vector3.forward;
        enemyDespawnedList[0].gameObject.SetActive(true);
        enemyDespawnedList.RemoveAt(0);

        spawnTotal = Random.Range(1, 3);
        spawnCurrent = spawnTotal;

    }

    public void SetNextSpawnToCarryChest(ChestAbility chestAbilityTemplate)
    {
        this.chestAbilityTemplate = chestAbilityTemplate;
    }


    //so as it stands its localhandler who tells who has the chest ability.
    //but now what he should do is the following: check if should spawn ability, then check for ammo, then check for drop
    //most of the time none of them should be spawned.

    //


    public void OpenForSpawn()
    {
        isRoomOpen = true;
    }

    public bool HasEnemyToSpawn()
    {
        return spawnQueueList.Count > 0;    
    }
}


//how do i know what room the player is currently in?