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

    List<EnemyData> spawnQueueList = new();

    [SerializeField] ChestAbility chestAbilityTemplate;

    private void FixedUpdate()
    {
        if (spawnTotal == 0) return;

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

    public void Spawn(EnemyData enemy)
    {

        Debug.Log("called spawn");
        //put the fella in the world.
        if (spawnCurrent > 0 && spawnQueueList.Count < 4)
        {
            //we can keep spawning.
            spawnQueueList.Add(enemy);
            return;
        }
        int round = LocalHandler.instance.round;
        EnemyBase newObject = Instantiate(enemy.enemyModel, transform.position + Vector3.forward , Quaternion.identity);
        newObject.SetStats(round);
        newObject.SetChest(chestAbilityTemplate);
        chestAbilityTemplate = null;

        spawnTotal = Random.Range(5, 10);
        spawnCurrent = spawnTotal;
        
        


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