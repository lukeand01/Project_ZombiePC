using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] ChestAbility chestAbilityTemplate;

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
    [SerializeField] EnemyData data;
    [ContextMenu("DEBUG SPAWN")]
    public void DebugSpawn()
    {
        Spawn(data);
    }

    public void OrderSpawn(EnemyData enemy)
    {

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
        Debug.Log("we call to spawn");

        int round = LocalHandler.instance.round;
        EnemyBase newObject = Instantiate(enemy.enemyModel, transform.position + Vector3.forward, Quaternion.identity);
        newObject.SetStats(round);
        newObject.SetChest(chestAbilityTemplate);
        chestAbilityTemplate = null;

        spawnTotal = Random.Range(3, 5);
        spawnCurrent = spawnTotal;

        newObject.eventDied += handler.EnemyDied;
    }


    public void SetNextSpawnToCarryChest(ChestAbility chestAbilityTemplate)
    {
        this.chestAbilityTemplate = chestAbilityTemplate;
    }

    public void OpenForSpawn()
    {
        isRoomOpen = true;
    }


}


//how do i know what room the player is currently in?