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

    [SerializeField] GameObject graphic;
    Vector3 originalPos;

    LocalHandler handler;

    bool isSpawning;

    private void Start()
    {
        handler = LocalHandler.instance;
        originalPos = transform.position;
    }

    private void Awake()
    {
        spawnTotal = Random.Range(0.5f, 3.5f);
    }

    private void FixedUpdate()
    {
        if (spawnTotal == 0)
        {
            Debug.Log("spawn total");
            return;
        }
        if (isSpawning) return;

        if (spawnCurrent > 0)
        {
            spawnCurrent -= Time.fixedDeltaTime;

        }
        else
        {
            if (enemyDespawnedList.Count > 0)
            {
                SpawnDespawned();
                return;
            }

            if (spawnQueueList.Count > 0)
            {

                StartCoroutine(SpawnProcess());
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

        //we dont spawn this if it has passed hte cpa.

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
    [ContextMenu("yo")]
    public void Yo()
    {
        StartCoroutine(SpawnProcess());
    }
    IEnumerator SpawnProcess()
    {
        //we shake this thing a bit
        //

        isSpawning = true;

        int rounds = 25;
        float timer = 1.5f / rounds;

        float randomValue = 0.08f;

        for (int i = 0; i < rounds; i++)
        {
            float randomX = Random.Range(-randomValue, randomValue);
            float randomY = 0;

            transform.position = originalPos + transform.right * randomX ;
            yield return new WaitForSeconds(timer);
        }

        //this wont work

        Spawn(spawnQueueList[0]);
        spawnQueueList.RemoveAt(0);
        isSpawning = false;
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